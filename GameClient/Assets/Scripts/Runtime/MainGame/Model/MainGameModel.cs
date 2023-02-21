using System.Collections.Generic;
using Runtime.MainGame.Vo;

namespace Runtime.MainGame.Model
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