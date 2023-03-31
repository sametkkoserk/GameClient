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
 * @class strange.extensions.command.impl.EventCommandBinder
 * 
 * A subclass of CommandBinder which relies on an IEventDispatcher as the common system bus.
 */

using StrangeIoC.scripts.strange.extensions.command.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.pool.api;

namespace StrangeIoC.scripts.strange.extensions.command.impl
{
  public class EventCommandBinder : CommandBinder
  {
    /// 
    protected override ICommand createCommand(object cmd, object data)
    {
      injectionBinder.Bind<ICommand>().To(cmd);
      if (data is IEvent) injectionBinder.Bind<IEvent>().ToValue(data).ToInject(false);

      var command = injectionBinder.GetInstance<ICommand>();
      if (command == null)
      {
        var msg = "A Command ";
        if (data is IEvent)
        {
          var evt = (IEvent)data;
          msg += "tied to event " + evt.type;
        }

        msg += " could not be instantiated.\nThis might be caused by a null pointer during instantiation or failing to override Execute (generally you shouldn't have constructor code in Commands).";
        throw new CommandException(msg, CommandExceptionType.BAD_CONSTRUCTOR);
      }

      command.data = data;
      if (data is IEvent) injectionBinder.Unbind<IEvent>();
      injectionBinder.Unbind<ICommand>();
      return command;
    }

    protected override void disposeOfSequencedData(object data)
    {
      if (data is IPoolable) (data as IPoolable).Release();
    }
  }
}