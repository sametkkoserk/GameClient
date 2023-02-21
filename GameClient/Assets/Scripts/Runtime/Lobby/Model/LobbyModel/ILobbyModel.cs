using System.Collections.Generic;
using Runtime.Lobby.Vo;
using UnityEngine;

namespace Runtime.Lobby.Model.LobbyModel
{
    public interface ILobbyModel
    {
        LobbyVo lobbyVo{ get; set; }
        ushort inLobbyId{ get; set; }
        
        List<Color> colors { get; set; }
        
        List<Material> materials { get; set; }
        void OutFromLobby(ushort _inLobbyId);
        void LobbyReset();
    }
}