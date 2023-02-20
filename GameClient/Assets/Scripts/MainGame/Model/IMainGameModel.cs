using System.Collections.Generic;
using MainGame.Vo;

namespace MainGame.Model
{
    public interface IMainGameModel
    {
      Dictionary<int, CityVo> cities { get; set; }
    }
}