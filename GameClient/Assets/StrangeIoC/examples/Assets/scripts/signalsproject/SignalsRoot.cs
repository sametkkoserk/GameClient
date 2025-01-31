/// If you're new to Strange, start with MyFirstProject.
/// If you're interested in how Signals work, return here once you understand the
/// rest of Strange. This example shows how Signals differ from the default
/// EventDispatcher.

using StrangeIoC.scripts.strange.extensions.context.impl;

namespace StrangeIoC.examples.Assets.scripts.signalsproject
{
  public class SignalsRoot : ContextView
  {
    private void Awake()
    {
      context = new SignalsContext(this);
    }
  }
}