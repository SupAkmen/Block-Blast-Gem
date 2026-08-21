using System;
using Block_Blast.Scripts;
using UnityEngine;

public class GameBoardManager : SingletonBehaviour<GameBoardManager>
{
    [SerializeField] private GridBoard gridBoard;
    [SerializeField] private ShapePool shapePool;
    
    public Action<EGameState> OnGameStateChanged;
    public Action<bool> OnGameOver;

    private EGameState currentState;

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        SetGameState(EGameState.Playing);
    }

    public void SetGameState(EGameState newState)
    {
        currentState = newState;
        OnGameStateChanged?.Invoke(currentState);
    }
    
    public EGameState GetGameState() => currentState;

    public void OnShapePlaced(bool wasLineCleared)
    {
        if (currentState != EGameState.Playing) return;

        if (!wasLineCleared)
        {
            ComboManager.instance.ResetComboTimer();
        }

        if (shapePool != null && !shapePool.HasAvailableShapes())
        {
            SetGameState(EGameState.Failed);
            OnGameOver?.Invoke(false);
        }
    }
}
