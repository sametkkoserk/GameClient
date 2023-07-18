using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Main.Model.PopupPanelModel;
using Runtime.Contexts.Main.Vo;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Main.Command
{
  public class OpenPopupPanelCommand : EventCommand
  {
    [Inject]
    public IPopupPanelModel popupPanelModel { get; set; }
    
    [Inject]
    public IScreenManagerModel screenManagerModel { get; set; }

    public override void Execute()
    {
      PopupInfoVo vo = (PopupInfoVo)evt.data;

      popupPanelModel.popupInfoVo.titleText = vo.titleText;
      popupPanelModel.popupInfoVo.contentText = vo.contentText;

      popupPanelModel.popupInfoVo.onConfirmButton = vo.onConfirmButton;
      popupPanelModel.popupInfoVo.onDeclineButton = vo.onDeclineButton;
      
      screenManagerModel.OpenPanel(MainPanelKey.PopupPanel, SceneKey.Main, LayerKey.PopupLayer, PanelMode.Destroy, PanelType.PopupPanel);
    }
  }
}