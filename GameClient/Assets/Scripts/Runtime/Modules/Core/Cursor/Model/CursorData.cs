using System;
using System.Collections.Generic;
using Runtime.Modules.Core.Cursor.Vo;
using UnityEngine;

namespace Runtime.Modules.Core.Cursor.Model
{
  [CreateAssetMenu(menuName = "Tools/DeviceCursor/Create", fileName = "DeviceCursorSettings")]
  [Serializable]
  public class CursorData : ScriptableObject
  {
    public List<CursorVo> list;
  }
}