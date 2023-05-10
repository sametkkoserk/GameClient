using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;

namespace Runtime.Contexts.Main.View.RegisterPanel
{
  public class RegisterPanelView : EventView
  {
    public TMP_InputField usernameInputField;
    
    public void OnRegister()
    {
      // only username for now.
      dispatcher.Dispatch(RegisterPanelEvent.Register, usernameInputField.text);
    }
  }
}