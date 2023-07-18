using Runtime.Contexts.Main.Vo;

namespace Runtime.Contexts.Main.Model.PopupPanelModel
{
  public interface IPopupPanelModel
  {
    public PopupInfoVo popupInfoVo { get; set; }
    
    void OnConfirmButtonClick();
    
    void OnDeclineButtonClick();
  }
}