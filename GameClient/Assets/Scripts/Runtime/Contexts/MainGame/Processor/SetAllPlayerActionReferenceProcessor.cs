using System.Collections.Generic;
using Editor.Tools.DebugX.Runtime;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Services.NetworkManager;
using Runtime.Contexts.Network.Vo;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Processor
{
  public class SetAllPlayerActionReferenceProcessor : EventCommand
  {
    [Inject]
    public INetworkManagerService networkManager { get; set; }
    
    [Inject]
    public IMainGameModel mainGameModel { get; set; }

    public override void Execute()
    {
      MessageReceivedVo vo = (MessageReceivedVo)evt.data;
      Dictionary<PlayerActionKey, PlayerActionPermissionReferenceVo>  playerActionPermissionReferenceVos =
        networkManager.GetData<Dictionary<PlayerActionKey, PlayerActionPermissionReferenceVo>>(vo.message);

      mainGameModel.actionsReferenceList = playerActionPermissionReferenceVos;
      
      dispatcher.Dispatch(MainGameEvent.PlayerActionsReferenceListExecuted);
      
      DebugX.Log(DebugKey.MainGame,$"{playerActionPermissionReferenceVos.Count} Player Action added.");
    }
  }
}