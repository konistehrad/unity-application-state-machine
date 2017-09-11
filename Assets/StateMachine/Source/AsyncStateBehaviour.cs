using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

#if USING_INCONTROL
using InControl;
#endif

namespace PillowTalk.StateMachine
{
    public class AsyncStateBehaviour : MonoBehaviour, IAsyncState
    {
        public virtual bool InputBlocked
        {
            get; 
            set;
        }

        /// <summary>
        /// Standard Unity Awake message.
        /// </summary>
        protected virtual void Awake()
        {
            gameObject.SetActive(false);
            DisableInteraction();
        }

        /// <inheritdoc/>
        public virtual IEnumerator Enter(IAsyncState lastState)
        {
            gameObject.SetActive(true);
            EnableInteraction();
            yield break;
        }

        /// <inheritdoc/>
        public virtual IEnumerator Exit(IAsyncState nextState)
        {
            gameObject.SetActive(false);
            DisableInteraction();
            yield break;
        }

        /// <summary>
        /// Standard Unity Update message. Note: this method will be called even if this is not the
        /// active state! Check using <c>StateMachine.State</c> with a reference to <c>this</c> if required.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Standard Unity LateUpdate message. Note: this method will be called even if this is not the
        /// active state! Check using <c>StateMachine.State</c> with a reference to <c>this</c> if required.
        /// </summary>
        public virtual void LateUpdate()
        {
        }

        /// <inheritdoc/>
        public virtual void Interrupt()
        {
        }

        /// <summary>
        /// Marks this state as being active, but not eligible for interaction. By default, this will try
        /// to set an attached <c>CanvasGroup.interactable</c> to <c>false</c> and mask out
        /// the concenience <c>InputDown</c> and <c>InputHeld</c> properties.
        /// </summary>
        protected virtual void DisableInteraction()
        {
            InputBlocked = true;
            var canvasGroup = this.GetComponent<CanvasGroup>();
            if (canvasGroup)
            {
                canvasGroup.interactable = false;
            }
        }

        /// <summary>
        /// Marks this state as being eligible for interaction. By default, this will try
        /// to set an attached <c>CanvasGroup.interactable</c> to <c>true</c> and resume proper output of
        /// the concenience <c>InputDown</c> and <c>InputHeld</c> properties.
        /// </summary>
        protected virtual void EnableInteraction()
        {
            InputBlocked = false;
            var canvasGroup = this.GetComponent<CanvasGroup>();
            if (canvasGroup)
            {
                canvasGroup.interactable = true;
            }
        }

        protected virtual bool InputDown
        {
            get
            {
                bool submitWasPressed = false;
                bool cancelIsPressed = false;

                if(InputBlocked) return false;

                #if USING_INCONTROL
                // keyboard and controller parsing ...
                InControlInputModule incontrolInputModule = EventSystem.current.currentInputModule as InControlInputModule;
                if (incontrolInputModule)
                {
                    submitWasPressed = incontrolInputModule.SubmitAction.WasPressed || Input.GetMouseButtonDown(0);
                    cancelIsPressed = incontrolInputModule.CancelAction.IsPressed;
                }
                else
                #endif
                {
                    StandaloneInputModule currentStandaloneInputModule = EventSystem.current.currentInputModule as StandaloneInputModule;
                    submitWasPressed = Input.GetButtonDown(currentStandaloneInputModule.submitButton);            
                    cancelIsPressed = Input.GetButton(currentStandaloneInputModule.cancelButton);
                }
                
                return submitWasPressed || cancelIsPressed;
            }
        }

        protected virtual bool InputHeld
        {
            get
            {
                bool submitWasPressed = false;
                bool cancelIsPressed = false;

                if(InputBlocked) return false;

                #if USING_INCONTROL
                // keyboard and controller parsing ...
                InControlInputModule incontrolInputModule = EventSystem.current.currentInputModule as InControlInputModule;
                if (incontrolInputModule)
                {
                    submitWasPressed = incontrolInputModule.SubmitAction.IsPressed || Input.GetMouseButton(0);
                    cancelIsPressed = incontrolInputModule.CancelAction.IsPressed;
                }
                else
                #endif
                {
                    StandaloneInputModule currentStandaloneInputModule = EventSystem.current.currentInputModule as StandaloneInputModule;
                    submitWasPressed = Input.GetButtonDown(currentStandaloneInputModule.submitButton);            
                    cancelIsPressed = Input.GetButton(currentStandaloneInputModule.cancelButton);
                }
                
                return submitWasPressed || cancelIsPressed;
            }
        }
    }
}
