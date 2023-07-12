using Runtime.Contexts.Main.Enum;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Main.Command
{
  public class OpenRegisterPanelCommand : EventCommand
  {
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void Execute()
    {
      screenManagerModel.OpenPanel(MainPanelKey.RegisterPanel, SceneKey.Main, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
      screenManagerModel.OpenPanel(MainPanelKey.TooltipPanel, SceneKey.Main, LayerKey.TooltipLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
    }
  }
}