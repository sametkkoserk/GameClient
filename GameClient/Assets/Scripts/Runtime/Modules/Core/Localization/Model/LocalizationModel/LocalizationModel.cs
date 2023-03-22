using System.Collections;
using Editor.Tools.DebugX.Runtime;
using Runtime.Modules.Core.Localization.Enum;
using Runtime.Modules.Core.Localization.View;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Runtime.Modules.Core.Localization.Model.LocalizationModel
{
  public class LocalizationModel : ILocalizationModel
  {
    private string languageKey { get; set; }
    
    public string GetLanguageCode()
    {
      languageKey = LocalizationSettings.SelectedLocale.Identifier.Code;
      return LocalizationSettings.SelectedLocale.Identifier.Code;
    }

     public IEnumerator ChangeLanguage(string newLanguageKey)
    {
      short localID = 0;

      switch (newLanguageKey)
      {
        case LanguageKey.en:
          localID = 0;
          DebugX.Log(DebugKey.Localization, "Language changed as an English.");
          break;
        case LanguageKey.tr:
          localID = 1;
          DebugX.Log(DebugKey.Localization, "Language changed as an Turkish.");
          break;
        default:
          DebugX.Log(DebugKey.Localization, "Language could not changed!", LogKey.Error);
          break; 
      }

      yield return LocalizationSettings.InitializationOperation;
      LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localID];
      
      languageKey = LocalizationSettings.SelectedLocale.Identifier.Code;
    }

     private StringTable CheckKeys(TableKey tableKey, string translateKey)
     {
       languageKey = LocalizationSettings.SelectedLocale.Identifier.Code;
       
       if (LocalizationSettings.StringDatabase.GetTable(tableKey.ToString()) == null)
       {
         DebugX.Log(DebugKey.Localization, "Table Key is invalid!", LogKey.Error);
         return null;
       }

       StringTable stringTable = LocalizationSettings.StringDatabase.GetTable(tableKey.ToString());
       
       if (stringTable == null)
       {
         DebugX.Log(DebugKey.Localization, "Table of Language Key is invalid!", LogKey.Error);
         return null;
       }
       
       if (stringTable.GetEntry(translateKey) == null)
       {
         // collection.StringTables[i].Values.ElementAt(j).Key   // If it does not work properly, can be tried in this way.
         DebugX.Log(DebugKey.Localization, "Translate Key is invalid!", LogKey.Error);
         return null;
       }

       return stringTable;
     }
    
    public string GetText(TableKey tableKey, string translateKey)
    {
      StringTable stringTable = CheckKeys(tableKey, translateKey);

      if (stringTable == null)
        return "";

      StringTableEntry entry = stringTable.GetEntry(translateKey);
      return entry.Value;
    }

    public void GetTextArguments(TextMeshProUGUI tmp, TableKey tableKey, string translateKey, params string[] arguments)
    {
      StringTable stringTable = CheckKeys(tableKey, translateKey);

      if (stringTable == null)
        return;

      if (tmp.GetComponent<LocalizationTextBehaviour>() == null)
        tmp.AddComponent<LocalizationTextBehaviour>();
      
      LocalizationTextBehaviour behaviour = tmp.GetComponent<LocalizationTextBehaviour>();
      behaviour.OnChangeArguments(translateKey, tableKey.ToString(),arguments);
    }
  }
}