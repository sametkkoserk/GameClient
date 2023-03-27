using Editor.Tools.HierarchyHeader.Runtime;
using UnityEditor;
using UnityEditor.SettingsManagement;
using UnityEngine;

namespace Editor.Tools.HierarchyHeader.Editor
{
  public static class HeaderSettings
  {
    internal const string k_PackageName = "com.bennykok.hierarchy-header";
    internal const string k_PreferencesPath = "Project/BK/Hierarchy Header";

    [UserSetting]
    public static UserSetting<int> maxLength = new(Instance, "general.maxLength", 30);

    [UserSetting]
    public static UserSetting<int> minPrefixLength = new(Instance, "general.minPrefixLength", 10);

    [UserSetting]
    public static UserSetting<HeaderType> headerType = new(Instance, "general.headerType", HeaderType.Default);

    [UserSetting]
    public static UserSetting<string> customPrefix = new(Instance, "general.customPrefix", null);

    [UserSetting]
    public static UserSetting<HeaderAlignment> alignment = new(Instance, "general.alignment", HeaderAlignment.Center);

    private static Settings s_Instance;

    internal static Settings Instance
    {
      get
      {
        if (s_Instance == null)
          s_Instance = new Settings(k_PackageName);

        return s_Instance;
      }
    }


    [UserSettingBlock("General")]
    private static void CustomSettingsGUI(string searchContext)
    {
      using (var scope = new EditorGUI.ChangeCheckScope())
      {
        maxLength.value = SettingsGUILayout.SettingsSlider("Max Length", maxLength, 10, 60, searchContext);

        headerType.value = (HeaderType)EditorGUILayout.EnumPopup("Header Type", headerType.value);
        SettingsGUILayout.DoResetContextMenuForLastRect(headerType);

        if (headerType.value == HeaderType.Custom)
        {
          var v = SettingsGUILayout.SettingsTextField("Custom Prefix", customPrefix, searchContext);
          if (v?.Length <= 1)
            customPrefix.value = v;
        }

        alignment.value = (HeaderAlignment)EditorGUILayout.EnumPopup("Header Alignment", alignment.value);
        SettingsGUILayout.DoResetContextMenuForLastRect(alignment);

        if (alignment.value == HeaderAlignment.Start || alignment.value == HeaderAlignment.End)
          minPrefixLength.value = SettingsGUILayout.SettingsSlider("Min Prefix Length", minPrefixLength, 0, 10, searchContext);

        if (scope.changed)
        {
          Instance.Save();
          HeaderEditor.UpdateAllHeader();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Update Headers")) HeaderEditor.UpdateAllHeader();
      }
    }

    [SettingsProvider]
    private static SettingsProvider CreateSettingsProvider()
    {
      var provider = new UserSettingsProvider(k_PreferencesPath, Instance,
        new[] { typeof(HeaderSettings).Assembly }, SettingsScope.Project);

      return provider;
    }
  }
}