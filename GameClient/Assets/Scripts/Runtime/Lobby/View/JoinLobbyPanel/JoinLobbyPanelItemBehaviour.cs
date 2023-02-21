using Runtime.Lobby.Vo;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Runtime.Lobby.View.JoinLobbyPanel
{
    public class JoinLobbyPanelItemBehaviour : MonoBehaviour
    {
        public TMP_Text lobbyName;
        public Button joinButton;

        public LobbyVo lobbyVo;


        public void Init(LobbyVo vo,UnityAction buttonAction)
        {
            lobbyVo = vo;
            lobbyName.text = vo.lobbyName;
            joinButton.onClick.AddListener(buttonAction); 

        }
        
    }
}
