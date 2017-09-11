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

    public ExampleStateMachine StateMachine
    {
        get { return stateMachine; }
        set { stateMachine = value; }
    }

    protected override void Awake()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        DisableInteraction();
        gameObject.SetActive(false);
    }

    public override IEnumerator Enter(IAsyncState lastState)
    {
        DisableInteraction();
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<RectTransform>().SetAsLastSibling();
        gameObject.SetActive(true);
        yield return FadeIn();
        EnableInteraction();
    }


    public override IEnumerator Exit(IAsyncState nextState)
    {
        DisableInteraction();
        yield return stateMachine.WaitForNewStateEnter();
        GetComponent<CanvasGroup>().alpha = 0;
        gameObject.SetActive(false);
        yield break;
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
            GetComponent<CanvasGroup>().alpha = curve.Evaluate(
                Mathf.Clamp01(elapsed/fadeTime)
            );
            yield return null;
            elapsed += useScaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
        }
        GetComponent<CanvasGroup>().alpha = ending;
    }
}
