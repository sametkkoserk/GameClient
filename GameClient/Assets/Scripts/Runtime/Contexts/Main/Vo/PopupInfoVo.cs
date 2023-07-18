using System;

namespace Runtime.Contexts.Main.Vo
{
  public class PopupInfoVo
  {
    public string titleText;

    public string contentText;

    public Action onConfirmButton;

    public Action onDeclineButton;
  }
}