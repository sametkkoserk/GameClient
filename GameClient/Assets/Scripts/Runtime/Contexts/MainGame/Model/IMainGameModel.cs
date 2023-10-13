using System.Collections.Generic;
using Runtime.Contexts.Lobby.Vo;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Vo;

namespace Runtime.Contexts.MainGame.Model
{
  public interface IMainGameModel
  {
    Dictionary<int, CityVo> cities { get; set; }
    
    GameStateKey gameStateKey { get; set; }
    
    List<PlayerActionKey> playerActionKey { get; set; }
    
    PlayerFeaturesVo playerFeaturesVo { get; set; }
    
    int selectedCityId { get; set; }
    
    Dictionary<PlayerActionKey, PlayerActionPermissionReferenceVo> actionsReferenceList { get; set; }
    
    List<MiniGameResultVo> miniGameResultVos { get; set; }
    
    ClientVo clientVo { get; set; }
  }
}