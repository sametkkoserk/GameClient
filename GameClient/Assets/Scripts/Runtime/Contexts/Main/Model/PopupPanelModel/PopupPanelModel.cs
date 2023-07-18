using Runtime.Contexts.Main.Vo;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Main.Model.PopupPanelModel
{
  public class PopupPanelModel : IPopupPanelModel
  {
    public PopupInfoVo popupInfoVo { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      popupInfoVo = new PopupInfoVo();
    }

    public void OnConfirmButtonClick()
    {
      popupInfoVo.onConfirmButton?.Invoke();
    }
    
    public void OnDeclineButtonClick()
    {
      popupInfoVo.onDeclineButton?.Invoke();
    }
  }
}