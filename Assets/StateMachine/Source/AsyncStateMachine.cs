using UnityEngine;
using System.Collections;

namespace PillowTalk.StateMachine
{
    /// <summary>
    /// A default implementation of AsyncStateMachine that uses generic
    /// <c>IAsyncState</c>s as the state type.
    /// </summary>
    public class AsyncStateMachine : AsyncStateMachine<IAsyncState>
    {
    }

    /// <summary>
    /// The base class for all async state machines. Can be customized to use different
    /// base classes for states, so long as they are all guaranteed to inherit from
    /// <c>IAsyncState</c>.
    /// </summary>
    public class AsyncStateMachine<TState> : MonoBehaviour where TState : IAsyncState
    {
        private bool oldStateTransitionComplete = false;
        private bool newStateTransitionComplete = false;

        /// <summary>
        /// Returns <c>true</c> if this state machine is suffering an async state transition,
        /// otherwise <c>false</c>. 
        /// </summary>
        public bool IsTransitioning 
        { 
            get; private set; 
        }

        /// <summary>
        /// Returns the current state of the state machine. Note: if you call
        /// <c>TransitionToState</c>, this will show the incoming state, not the
        /// previous state.
        /// </summary>
        public TState State
        {
            get; private set;
        }

        /// <summary>
        /// Returns the state of the state machine used before the current. May return <c>null</c>.
        /// </summary>
        public TState PreviousState
        {
            get; private set;
        }

        /// <summary>
        /// Returns the time, in seconds, since the last state transition. Affected by <c>Time.timeScale</c>.
        /// </summary>
        public float TimeInState 
        { 
            get; private set; 
        }

        /// <summary>
        /// Returns the real time, in seconds, since the last state transition. 
        /// Unaffected by <c>Time.timeScale</c>.
        /// </summary>
        public float TimeInStateUnscaled
        { 
            get; private set; 
        }

        /// <summary>
        /// Kicks off an asynchronous state transition.
        /// </summary>
        /// <param name="state">The state to transition to. Cannot be <c>null</c>.</param>
        /// <param name="force">If <c>true</c>, allow a state to transition to itself. If <c>false</c>, self-transitions will result in a no-op.</param>
        /// <param name="finished">An optional callback to raise after the transition is complete. Pass <c>null</c> if unwanted.</param>
        public virtual void TransitionToState(TState state, bool force = false, System.Action finished = null)
        {
            // no we're not doing that
            if( state == null )
            {
                throw new System.ArgumentNullException("state");
            }

            // don't self-transition unless explicitly asked
            if( state.Equals(State) && !force )
            {
                return;
            }

            if( IsTransitioning )
            {
                throw new System.InvalidOperationException("Cannot issue transition before one completes");
            }

            PreviousState = State;
            State = state;

            StartCoroutine(DoTransition(State, PreviousState, finished));
        }

        /// <summary>
        /// Try to force a transition to complete immediately
        /// </summary>
        public void Interrupt()
        {
            if (!IsTransitioning)
            {
                return;
            }

            if (PreviousState != null)
            {
                PreviousState.Interrupt();
            }

            if (State != null)
            {
                State.Interrupt();
            }
        }

        /// <returns>A coroutine representing the completion of the last state's transition.</returns>
        public Coroutine WaitForLastStateExit()
        {
            return StartCoroutine(WaitForLastStateExitInternal());
        }

        /// <returns>A coroutine representing the completion of the new state's transition</returns>
        public Coroutine WaitForNewStateEnter()
        {
            return StartCoroutine(WaitForNewStateEnterInternal());
        }

        public Coroutine WaitForTransitionComplete()
        {
            return StartCoroutine(WaitForTransitionCompleteInternal());
        }

        /// <summary>
        /// Standard Unity Awake message.
        /// </summary>
        protected virtual void Awake()
        {
            TimeInState = 0;
            TimeInStateUnscaled = 0;
        }

        /// <summary>
        /// Standard Unity Start message.
        /// </summary>
        protected virtual void Start()
        {

        }

        /// <summary>
        /// Standard Unity FixedUpdate message.
        /// </summary>
        protected virtual void FixedUpdate()
        {

        }

        /// <summary>
        /// Standard Unity Update message.
        /// </summary>
        protected virtual void Update()
        {
            TimeInState += Time.deltaTime;
            TimeInStateUnscaled += Time.unscaledDeltaTime;
        }

        /// <summary>
        /// Standard Unity LateUpdate message.
        /// </summary>
        protected virtual void LateUpdate()
        {
            
        }

        /// <summary>
        /// The method responsible for actually preforming the state transitions before old and new.
        /// </summary>
        /// <param name="newState">A reference to the state to which we are transitioning.</param>
        /// <param name="oldState">A reference to the state previously held before the new one.</param>
        /// <param name="finished">An optional callback to raise when both new and previous state transitions are complete.</param>
        /// <returns>A continuation representing both the new state entering and the previous state exiting.</returns>
        protected IEnumerator DoTransition(TState newState, TState oldState, System.Action finished)
        {
            TimeInState = 0;
            TimeInStateUnscaled = 0;
            IsTransitioning = true;

            Coroutine exitContinuation = null;
            Coroutine enterContinuation;
            oldStateTransitionComplete = false;
            newStateTransitionComplete = false;

            if (oldState != null)
            {
                exitContinuation = StartCoroutine(WaitForExit());
            }
            else
            {
                oldStateTransitionComplete = true;
            }

            enterContinuation = StartCoroutine(WaitForEnter());

            if(exitContinuation != null)
            {
                yield return exitContinuation;
            }   
            yield return enterContinuation;

            IsTransitioning = false;
            
            if( finished != null )
            {
                finished();
            }
        }

        private IEnumerator WaitForLastStateExitInternal()
        {
            while (!oldStateTransitionComplete)
            {
                yield return null;
            }
        }

        private IEnumerator WaitForNewStateEnterInternal()
        {
            while (!newStateTransitionComplete)
            {
                yield return null;
            }
        }

        private IEnumerator WaitForTransitionCompleteInternal()
        {
            while (IsTransitioning)
            {
                yield return null;
            }
        }

        private IEnumerator WaitForExit()
        {
            yield return StartCoroutine(PreviousState.Exit(State));
            oldStateTransitionComplete = true;
        }

        private IEnumerator WaitForEnter()
        {
            yield return StartCoroutine(State.Enter(PreviousState));
            newStateTransitionComplete = true;
        }
    }
}
