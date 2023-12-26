using StrangeIoC.scripts.strange.extensions.context.impl;

namespace Runtime.Contexts.MiniGames.Config
{
  public class MiniGamesRoot : ContextView
  {
    private void Awake()
    {
      //Instantiate the context, passing it this instance.
      context = new MiniGamesContext(this);
    }
  }
}