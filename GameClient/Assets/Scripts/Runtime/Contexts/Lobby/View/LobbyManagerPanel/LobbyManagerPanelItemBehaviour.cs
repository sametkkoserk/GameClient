using Runtime.Contexts.Lobby.Vo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Contexts.Lobby.View.LobbyManagerPanel
{
  public class LobbyManagerPanelItemBehaviour : MonoBehaviour
  {
    public TMP_Text userNameText;

    public Image colorImage;

    public GameObject readyObj;

    [HideInInspector]
    public ushort inLobbyId;

    public void Init(ClientVo clientVo, Color color)
    {
      userNameText.text = clientVo.userName;
      colorImage.color = color;
    }

    public void PlayerReady()
    {
      readyObj.SetActive(true);
    }

    public void PlayerIsOut()
    {
      Destroy(gameObject);
    }
  }
}