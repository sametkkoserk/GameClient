/// An Asynchronous Command
/// ============================
/// This demonstrates how to use a Command to perform an asynchronous action;
/// for example, if you need to call a web service. The two most important lines
/// are the Retain() and Release() calls.

using StrangeIoC.examples.Assets.scripts.signalsproject.model;
using StrangeIoC.examples.Assets.scripts.signalsproject.service;
using StrangeIoC.examples.Assets.scripts.signalsproject.signal;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace StrangeIoC.examples.Assets.scripts.signalsproject.controller
{
  //Again, we extend Command, not EventCommand
  public class CallWebServiceCommand : Command
  {
    private static int counter;

    public CallWebServiceCommand()
    {
      ++counter;
    }

    [Inject]
    public IExampleModel model { get; set; }

    [Inject]
    public IExampleService service { get; set; }

    //We inject the signal instead of using EventDispatcher
    [Inject]
    public ScoreChangedSignal scoreChangedSignal { get; set; }

    //Injecting this string just to demonstrate that you can do this with Signals
    [Inject]
    public string someValue { get; set; }

    public override void Execute()
    {
      Retain();

      Debug.LogWarning("CallWebServiceCommand is Executing and received a value via Signal: " + someValue);

      //IExampleService defines fulfillSignal as part of its API
      service.fulfillSignal.AddListener(onComplete);
      service.Request("http://www.thirdmotion.com/ ::: " + counter);
    }

    //The payload is now a type-safe string
    private void onComplete(string url)
    {
      service.fulfillSignal.RemoveListener(onComplete);

      model.data = url;

      //Dispatch using a signal
      scoreChangedSignal.Dispatch(url);

      Release();
    }
  }
}