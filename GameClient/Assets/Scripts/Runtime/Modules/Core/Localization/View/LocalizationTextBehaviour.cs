using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace Runtime.Modules.Core.Localization.View
{
  public class LocalizationTextBehaviour : MonoBehaviour
  {
    [SerializeField] 
    public LocalizedString localizedString;

    [SerializeField]
    public TextMeshProUGUI label;
    
    private string arg0, arg1, arg2, arg3, arg4;
    
    private void Init()
    {
      localizedString.Arguments = new object[] { arg0, arg1, arg2, arg3, arg4 };

      localizedString.StringChanged += OnUpdateText;
    }

    private void OnUpdateText(string value)
    {
      label.text = value;
    }
    
    public void OnChangeArguments(string value, string[] arguments)
    {
      if (localizedString.Arguments == null)
        Init();
      
      label.text = value;

      for (int i = 0; i < arguments.Length; i++)
      {
        if (i > 4)
          break;
        
        localizedString.Arguments[i] = arguments[i];
      }

      localizedString.RefreshString();
    }
    
    public void OnChangeArguments(string[] arguments)
    {
      if (localizedString.Arguments == null)
        Init();
      
      for (int i = 0; i < arguments.Length; i++)
      {
        if (i > 4)
          break;
        
        localizedString.Arguments[i] = arguments[i];
      }

      localizedString.RefreshString();
    }
  }
}