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
 * @class strange.extensions.context.impl.Context
 * 
 * A Context is the entry point to the binding framework.
 * 
 * Extend this class to create the binding context suitable 
 * for your application.
 * 
 * In a typical Unity3D setup, extend MVCSContext and instantiate 
 * your extension from the ContextView.
 */

using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.framework.impl;

namespace StrangeIoC.scripts.strange.extensions.context.impl
{
  public class Context : Binder, IContext
  {
    /// In a multi-Context app, this represents the first Context to instantiate.
    public static IContext firstContext;

    /// If false, the `Launch()` method won't fire.
    public bool autoStartup;

    public Context()
    {
    }

    public Context(object view, ContextStartupFlags flags)
    {
      //If firstContext was unloaded, the contextView will be null. Assign the new context as firstContext.
      if (firstContext == null || firstContext.GetContextView() == null)
        firstContext = this;
      else
        firstContext.AddContext(this);
      SetContextView(view);
      addCoreComponents();
      autoStartup = (flags & ContextStartupFlags.MANUAL_LAUNCH) != ContextStartupFlags.MANUAL_LAUNCH;
      if ((flags & ContextStartupFlags.MANUAL_MAPPING) != ContextStartupFlags.MANUAL_MAPPING) Start();
    }

    public Context(object view) : this(view, ContextStartupFlags.AUTOMATIC)
    {
    }

    public Context(object view, bool autoMapping) : this(view, autoMapping ? ContextStartupFlags.MANUAL_MAPPING : ContextStartupFlags.MANUAL_LAUNCH | ContextStartupFlags.MANUAL_MAPPING)
    {
    }

    /// The top of the View hierarchy.
    /// In MVCSContext, this is your top-level GameObject
    public object contextView { get; set; }

    public virtual object GetContextView()
    {
      return contextView;
    }

    /// Call this from your Root to set everything in action.
    public virtual IContext Start()
    {
      instantiateCoreComponents();
      mapBindings();
      postBindings();
      if (autoStartup)
        Launch();
      return this;
    }

    /// The final method to fire after mappings.
    /// If autoStartup is false, you need to call this manually.
    public virtual void Launch()
    {
    }

    /// Add another Context to this one.
    public virtual IContext AddContext(IContext context)
    {
      return this;
    }

    /// Remove a context from this one.
    public virtual IContext RemoveContext(IContext context)
    {
      //If we're removing firstContext, set firstContext to null
      if (context == firstContext)
        firstContext = null;
      else
        context.OnRemove();
      return this;
    }

    /// Register a View with this Context
    public virtual void AddView(object view)
    {
      //Override in subclasses
    }

    /// Remove a View from this Context
    public virtual void RemoveView(object view)
    {
      //Override in subclasses
    }

    /// Override to add componentry. Or just extend MVCSContext.
    protected virtual void addCoreComponents()
    {
    }

    /// Override to instantiate componentry. Or just extend MVCSContext.
    protected virtual void instantiateCoreComponents()
    {
    }

    /// Set the object that represents the top of the Context hierarchy.
    /// In MVCSContext, this would be a GameObject.
    public virtual IContext SetContextView(object view)
    {
      contextView = view;
      return this;
    }

    /// Override to map project-specific bindings
    protected virtual void mapBindings()
    {
    }

    /// Override to do things after binding but before app launch
    protected virtual void postBindings()
    {
    }

    /// Retrieve a component from this Context by generic type
    public virtual object GetComponent<T>()
    {
      return null;
    }


    /// Retrieve a component from this Context by generic type and name
    public virtual object GetComponent<T>(object name)
    {
      return null;
    }
  }
}