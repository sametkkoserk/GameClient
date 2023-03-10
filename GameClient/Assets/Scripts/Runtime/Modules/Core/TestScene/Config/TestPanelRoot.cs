using strange.extensions.context.impl;

namespace Runtime.Modules.Core.TestScene.Config
{
  public class TestPanelRoot : ContextView
  {
    private void Awake()
    {
      context = new TestPanelContext(this);
    }
  }
}