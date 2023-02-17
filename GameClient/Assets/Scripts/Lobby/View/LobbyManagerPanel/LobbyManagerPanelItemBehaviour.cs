using System;
using Lobby.Vo;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Lobby.View.JoinLobbyPanel
{
    public class LobbyManagerPanelItemBehaviour : MonoBehaviour
    {
        public TMP_Text userNameText;
        public Image colorImage;


        public void Init(ClientVo clientVo,Color color)
        {
            userNameText.text = clientVo.id+"  " + clientVo.inLobbyId;
            colorImage.color = color;

        }
        
    }
}
