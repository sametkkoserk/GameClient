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

    public void Init(ClientVo clientVo, Color color, bool isMe)
    {
      userNameText.text = clientVo.userName;
      colorImage.color = color;

      if (!isMe) return;
      userNameText.color = Color.green;
      userNameText.fontStyle = FontStyles.Bold;
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