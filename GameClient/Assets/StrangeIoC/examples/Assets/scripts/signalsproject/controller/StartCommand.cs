/// The only change in StartCommand is that we extend Command, not EventCommand

using StrangeIoC.examples.Assets.scripts.signalsproject.view;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace StrangeIoC.examples.Assets.scripts.signalsproject.controller
{
  public class StartCommand : Command
  {
    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView { get; set; }

    public override void Execute()
    {
      GameObject go = new GameObject();
      go.name = "ExampleView";
      go.AddComponent<ExampleView>();
      go.transform.parent = contextView.transform;
    }
  }
}