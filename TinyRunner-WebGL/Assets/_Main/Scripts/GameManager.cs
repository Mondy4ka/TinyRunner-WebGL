using System.Collections;
using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    public Player Player { get; private set; }
    public CameraService CameraFollow { get; private set; }
    public SpeedService SpeedService { get; private set; }
    public LevelService LevelService { get; private set; }
    public ScoreService ScoreService { get; private set; }
    public CoinService CoinService { get; private set; }
    public ShopService ShopService { get; private set; }
    public SoundManager SoundManager { get; private set; }
    public SaveService SaveService { get; private set; }

    [Header("Shop Settings")]
    [SerializeField] private Transform _shopCellsParent;
    [SerializeField] private InputArea _openShopArea;
    [SerializeField] private InputArea _closeShopArea;

    [Header("Game Config")]
    [SerializeField] private GameConfig _gameConfig;

    [Header("Camera Settings")]
    [SerializeField] private Transform _cameraTransform;

    [Header("Player Settings")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerCollision _playerCollision;
    [SerializeField] private PlayerVisual _playerVisual;

    [Header("Input Settings")]
    [SerializeField] private InputArea _inputArea;
    [SerializeField] private InputArea _startArea;
    [SerializeField] private InputArea _restartArea;
    [SerializeField] private InputArea _continueArea;

    [Header("UI Settings")]
    [SerializeField] private UIManager _uiManager;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource _sfxSource;

    private int _restartCount = 0;
    private float _traveledDistance = 0;
    private GameState _gameState = GameState.Menu;

    private void Awake()
    {

        SoundManager = new(_sfxSource,
            _gameConfig.SwitchLineSound,
            _gameConfig.JumpSound,
            _gameConfig.CoinSound,
            _gameConfig.DeathSound,
            _gameConfig.PurchaseSound);

        Player = new(_inputArea,
            _playerTransform,
            _gameConfig.TopYPosition,
            _gameConfig.DownYPosition,
            _gameConfig.SwitchAnimationDuration,
            _gameConfig.SwitchAnimationEase,
            _gameConfig.JumpCurve,
            _gameConfig.JumpDuration,
            SoundManager);

        CameraFollow = new(_playerTransform,
            _cameraTransform,
            _gameConfig.CameraOffsetX);

        LevelService = new(_gameConfig.ChunkLength, _gameConfig.ActiveChunksCount);

        SpeedService = new(Player,
            _gameConfig.StartSpeed,
            _gameConfig.Acceleration,
            _gameConfig.AccelerationTime);

        ScoreService = new(_gameConfig.AdditionScore,
            _gameConfig.AdditionScoreTime,
            _uiManager);

        CoinService = new(_uiManager,
            _gameConfig.CoinsRewardAdv);

        ShopService = new(_gameConfig.Skins,
            _gameConfig.SkinCellPrefab,
            _shopCellsParent,
            _playerVisual,
            CoinService, SoundManager);

        SaveService = new(CoinService, ScoreService, ShopService);

        LevelService.Initialize(_gameConfig.Chunks, _gameConfig.ChunksRepeat);
        ShopService.Initialize();
        Player.Initialize();
        Player.SetMoveSpeed(_gameConfig.StartSpeed);
        _playerCollision.Initialize(this);
        _playerVisual.Initialize(SoundManager);
        CoinService.Initialize();

        StartCoroutine(LoadSaves());
    }

    private IEnumerator LoadSaves()
    {
        while (YG2.isSDKEnabled == false) yield return null;

        SaveService.LoadCoins();
        SaveService.LoadBestScore();
        ShopService.LoadSkins(YG2.saves.UnlockedSkins, YG2.saves.EquippedSkin);
    }

    private void OnEnable()
    {
        _startArea.OnClick += StartGame;
        _continueArea.OnClick += ContinueGame;
        _restartArea.OnClick += RestartGame;
        _openShopArea.OnClick += OpenShop;
        _closeShopArea.OnClick += CloseShop;
        YG2.onRewardAdv += OnRewardedAdv;
    }

    private void OnDisable()
    {
        _startArea.OnClick -= StartGame;
        _continueArea.OnClick -= ContinueGame;
        _restartArea.OnClick -= RestartGame;
        _openShopArea.OnClick -= OpenShop;
        _closeShopArea.OnClick -= CloseShop;
        YG2.onRewardAdv -= OnRewardedAdv;
    }

    private void Update()
    {
        OnGameStateHandler();
    }

    public void CallRewardedAdv(string id) => YG2.RewardedAdvShow(id);

    private void StartGame()
    {
        _gameState = GameState.Playing;
        LevelService.SpawnChunk(true);
        LevelService.SpawnChunk();

        _playerVisual.ActivateTrail();

        _uiManager.SetActiveMenuUI(false);
        _uiManager.SetActiveGameUI(true);
        _uiManager.SetActivePauseUI(false);
        _uiManager.SetActivePlayingUI(true);
        _uiManager.SetActiveGameOverUI(false);
        _uiManager.UpdateScoreText(0);
    }

    public void PauseGame()
    {
        _gameState = GameState.Paused;

        _playerVisual.DeactivateTrail();

        _uiManager.SetActivePauseUI(true);
        _uiManager.SetActivePlayingUI(false);
    }

    private void OnRewardedAdv(string id)
    {
        if (id != "Continue") return;

        ContinueGame();
        _playerVisual.Revert();
        _uiManager.SetActiveGameOverUI(false);
    }

    private void ContinueGame()
    {
        _gameState = GameState.Playing;

        _playerVisual.ActivateTrail();

        _uiManager.SetActivePauseUI(false);
        _uiManager.SetActivePlayingUI(true);
    }

    public void GameOver()
    {
        _gameState = GameState.GameOver;

        _playerVisual.Death();
        ScoreService.UpdateBestScore();

        _uiManager.SetActivePauseUI(false);
        _uiManager.SetActivePlayingUI(false);
        _uiManager.SetActiveGameOverUI(true);
    }

    public void RestartGame()
    {
        _restartCount++;

        _gameState = GameState.Menu;
        ScoreService.UpdateBestScore();
        Player?.MoveToStartPoint();
        CameraFollow?.Follow();
        LevelService?.ClearLevel();
        SpeedService?.ResetSpeed();
        ScoreService?.ResetScore();

        _playerVisual.Revert();
        _playerVisual.DeactivateTrail();

        _uiManager.SetActiveMenuUI(true);
        _uiManager.SetActiveGameUI(false);
        _uiManager.UpdateBestScoreText(ScoreService.BestScore);


        if (_restartCount >= _gameConfig.RestartCountAdv)
        {
            _restartCount = 0;
            YG2.InterstitialAdvShow();
        }
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

    private void OpenShop()
    {
        if (_gameState != GameState.Menu) return;

        _uiManager.SetActiveMenuUI(false);
        _uiManager.SetActiveShopUI(true);
    }

    private void CloseShop()
    {
        _uiManager.SetActiveMenuUI(true);
        _uiManager.SetActiveShopUI(false);

        _uiManager.UpdateBestScoreText(ScoreService.BestScore);
    }
}

public enum GameState
{
    Menu,
    Playing,
    Paused,
    GameOver
}