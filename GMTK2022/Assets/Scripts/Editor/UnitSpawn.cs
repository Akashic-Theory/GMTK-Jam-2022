using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(WaveData.UnitSpawn))]
public class UnitSpawnDrawer : PropertyDrawer
{
        // var container = new VisualElement();
        // var type = new    PropertyField(property.FindPropertyRelative("type"));
        // var delay = new   PropertyField(property.FindPropertyRelative("delay"));
        // var growMod = new PropertyField(property.FindPropertyRelative("growMod"));
        
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), GUIContent.none);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var typeRect = new Rect(position.x, position.y, 100, position.height);
        var delayRect = new Rect(position.x + 105, position.y, 30, position.height);
        var growRect = new Rect(position.x + 140, position.y, 30, position.height);
        var rewardRect = new Rect(position.x + 175, position.y, 30, position.height);
        
        
        EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("type"), GUIContent.none);
        EditorGUI.PropertyField(delayRect, property.FindPropertyRelative("delay"), GUIContent.none);
        EditorGUI.PropertyField(growRect, property.FindPropertyRelative("growMod"), GUIContent.none);
        EditorGUI.PropertyField(rewardRect, property.FindPropertyRelative("reward"), GUIContent.none);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}