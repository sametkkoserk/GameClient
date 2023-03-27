using StrangeIoC.scripts.strange.extensions.context.impl;

namespace Runtime.Contexts.Main.Config
{
  public class MainRoot : ContextView
  {
    private void Awake()
    {
      //Instantiate the context, passing it this instance.
      context = new MainContext(this);
    }
  }
}