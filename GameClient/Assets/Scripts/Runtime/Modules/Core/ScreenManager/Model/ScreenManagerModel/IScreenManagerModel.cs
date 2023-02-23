using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel
{
    public interface IScreenManagerModel
    {
      Dictionary<string, int> layerMap { get; set; }
      
      Dictionary<string, GameObject> layerContainerDictionary { get; set; }

      void SetSortOrder();
    }
}