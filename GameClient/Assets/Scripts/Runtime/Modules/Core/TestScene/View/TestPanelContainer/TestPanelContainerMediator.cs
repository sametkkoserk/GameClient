using System;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Modules.Core.TestScene.View.TestPanelContainer
{
  public class TestPanelContainerMediator : EventMediator
  {
    [Inject]
    public TestPanelContainerView view { get; set; }

    public override void OnRegister()
    {
    }

    private void Update()
    {
      if (!Input.GetKeyDown(KeyCode.F5)) return;
      
      for (int i = 0; i < transform.childCount; i++)
        DestroyImmediate(transform.GetChild(i).gameObject);
      
      Addressables.InstantiateAsync(view.testPanelKeys.ToString(), transform);
    }

    public override void OnRemove()
    {
    }
  }
}