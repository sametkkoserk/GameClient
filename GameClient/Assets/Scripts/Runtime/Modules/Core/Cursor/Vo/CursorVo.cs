using System;
using Runtime.Modules.Core.Cursor.Enum;
using UnityEngine;

namespace Runtime.Modules.Core.Cursor.Vo
{
  [Serializable]
  public class CursorVo
  {
    public CursorKey name;

    public Texture2D texture;

    public Vector2 hotPoint;
  }
}