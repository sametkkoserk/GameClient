using System.Collections.Generic;
using System.Linq;
using Editor.Tools.DebugX.Runtime;
using Runtime.Modules.Core.ScreenManager.Enum;
using Runtime.Modules.Core.ScreenManager.Vo;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Modules.Core.ScreenManager.Model.ScreenManagerModel
{
  public class ScreenManagerModel : IScreenManagerModel
  {
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }

    public Dictionary<string, int> layerMap { get; set; }
    public List<string> sceneKeys { get; set; }
    public Dictionary<PanelVo, AsyncOperationHandle<GameObject>> instantiatedPanels { get; set; }

    public void AddLayerContainer(string sceneKey)
    {
      if (sceneKeys.Contains(sceneKey))
        return;

      sceneKeys.Add(sceneKey);

      DebugX.Log(DebugKey.ScreenManager, $"New Scene added to Layer Container: {sceneKey}");
    }

    public void SetSortOrder()
    {
      string[] layers = System.Enum.GetNames(typeof(LayerKey));
      layers = layers.Reverse().ToArray();
      for (int i = 0; i < layers.Length; i++)
      {
        if (layerMap.ContainsKey(layers[i])) continue;
        layerMap.Add(layers[i], i * 10);
      }
    }

    public void OpenPanel(string panelAddressableKey, SceneKey sceneKey, LayerKey layerKey, PanelMode panelMode, PanelType panelType)
    {
      if (!sceneKeys.Contains(sceneKey.ToString()))
      {
        // If panel does not stop in there and nothing happens, there is a sequence problem or scene does not exist in the hierarchy.
        DebugX.Log(DebugKey.ScreenManager, "There is no Scene key like that!", LogKey.Error);
        return;
      }

      if (!layerMap.ContainsKey(layerKey.ToString()))
      {
        DebugX.Log(DebugKey.ScreenManager, "There is no Layer key like that!", LogKey.Error);
        return;
      }

      PanelVo vo = new()
      {
        layerKey = layerKey,
        panelMode = panelMode,
        panelType = panelType,
        sceneKey = sceneKey,
        addressableKey = panelAddressableKey
      };

      dispatcher.Dispatch(PanelEvent.OpenPanel, vo);
    }

    public void CloseAllPanels()
    {
      dispatcher.Dispatch(PanelEvent.CloseAllPanels);
    }

    public void CloseScenePanels(SceneKey sceneKey)
    {
      dispatcher.Dispatch(PanelEvent.CloseScenePanels, sceneKey);
    }

    public void CloseLayerPanels(LayerKey layerKey)
    {
      dispatcher.Dispatch(PanelEvent.CloseLayerPanels, layerKey);
    }

    public void CloseSpecificLayer(SceneKey sceneKey, LayerKey layerKey)
    {
      KeyValuePair<SceneKey, LayerKey> specificLayer = new(sceneKey, layerKey);

      dispatcher.Dispatch(PanelEvent.CloseSpecificLayerPanels, specificLayer);
    }

    public void CloseSpecificPanel(string panelAddressableKey)
    {
      dispatcher.Dispatch(PanelEvent.CloseSpecificPanel, panelAddressableKey);
    }

    [PostConstruct]
    public void OnPostConstruct()
    {
      layerMap = new Dictionary<string, int>();
      sceneKeys = new List<string>();
      instantiatedPanels = new Dictionary<PanelVo, AsyncOperationHandle<GameObject>>();
    }
  }
}