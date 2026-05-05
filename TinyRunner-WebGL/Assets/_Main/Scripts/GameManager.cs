using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player Player {  get; private set; }
    public CameraFollow CameraFollow { get; private set; }
    public SpeedService SpeedService { get; private set; }
    public LevelService LevelService { get; private set; }
    public ScoreService ScoreService { get; private set; }
    public ColorService ColorService { get; private set; }
    public CoinService CoinService { get; private set; }

    [SerializeField] private GameConfig _gameConfig;

    [Header("Camera Settings")]
    [SerializeField] private Transform _cameraTransform;

    [Header("Player Settings")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerCollision _playerCollision;
    [SerializeField] private PlayerVisual _playerVisual;
    [SerializeField] private SpriteRenderer _playerRenderer;

    [Header("Input Settings")]
    [SerializeField] private InputArea _inputArea;
    [SerializeField] private InputArea _startArea;
    [SerializeField] private InputArea _restartArea;
    [SerializeField] private InputArea _continueArea;

    [Header("UI Settings")]
    [SerializeField] private UIManager _uiManager;


    private GameState _gameState = GameState.Menu;

    private float _traveledDistance = 0;

    private void Awake()
    {
        CoinService = new(_uiManager);

        Player = new(_inputArea,
            _playerTransform,
            _gameConfig.TopYPosition,
            _gameConfig.DownYPosition,
            _gameConfig.SwitchAnimationDuration,
            _gameConfig.SwitchAnimationEase,
            _gameConfig.JumpCurve,
            _gameConfig.JumpDuration);

        CameraFollow = new(_playerTransform,
            _cameraTransform,
            _gameConfig.CameraOffsetX);

        LevelService = new(_gameConfig.ChunkLength);

        SpeedService = new(Player,
            _gameConfig.StartSpeed,
            _gameConfig.Acceleration,
            _gameConfig.AccelerationTime);

        ScoreService = new(_gameConfig.AdditionScore,
            _gameConfig.AdditionScoreTime,
            _uiManager);

        LevelService.Initialize(_gameConfig.Chunks,
            _gameConfig.ChunksRepeat,
            out List<SpriteRenderer> renderers);

        Player.Initialize();
        Player.SetMoveSpeed(_gameConfig.StartSpeed);
        _playerCollision.Initialize(this);
        renderers.Add(_playerRenderer);

        ColorService = new(_gameConfig.ColorTransitionDuration,
            _gameConfig.ColorSwitchDelay,
            _gameConfig.Colors,
            renderers,
            _uiManager.Images, _uiManager.GetTMPs());

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
        LevelService.SpawnChunk();
        LevelService.SpawnChunk();

        _playerVisual.ActivateTrail();

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

        _playerVisual.Death();

        _uiManager.SetActivePauseUI(false);
        _uiManager.SetActivePlayingUI(false);
        _uiManager.SetActiveGameOverUI(true);
    }

    public void RestartGame()
    {
        _gameState = GameState.Menu;
        Player?.MoveToStartPoint();
        CameraFollow?.Follow();
        LevelService?.ClearLevel();
        SpeedService?.ResetSpeed();
        ScoreService?.ResetScore();
        ColorService?.ResetColor();

        _playerVisual.Revert();
        _playerVisual.DeactivateTrail();

        _uiManager.SetActiveMenuUI(true);
        _uiManager.SetActiveGameUI(false);
    }

    private void OnGameStateHandler()
    {
        switch (_gameState)
        {
            case GameState.Playing:
                MovePlayer();
                CameraFollowing();
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

    private void UpdateColor() => ColorService?.Update();

    private void AddScore() => ScoreService?.Update();

    private void CameraFollowing() => CameraFollow?.Follow();

    private void SpeedUpdate() => SpeedService?.Update();

    private void MovePlayer()
    {
        if (Player == null) return;

        Player.Move(out float offset);
        _traveledDistance += offset;

        if (_traveledDistance >= _gameConfig.ChunkLength)
        {
            _traveledDistance -= _gameConfig.ChunkLength;
            LevelService?.SpawnChunk();
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