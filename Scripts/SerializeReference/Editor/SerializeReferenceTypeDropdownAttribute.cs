// Copyright 2021 Mineral Entertainment. All Rights Reserved.

using UnityEditor;
using UnityEngine;

/// <summary>
/// <see cref="PropertyDrawer"/> drawing the dropdown menu for <see cref="SerializeReferenceTypeDropdownAttribute"/>
/// </summary>
[CustomPropertyDrawer(typeof(SerializeReferenceTypeDropdownAttribute))]
public class SerializeReferenceTypeDropdownAttributeDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, true);
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		var labelPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
		EditorGUI.LabelField(labelPosition, label);

		SerializeReferenceDropdownDrawer.Draw(property, position);

		EditorGUI.PropertyField(position, property, GUIContent.none, true);

		EditorGUI.EndProperty();
	}
}