// Copyright 2021 Mineral Entertainment. All Rights Reserved.

using System;
using UnityEngine;

namespace Mineral
{
	/// <summary>
	/// Attribute that will create a dropdown of all const string fields of the given types
	/// </summary>
	public class ConstTagDropdownAttribute : PropertyAttribute
	{
		public readonly Type[] Types;

		public ConstTagDropdownAttribute(params Type[] types)
		{
			Types = types;
		}
	}
}