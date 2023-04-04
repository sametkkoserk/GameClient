using Runtime.Modules.Core.Cursor.Enum;
using Runtime.Modules.Core.PromiseTool;

namespace Runtime.Modules.Core.Cursor.Model.CursorModel
{
  public interface ICursorModel
  {
    IPromise Init();
    void OnChangeCursor(CursorKey cursorKey){}
  }
}