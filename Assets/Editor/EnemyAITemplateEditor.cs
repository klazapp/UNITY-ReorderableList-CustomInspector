using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(EnemyAITemplate))]
public class EnemyAITemplateEditor : Editor
{ 
    public EnemyAITemplate enemyAITemplate;
    private SerializedProperty mainStates;

    //Reorderable list variable
    private class FinalActionReorderableListHolder
    {
        public List<ReorderableList> finalActionsList = new();
    }
    private List<FinalActionReorderableListHolder> finalActionReorderableListHolders = new();

    private void OnEnable()
    {
        enemyAITemplate = (EnemyAITemplate)target;
            
        if (enemyAITemplate is null)
            return;

        finalActionReorderableListHolders = new();
        
        //Find properties
        mainStates = serializedObject.FindProperty("mainStates");
        
        for (var i = 0; i < mainStates.arraySize; i++)
        {
            var mainState = mainStates.GetArrayElementAtIndex(i);
            var primaryState = mainState.FindPropertyRelative("primaryState");
            var secondaryState = mainState.FindPropertyRelative("secondaryState");
            var subState = secondaryState.FindPropertyRelative("subState");
            var tertiaryStates = secondaryState.FindPropertyRelative("tertiaryStates");
         
            var i1 = i;

            finalActionReorderableListHolders.Add(new FinalActionReorderableListHolder());

            if (tertiaryStates.arraySize <= 0)
            {
                //Add empty reorderable list if array size is zero
                finalActionReorderableListHolders[i].finalActionsList = 
                    new List<ReorderableList>
                    {
                        new(serializedObject, tertiaryStates, true, true, true, true)
                        {
                            drawElementCallback = (rect, index, b, focused) =>
                                DrawFinalActionsListItems(rect, index, b, focused, i1, 0),
                            drawHeaderCallback =
                                DrawFinalActionsHeader, 
                            drawNoneElementCallback = DrawNone
                        }
                    };
            }
            else
            {
                for (var j = 0; j < tertiaryStates.arraySize; j++)
                {
                    var j1 = j;
                    finalActionReorderableListHolders[i1].finalActionsList.Add(
                        new ReorderableList(serializedObject, tertiaryStates, true, true, true, true)
                        {
                            drawElementCallback = (rect, index, b, focused) =>
                                DrawFinalActionsListItems(rect, index, b, focused, i1, j1), // Delegate to draw the elements on the list
                            drawHeaderCallback = DrawFinalActionsHeader,
                            drawNoneElementCallback = DrawNone
                        });
                }
            }
        }
    }


    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        
        serializedObject.Update();
        
        EditorGUILayout.BeginVertical("GroupBox");
        EditorGUILayout.LabelField("Enemy AI Behaviour", CustomEditorStyling.titleStyle);
        EditorGUILayout.EndVertical();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space(20);

        for (var i = 0; i < mainStates.arraySize; i++)
        {
            var mainState = mainStates.GetArrayElementAtIndex(i);
            var primaryState = mainState.FindPropertyRelative("primaryState");
            var secondaryState = mainState.FindPropertyRelative("secondaryState");
            var subState = secondaryState.FindPropertyRelative("subState");
            var tertiaryStates = secondaryState.FindPropertyRelative("tertiaryStates");
           
            //Display primary state
            EditorGUILayout.PropertyField(primaryState);
            EditorGUILayout.Space(5);

            //Display secondary state
            EditorGUILayout.PropertyField(subState);

            for (var j = 0; j < finalActionReorderableListHolders[i].finalActionsList.Count; j++)
            {
                //Draw reorderable list only once
                if (j == 0)
                    finalActionReorderableListHolders[i].finalActionsList[j].DoLayoutList();
            }

            EditorGUILayout.Space(30);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
    
    //Draw elements in the list
    private void DrawFinalActionsListItems(Rect rect, int index, bool isActive, bool isFocused, int i, int j)
    {
        var element = finalActionReorderableListHolders[i].finalActionsList[j].serializedProperty.GetArrayElementAtIndex(index);

        const float EXTRA_PADDING_X_VAL = 20f;
        var currentViewWidth = EditorGUIUtility.currentViewWidth;
        currentViewWidth -= EXTRA_PADDING_X_VAL * 2f;
        var individualViewWidth = currentViewWidth / 3f - EXTRA_PADDING_X_VAL;
        EditorGUIUtility.labelWidth = 80;

        //Draw final actions
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, individualViewWidth, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("finalActions"), GUIContent.none);
        
        //Draw final actions probability
        var probability = element.FindPropertyRelative("finalActionProbability").floatValue;
        probability = EditorGUI.Slider(new Rect(rect.x + individualViewWidth + EXTRA_PADDING_X_VAL, rect.y, individualViewWidth, 
                EditorGUIUtility.singleLineHeight), 
            "Probability",
            probability, 0f, 1f);
        element.FindPropertyRelative("finalActionProbability").floatValue = probability;

        EditorGUI.PropertyField(
            new Rect(rect.x + (individualViewWidth + EXTRA_PADDING_X_VAL) * 2f, rect.y, individualViewWidth, 
                EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("priorityLevel"),
            new GUIContent()
            {
                text = "Priority Level",
   
            }, false);
    }

    //Draw header of list
    private static void DrawFinalActionsHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Final Actions");
    }

    private static void DrawNone(Rect rect)
    {
        EditorGUI.LabelField(rect, "Final Actions Are Empty");
    }
}


