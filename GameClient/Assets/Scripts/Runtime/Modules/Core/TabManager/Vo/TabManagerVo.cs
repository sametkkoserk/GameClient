using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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