using System.Collections;
using System.Collections.Generic;
using PillowTalk.StateMachine;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Image))]
public class ExampleBaseState : AsyncStateBehaviour 
{
    [SerializeField] private ExampleStateMachine stateMachine;
    [SerializeField] private AnimationCurve fadeInCurve;
    [SerializeField] private AnimationCurve fadeOutCurve;

    private bool interrupted;

    public ExampleStateMachine StateMachine
    {
        get { return stateMachine; }
        set { stateMachine = value; }
    }

    /// <inheritdoc/>
    protected override void Awake()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        DisableInteraction();
        gameObject.SetActive(false);
    }

    /// <inheritdoc/>
    public override IEnumerator Enter(IAsyncState lastState)
    {
        interrupted = false;
        DisableInteraction();
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<RectTransform>().SetAsLastSibling();
        gameObject.SetActive(true);
        yield return FadeIn();
        EnableInteraction();
    }

    /// <inheritdoc/>
    public override IEnumerator Exit(IAsyncState nextState)
    {
        DisableInteraction();
        yield return stateMachine.WaitForNewStateEnter();
        GetComponent<CanvasGroup>().alpha = 0;
        gameObject.SetActive(false);
        interrupted = false;
        yield break;
    }

    /// <inheritdoc/>
    public override void Interrupt()
    {
        // ensure that we're transitioning to this state; otherwise this flag won't be reset
        if(stateMachine.IsTransitioning && stateMachine.State == this)
        {
            interrupted = true;
        }
    }

    protected IEnumerator FadeIn(float fadeTime = 0.5f, bool useScaledTime = true)
    {
        return RunFade(0, 1, fadeInCurve, fadeTime, useScaledTime);
    }

    protected IEnumerator FadeOut(float fadeTime = 0.5f, bool useScaledTime = true)
    {
        return RunFade(1, 0, fadeOutCurve, fadeTime, useScaledTime);
    }

    private IEnumerator RunFade(float starting, float ending, AnimationCurve curve, float fadeTime, bool useScaledTime)
    {
        float elapsed = 0;
        GetComponent<CanvasGroup>().alpha = starting;
        while(elapsed < fadeTime)
        {
            if(interrupted)
            {
                interrupted = false;
                break;
            }

            GetComponent<CanvasGroup>().alpha = curve.Evaluate(
                Mathf.Clamp01(elapsed/fadeTime)
            );

            yield return null;

            if(interrupted)
            {
                interrupted = false;
                break;
            }

            elapsed += useScaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
        }

        GetComponent<CanvasGroup>().alpha = ending;
    }
}
