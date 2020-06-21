using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button ResumeBtn;
    [SerializeField] Button RestartBtn;
    [SerializeField] Button QuitBtn;


    private void Start()
    {
        ResumeBtn.onClick.AddListener(HandleResumeClicked);
        RestartBtn.onClick.AddListener(HandleRestartClicked);
        QuitBtn.onClick.AddListener(HandleQuitClicked);
    }

    private void HandleRestartClicked()
    {
        GameManager.Instance.RestartGame();
    }

    private void HandleQuitClicked()
    {
        GameManager.Instance.QuitGame();
    }

    private void HandleResumeClicked()
    {
        GameManager.Instance.TogglePause();
    }
}
