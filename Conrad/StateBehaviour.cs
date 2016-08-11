using UnityEngine;
using System.Collections;

namespace Kahn.Unity.StateMachine
{
    public class StateBehaviour : MonoBehaviour, IUpdateState
    {
        public virtual void WillEnter(IState newState)
        {
        }

        public virtual void Enter(IState newState)
        {
            this.enabled = true;
        }

        public virtual void DidEnter(IState newState)
        {
        }

        public virtual void WillExit(IState newState)
        {
        }

        public virtual void Exit(IState newState)
        {
            this.enabled = false;
        }

        public virtual void DidExit(IState newState)
        {
        }

        public virtual void Update()
        {
            Update(Time.deltaTime);
        }

        public virtual void LateUpdate()
        {
            LateUpdate(Time.deltaTime);
        }

        public virtual void Update(float dt)
        {

        }

        public virtual void LateUpdate(float dt)
        {

        }
    }
}
