using Runtime.Modules.Core.TestScene.View.TestPanelContainer;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.context.impl;
using UnityEngine;

namespace Runtime.Modules.Core.TestScene.Config
{
  public class TestPanelContext : MVCSContext
  {
    public TestPanelContext(MonoBehaviour view) : base(view)
    {
    }

    public TestPanelContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {
    }

    protected override void mapBindings()
    {
      base.mapBindings();

      // commandBinder.Bind(ContextEvent.START).To<COMMANDNAME>();
      mediationBinder.Bind<TestPanelContainerView>().To<TestPanelContainerMediator>();
    }
  }
}