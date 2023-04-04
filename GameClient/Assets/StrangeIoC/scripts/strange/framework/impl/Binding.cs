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
 * @class strange.framework.impl.Binding
 * 
 * A binding maintains at least two — and optionally three — SemiBindings:
 * 
 * <ul>
 * <li>key - The Type or value that a client provides in order to unlock a value.</li>
 * <li>value - One or more things tied to and released by the offering of a key</li>
 * <li>name - An optional discriminator, allowing a client to differentiate between multiple keys of the same Type</li>
 * </ul>
 * 
 * <p>Resolver</p>
 * The resolver method (type Binder.BindingResolver) is a callback passed in to resolve
 * instantiation chains.
 *
 * Strange v0.7 adds Pools as an alternative form of SemiBinding. Pools can recycle groups of instances.
 * Binding implements IPool to act as a facade on any Pool SemiBinding.
 * 
 * @see strange.framework.api.IBinding;
 * @see strange.framework.api.IPool;
 * @see strange.framework.impl.Binder;
 */

using System;
using StrangeIoC.scripts.strange.framework.api;

namespace StrangeIoC.scripts.strange.framework.impl
{
  public class Binding : IBinding
  {
    protected ISemiBinding _key;
    protected ISemiBinding _name;
    protected ISemiBinding _value;
    public Binder.BindingResolver resolver;

    public Binding(Binder.BindingResolver resolver)
    {
      this.resolver = resolver;

      _key = new SemiBinding();
      _value = new SemiBinding();
      _name = new SemiBinding();

      keyConstraint = BindingConstraintType.ONE;
      nameConstraint = BindingConstraintType.ONE;
      valueConstraint = BindingConstraintType.MANY;
    }

    public Binding() : this(null)
    {
    }

    #region IBinding implementation

    public object key => _key.value;

    public object value => _value.value;

    public object name => _name.value == null ? BindingConst.NULLOID : _name.value;

    public Enum keyConstraint
    {
      get => _key.constraint;
      set => _key.constraint = value;
    }

    public Enum valueConstraint
    {
      get => _value.constraint;
      set => _value.constraint = value;
    }

    public Enum nameConstraint
    {
      get => _name.constraint;
      set => _name.constraint = value;
    }

    protected bool _isWeak;

    public bool isWeak => _isWeak;

    public virtual IBinding Bind<T>()
    {
      return Bind(typeof(T));
    }

    public virtual IBinding Bind(object o)
    {
      _key.Add(o);
      return this;
    }

    public virtual IBinding To<T>()
    {
      return To(typeof(T));
    }

    public virtual IBinding To(object o)
    {
      _value.Add(o);
      if (resolver != null)
        resolver(this);
      return this;
    }

    public virtual IBinding ToName<T>()
    {
      return ToName(typeof(T));
    }

    public virtual IBinding ToName(object o)
    {
      object toName = o == null ? BindingConst.NULLOID : o;
      _name.Add(toName);
      if (resolver != null)
        resolver(this);
      return this;
    }

    public virtual IBinding Named<T>()
    {
      return Named(typeof(T));
    }

    public virtual IBinding Named(object o)
    {
      return _name.value == o ? this : null;
    }

    public virtual void RemoveKey(object o)
    {
      _key.Remove(o);
    }

    public virtual void RemoveValue(object o)
    {
      _value.Remove(o);
    }

    public virtual void RemoveName(object o)
    {
      _name.Remove(o);
    }

    public virtual IBinding Weak()
    {
      _isWeak = true;
      return this;
    }

    #endregion
  }
}