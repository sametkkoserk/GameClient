using System.Collections.Generic;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Vo;

namespace Runtime.Contexts.MainGame.Model
{
  public interface IMainGameModel
  {
    Dictionary<int, CityVo> cities { get; set; }
    
    GameStateKey gameStateKey { get; set; }
    
    List<PlayerActionKey> playerActionKey { get; set; }
    
    int selectedCityId { get; set; }
    
    Dictionary<PlayerActionKey, PlayerActionPermissionReferenceVo> actionsReferenceList { get; set; }
  }
}