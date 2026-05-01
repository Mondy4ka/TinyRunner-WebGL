using PrimeTween;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _cameraOffsetX;

    [Header("Player Settings")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerCollision _playerCollision;

    [Header("Input Settings")]
    [SerializeField] private InputArea _inputArea;
    [SerializeField] private InputArea _startArea;
    [SerializeField] private InputArea _restartArea;
    [SerializeField] private InputArea _continueArea;

    [Header("Line Switch Settings")]
    [SerializeField] private float _topYPosition;
    [SerializeField] private float _downYPosition;

    [Header("Animation Settings")]
    [SerializeField] private float _switchAnimationDuration;
    [SerializeField] private Ease _switchAnimationEase;
    [SerializeField] private AnimationCurve _jumpCurve;
    [SerializeField] private float _jumpDuration;

    [Header("Speed Settings")]
    [SerializeField] private float _startSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _accelerationTime;

    [Header("Level Settings")]
    [SerializeField] private float _chunkLength;
    [SerializeField] private Chunk[] _chunks;
    [SerializeField] private int _chunksCount;

    [Header("Score Settings")]
    [SerializeField] private int _additionScore;
    [SerializeField] private float _additionScoreTime;

    [Header("UI Settings")]
    [SerializeField] private UIManager _uiManager;

    private Player _player;
    private CameraFollow _cameraFollow;
    private SpeedManager _speedManager;
    private LevelManager _levelManager;
    private ScoreManager _scoreManager;

    private GameState _gameState = GameState.Menu;

    private float _traveledDistance = 0;

    private void Awake()
    {
        _player = new(_inputArea, _playerTransform, _topYPosition, _downYPosition, _switchAnimationDuration, _switchAnimationEase, _jumpCurve, _jumpDuration);
        _cameraFollow = new(_playerTransform, _cameraTransform, _cameraOffsetX);
        _levelManager = new(_chunkLength);
        _speedManager = new(_player, _startSpeed, _acceleration, _accelerationTime);
        _scoreManager = new(_additionScore, _additionScoreTime, _uiManager);

        _levelManager.InitializePool(_chunks, _chunksCount);
        _player.Initialize();
        _player.SetMoveSpeed(_startSpeed);
        _playerCollision.Initialize(_player, this);
    }

    private void OnEnable()
    {
        _startArea.OnClick += StartGame;
        _continueArea.OnClick += ContinueGame;
        _restartArea.OnClick += RestartGame;
    }

    private void OnDisable()
    {
        _startArea.OnClick -= StartGame;
        _continueArea.OnClick -= ContinueGame;
        _restartArea.OnClick -= RestartGame;
    }

    private void Update()
    {
        OnGameStateHandler();
    }

    private void StartGame()
    {
        _gameState = GameState.Playing;
        _levelManager.SpawnChunk();

        _uiManager.SetActiveMenuUI(false);
        _uiManager.SetActiveGameUI(true);
        _uiManager.SetActiveGameOverUI(false);
    }

    public void PauseGame()
    {
        _gameState = GameState.Paused;
        _uiManager.SetActivePauseUI(true);
        _uiManager.SetActivePlayingUI(false);
    }

    private void ContinueGame()
    {
        _gameState = GameState.Playing;
        _uiManager.SetActivePauseUI(false);
        _uiManager.SetActivePlayingUI(true);
    }

    public void GameOver()
    {
        _gameState = GameState.GameOver;

        _uiManager.SetActivePauseUI(false);
        _uiManager.SetActivePlayingUI(false);
        _uiManager.SetActiveGameOverUI(true);
    }

    public void RestartGame()
    {
        _gameState = GameState.Menu;
        _player?.MoveToStartPoint();
        _cameraFollow?.Follow();
        _levelManager?.ClearLevel();
        _speedManager?.ResetSpeed();
        _scoreManager?.ResetScore();

        _uiManager.SetActiveMenuUI(true);
        _uiManager.SetActiveGameUI(false);
    }

    private void OnGameStateHandler()
    {
        switch (_gameState)
        {
            case GameState.Menu:
                break;
            case GameState.Playing:
                MovePlayer();
                CameraFollow();
                SpeedUpdate();
                AddScore();
                break;
            case GameState.Paused:
                break;
            case GameState.GameOver:
                break;
        }
    }

    private void AddScore() => _scoreManager.Update();

    private void CameraFollow() => _cameraFollow?.Follow();

    private void SpeedUpdate() => _speedManager?.Update();

    private void MovePlayer()
    {
        if (_player == null) return;

        _player.Move(out float offset);
        _traveledDistance += offset;

        if (_traveledDistance >= _chunkLength)
        {
            _traveledDistance -= _chunkLength;
            _levelManager.SpawnChunk();
        }
    }
}

public enum GameState
{
    Menu,
    Playing,
    Paused,
    GameOver
}