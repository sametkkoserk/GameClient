using System.Collections.Generic;
using Runtime.MainGame.Vo;

namespace Runtime.MainGame.Model
{
    public interface IMainGameModel
    {
      Dictionary<int, CityVo> cities { get; set; }
    }
}