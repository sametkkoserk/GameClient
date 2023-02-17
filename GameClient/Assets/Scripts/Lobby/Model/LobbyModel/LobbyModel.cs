using System.Collections.Generic;
using Lobby.Vo;
using UnityEngine;

namespace Lobby.Model.LobbyModel
{
    public class LobbyModel : ILobbyModel
    {
        public LobbyVo lobbyVo{ get; set; }
        public List<Color> colors { get; set; }
        public List<Material> materials { get; set; }

        [PostConstruct]
        public void OnPostContruct()
        {
            colors = new List<Color>() { Color.black ,Color.blue,Color.green,Color.magenta,Color.red,Color.yellow};
            // Each player will has special material. In the future we can sell materials. In the lobby, player have to choose material.
            // Then, this list will be filled when game start.
            materials = new List<Material>();
        }
    }
}