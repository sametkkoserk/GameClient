/*
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

/**
 * @class strange.extensions.reflector.impl.ReflectionBinder
 * 
 * Uses System.Reflection to create `ReflectedClass` instances.
 * 
 * Reflection is a slow process. This binder isolates the calls to System.Reflector 
 * and caches the result, meaning that Reflection is performed only once per class.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.reflector.api;
using StrangeIoC.scripts.strange.framework.api;
using Binder = StrangeIoC.scripts.strange.framework.impl.Binder;

namespace StrangeIoC.scripts.strange.extensions.reflector.impl
{
  public class ReflectionBinder : Binder, IReflectionBinder
  {
    public IReflectedClass Get<T>()
    {
      return Get(typeof(T));
    }

    public IReflectedClass Get(Type type)
    {
      var binding = GetBinding(type);
      IReflectedClass retv;
      if (binding == null)
      {
        binding = GetRawBinding();
        IReflectedClass reflected = new ReflectedClass();
        mapPreferredConstructor(reflected, binding, type);
        mapPostConstructors(reflected, binding, type);
        mapSetters(reflected, binding, type);
        binding.Bind(type).To(reflected);
        retv = binding.value as IReflectedClass;
        retv.PreGenerated = false;
      }
      else
      {
        retv = binding.value as IReflectedClass;
        retv.PreGenerated = true;
      }

      return retv;
    }

    public override IBinding GetRawBinding()
    {
      var binding = base.GetRawBinding();
      binding.valueConstraint = BindingConstraintType.ONE;
      return binding;
    }

    private void mapPreferredConstructor(IReflectedClass reflected, IBinding binding, Type type)
    {
      var constructor = findPreferredConstructor(type);
      if (constructor == null)
        throw new ReflectionException("The reflector requires concrete classes.\nType " + type + " has no constructor. Is it an interface?", ReflectionExceptionType.CANNOT_REFLECT_INTERFACE);
      var parameters = constructor.GetParameters();


      var paramList = new Type[parameters.Length];
      var i = 0;
      foreach (var param in parameters)
      {
        var paramType = param.ParameterType;
        paramList[i] = paramType;
        i++;
      }

      reflected.Constructor = constructor;
      reflected.ConstructorParameters = paramList;
    }

    //Look for a constructor in the order:
    //1. Only one (just return it, since it's our only option)
    //2. Tagged with [Construct] tag
    //3. The constructor with the fewest parameters
    private ConstructorInfo findPreferredConstructor(Type type)
    {
      var constructors = type.GetConstructors(BindingFlags.FlattenHierarchy |
                                              BindingFlags.Public |
                                              BindingFlags.Instance |
                                              BindingFlags.InvokeMethod);
      if (constructors.Length == 1) return constructors[0];
      int len;
      var shortestLen = int.MaxValue;
      ConstructorInfo shortestConstructor = null;
      foreach (var constructor in constructors)
      {
        var taggedConstructors = constructor.GetCustomAttributes(typeof(Construct), true);
        if (taggedConstructors.Length > 0) return constructor;
        len = constructor.GetParameters().Length;
        if (len < shortestLen)
        {
          shortestLen = len;
          shortestConstructor = constructor;
        }
      }

      return shortestConstructor;
    }

    private void mapPostConstructors(IReflectedClass reflected, IBinding binding, Type type)
    {
      var methods = type.GetMethods(BindingFlags.FlattenHierarchy |
                                    BindingFlags.Public |
                                    BindingFlags.Instance |
                                    BindingFlags.InvokeMethod);
      var methodList = new ArrayList();
      foreach (var method in methods)
      {
        var tagged = method.GetCustomAttributes(typeof(PostConstruct), true);
        if (tagged.Length > 0) methodList.Add(method);
      }

      methodList.Sort(new PriorityComparer());
      var postConstructors = (MethodInfo[])methodList.ToArray(typeof(MethodInfo));
      reflected.postConstructors = postConstructors;
    }

    private void mapSetters(IReflectedClass reflected, IBinding binding, Type type)
    {
      var pairs = new KeyValuePair<Type, PropertyInfo>[0];
      var names = new object[0];

      var privateMembers = type.FindMembers(MemberTypes.Property,
        BindingFlags.FlattenHierarchy |
        BindingFlags.SetProperty |
        BindingFlags.NonPublic |
        BindingFlags.Instance,
        null, null);
      foreach (var member in privateMembers)
      {
        var injections = member.GetCustomAttributes(typeof(Inject), true);
        if (injections.Length > 0)
          throw new ReflectionException("The class " + type.Name + " has a non-public Injection setter " + member.Name + ". Make the setter public to allow injection.",
            ReflectionExceptionType.CANNOT_INJECT_INTO_NONPUBLIC_SETTER);
      }

      var members = type.FindMembers(MemberTypes.Property,
        BindingFlags.FlattenHierarchy |
        BindingFlags.SetProperty |
        BindingFlags.Public |
        BindingFlags.Instance,
        null, null);

      foreach (var member in members)
      {
        var injections = member.GetCustomAttributes(typeof(Inject), true);
        if (injections.Length > 0)
        {
          var attr = injections[0] as Inject;
          var point = member as PropertyInfo;
          var pointType = point.PropertyType;
          var pair = new KeyValuePair<Type, PropertyInfo>(pointType, point);
          pairs = AddKV(pair, pairs);

          var bindingName = attr.name;
          names = Add(bindingName, names);
        }
      }

      reflected.Setters = pairs;
      reflected.SetterNames = names;
    }

    /**
		 * Add an item to a list
		 */
    private object[] Add(object value, object[] list)
    {
      var tempList = list;
      var len = tempList.Length;
      list = new object[len + 1];
      tempList.CopyTo(list, 0);
      list[len] = value;
      return list;
    }

    /**
		 * Add an item to a list
		 */
    private KeyValuePair<Type, PropertyInfo>[] AddKV(KeyValuePair<Type, PropertyInfo> value, KeyValuePair<Type, PropertyInfo>[] list)
    {
      var tempList = list;
      var len = tempList.Length;
      list = new KeyValuePair<Type, PropertyInfo>[len + 1];
      tempList.CopyTo(list, 0);
      list[len] = value;
      return list;
    }
  }

  internal class PriorityComparer : IComparer
  {
    int IComparer.Compare(object x, object y)
    {
      var pX = getPriority(x as MethodInfo);
      var pY = getPriority(y as MethodInfo);

      return pX < pY ? -1 : 1;
    }

    private int getPriority(MethodInfo methodInfo)
    {
      var attr = methodInfo.GetCustomAttributes(true)[0] as PostConstruct;
      var priority = attr.priority;
      return priority;
    }
  }
}