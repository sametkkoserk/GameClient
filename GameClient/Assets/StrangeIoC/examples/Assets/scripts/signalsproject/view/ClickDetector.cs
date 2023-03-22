/// Uses a signal instead of an EventDispatcher

using StrangeIoC.scripts.strange.extensions.mediation.impl;
using StrangeIoC.scripts.strange.extensions.signal.impl;

namespace StrangeIoC.examples.Assets.scripts.signalsproject.view
{
	public class ClickDetector : View
	{
		// Note how we're using a signal now
		public Signal clickSignal = new Signal();
		
		void OnMouseDown()
		{
			clickSignal.Dispatch();
		}
	}
}

