using System.Collections.Generic;
using Runtime.Contexts.MainGame.Vo;

namespace Runtime.Contexts.MainGame.Model
{
    public interface IMainGameModel
    {
      Dictionary<int, CityVo> cities { get; set; }
    }
}