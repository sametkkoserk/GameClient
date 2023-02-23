using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using UnityEngine;

namespace Runtime.Contexts.Lobby.Model.LobbyModel
{
    public class LobbyModel : ILobbyModel
    {
        public LobbyVo lobbyVo{ get; set; }
        public ushort inLobbyId{ get; set; }
        public List<Color> colors { get; set; }
        public List<Material> materials { get; set; }

        [PostConstruct]
        public void OnPostConstruct()
        {
            colors = new List<Color>() { Color.black ,Color.blue,Color.green,Color.magenta,Color.red,Color.yellow};
            // Each player will has special material. In the future we can sell materials. In the lobby, player have to choose material.
            // Then, this list will be filled when game start.
            materials = new List<Material>();
        }

        public void OutFromLobby(ushort _inLobbyId)
        {
            if (lobbyVo.clients[_inLobbyId].ready)
            {
                lobbyVo.readyCount -= 1;
            }
            lobbyVo.playerCount -= 1;
            for (ushort i = _inLobbyId; i < lobbyVo.playerCount; i++)
            {
                lobbyVo.clients[i] = lobbyVo.clients[(ushort)(i+1)];
                lobbyVo.clients[i].inLobbyId = i;
            }
            lobbyVo.clients.Remove(lobbyVo.playerCount);

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