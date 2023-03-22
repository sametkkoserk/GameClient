using System;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Runtime.Modules.Core.TabManager.Vo
{
  [Serializable]
  public class TabManagerVo
  {
    public Button button;

    public TextMeshProUGUI title;

    public GameObject tab;
  }
}