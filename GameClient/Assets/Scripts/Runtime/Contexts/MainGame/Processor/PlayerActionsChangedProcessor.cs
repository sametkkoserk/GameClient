using System.Collections.Generic;
using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Processor
{
  public class PlayerActionsChangedProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      List<PlayerActionKey> playerActionKey = networkManager.GetData<List<PlayerActionKey>>(vo.message);

      mainGameModel.playerActionKey = playerActionKey;

      dispatcher.Dispatch(MainGameEvent.PlayerActionsChanged);

      string keys = "";
      for (int i = 0; i < playerActionKey.Count; i++)
      {
        if (i == playerActionKey.Count - 1)
          keys += playerActionKey[i];
        else
          keys += playerActionKey[i] + ", ";
      }
      DebugX.Log(DebugKey.MainGame,$"Player Actions Changed: {keys}");
    }
  }
}