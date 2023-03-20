using System.Collections;
using Editor.Tools.DebugX.Runtime;
using Runtime.Modules.Core.Localization.Enum;
using Runtime.Modules.Core.Localization.View;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Runtime.Modules.Core.Localization.Model.LocalizationModel
{
  public class LocalizationModel : ILocalizationModel
  {
    public string languageKey { get; set; }

     public IEnumerator ChangeLanguage(string newLanguageKey)
    {
      int localID = 0;
      languageKey = newLanguageKey;

      localID = newLanguageKey switch
      {
        LanguageKey.en => 0,
        LanguageKey.tr => 1,
        _ => localID
      };

      yield return LocalizationSettings.InitializationOperation;
      LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localID];
    }

     private StringTable CheckKeys(TableKey tableKey, string translateKey)
     {
       languageKey ??= LanguageKey.en; // If null
      
       if (LocalizationEditorSettings.GetStringTableCollection(tableKey.ToString()) == null)
       {
         DebugX.Log(DebugKey.Localization, "Table Key is invalid!", LogKey.Error);
         return null;
       }
      
       StringTableCollection collection = LocalizationEditorSettings.GetStringTableCollection(tableKey.ToString());

       if (collection.GetTable(languageKey) == null)
       {
         DebugX.Log(DebugKey.Localization, "Language Key is invalid!", LogKey.Error);
         return null;
       }
      
       StringTable stringTable = collection.GetTable(languageKey) as StringTable;

       if (stringTable == null)
       {
         DebugX.Log(DebugKey.Localization, "Table of Language Key is invalid!", LogKey.Error);
         return null;
       }

       if (stringTable.GetEntry(translateKey) == null)
       {
         // collection.StringTables[i].Values.ElementAt(j).Key
         // If it does not work properly, can be tried in this way.
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
      behaviour.OnChangeArguments(translateKey, arguments);
    }
  }
}