using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Main.Vo;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;

namespace Runtime.Contexts.Main.View.RegisterPanel
{
  public enum RegisterPanelEvent
  {
    Register,
  }
  public class RegisterPanelMediator : EventMediator
  {
    [Inject]
    public RegisterPanelView view { get; set; }

    public override void OnRegister()
    {
      view.dispatcher.AddListener(RegisterPanelEvent.Register, OnRegisterToGame);
    }

    private void OnRegisterToGame(IEvent payload)
    {
      string userName = (string)payload.data;

      PlayerRegisterInfoVo playerRegisterInfoVo = new()
      {
        username = userName
      };
      
      dispatcher.Dispatch(MainEvent.RegisterInfoSend, playerRegisterInfoVo);
    }

    public override void OnRemove()
    {
      view.dispatcher.RemoveListener(RegisterPanelEvent.Register, OnRegisterToGame);
    }
  }
}