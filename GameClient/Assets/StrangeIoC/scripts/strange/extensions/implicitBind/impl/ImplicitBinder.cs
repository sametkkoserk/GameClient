﻿/*
 * Copyright 2013 ThirdMotion, Inc.
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StrangeIoC.scripts.strange.extensions.implicitBind.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.injector.api;
using StrangeIoC.scripts.strange.extensions.injector.impl;
using StrangeIoC.scripts.strange.extensions.mediation.api;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace StrangeIoC.scripts.strange.extensions.implicitBind.impl
{
  public class ImplicitBinder : IImplicitBinder
  {
    //Hold a copy of the assembly so we aren't retrieving this multiple times. 
    private Assembly assembly;

    [Inject]
    public IInjectionBinder injectionBinder { get; set; }

    [Inject]
    public IMediationBinder mediationBinder { get; set; }

    /// <summary>
    ///   Search through indicated namespaces and scan for all annotated classes.
    ///   Automatically create bindings
    /// </summary>
    /// <param name="usingNamespaces">Array of namespaces. Compared using StartsWith. </param>
    public virtual void ScanForAnnotatedClasses(string[] usingNamespaces)
    {
      if (assembly != null)
      {
        IEnumerable<Type> types = assembly.GetExportedTypes();

        var typesInNamespaces = new List<Type>();
        var namespacesLength = usingNamespaces.Length;
        for (var ns = 0; ns < namespacesLength; ns++) typesInNamespaces.AddRange(types.Where(t => !string.IsNullOrEmpty(t.Namespace) && t.Namespace.StartsWith(usingNamespaces[ns])));

        var implementsBindings = new List<ImplicitBindingVO>();
        var implementedByBindings = new List<ImplicitBindingVO>();

        foreach (var type in typesInNamespaces)
        {
          var implements = type.GetCustomAttributes(typeof(Implements), true);
          var implementedBy = type.GetCustomAttributes(typeof(ImplementedBy), true);
          var mediated = type.GetCustomAttributes(typeof(MediatedBy), true);
          var mediates = type.GetCustomAttributes(typeof(Mediates), true);

          #region Concrete and Interface Bindings

          //Interfaces first
          if (implementedBy.Any())
          {
            var implBy = (ImplementedBy)implementedBy.First();
            if (implBy.DefaultType.GetInterfaces().Contains(type)) //Verify this DefaultType exists and implements the tagged interface
              implementedByBindings.Add(new ImplicitBindingVO(type, implBy.DefaultType, implBy.Scope == InjectionBindingScope.CROSS_CONTEXT, null));
            else
              throw new InjectionException("Default Type: " + implBy.DefaultType.Name + " does not implement annotated interface " + type.Name,
                InjectionExceptionType.IMPLICIT_BINDING_IMPLEMENTOR_DOES_NOT_IMPLEMENT_INTERFACE);
          }

          if (implements.Any())
          {
            var interfaces = type.GetInterfaces();

            object name = null;
            var isCrossContext = false;
            var bindTypes = new List<Type>();

            foreach (Implements impl in implements)
            {
              //Confirm this type implements the type specified
              if (impl.DefaultInterface != null)
              {
                //Verify this Type implements the passed interface
                if (interfaces.Contains(impl.DefaultInterface) || type == impl.DefaultInterface)
                  bindTypes.Add(impl.DefaultInterface);
                else
                  throw new InjectionException("Annotated type " + type.Name + " does not implement Default Interface " + impl.DefaultInterface.Name,
                    InjectionExceptionType.IMPLICIT_BINDING_TYPE_DOES_NOT_IMPLEMENT_DESIGNATED_INTERFACE);
              }
              else //Concrete
              {
                bindTypes.Add(type);
              }

              isCrossContext = isCrossContext || impl.Scope == InjectionBindingScope.CROSS_CONTEXT;
              name = name ?? impl.Name;
            }

            var thisBindingVo = new ImplicitBindingVO(bindTypes, type, isCrossContext, name);

            implementsBindings.Add(thisBindingVo);
          }

          #endregion

          //Handle mediations here. We have no need to re-iterate over them to prioritize anything. Not yet, at least.

          #region Mediations

          Type mediatorType = null;
          Type viewType = null;
          if (mediated.Any())
          {
            viewType = type;
            mediatorType = ((MediatedBy)mediated.First()).MediatorType;

            if (mediatorType == null)
              throw new MediationException("Cannot implicitly bind view of type: " + type.Name + " due to null MediatorType",
                MediationExceptionType.MEDIATOR_VIEW_STACK_OVERFLOW);
          }
          else if (mediates.Any())
          {
            mediatorType = type;
            viewType = ((Mediates)mediates.First()).ViewType;

            if (viewType == null)
              throw new MediationException("Cannot implicitly bind Mediator of type: " + type.Name + " due to null ViewType",
                MediationExceptionType.MEDIATOR_VIEW_STACK_OVERFLOW);
          }

          if (mediationBinder != null && viewType != null && mediatorType != null) //Bind this mediator!
            mediationBinder.Bind(viewType).To(mediatorType);

          #endregion
        }

        //implementedBy/interfaces first, then implements to give them priority (they will overwrite)
        implementedByBindings.ForEach(Bind);
        //Next implements tags, which have priority over interfaces
        implementsBindings.ForEach(Bind);
      }
      else
      {
        throw new InjectionException("Assembly was not initialized yet for Implicit Bindings!", InjectionExceptionType.UNINITIALIZED_ASSEMBLY);
      }
    }


    [PostConstruct]
    public void PostConstruct()
    {
      assembly = Assembly.GetExecutingAssembly();
    }

    private void Bind(ImplicitBindingVO toBind)
    {
      //We do not check for the existence of a binding. Because implicit bindings are weak bindings, they are overridden automatically by other implicit bindings
      //Therefore, ImplementedBy will be overriden by an Implements to that interface.

      var binding = injectionBinder.Bind(toBind.BindTypes.First());
      binding.Weak(); //SDM2014-0120: added as part of cross-context implicit binding fix (moved from below)

      for (var i = 1; i < toBind.BindTypes.Count; i++)
      {
        var bindType = toBind.BindTypes.ElementAt(i);
        binding.Bind(bindType);
      }

      binding = toBind.ToType != null ? binding.To(toBind.ToType).ToName(toBind.Name).ToSingleton() : binding.ToName(toBind.Name).ToSingleton();

      if (toBind.IsCrossContext) //Bind this to the cross context injector
        binding.CrossContext();

      //binding.Weak();//SDM2014-0120: removed as part of cross-context implicit binding fix (moved up higher)
    }

    private sealed class ImplicitBindingVO
    {
      public readonly List<Type> BindTypes = new();
      public readonly bool IsCrossContext;
      public readonly object Name;
      public readonly Type ToType;

      public ImplicitBindingVO(Type bindType, Type toType, bool isCrossContext, object name)
      {
        BindTypes.Add(bindType);
        ToType = toType;
        IsCrossContext = isCrossContext;
        Name = name;
      }

      public ImplicitBindingVO(List<Type> bindTypes, Type toType, bool isCrossContext, object name)
      {
        BindTypes = bindTypes;
        ToType = toType;
        IsCrossContext = isCrossContext;
        Name = name;
      }
    }
  }
}