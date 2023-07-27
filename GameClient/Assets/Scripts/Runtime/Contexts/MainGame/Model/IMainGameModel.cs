using System.Collections.Generic;
using Runtime.Contexts.MainGame.Vo;
using UnityEngine;

namespace Runtime.Contexts.MainGame.Model
{
  public interface IMainGameModel
  {
    Dictionary<int, CityVo> cities { get; set; }

    List<Material> materials { get; set; }

    ushort lobbyId { get; set; }

    ushort queue { get; set; }
  }
}