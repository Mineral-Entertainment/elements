// Copyright 2021 Mineral Entertainment. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static UnityEditor.TypeCache;

namespace Mineral
{
	/// <summary>
	/// PropertyDrawer that draws all const string fields of a type in a dropdown menu
	/// </summary>
	[CustomPropertyDrawer(typeof(ConstTagDropdownAttribute))]
	public class ConstTagDropdownDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			ConstTagDropdownAttribute tagAttribute = attribute as ConstTagDropdownAttribute;

			List<string> availableTags = new List<string>();

			foreach (Type type in tagAttribute.Types)
			{
				availableTags.AddRange(GetAvailableTags(type));

				TypeCollection derivedTypes = TypeCache.GetTypesDerivedFrom(type);
				foreach (Type derivedType in derivedTypes)
				{
					availableTags.AddRange(GetAvailableTags(derivedType));
				}
			}

			if (availableTags.Count == 0)
			{
				return;
			}

			string currentTag = property.stringValue;
			int selectedIndex = availableTags.IndexOf(currentTag);

			if (selectedIndex == -1)
			{
				selectedIndex = 0;
			}

			EditorGUI.BeginProperty(position, label, property);
			property.stringValue = availableTags[EditorGUI.Popup(position, label.text, selectedIndex, availableTags.ToArray())];
			EditorGUI.EndProperty();
		}

		private List<string> GetAvailableTags(Type type)
		{
			List<string> availableTags = new List<string>();

			IEnumerable<FieldInfo> fieldInfos = GetFieldInfos(type);

			foreach (FieldInfo fieldInfo in fieldInfos)
			{
				if (fieldInfo.FieldType == typeof(string) && fieldInfo.IsLiteral)
				{
					string tag = (string)fieldInfo.GetRawConstantValue();
					if (!availableTags.Contains(tag))
					{
						availableTags.Add(tag);
					}
				}
			}

			return availableTags;
		}

		private IEnumerable<FieldInfo> GetFieldInfos(Type type)
		{
			List<FieldInfo> fieldInfo = new List<FieldInfo>(type.GetFields());

			if (type.BaseType != null)
			{
				fieldInfo.AddRange(GetFieldInfos(type.BaseType));
			}

			return fieldInfo;
		}
	}
}