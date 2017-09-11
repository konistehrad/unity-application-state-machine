using System.Collections;

namespace PillowTalk.StateMachine
{
    public class BaseAsyncState : IAsyncState
    {
        /// <inheritdoc/>
        public virtual IEnumerator Enter(IAsyncState previousState) { yield break; }
        /// <inheritdoc/>
        public virtual IEnumerator Exit(IAsyncState newState) { yield break; }
        /// <inheritdoc/>
        public virtual void Interrupt() {}
    }
}
