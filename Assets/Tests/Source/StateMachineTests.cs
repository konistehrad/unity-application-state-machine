using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class StateMachineTests
{
    private GameObject instanceRoot;
    private ExampleStateMachine stateMachine;

    [SetUp]
    public void StateMachineSetup()
    {
        GameObject prefab = Resources.Load<GameObject>("TestUIStack");
        instanceRoot = Object.Instantiate(prefab) as GameObject;
        stateMachine = instanceRoot.GetComponent<ExampleStateMachine>();
    }

    [TearDown]
    public void StateMachineTearDown()
    {
        if(instanceRoot != null)
        {
            Object.Destroy(instanceRoot);
        }
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator TransitionTests() {
        
        // Make sure that a null transition is bounced immediately.
        Assert.Throws<System.ArgumentNullException>(
            () => {
                stateMachine.TransitionToState(null);
            }
        );

        yield return null;

        stateMachine.TransitionToState(stateMachine.StateA);
        Assert.That(stateMachine.State, Is.EqualTo(stateMachine.StateA));
        Assert.That(stateMachine.IsTransitioning, Is.EqualTo(true));

        // ensure that the transitioning flag is set to false
        yield return stateMachine.WaitForTransitionComplete();
        Assert.That(stateMachine.IsTransitioning, Is.EqualTo(false));

        // transition from state A to B
        stateMachine.TransitionToState(stateMachine.StateB);
        yield return null;

        // make sure the previous and current variables are in place
        Assert.That(stateMachine.PreviousState, Is.EqualTo(stateMachine.StateA));
        Assert.That(stateMachine.State, Is.EqualTo(stateMachine.StateB));
        Assert.That(stateMachine.IsTransitioning, Is.EqualTo(true));
        yield return null;
        yield return null;
        // ensure that this is still technically running
        Assert.That(stateMachine.IsTransitioning, Is.EqualTo(true));

        // this should have been tested, but ensure that it works
        // between states and not just from null -> StateA
        yield return stateMachine.WaitForTransitionComplete();
        Assert.That(stateMachine.IsTransitioning, Is.EqualTo(false));
        // ensure that StateA has well and truly gone away
        Assert.That(stateMachine.StateA.gameObject.activeInHierarchy, Is.EqualTo(false));

    }
}
