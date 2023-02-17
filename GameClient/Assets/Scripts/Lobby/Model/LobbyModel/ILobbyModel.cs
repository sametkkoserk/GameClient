using System.Collections.Generic;
using Lobby.Vo;
using UnityEngine;

namespace Lobby.Model.LobbyModel
{
    public interface ILobbyModel
    {
        LobbyVo lobbyVo{ get; set; }
        
        List<Color> colors { get; set; }
        
    }
}