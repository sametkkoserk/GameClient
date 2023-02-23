using System.Collections.Generic;
using System.Linq;
using Runtime.Modules.Core.ScreenManager.Enum;
using UnityEngine;

namespace Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel
{
  public class ScreenManagerModel : IScreenManagerModel
  {
    public Dictionary<string, int> layerMap { get; set; }

    public Dictionary<string, GameObject> layerContainerDictionary { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      layerMap = new Dictionary<string, int>();
      layerContainerDictionary = new Dictionary<string, GameObject>();
    }

    public void SetSortOrder()
    {
      string[] layers = System.Enum.GetNames(typeof(LayerKey));
      layers = layers.Reverse().ToArray();
      for (int i = 0; i < layers.Length; i++)
      {
        layerMap.Add(layers[i], i * 10);
      }
    }

    public void OpenPanel(LayerKey layerKey, PanelType panelType, PanelMode panelContainerMode)
    {
      
    }
  }
}