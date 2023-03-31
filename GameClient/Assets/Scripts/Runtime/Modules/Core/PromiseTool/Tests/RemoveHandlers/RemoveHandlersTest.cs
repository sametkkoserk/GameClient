#if UNITY_EDITOR

using UnityEngine;

namespace Runtime.Modules.Core.PromiseTool.Tests.RemoveHandlers
{
  public class RemoveHandlersTest : MonoBehaviour
  {
    public RemoveHandlersServerTest dummyServer;

    private void Awake()
    {
      Promise.EnablePromiseTracking = true;
    }

    public void Start()
    {
      var request = dummyServer.Request();
      Debug.Log("request: " + request.Id);
      var promise = request.Then(OnResponse);
      Debug.Log("promise.RemoveHandlers()");
      Debug.Log("promise.GetHashCode() 2 " + promise.Id);

      Debug.Log(Promise.PendingPromises);
      request.Cancel();
      DestroyImmediate(gameObject);
    }

    private void OnResponse()
    {
      Debug.Log("OnResponse: " + gameObject.name);
    }
  }
}
#endif