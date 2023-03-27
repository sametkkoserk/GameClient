using System.Collections.Generic;
using Runtime.Modules.Core.TabManager.Vo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Modules.Core.TabManager.View
{
  public class TabManagerBehaviour : MonoBehaviour
  {
    [SerializeField]
    private List<TabManagerVo> tabManagerVoList;

    public void OnEnable()
    {
      for (var i = 0; i < tabManagerVoList.Count; i++)
      {
        var count = i;
        tabManagerVoList[count].button.onClick.AddListener(delegate { OnClick(tabManagerVoList[count].button); });

        if (i == 0)
        {
          tabManagerVoList[i].tab.SetActive(true);
          tabManagerVoList[i].title.fontStyle = FontStyles.Bold;
        }
        else
        {
          tabManagerVoList[i].tab.SetActive(false);
          tabManagerVoList[i].title.fontStyle = FontStyles.Normal;
        }
      }
    }

    public void OnClick(Button button)
    {
      for (var i = 0; i < tabManagerVoList.Count; i++)
        if (tabManagerVoList[i].button == button)
        {
          tabManagerVoList[i].tab.SetActive(true);
          tabManagerVoList[i].title.fontStyle = FontStyles.Bold;
        }
        else
        {
          tabManagerVoList[i].tab.SetActive(false);
          tabManagerVoList[i].title.fontStyle = FontStyles.Normal;
        }
    }
  }
}