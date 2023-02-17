using System.Collections.Generic;
using Lobby.Vo;
using UnityEngine;

namespace Lobby.Model.LobbyModel
{
    public class LobbyModel : ILobbyModel
    {
        public LobbyVo lobbyVo{ get; set; }

        public List<Color> colors { get; set; }
        
        [PostConstruct]
        public void OnPostContruct()
        {
            colors = new List<Color>() { Color.black ,Color.blue,Color.green,Color.magenta,Color.red,Color.yellow};
            
        }



    }
}