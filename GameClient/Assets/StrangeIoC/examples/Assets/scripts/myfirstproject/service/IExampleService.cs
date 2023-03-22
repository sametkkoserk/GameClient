using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;

namespace StrangeIoC.examples.Assets.scripts.myfirstproject.service
{
	public interface IExampleService
	{
		void Request(string url);
		IEventDispatcher dispatcher{get;set;}
	}
}

