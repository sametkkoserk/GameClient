using System.Collections.Generic;
using Runtime.Modules.Core.Bundle.Model.BundleModel;
using Runtime.Modules.Core.Cursor.Enum;
using Runtime.Modules.Core.Cursor.Vo;
using Runtime.Modules.Core.PromiseTool;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace Runtime.Modules.Core.Cursor.Model.CursorModel
{
  public class CursorModel : ICursorModel
  {
    public static ICursorModel instance;
    
    [Inject]
    public IBundleModel bundleModel { get; set; }

    private Dictionary<CursorKey, CursorVo> cursorsData { get; set; }

    [PostConstruct]
    private void OnPostConstruct()
    {
      cursorsData = new Dictionary<CursorKey, CursorVo>();
    }

    public IPromise Init()
    {
      Promise promise = new();
      Debug.Log("init1");

      bundleModel.LoadAssetAsync<CursorData>("DeviceCursorSettings").Then(data =>
      {
        cursorsData = new Dictionary<CursorKey, CursorVo>();

        foreach (CursorVo deviceCursorVo in data.list)
          cursorsData.Add(deviceCursorVo.name, deviceCursorVo);

        instance = this;

        promise.Resolve();
      }).Catch(promise.Reject);

      return promise;
    }
    
    public void OnLoadCursors()
    {
      // string folderPath = "Assets/Scripts/Modules/Runtime/Modules/Cursor/2D Textures";
      // string[] filePaths = Directory.GetFiles(folderPath);
      //
      // for (int i = 0; i < filePaths.Length; i++)
      // {
      //   Texture2D cursorTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(filePaths[i]);
      //
      //   cursors.Add(cursorTexture.name, cursorTexture);
      // }
    }

    public void OnChangeCursor(CursorKey cursorKey)
    {
      if (!cursorsData.ContainsKey(cursorKey)) return;
      CursorVo CursorVo = cursorsData[cursorKey];
      UnityEngine.Cursor.SetCursor(CursorVo.texture, CursorVo.hotPoint, CursorMode.Auto);
    }
  }
}