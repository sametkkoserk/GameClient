using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Contexts.MainGame.Vo
{
  public class CityVo
  {
    public int ID;

    public bool isPlayable;

    public List<ushort> neighbors;

    public int ownerID;

    public Vector3 position;

    public int soldierCount;
  }
}