using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] MainMenu _mainMenu;
    [SerializeField] Camera _dummyCamera;
    [SerializeField] PauseMenu _pauseMenu;

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    private void Start()
    {
        _mainMenu.OnMainMenuFadeComplete.AddListener(handleMainMenuFadeComplete);
        GameManager.Instance.OnGameStateChanged.AddListener(HandelGameStateChanged);
    }

    private void handleMainMenuFadeComplete(bool fadeOut)
    {
        OnMainMenuFadeComplete.Invoke(fadeOut);
    }

    private void HandelGameStateChanged(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
    {
        _pauseMenu.gameObject.SetActive(currentGameState == GameManager.GameState.PAUSED);
    }


    public void OnClickStartButton()
    {
        if(GameManager.Instance.CurrentGameState != GameManager.GameState.PREGAME)
        {
            return;
        }
       // _mainMenu.FadeOut();
        GameManager.Instance.StartGame();
    }

    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }

    private void Update()
    {
        
    }
}
