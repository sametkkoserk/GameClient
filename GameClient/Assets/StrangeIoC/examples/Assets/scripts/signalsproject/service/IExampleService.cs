using StrangeIoC.examples.Assets.scripts.signalsproject.signal;

namespace StrangeIoC.examples.Assets.scripts.signalsproject.service
{
	public interface IExampleService
	{
		void Request(string url);
		//Instead of an EventDispatcher, we put the actual Signals into the Interface
		FulfillWebServiceRequestSignal fulfillSignal{get;}
	}
}

