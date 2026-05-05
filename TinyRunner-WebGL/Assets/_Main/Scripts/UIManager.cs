using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<Image> Images => _images;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _bestScoreText;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private TMP_Text _gameOverHeaderText;
    [SerializeField] private TMP_Text _pauseHeaderText;

    [SerializeField] private float _pulseSpeed;
    [SerializeField] private TMP_Text _continueTipText;
    [SerializeField] private TMP_Text _exitTipText;
    [SerializeField] private TMP_Text _startTipText;
    [SerializeField] private TMP_Text _restartTipText;

    [SerializeField] private List<Image> _images;

    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _playingUI;
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject _gameOverUI;

    public List<TMP_Text> GetTMPs() => new() { _pauseHeaderText, _gameOverHeaderText, _scoreText, _bestScoreText, _coinsText, _continueTipText, _exitTipText, _startTipText, _restartTipText };

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
}