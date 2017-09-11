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
        if(lastState == null)
        {
            GetComponent<CanvasGroup>().alpha = 1;
            gameObject.SetActive(true);
            EnableInteraction();
            yield break;
        }

        DisableInteraction();
        GetComponent<CanvasGroup>().alpha = 0;
        gameObject.SetActive(true);
        yield return FadeIn();
        EnableInteraction();
    }


    public override IEnumerator Exit(IAsyncState nextState)
    {
        DisableInteraction();
        yield return FadeOut();
        gameObject.SetActive(false);
        yield break;
    }


    protected IEnumerator FadeIn(float fadeTime = 0.5f, bool useScaledTime = true)
    {
        float elapsed = 0;
        GetComponent<CanvasGroup>().alpha = 0;
        while(elapsed < fadeTime)
        {
            GetComponent<CanvasGroup>().alpha = fadeInCurve.Evaluate(
                Mathf.Clamp01(elapsed/fadeTime)
            );

            Debug.Log(gameObject.name + " Fade In current value: " + GetComponent<CanvasGroup>().alpha);
            yield return null;
            elapsed += useScaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
        }
        GetComponent<CanvasGroup>().alpha = 1;
    }

    protected IEnumerator FadeOut(float fadeTime = 0.5f, bool useScaledTime = true)
    {
        float elapsed = 0;
        GetComponent<CanvasGroup>().alpha = 1;
        while(elapsed < fadeTime)
        {
            GetComponent<CanvasGroup>().alpha = fadeOutCurve.Evaluate(
                Mathf.Clamp01(elapsed/fadeTime)
            );
            Debug.Log(gameObject.name + " Fade Out current value: " + GetComponent<CanvasGroup>().alpha);
            yield return null;
            elapsed += useScaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
        }
        GetComponent<CanvasGroup>().alpha = 0;
    }
}
