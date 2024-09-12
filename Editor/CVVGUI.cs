using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CycleValueVarible<>))]
public class CVVGUI : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        // 获取需要显示的属性
        SerializedProperty valueProperty = property.FindPropertyRelative("_EqualText");
        
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.TextField(position, label.text, valueProperty.stringValue);
        EditorGUI.EndDisabledGroup();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // 返回单行高度
        return EditorGUIUtility.singleLineHeight;
    }
}
