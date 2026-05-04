using PrimeTween;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;

    [Header("Camera Settings")]
    [SerializeField] private Transform _cameraTransform;

    [Header("Player Settings")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerCollision _playerCollision;
    [SerializeField] private SpriteRenderer _playerRenderer;

    [Header("Input Settings")]
    [SerializeField] private InputArea _inputArea;
    [SerializeField] private InputArea _startArea;
    [SerializeField] private InputArea _restartArea;
    [SerializeField] private InputArea _continueArea;

    [Header("UI Settings")]
    [SerializeField] private UIManager _uiManager;

    private Player _player;
    private CameraFollow _cameraFollow;
    private SpeedService _speedManager;
    private LevelManager _levelManager;
    private ScoreService _scoreManager;
    private ColorManager _colorManager;

    private GameState _gameState = GameState.Menu;

    private float _traveledDistance = 0;

    private void Awake()
    {
        _player = new(_inputArea,
            _playerTransform,
            _gameConfig.TopYPosition,
            _gameConfig.DownYPosition,
            _gameConfig.SwitchAnimationDuration,
            _gameConfig.SwitchAnimationEase,
            _gameConfig.JumpCurve,
            _gameConfig.JumpDuration);

        _cameraFollow = new(_playerTransform,
            _cameraTransform,
            _gameConfig.CameraOffsetX);

        _levelManager = new(_gameConfig.ChunkLength);

        _speedManager = new(_player,
            _gameConfig.StartSpeed,
            _gameConfig.Acceleration,
            _gameConfig.AccelerationTime);

        _scoreManager = new(_gameConfig.AdditionScore,
            _gameConfig.AdditionScoreTime,
            _uiManager);

        _levelManager.InitializePool(_gameConfig.Chunks,
            _gameConfig.ChunksRepeat,
            out List<SpriteRenderer> renderers);

        _player.Initialize();
        _player.SetMoveSpeed(_gameConfig.StartSpeed);
        _playerCollision.Initialize(_player, this);
        renderers.Add(_playerRenderer);

        _colorManager = new(_gameConfig.ColorTransitionDuration,
            _gameConfig.ColorSwitchDelay,
            _gameConfig.Colors,
            renderers,
            _uiManager.Images);
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
        _uiManager.SetActivePauseUI(false);
        _uiManager.SetActivePlayingUI(true);
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
        _colorManager?.ResetColor();

        _uiManager.SetActiveMenuUI(true);
        _uiManager.SetActiveGameUI(false);
    }

    private void OnGameStateHandler()
    {
        switch (_gameState)
        {
            case GameState.Playing:
                MovePlayer();
                CameraFollow();
                SpeedUpdate();
                AddScore();
                UpdateColor();
                break;
            case GameState.GameOver:
                _uiManager.PulseGameOverTips();
                break;
            case GameState.Paused:
                _uiManager.PulsePauseTips();
                break;
            case GameState.Menu:
                _uiManager.PulseMenuTips();
                break;
        }
    }

    private void UpdateColor() => _colorManager?.Update();

    private void AddScore() => _scoreManager?.Update();

    private void CameraFollow() => _cameraFollow?.Follow();

    private void SpeedUpdate() => _speedManager?.Update();

    private void MovePlayer()
    {
        if (_player == null) return;

        _player.Move(out float offset);
        _traveledDistance += offset;

        if (_traveledDistance >= _gameConfig.ChunkLength)
        {
            _traveledDistance -= _gameConfig.ChunkLength;
            _levelManager?.SpawnChunk();
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