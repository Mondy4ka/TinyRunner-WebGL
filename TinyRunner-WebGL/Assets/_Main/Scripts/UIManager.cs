using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _bestScoreText;

    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _playingUI;
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private GameObject _gameOverUI;

    public void UpdateScoreText(int newScore) => _scoreText.SetText($"Score: {newScore}");

    public void UpdateBestScoreText(int newBestScore) => _bestScoreText.SetText($"Best Score: {newBestScore}");

    public void SetActiveMenuUI(bool isActive) => _menuUI.SetActive(isActive);

    public void SetActiveGameUI(bool isActive) => _gameUI.SetActive(isActive);

    public void SetActivePlayingUI(bool isActive) => _playingUI.SetActive(isActive);

    public void SetActivePauseUI(bool isActive) => _pauseUI.SetActive(isActive);

    public void SetActiveGameOverUI(bool isActive) => _gameOverUI.SetActive(isActive);
}