using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Assets.SimpleLocalization;
using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine.EventSystems;


public class LanguageDropdownBehaviour : MonoBehaviour, IPointerClickHandler
{
    // public Image arrowUI;
    // public Sprite upArrowsprite;
    // public Sprite downArrowsprite;

    private TMP_Dropdown dropdown;
    private List<string> languages;
    private bool isDropdownClicked = false;
    private int clickCount = 0;

    public void Start()
    {
        languages = LocalizationManager.GetLanguages();
        dropdown = GetComponent<TMP_Dropdown>();
        SetDropdown();

    }

    public void SetDropdown()
    {
        dropdown.onValueChanged.RemoveListener(OnChangeLanguage);
        dropdown.ClearOptions();
        dropdown.AddOptions(languages.ConvertAll(lng => LocalizationManager.Localize(lng)).ToList());

        int currentLanguageIndex = languages.FindIndex(opt => opt == PlayerPrefs.GetString("language"));
        dropdown.value = currentLanguageIndex;
        dropdown.onValueChanged.AddListener(OnChangeLanguage);

    }

    public void OnChangeLanguage(int index)
    {
        PlayerPrefs.SetString("language", languages[index]);
        LocalizationManager.Language = languages[index];
        SetDropdown();
    }


    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnChangeLanguage);
        //Multilanguage.instance.languagesReset -= SetDropdown;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickCount++;

        if (clickCount == 1)
        {
            //arrowUI.sprite = upArrowsprite;
            clickCount++;
        }
        else if (clickCount == 2)
        {
            //arrowUI.sprite = downArrowsprite;
            clickCount = 0;
        }
    }

}