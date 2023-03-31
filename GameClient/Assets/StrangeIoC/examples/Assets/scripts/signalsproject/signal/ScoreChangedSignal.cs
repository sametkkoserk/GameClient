/// Signal fired when the score changes
/// 
/// string The new score (already formatted)

using StrangeIoC.scripts.strange.extensions.signal.impl;

namespace StrangeIoC.examples.Assets.scripts.signalsproject.signal
{
  public class ScoreChangedSignal : Signal<string>
  {
  }
}