// Copyright 2021 Mineral Entertainment. All Rights Reserved.

using UnityEngine;

namespace Mineral
{
	public class StateMachineRunner : MonoBehaviour
	{
		[SerializeField] private StateMachine stateMachine;

		protected void Awake()
		{
			stateMachine.Initialize();
		}

		protected void Update()
		{
			stateMachine.Update();
		}
	}
}