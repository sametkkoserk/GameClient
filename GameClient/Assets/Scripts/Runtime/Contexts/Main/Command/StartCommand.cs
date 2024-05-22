using Runtime.Contexts.Main.Enum;
using StrangeIoC.scripts.strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Runtime.Contexts.Main.Command
{
  public class StartCommand : EventCommand
  {
    public override void Execute()
    {
      Application.targetFrameRate = 30;
      Addressables.LoadSceneAsync(SceneKeys.NetworkScene, LoadSceneMode.Additive);
    }
  }
}