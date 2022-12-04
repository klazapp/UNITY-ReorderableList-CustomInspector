using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;
public class EnemyAITemplateWindow : EditorWindow
{
   #region variables
   public EnemyAITemplate enemyAITemplate;
   private Editor enemyAITemplateEditor;
   
   private Vector2 scrollPos = Vector2.zero;
   
   //Save path
   private const string pathSuffix = "Assets/Data/";
   #endregion
   
   [MenuItem("Tools/Generator/Enemy AI")]
   public static void ShowWindow()
   {
      var inspectorType = Type.GetType("UnityEditor.InspectorWindow,UnityEditor.dll");
      
      //Docks next to inspector window
      EditorWindow window = GetWindow<EnemyAITemplateWindow>("Enemy AI Generator", new Type[]
      {
         inspectorType
      });
   }

   private void OnEnable()
   {
      if (enemyAITemplate is null)
         enemyAITemplate = (EnemyAITemplate)AssetDatabase.LoadAssetAtPath(pathSuffix + "EnemyAI.asset", typeof(EnemyAITemplate));
      
      enemyAITemplateEditor = Editor.CreateEditor(enemyAITemplate);
   }

   private void OnGUI()
   {
      //Enables scrolling 
      scrollPos = EditorGUILayout.BeginScrollView(scrollPos, true, true, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
      
      EditorGUILayout.Space(20);
      EditorGUILayout.LabelField("Enemy AI Generator", CustomEditorStyling.titleStyle);
      
      EditorGUILayout.Space(20);
      EditorGUILayout.LabelField("Tool", CustomEditorStyling.titleStyle);
      EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
      
      EditorGUILayout.BeginVertical("GroupBox");
      if (GUILayout.Button("Create New Template",GUILayout.Height(60)))
      {
         var asset = CreateInstance<EnemyAITemplate>();
         var assetName = AssetDatabase.GenerateUniqueAssetPath(pathSuffix + "EnemyAI.asset");
         
         AssetDatabase.CreateAsset(asset, assetName);
         AssetDatabase.SaveAssets();

         enemyAITemplate = asset;
         enemyAITemplateEditor = Editor.CreateEditor(enemyAITemplate);
      }
      
      EditorGUILayout.Space(20);
      
      EditorGUI.BeginChangeCheck();
      EditorGUILayout.LabelField("Current Template", CustomEditorStyling.subTitleStyle);
      enemyAITemplate = (EnemyAITemplate)EditorGUILayout.ObjectField(enemyAITemplate, typeof(EnemyAITemplate), true);
      if (EditorGUI.EndChangeCheck())
      {
         enemyAITemplateEditor = Editor.CreateEditor(enemyAITemplate);
      }
      
      EditorGUILayout.EndVertical();
      
      EditorGUILayout.Space(20);
      EditorGUILayout.LabelField("Template", CustomEditorStyling.titleStyle);
      EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
      
      EditorGUILayout.Space(20);

      enemyAITemplateEditor.OnInspectorGUI();

      EditorGUILayout.EndScrollView();
   }
      
   private static void PingFolderOrFirstAsset(string folderPath)
   {
      var path = GetFirstAssetPathInFolder(folderPath);
      if (string.IsNullOrEmpty(path))
         path = folderPath;
      var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
      EditorGUIUtility.PingObject(obj);
   }
 
   private static string GetFirstAssetPathInFolder(string folder, bool includeFolders = true)
   {
      if (!includeFolders) 
         return GetFirstValidAssetPath(System.IO.Directory.GetFiles(folder));
            
      var path = GetFirstValidAssetPath(System.IO.Directory.GetDirectories(folder));
      return path ?? GetFirstValidAssetPath(System.IO.Directory.GetFiles(folder));
   }
 
   private static string GetFirstValidAssetPath(IEnumerable<string> paths)
   {
      return paths.FirstOrDefault(t => !string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(t)));
   }
}
