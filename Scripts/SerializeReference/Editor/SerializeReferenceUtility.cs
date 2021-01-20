// Copyright 2021 Mineral Entertainment. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.TypeCache;
using Object = UnityEngine.Object;

public static class SerializeReferenceUtility
{
	/// <summary>
	/// Get all the assignable types from the serializedPropery type that can be applied to the managed reference value of that property
	/// </summary>
	/// <param name="property">The SerializedProperty to get the assignable types from</param>
	/// <returns>All assignable types</returns>
	public static IEnumerable<Type> GetAssignableTypes(SerializedProperty property)
	{
		Type fieldType = GetType(property);
		return GetAssignableTypes(fieldType);
	}

	/// <summary>
	/// String helper to split a type name with the assemblyname and class name, including namespace
	/// </summary>
	/// <param name="typename">The type as given by the serialized property</param>
	/// <returns>Assembly and class name</returns>
	public static (string AssemblyName, string ClassName) SplitFullTypeName(string typename)
	{
		if (string.IsNullOrEmpty(typename))
		{
			return (string.Empty, string.Empty);
		}

		string[] typeSplitString = typename.Split(' ');

		return (typeSplitString[0], typeSplitString[1]);
	}

	private static IEnumerable<Type> GetAssignableTypes(Type type)
	{
		TypeCollection derivedTypes = GetTypesDerivedFrom(type);

		return derivedTypes.Where(dt => !dt.IsSubclassOf(typeof(Object)) ||
										!dt.IsAbstract ||
										(dt.IsClass && dt.GetConstructor(Type.EmptyTypes) == null));
	}

	private static Type GetType(SerializedProperty property)
	{
		Type realPropertyType = GetType(property.managedReferenceFieldTypename);

		if (realPropertyType != null)
		{
			return realPropertyType;
		}

		Debug.LogError($"Unable to get field type of managed reference : {property.managedReferenceFieldTypename}");

		return null;
	}

	private static Type GetType(string typeString)
	{
		(string AssemblyName, string ClassName) names = SplitFullTypeName(typeString);

		Type type = Type.GetType($"{names.ClassName}, {names.AssemblyName}");

		return type;
	}
}