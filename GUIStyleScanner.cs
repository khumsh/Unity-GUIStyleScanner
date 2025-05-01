using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public sealed class GUIStyleScanner : EditorWindow
{
    private Vector2 scrollPos;
    private List<string> supportedStyles = new();
    private bool stylesLoaded = false;

    [MenuItem("Tools/Scan GUI Styles")]
    private static void Open()
    {
        GetWindow<GUIStyleScanner>("GUI Style Scanner");
    }

    private void OnGUI()
    {
        if (!stylesLoaded)
        {
            LoadSupportedStyles();
            stylesLoaded = true;
        }

        EditorGUILayout.LabelField("GUIStyles", EditorStyles.boldLabel);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        foreach (string styleName in supportedStyles)
        {
            GUIStyle style = GUI.skin.FindStyle(styleName);
            if (style == null) continue;

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Style: {styleName}", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Preview:", style, GUILayout.Height(24));
            EditorGUILayout.TextField("Sample Text", style);
            EditorGUILayout.Space(8);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
    }

    private void LoadSupportedStyles()
    {
        supportedStyles.Clear();

        // Unity에서 기본 제공하는 모든 스타일 이름 얻기
        GUIStyle[] allStyles = GUI.skin.customStyles;
        HashSet<string> styleSet = new HashSet<string>();

        foreach (GUIStyle style in allStyles)
        {
            if (!string.IsNullOrEmpty(style.name))
                styleSet.Add(style.name);
        }

        // Unity가 내부적으로 사용하는 기본 스타일들도 포함
        string[] wellKnownStyles = new[]
        {
            "label", "button", "box", "textField", "textArea", "toggle", "window",
            "horizontalSlider", "verticalSlider", "horizontalScrollbar", "verticalScrollbar",
            "scrollView", "popup", "toolbar", "helpbox", "BoldLabel", "Foldout"
        };

        foreach (string s in wellKnownStyles)
            styleSet.Add(s);

        supportedStyles.AddRange(styleSet);
        supportedStyles.Sort();
    }
}
