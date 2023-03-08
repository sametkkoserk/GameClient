using System.Collections.Generic;
using Runtime.Contexts.MainGame.Vo;
using UnityEngine;

namespace Runtime.Contexts.MainGame.Model
{
    public class MainGameModel : IMainGameModel
    {
        public Dictionary<int, CityVo> cities { get; set; }
        public List<Material> materials { get; set; }
        
        public ushort lobbyID { get; set; }
        
        public ushort queue { get; set; }
        
        [PostConstruct]
        public void OnPostConstruct()
        {
            cities = new Dictionary<int, CityVo>();
            // Each player will has special material. In the future we can sell materials. In the lobby, player have to choose material.
            // Then, this list will be filled when game start.
            materials = new List<Material>();
        }
    }
}