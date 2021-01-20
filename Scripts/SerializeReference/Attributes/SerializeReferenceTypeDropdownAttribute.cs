// Copyright 2021 Mineral Entertainment. All Rights Reserved.

using System;
using UnityEngine;

/// <summary>
/// <see cref="PropertyAttribute"/> that shows a dropdown with eligible types for a <see cref="SerializeReference"/>
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class SerializeReferenceTypeDropdownAttribute : PropertyAttribute
{
}