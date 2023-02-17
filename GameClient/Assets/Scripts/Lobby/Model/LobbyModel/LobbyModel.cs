using System.Collections.Generic;
using UnityEngine;

namespace Lobby.Model.LobbyModel
{
    public class LobbyModel : ILobbyModel
    {
        public ushort lobbyId { get; set; }
        public string lobbyName { get; set; }
        public bool isPrivate { get; set; }
        public ushort leaderId { get; set; }

        public List<Material> materials { get; set; }

        [PostConstruct]
        public void OnPostConstruct()
        {
            // Each player will has special material. In the future we can sell materials. In the lobby, player have to choose material.
            // Then, this list will be filled when game start.
            materials = new List<Material>();
        }
    }
}