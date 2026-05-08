using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _bestScoreText;
    [SerializeField] private TMP_Text _coinsText;

    [SerializeField] private float _pulseSpeed;
    [SerializeField] private TMP_Text _continueTipText;
    [SerializeField] private TMP_Text _exitTipText;
    [SerializeField] private TMP_Text _startTipText;
    [SerializeField] private TMP_Text _restartTipText;

    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _playingUI;
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _shopUI;

    public void PulsePauseTips()
    {
        float alpha = Mathf.PingPong(Time.time * _pulseSpeed, 1.00f);

        Color newColor1 = _continueTipText.color;
        Color newColor2 = _exitTipText.color;

        newColor1.a = alpha;
        newColor2.a = alpha;

        _continueTipText.color = newColor1;
        _exitTipText.color = newColor2;
    }

    public void PulseGameOverTips()
    {
        float alpha = Mathf.PingPong(Time.time * _pulseSpeed, 1.00f);

        Color newColor1 = _restartTipText.color;

        newColor1.a = alpha;

        _restartTipText.color = newColor1;
    }

    public void PulseMenuTips()
    {
        float alpha = Mathf.PingPong(Time.time * _pulseSpeed, 1.00f);

        Color newColor1 = _startTipText.color;

        newColor1.a = alpha;

        _startTipText.color = newColor1;
    }

    public void UpdateCoinsText(int coins) => _coinsText.SetText($"Coins: {coins}");

    public void UpdateScoreText(int newScore) => _scoreText.SetText($"Score: {newScore}");

    public void UpdateBestScoreText(int newBestScore) => _bestScoreText.SetText($"Best Score: {newBestScore}");

    public void SetActiveMenuUI(bool isActive) => _menuUI.SetActive(isActive);

    public void SetActiveGameUI(bool isActive) => _gameUI.SetActive(isActive);

    public void SetActivePlayingUI(bool isActive) => _playingUI.SetActive(isActive);

    public void SetActivePauseUI(bool isActive) => _pauseUI.SetActive(isActive);

    public void SetActiveGameOverUI(bool isActive) => _gameOverUI.SetActive(isActive);

    public void SetActiveShopUI(bool isActive) => _shopUI.SetActive(isActive);
}