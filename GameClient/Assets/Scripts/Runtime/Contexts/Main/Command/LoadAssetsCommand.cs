using System;
using System.Collections.Generic;
using Runtime.Contexts.Main.Enum;
using Runtime.Modules.Core.ColorPalette.Model.ColorPaletteModel;
using Runtime.Modules.Core.Cursor.Model.CursorModel;
using Runtime.Modules.Core.PromiseTool;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.Main.Command
{
  public class LoadAssetsCommand : EventCommand
  {
    [Inject]
    public ICursorModel cursorModel { get; set; }
    
    [Inject]
    public IColorPaletteModel colorPaletteModel { get; set; }

    public override void Execute()
    {
      List<Func<IPromise>> list = new()
      {
        colorPaletteModel.Init,
        cursorModel.Init,
      };

      IPromise operation = list[0]();
      for (int i = 1; i < list.Count; i++)
      {
        operation = operation.Then(list[i]);
      }
      
      operation.Then(AssetsLoaded);
    }

    private void AssetsLoaded()
    {
      dispatcher.Dispatch(MainEvent.StarterSettings);
    }
  }
}