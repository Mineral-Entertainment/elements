// Copyright 2021 Mineral Entertainment. All Rights Reserved.

namespace Mineral
{
	public interface IStateTransitioner
	{
		State FromState { get; }
		State ToState { get; }

		bool CanTransition { get; }

		void Evaluate();
	}
}