/// Just a simple MonoBehaviour Click Detector

using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace StrangeIoC.examples.Assets.scripts.myfirstproject.view
{
  public class ClickDetector : EventView
  {
    public const string CLICK = "CLICK";

    private void OnMouseDown()
    {
      dispatcher.Dispatch(CLICK);
    }
  }
}