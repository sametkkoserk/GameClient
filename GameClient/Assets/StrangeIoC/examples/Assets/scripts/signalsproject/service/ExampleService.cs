/// The service, now with signals

using System.Collections;
using StrangeIoC.examples.Assets.scripts.signalsproject.signal;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace StrangeIoC.examples.Assets.scripts.signalsproject.service
{
  public class ExampleService : IExampleService
  {
    private string url;

    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView { get; set; }

    //The interface demands this signal
    [Inject]
    public FulfillWebServiceRequestSignal fulfillSignal { get; set; }

    public void Request(string url)
    {
      this.url = url;

      MonoBehaviour root = contextView.GetComponent<SignalsRoot>();
      root.StartCoroutine(waitASecond());
    }

    private IEnumerator waitASecond()
    {
      yield return new WaitForSeconds(1f);

      //Pass back some fake data via a Signal
      fulfillSignal.Dispatch(url);
    }
  }
}