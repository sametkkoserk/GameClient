using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Vo;
using StrangeIoC.scripts.strange.extensions.injector;

namespace Runtime.Contexts.MainGame.Model
{
  public class MainGameModel : IMainGameModel
  {
    public Dictionary<int, CityVo> cities { get; set; }
    
    public GameStateKey gameStateKey { get; set; }
    
    public List<PlayerActionKey> playerActionKey { get; set; }
    
    public PlayerFeaturesVo playerFeaturesVo { get; set; }
    
    public int selectedCityId { get; set; }
    
    public Dictionary<PlayerActionKey, PlayerActionPermissionReferenceVo> actionsReferenceList { get; set; }

    public List<MiniGameResultVo> miniGameResultVos { get; set; }
    
    public ClientVo clientVo { get; set; }
    
    public int queueID { get; set; }

    [PostConstruct]
    public void OnPostConstruct()
    {
      cities = new Dictionary<int, CityVo>();
      actionsReferenceList = new Dictionary<PlayerActionKey, PlayerActionPermissionReferenceVo>();
      playerActionKey = new List<PlayerActionKey>();
      miniGameResultVos = new List<MiniGameResultVo>();
      // Each player will has a special material. In the future we can sell materials. In the lobby, player have to choose material.
      // Then, this list will be filled when game start.
    }
  }
}