/// An Asynchronous Command
/// ============================
/// This demonstrates how to use a Command to perform an asynchronous action;
/// for example, if you need to call a web service. The two most important lines
/// are the Retain() and Release() calls.

using StrangeIoC.examples.Assets.scripts.myfirstproject.model;
using StrangeIoC.examples.Assets.scripts.myfirstproject.service;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;

namespace StrangeIoC.examples.Assets.scripts.myfirstproject.controller
{
  public class CallWebServiceCommand : EventCommand
  {
    private static int counter;

    public CallWebServiceCommand()
    {
      ++counter; //This counter is here to demonstrate that a new Command is created each time.
    }

    [Inject]
    public IExampleModel model { get; set; }

    [Inject]
    public IExampleService service { get; set; }

    public override void Execute()
    {
      //Retain marks the Command as requiring time to execute.
      //If you call Retain, you MUST have corresponding Release()
      //calls, or you will get memory leaks.
      Retain();

      //Call the service. Listen for a response
      service.dispatcher.AddListener(ExampleEvent.FULFILL_SERVICE_REQUEST, onComplete);
      service.Request("http://www.thirdmotion.com/ ::: " + counter);
    }

    //The payload is in the form of a IEvent
    private void onComplete(IEvent evt)
    {
      //Remember to clean up. Remove the listener.
      service.dispatcher.RemoveListener(ExampleEvent.FULFILL_SERVICE_REQUEST, onComplete);

      model.data = evt.data as string;
      dispatcher.Dispatch(ExampleEvent.SCORE_CHANGE, evt.data);

      //Remember to call release when done.
      Release();
    }
  }
}