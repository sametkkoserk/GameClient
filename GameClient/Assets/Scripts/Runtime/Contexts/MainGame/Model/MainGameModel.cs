using System.Collections.Generic;
using Runtime.Contexts.MainGame.Vo;

namespace Runtime.Contexts.MainGame.Model
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