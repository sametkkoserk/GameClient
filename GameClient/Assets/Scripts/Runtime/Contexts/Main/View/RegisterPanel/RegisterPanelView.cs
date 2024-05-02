using System.Text.RegularExpressions;
using Runtime.Contexts.Main.Vo;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using TMPro;

namespace Runtime.Contexts.Main.View.RegisterPanel
{
  public class RegisterPanelView : EventView
  {
    public TMP_InputField usernameInputField;
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TextMeshProUGUI errorText;

    public const string MatchEmailPattern =
      @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
      + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
      + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
      + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    public void OnRegister()
    {
      // if (!RegisterControl())return;

      PlayerRegisterInfoVo registerVo = new PlayerRegisterInfoVo()
      {
        username = usernameInputField.text,
        email = emailInputField.text,
        password = passwordInputField.text
      };
      // only username for now.
      dispatcher.Dispatch(RegisterPanelEvent.Register, registerVo);
    }




    public bool RegisterControl()
    {
      if (!IsEmail(emailInputField.text))
      {
        errorText.text = "Email yanlış girilmiştir!";
        return false;
      }

      if (passwordInputField.text.Length<6)
      {
        errorText.text = "şifre 6 haneden kısa olamaz";
        return false;
      }

      return true;
    }
    public bool IsEmail(string email)
    {
      if (email != null) return Regex.IsMatch(email, MatchEmailPattern);
      else return false;
    }
  }
}