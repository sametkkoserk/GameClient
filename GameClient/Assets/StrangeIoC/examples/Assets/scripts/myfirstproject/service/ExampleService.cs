/// Example Service
/// ======================
/// Nothing to see here. Just your typical place to store some data.

using System.Collections;
using StrangeIoC.examples.Assets.scripts.myfirstproject.controller;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace StrangeIoC.examples.Assets.scripts.myfirstproject.service
{
  public class ExampleService : IExampleService
  {
    private string url;

    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView { get; set; }

    [Inject]
    public IEventDispatcher dispatcher { get; set; }

    public void Request(string url)
    {
      this.url = url;

      //For now, we'll spoof a web service by running a coroutine for 1 second...
      MonoBehaviour root = contextView.GetComponent<MyFirstProjectRoot>();
      root.StartCoroutine(waitASecond());
    }

    private IEnumerator waitASecond()
    {
      yield return new WaitForSeconds(1f);

      //...then pass back some fake data
      dispatcher.Dispatch(ExampleEvent.FULFILL_SERVICE_REQUEST, url);
    }
  }
}