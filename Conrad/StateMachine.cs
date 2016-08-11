using UnityEngine;
using System.Collections;

namespace Kahn.Unity.StateMachine
{
    public abstract class StateMachine<TState> : MonoBehaviour where TState : IState
    {
        public TState State
        {
            get; private set;
        }

        public TState PreviousState
        {
            get; private set;
        }

        public virtual void TransitionToState(TState state, bool force = false)
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

            PreviousState = State;
            State = state;

            PreviousState.WillExit(State);
            State.WillEnter(PreviousState);

            PreviousState.Exit(State);
            State.Enter(PreviousState);

            PreviousState.DidExit(State);
            State.DidEnter(PreviousState);
        }
    }
}
