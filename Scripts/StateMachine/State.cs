// Copyright 2021 Mineral Entertainment. All Rights Reserved.

using System;
using UnityEngine;

namespace Mineral
{
	[Serializable]
	public class State
	{
		public bool StartingState => startingState;
		public string Name => name;

		[SerializeField] private bool startingState;
		[SerializeField, ConstTagDropdown(typeof(StateTags))] private string name;

		public void OnStateEnter()
		{
		}

		public void OnStateUpdate()
		{
		}

		public void OnStateExit()
		{
		}
	}
}