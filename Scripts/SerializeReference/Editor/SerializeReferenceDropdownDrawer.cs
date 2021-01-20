using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Utility class with functionality to draw a dropdown menu with all assignable types for <see cref="SerializeReference"/>
/// </summary>
public static class SerializeReferenceDropdownDrawer
{
	private readonly struct MenuData
	{
		public MenuData(Type type, SerializedProperty property)
		{
			Type = type;
			Property = property;
		}

		public readonly SerializedProperty Property;
		public readonly Type Type;
	}

	/// <summary>
	/// Draws the dropdown menu for the specified property
	/// </summary>
	/// <param name="property">The property holding the managed reference</param>
	/// <param name="position">Where to draw the menu</param>
	public static void Draw(SerializedProperty property, Rect position)
	{
		Rect buttonPosition = position;
		buttonPosition.x += EditorGUIUtility.labelWidth + EditorGUIUtility.standardVerticalSpacing;
		buttonPosition.width = position.width - EditorGUIUtility.labelWidth - EditorGUIUtility.standardVerticalSpacing;
		buttonPosition.height = EditorGUIUtility.singleLineHeight;

		(string AssemblyName, string ClassName) names = SerializeReferenceUtility.SplitFullTypeName(property.managedReferenceFullTypename);

		Color backgroundColor = GUI.backgroundColor;
		string className = "Null";

		if (string.IsNullOrEmpty(names.ClassName))
		{
			GUI.backgroundColor = Color.red;
		}
		else
		{
			className = names.ClassName;
		}

		if (EditorGUI.DropdownButton(buttonPosition, new GUIContent(className), FocusType.Keyboard))
		{
			DrawDropdown(property, buttonPosition, className);
		}

		GUI.backgroundColor = backgroundColor;
	}

	private static void DrawDropdown(SerializedProperty property, Rect position, string currentPropertyType)
	{
		GenericMenu menu = new GenericMenu();
		PopulateMenu(menu, property, currentPropertyType);

		menu.DropDown(position);
	}

	private static void PopulateMenu(GenericMenu menu, SerializedProperty property, string currentPropertyType)
	{
		menu.AddItem(new GUIContent("Null"), false, SetManagedReferenceValue, new MenuData(null, property));

		IEnumerable<Type> types = SerializeReferenceUtility.GetAssignableTypes(property);

		foreach (Type type in types)
		{
			menu.AddItem(new GUIContent(type.ToString()), type.ToString() == currentPropertyType, SetManagedReferenceValue, new MenuData(type, property));
		}
	}

	private static void SetManagedReferenceValue(object menuParameter)
	{
		var parameter = (MenuData)menuParameter;
		Type type = parameter.Type;
		SerializedProperty property = parameter.Property;

		object instance = type != null ? Activator.CreateInstance(type) : null;

		property.serializedObject.Update();
		property.managedReferenceValue = instance;
		property.serializedObject.ApplyModifiedProperties();
	}
}