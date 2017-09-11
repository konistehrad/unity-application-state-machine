using System.Collections;

namespace PillowTalk.StateMachine
{
    public interface IAsyncState
    {
        /// <summary>
        /// Called by a state machine, this method signals that the async tasks required to make this
        /// state active should be run.
        /// </summary>
        /// <param name="previousState">A reference to the state we are replacing as active state</param>
        /// <returns>A continuation representing the async tasks required to enter this state</returns>
        IEnumerator Enter(IAsyncState previousState);

        /// <summary>
        /// Called by a state machine, this method signals that the async tasks required to make this
        /// state no longer active should be run.
        /// </summary>
        /// <param name="previousState">A reference to the state that is replacing us as active</param>
        /// <returns>A continuation representing the async tasks required to exit this state</returns>
        IEnumerator Exit(IAsyncState newState);
		
        /// <summary>
        /// Attempt to stop the async exit or enter action of this state
        /// </summary>
        void Interrupt();
    }
}