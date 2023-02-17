using System.Collections.Generic;
using Lobby.Vo;
using Unity.VisualScripting;
using UnityEngine;

namespace Lobby.Model.LobbyModel
{
    public class LobbyModel : ILobbyModel
    {
        public LobbyVo lobbyVo{ get; set; }
        public ushort inLobbyId{ get; set; }
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

        public void OutFromLobby(ushort _inLobbyId)
        {
            lobbyVo.clients.RemoveAt(_inLobbyId);
            lobbyVo.playerCount -= 1;
            for (ushort i = _inLobbyId; i < lobbyVo.playerCount; i++)
            {
                lobbyVo.clients[i].inLobbyId = i;
            }

            if (_inLobbyId<inLobbyId)
            {
                inLobbyId -= 1;
            }

            
        }

        public void LobbyReset()
        {
            lobbyVo = new LobbyVo();
            inLobbyId = 0;
        }
    }
}