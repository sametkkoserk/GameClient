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
 * @class strange.extensions.sequencer.impl.SequenceBinding
 * 
 * @deprecated
 */

using System;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.sequencer.api;
using StrangeIoC.scripts.strange.framework.impl;

namespace StrangeIoC.scripts.strange.extensions.sequencer.impl
{
  public class SequenceBinding : CommandBinding, ISequenceBinding
  {
    public SequenceBinding()
    {
    }

    public SequenceBinding(Binder.BindingResolver resolver) : base(resolver)
    {
    }

    public new bool isOneOff { get; set; }

    public new ISequenceBinding Once()
    {
      isOneOff = true;
      return this;
    }

    //Everything below this point is simply facade on Binding to ensure fluent interface
    public new ISequenceBinding Bind<T>()
    {
      return Bind<T>();
    }

    public new ISequenceBinding Bind(object key)
    {
      return Bind(key);
    }

    public new ISequenceBinding To<T>()
    {
      return To(typeof(T));
    }

    public new ISequenceBinding To(object o)
    {
      Type oType = o as Type;
      Type sType = typeof(ISequenceCommand);


      if (sType.IsAssignableFrom(oType) == false)
        throw new SequencerException("Attempt to bind a non SequenceCommand to a Sequence. Perhaps your command needs to extend SequenceCommand or implement ISequenCommand?\n\tType: " + oType,
          SequencerExceptionType.COMMAND_USED_IN_SEQUENCE);

      return base.To(o) as ISequenceBinding;
    }

    public new ISequenceBinding ToName<T>()
    {
      return base.ToName<T>() as ISequenceBinding;
    }

    public new ISequenceBinding ToName(object o)
    {
      return base.ToName(o) as ISequenceBinding;
    }

    public new ISequenceBinding Named<T>()
    {
      return base.Named<T>() as ISequenceBinding;
    }

    public new ISequenceBinding Named(object o)
    {
      return base.Named(o) as ISequenceBinding;
    }
  }
}