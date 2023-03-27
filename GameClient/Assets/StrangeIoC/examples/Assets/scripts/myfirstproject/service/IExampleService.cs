using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;

namespace StrangeIoC.examples.Assets.scripts.myfirstproject.service
{
  public interface IExampleService
  {
    IEventDispatcher dispatcher { get; set; }
    void Request(string url);
  }
}