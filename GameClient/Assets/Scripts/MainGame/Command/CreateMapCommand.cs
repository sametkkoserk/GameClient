using MainGame.Enum;
using strange.extensions.command.impl;

namespace MainGame.Command
{
  public class CreateMapCommand : EventCommand
  {
    public override void Execute()
    {
      dispatcher.Dispatch(MainGameEvent.CreateMap);
    }
  }
}