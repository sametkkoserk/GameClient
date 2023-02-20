using System.Collections.Generic;
using MainGame.Vo;

namespace MainGame.Model
{
    public class MainGameModel : IMainGameModel
    {
        public Dictionary<int, CityVo> cities { get; set; }
        
        [PostConstruct]
        public void OnPostConstruct()
        {
            cities = new Dictionary<int, CityVo>();
        }
    }
}