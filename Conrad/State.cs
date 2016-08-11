namespace Kahn.Unity.StateMachine
{
    public interface IState
    {
        void WillEnter(IState previousState);
        void Enter(IState previousState);
        void DidEnter(IState previousState);

        void WillExit(IState newState);
        void Exit(IState newState);
        void DidExit(IState newState);
    }

    public interface IUpdateState : IState
    {
        void Update(float dt);
        void LateUpdate(float dt);
    }

    public class State : IState
    {
        public virtual void WillEnter(IState previousState) { }
        public virtual void Enter(IState previousState) { }
        public virtual void DidEnter(IState previousState) { }

        public virtual void WillExit(IState newState) { }
        public virtual void Exit(IState newState) { }
        public virtual void DidExit(IState newState) { }
    }

    public class UpdateState : State, IUpdateState
    {
        public virtual void Update(float dt)
        {

        }

        public virtual void LateUpdate(float dt)
        {

        }
    }
}
