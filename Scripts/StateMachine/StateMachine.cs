// Copyright 2021 Mineral Entertainment. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mineral
{
	[Serializable]
	public class StateMachine
	{
		[SerializeField] private List<State> states;

		private State activeState;

		public void Initialize()
		{
			activeState = states.FirstOrDefault(state => state.StartingState);
			activeState.OnStateEnter();
		}

		public void Update()
		{
			if (activeState != null)
			{
				activeState.OnStateUpdate();
			}
		}
	}
}