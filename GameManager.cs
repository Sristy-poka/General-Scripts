using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;



public class GameManager : Singleton<GameManager>
{
    //game states -  pregame, running, paused

    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }



    public GameObject[] SystemPrefabs;
    public Events.EventGameState OnGameStateChanged;


    List<GameObject> _instancedSystemPrefabs;
    List<AsyncOperation> _loadOperations;



    GameState _currentGameState = GameState.PREGAME;



    private string _currentLevelName = string.Empty;


    public GameState CurrentGameState{
        get { return _currentGameState; }
       private set { _currentGameState = value; }
     }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        InstantiateSystemPrefabs();

        UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFaidComplete);

      
    }


    private void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            if (_loadOperations.Count == 0)
            {
                UpdateState(GameState.RUNNING);
            }
        }
        Debug.Log("Load complete..");
    }

    private void OnUnloadOperationComplete(AsyncOperation obj)
    {
        Debug.Log("Unload complete..");
    }

    void HandleMainMenuFaidComplete(bool fadeOut)
    {
        if (!fadeOut)
        {
            UnloadLevel(_currentLevelName);
        }
    }
    void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;

            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;

            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;
            default:
                break;
        }

        OnGameStateChanged.Invoke(_currentGameState, previousGameState);
    }

    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for(int i = 0; i< SystemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if(ao == null)
        {
            Debug.LogError("[GameManager] unable to load level " + levelName);
            return;
        }
        ao.completed += OnLoadOperationComplete;
        _currentLevelName = levelName;
    }

   
    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if(ao == null)
        {
            Debug.LogError("[GameManager] Unable to unload level " + levelName);
            return;
        }
        ao.completed += OnUnloadOperationComplete;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        for(int i = 0; i < _instancedSystemPrefabs.Count; ++i)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }
        _instancedSystemPrefabs.Clear();
    }

   public void StartGame()
    {
        LoadLevel("Main");
    }

    public void TogglePause()
    {
        UpdateState( _currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);   //condition? true : false
    }


    public void RestartGame()
    {
        UpdateState(GameState.PREGAME); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
