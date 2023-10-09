using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.SimpleLocalization.Scripts.Editor
{
    public class LocalizedTranslate : PopupWindowContent
    {
        public static string DestinationLanguage;

        public static string SourceLanguage = "English";

        private static bool _emptyOnly;

        private static LocalizationEditor Editor => LocalizationEditor.Instance;

        private const int ButtonWidth = 220;

        public override void OnGUI(Rect rect)
        {
            GUILayout.BeginVertical();

            if (GUILayout.Button("Set primary language", GUILayout.Width(ButtonWidth)))
            {
                SourceLanguage = DestinationLanguage;
            }

            if (GUILayout.Button("Translate cell", GUILayout.Width(ButtonWidth)))
            {
                if (ValidateInput())
                {
                    EditorCoroutineUtility.StartCoroutine(TranslateCell(), this);
                }
            }
            if (GUILayout.Button("Translate row (empty cells only)", GUILayout.Width(ButtonWidth)))
            {
                if (ValidateInput())
                {
                    _emptyOnly = true;
                    EditorCoroutineUtility.StartCoroutine(TranslateKey(), this);
                }
            }
            if (GUILayout.Button("Translate row (override)", GUILayout.Width(ButtonWidth)))
            {
                if (ValidateInput())
                {
                    _emptyOnly = false;
                    EditorCoroutineUtility.StartCoroutine(TranslateKey(), this);
                }
            }
            if (GUILayout.Button("Translate column (empty cells only)", GUILayout.Width(ButtonWidth)))
            {
                if (ValidateInput())
                {
                    _emptyOnly = true;
                    EditorCoroutineUtility.StartCoroutine(TranslateColumn(), this);
                }
            }
            if (GUILayout.Button("Translate column (override)", GUILayout.Width(ButtonWidth)))
            {
                if (ValidateInput())
                {
                    _emptyOnly = false;
                    EditorCoroutineUtility.StartCoroutine(TranslateColumn(), this);
                }
            }

            GUILayout.EndVertical();
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(220, 150);
        }

        private IEnumerator TranslateCell()
        {
            var success = true;

            yield return Translate(new[] { DestinationLanguage }, Editor.CurrentKey, 1, (result) => { success = result; } );

            EditorUtility.ClearProgressBar();

            if (success)
            {
                EditorUtility.DisplayDialog("Message", "Auto Translate completed!", "OK");
            }
        }

        private IEnumerator TranslateColumn()
        {
            var emptyKeys = Editor.SheetDictionary[DestinationLanguage].Keys.Where(key => string.IsNullOrEmpty(Editor.SheetDictionary[DestinationLanguage][key])).ToArray();
            var translatedKeys = Editor.SheetDictionary[DestinationLanguage].Keys.Where(key => !string.IsNullOrEmpty(Editor.SheetDictionary[DestinationLanguage][key])).ToArray();

            if (!emptyKeys.Any() && _emptyOnly)
            {
                yield break;
            }

            var success = true;
            var progressMax = emptyKeys.Length + (_emptyOnly ? 0 : translatedKeys.Length);

            foreach(var key in emptyKeys)
            {
                yield return Translate(new[] { DestinationLanguage }, key, progressMax, (result) => { success = result; });
            }

            if (!_emptyOnly && success)
            {
                foreach (var key in translatedKeys)
                {
                    yield return Translate(new[] {DestinationLanguage}, key, progressMax, (result) => { success = result; });
                }
            }

            EditorUtility.ClearProgressBar();

            if (success)
            {
                EditorUtility.DisplayDialog("Message", "Auto Translate completed!", "OK");
            }
        }

        public static IEnumerator TranslateAuto()
        {
            if (ValidateInput(true))
            {
                yield return TranslateKey();
            }
        }

        private static IEnumerator TranslateKey()
        {
            var emptyLanguages = Editor.SheetDictionary.Keys.Where(language => string.IsNullOrEmpty(Editor.SheetDictionary[language][Editor.CurrentKey])).ToArray();
            var translatedLanguages = Editor.SheetDictionary.Keys.Where(language => !string.IsNullOrEmpty(Editor.SheetDictionary[language][Editor.CurrentKey])).ToArray();

            if (!emptyLanguages.Any() && _emptyOnly)
            {
                yield break;
            }

            var success = true;
            var progressMax = emptyLanguages.Length + (_emptyOnly ? 0 : translatedLanguages.Length);

            yield return Translate(emptyLanguages, Editor.CurrentKey, progressMax, (result) => { success = result; });

            if (!_emptyOnly && success)
            {
                yield return Translate(translatedLanguages, Editor.CurrentKey, progressMax, (result) => { success = result; });
            }

            EditorUtility.ClearProgressBar();

            if (success)
            {
                EditorUtility.DisplayDialog("Message", "Auto Translate completed!", "OK");
            }
        }

        private static IEnumerator Translate(string[] languages, string key, int progressMax, Action<bool> callback)
        {
            var error = "";
            var translatedText = "";

            for (var i = 0; i < languages.Length; i++)
            {
                var progress = (float)(i + 1) / progressMax;

                if (EditorUtility.DisplayCancelableProgressBar("Submitting data...", $"[{(int)(100 * progress)}%] [{i + 1}/{progressMax}] ...", progress))
                {
                    callback(false);

                    yield break;
                }

                if (string.IsNullOrEmpty(SourceLanguage))
                {
                    EditorUtility.DisplayDialog("Error", $"Assign the source text of {key} key for {SourceLanguage} language please!", "OK");

                    yield break;
                }

                yield return TranslateText(GetLangCode(languages[i]), Editor.SheetDictionary[SourceLanguage][key], SourceLanguage,
                    (message, translated) =>
                    {
                        translatedText = translated;
                        error = message;
                    });


                if (!string.IsNullOrEmpty(error))
                {
                    EditorUtility.ClearProgressBar();
                    EditorUtility.DisplayDialog("Error", $"Can't translate data: {error}", "OK");
                    callback(false);

                    yield break;
                }

                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(translatedText)) continue;

                Editor.SheetDictionary[languages[i]][key] = translatedText;

                if (!Editor.IsNewKey(key))
                {
                    if (!Editor.KeysActions.ContainsKey(key))
                    {
                        Editor.KeysActions.Add(key, ActionType.Edit);
                    }
                }
            }
        }

        private static IEnumerator TranslateText(string targetLang, string sourceText, string sourceLanguage, Action<string, string> callback)
        {
            var url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=" + GetLangCode(sourceLanguage) + "&tl=" + targetLang + "&dt=t&q=" + UnityWebRequest.EscapeURL(sourceText);
            var request = UnityWebRequest.Get(url);

            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            var error = request.error;
            var result = "";

            if (request.error == null)
            {
                var parsedTexts = request.downloadHandler.text;
                var start = parsedTexts.IndexOf('"') + 1;
                var length = parsedTexts.IndexOf('"', start + 1) - start;

                result = parsedTexts.Substring(start, length);
            }

            request.Dispose();
            callback(error, result);
        }

        private static string GetLangCode(string language)
        {
            return language switch
            {
                "Afrikaans" => "af",
                "Albanian" => "sq",
                "Amharic" => "am",
                "Arabic" => "ar",
                "Armenian" => "hy",
                "Azerbaijani" => "az",
                "Basque" => "eu",
                "Belarusian" => "be",
                "Bengali" => "bn",
                "Bosnian" => "bs",
                "Bulgarian" => "bg",
                "Catalan" => "ca",
                "Cebuano" => "ceb",
                "Chinese" => "zh",
                "Corsican" => "co",
                "Croatian" => "hr",
                "Czech" => "cs",
                "Danish" => "da",
                "Dutch" => "nl",
                "English" => "en",
                "Esperanto" => "eo",
                "Estonian" => "et",
                "Finnish" => "fi",
                "French" => "fr",
                "Frisian" => "fy",
                "Galician" => "gl",
                "Georgian" => "ka",
                "German" => "de",
                "Greek" => "el",
                "Gujarati" => "gu",
                "Haitian Creole" => "ht",
                "Hausa" => "ha",
                "Hawaiian" => "haw",
                "Hebrew" => "he",
                "Hindi" => "hi",
                "Hmong" => "hmn",
                "Hungarian" => "hu",
                "Icelandic" => "is",
                "Igbo" => "ig",
                "Indonesian" => "id",
                "Irish" => "ga",
                "Italian" => "it",
                "Japanese" => "ja",
                "Javanese" => "jw",
                "Kannada" => "kn",
                "Kazakh" => "kk",
                "Khmer" => "km",
                "Korean" => "ko",
                "Kurdish" => "ku",
                "Kyrgyz" => "ky",
                "Lao" => "lo",
                "Latin" => "la",
                "Latvian" => "lv",
                "Lithuanian" => "lt",
                "Luxembourgish" => "lb",
                "Macedonian" => "mk",
                "Malagasy" => "mg",
                "Malay" => "ms",
                "Malayalam" => "ml",
                "Maltese" => "mt",
                "Maori" => "mi",
                "Marathi" => "mr",
                "Mongolian" => "mn",
                "Myanmar" => "my",
                "Nepali" => "ne",
                "Norwegian" => "no",
                "Nyanja" => "ny",
                "Pashto" => "ps",
                "Persian" => "fa",
                "Polish" => "pl",
                "Portuguese" => "pt",
                "Punjabi" => "pa",
                "Romanian" => "ro",
                "Russian" => "ru",
                "Samoan" => "sm",
                "Scots Gaelic" => "gd",
                "Serbian" => "sr",
                "Sesotho" => "st",
                "Shona" => "sn",
                "Sindhi" => "sd",
                "Sinhala" => "si",
                "Slovak" => "sk",
                "Slovenian" => "sl",
                "Somali" => "so",
                "Spanish" => "es",
                "Sundanese" => "su",
                "Swahili" => "sw",
                "Swedish" => "sv",
                "Tagalog" => "tl",
                "Tajik" => "tg",
                "Tamil" => "ta",
                "Telugu" => "te",
                "Thai" => "th",
                "Turkish" => "tr",
                "Ukrainian" => "uk",
                "Urdu" => "ur",
                "Uzbek" => "uz",
                "Vietnamese" => "vi",
                "Welsh" => "cy",
                "Xhosa" => "xh",
                "Yiddish" => "yi",
                "Yoruba" => "yo",
                "Zulu" => "zu",
                _ => "en"
            };
        }

        private static bool ValidateInput(bool auto = false)
        {
            if (string.IsNullOrEmpty(SourceLanguage))
            {
                EditorUtility.DisplayDialog("Error", "Assign the source language please!", "OK");

                return false;
            }

            if (string.IsNullOrEmpty(Editor.SheetDictionary[SourceLanguage][Editor.CurrentKey]))
            {
                EditorUtility.DisplayDialog("Error", $"Assign the source text for {SourceLanguage} please!", "OK");

                return false;
            }

            if (!auto && string.IsNullOrEmpty(DestinationLanguage))
            {
                EditorUtility.DisplayDialog("Error", "Assign the destination language please!", "OK");

                return false;
            }

            return true;
        }
    }
}
