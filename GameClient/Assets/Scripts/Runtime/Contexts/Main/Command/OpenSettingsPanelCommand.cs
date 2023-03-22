using Runtime.Contexts.Main.Enum;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Main.Command
{
  public class OpenSettingsPanelCommand : EventCommand
  {
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void Execute()
    {
      screenManagerModel.OpenPanel(MainPanelKey.SettingsPanel, SceneKey.Main, LayerKey.SettingsLayer, PanelMode.Destroy, PanelType.FullScreenPanel);
    }
  }
}