using Runtime.MainGame.Enum;
using strange.extensions.command.impl;

namespace Runtime.MainGame.Command
{
  public class CreateMapCommand : EventCommand
  {
    public override void Execute()
    {
      dispatcher.Dispatch(MainGameEvent.CreateMap);
    }
  }
}