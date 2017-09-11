using System.Collections;
using System.Collections.Generic;
using PillowTalk.StateMachine;
using UnityEngine;

public class ExampleStateMachine : AsyncStateMachine 
{
    [SerializeField] private ExampleStateA stateA;
	[SerializeField] private ExampleStateB stateB;
	[SerializeField] private ExampleStateC stateC;

	public ExampleStateA StateA 
	{ 
		get { return stateA; } 
		set { stateA = value; } 
	}

	public ExampleStateB StateB 
	{ 
		get { return stateB; } 
		set { stateB = value; }  
	}

	public ExampleStateC StateC 
	{ 
		get { return stateC; } 
		set { stateC = value; }  
	}

	public void OnStateAPressed()
	{
		this.TransitionToState(StateA);
	}

	public void OnStateBPressed()
	{
		this.TransitionToState(StateB);
	}

	public void OnStateCPressed()
	{
		this.TransitionToState(StateC);
	}
}
