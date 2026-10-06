using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MicogameTutorials : EditorWindow
{
    string markdown = @"# Microgames - ";
    Vector2 scroll;

    public TutorialList tutorialList;

    // Store path to the ScriptableObject to load
    private static string tutorialListAssetPath;

    [MenuItem("Tutorials/Microgame Karting Tutorials")]
    public static void ShowKartingTutorials()
    {
        tutorialListAssetPath = "Assets/Pixelagent/Karting Tutorial List.asset";
        ShowWindow("Microgame Karting Tutorials");
    }

    [MenuItem("Tutorials/Microgame FPS Tutorials")]
    public static void ShowFPSTutorials()
    {
        tutorialListAssetPath = "Assets/Pixelagent/FPS Tutorial List.asset";
        ShowWindow("Microgame FPS Tutorials");
    }

    [MenuItem("Tutorials/Microgame 2D Platformer Tutorials")]
    public static void Show2DPlatformerTutorials()
    {
        tutorialListAssetPath = "Assets/Pixelagent/2D Platformer Tutorial List.asset";
        ShowWindow("Microgame 2D Platformer Tutorials");
    }

    private static void ShowWindow(string title)
    {
        var window = GetWindow<MicogameTutorials>(title);
        window.LoadTutorialList();
    }

    void LoadTutorialList()
    {
        tutorialList = AssetDatabase.LoadAssetAtPath<TutorialList>(tutorialListAssetPath);
        Repaint();
    }

    void OnGUI()
    {
        if (tutorialList == null)
        {
            EditorGUILayout.LabelField("TutorialList asset not found at:\n" + tutorialListAssetPath);
            return;
        }

        if (tutorialList.headerImage != null)
        {
            float aspectRatio = (float)tutorialList.headerImage.width / tutorialList.headerImage.height;
            float width = EditorGUIUtility.currentViewWidth - 20;
            float height = width / aspectRatio;

            Rect imageRect = GUILayoutUtility.GetRect(width, height, GUILayout.ExpandWidth(true));
            GUI.DrawTexture(imageRect, tutorialList.headerImage, ScaleMode.ScaleToFit);
            EditorGUILayout.Space(10);
        }



        scroll = EditorGUILayout.BeginScrollView(scroll);

        string richText = MarkdownToRichText(markdown + " " + tutorialList.headerName);

        GUIStyle richTextStyle = new GUIStyle(EditorStyles.label)
        {
            richText = true,
            wordWrap = true
        };
        EditorGUILayout.LabelField(richText, richTextStyle);
        EditorGUILayout.Space(20);

        GUIStyle linkStyle = new GUIStyle(EditorStyles.label)
        {
            normal = { textColor = Color.green },
            hover = { textColor = new Color(0.2f, 0.5f, 1f) },
            richText = true,
            wordWrap = false
        };

        var sortedTutorials = new List<Tutorial>(tutorialList.tutorials);
        sortedTutorials.Sort((a, b) => string.Compare(a.name, b.name, System.StringComparison.OrdinalIgnoreCase));

        for (int i = 0; i < sortedTutorials.Count; i++)
        {
            var tutorial = sortedTutorials[i];
            string buttonText = $"{i + 1}. {tutorial.name}";
            if (GUILayout.Button(buttonText, linkStyle))
            {
                Application.OpenURL(tutorial.url);
            }
        }

        EditorGUILayout.EndScrollView();
    }

    string MarkdownToRichText(string markdown)
    {
        string richText = markdown;

        richText = System.Text.RegularExpressions.Regex.Replace(richText, @"^# (.+)$", "<size=18><b>$1</b></size>", System.Text.RegularExpressions.RegexOptions.Multiline);
        richText = System.Text.RegularExpressions.Regex.Replace(richText, @"^## (.+)$", "<size=16><b>$1</b></size>", System.Text.RegularExpressions.RegexOptions.Multiline);
        richText = System.Text.RegularExpressions.Regex.Replace(richText, @"^### (.+)$", "<size=14><b>$1</b></size>", System.Text.RegularExpressions.RegexOptions.Multiline);

        richText = System.Text.RegularExpressions.Regex.Replace(richText, @"\*\*(.+?)\*\*", "<b>$1</b>");
        richText = System.Text.RegularExpressions.Regex.Replace(richText, @"\*(.+?)\*", "<i>$1</i>");
        richText = System.Text.RegularExpressions.Regex.Replace(richText, @"`(.+?)`", "<color=grey><i>$1</i></color>");

        return richText;
    }
}
