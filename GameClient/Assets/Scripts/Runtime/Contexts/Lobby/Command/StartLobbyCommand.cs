using Runtime.Contexts.Lobby.Enum;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Command
{
  public class StartLobbyCommand : EventCommand
  {
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void Execute()
    {
      screenManagerModel.OpenPanel(SceneKey.Lobby, LayerKey.FirstLayer, PanelMode.Destroy, PanelType.FullScreenPanel, LobbyKey.LobbyPanel);
    }
  }
}