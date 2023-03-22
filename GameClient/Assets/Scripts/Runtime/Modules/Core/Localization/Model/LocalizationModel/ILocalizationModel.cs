using System.Collections;
using Runtime.Modules.Core.Localization.Enum;
using Runtime.Modules.Core.Localization.View;
using TMPro;
using UnityEngine.Localization.Components;

namespace Runtime.Modules.Core.Localization.Model.LocalizationModel
{
  public interface ILocalizationModel
  {
    string GetLanguageCode();

    IEnumerator ChangeLanguage(string newLanguageKey);
    
    /// <summary>
    /// If word with arguments use this method. Do not forget to remove <see cref="LocalizationTextBehaviour"/> and add <see cref="LocalizeStringEvent"/>.
    /// </summary>
    /// <param name="tableKey">Table of Key. Window -> Panels -> Localization Tables. Find key and it's table.</param>
    /// <param name="translateKey">Each translate has different key.</param>
    /// <returns>String returns. Use like that label.text = localizationModel.GetText(...).</returns>
    string GetText(TableKey tableKey, TranslateKey translateKey);
    
    /// <summary>
    /// If word with arguments use this method. Do not forget to remove <see cref="LocalizeStringEvent"/> and add <see cref="LocalizationTextBehaviour"/>.
    /// </summary>
    /// <param name="tmp">TextMeshProGui object.</param>
    /// <param name="tableKey">Table of Key. Window -> Panels -> Localization Tables. Find key and it's table.</param>
    /// <param name="translateKey">Each translate has different key.</param>
    /// <param name="arguments">Changeable arguments. limitless strings arguments can be acceptable. However needs add to
    /// <see cref="LocalizationTextBehaviour"/>.</param>
    void GetTextArguments(TextMeshProUGUI tmp, TableKey tableKey, TranslateKey translateKey, params string[] arguments);
  }
}