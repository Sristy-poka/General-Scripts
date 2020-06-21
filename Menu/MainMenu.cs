using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    //track animation component .....1
    // track animation clips for fade in / out ....2
    // functions that can rcv animation events
    // functions to play fade in / out animations

    [SerializeField] Animation _mainMenuAnimator;    //decorators, inside square bracket
    [SerializeField] AnimationClip _fadeOutAnimation;
    [SerializeField] AnimationClip _fadeInAnimation;

    public Events.EventFadeComplete OnMainMenuFadeComplete;


    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandelGameStateChanged);
    }

 

    //1,2
    public void OnFadeOutComplete()
    {
        Debug.LogWarning("Fade out complete");
        OnMainMenuFadeComplete.Invoke(true);
        
    }
    public void OnFadeInComplete()
    {
        Debug.LogWarning("Fade in complete");
        UIManager.Instance.SetDummyCameraActive(true);
        OnMainMenuFadeComplete.Invoke(false);
    }

    private void HandelGameStateChanged(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
    {
        if (previousGameState == GameManager.GameState.PREGAME && currentGameState == GameManager.GameState.RUNNING)
        {
            FadeOut();
        }

        if(previousGameState != GameManager.GameState.PREGAME && currentGameState == GameManager.GameState.PREGAME)
        {
            FadeIn();
        }
    }
    public void FadeIn()
    {
        _mainMenuAnimator.Stop(); //anything currently playing stops
        _mainMenuAnimator.clip = _fadeInAnimation;
        _mainMenuAnimator.Play();

    }

    public void FadeOut()
    {
        UIManager.Instance.SetDummyCameraActive(false);
        _mainMenuAnimator.Stop(); //anything currently playing stops
        _mainMenuAnimator.clip = _fadeOutAnimation;
        _mainMenuAnimator.Play();
    }

}
