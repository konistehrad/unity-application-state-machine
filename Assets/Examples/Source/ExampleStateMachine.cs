using System.Collections;
using System.Collections.Generic;
using PillowTalk.StateMachine;
using UnityEngine;

public class ExampleStateMachine : AsyncStateMachine 
{
    [SerializeField] private ExampleStateA stateA;
	[SerializeField] private ExampleStateB stateB;
	[SerializeField] private ExampleStateC stateC;

	public ExampleStateA StateA { get { return stateA; } }
	public ExampleStateB StateB { get { return stateB; } }
	public ExampleStateC StateC { get { return stateC; } }
}
