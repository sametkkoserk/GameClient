using Runtime.Contexts.MainGame.Enum;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MainGame.Command
{
  public class CreateMapCommand : EventCommand
  {
    public override void Execute()
    {
      dispatcher.Dispatch(MainGameEvent.CreateMap);
    }
  }
}