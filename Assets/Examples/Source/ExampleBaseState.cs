using System.Collections;
using System.Collections.Generic;
using PillowTalk.StateMachine;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ExampleBaseState : AsyncStateBehaviour 
{
    [SerializeField] private ExampleStateMachine stateMachine;

    protected override void Awake()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        DisableInteraction();
        gameObject.SetActive(false);
    }

    
}
