using UnityEngine;

public class ScoreManager
{
    private readonly UIManager _uiManager;
    private readonly int _additionScore;
    private readonly float _additionTime;

    private float _additionTimer;
    private int _currentScore;
    private int _bestScore;

    public ScoreManager(int additionScore, float additionTime, UIManager uiManager)
    {
        _additionScore = additionScore;
        _additionTime = additionTime;
        _uiManager = uiManager;
    }

    public void Update()
    {
        _additionTimer += Time.deltaTime;

        if (_additionTimer >= _additionTime)
        {
            _additionTimer = 0;
            AddScore();
        }
    }

    public void ResetScore()
    {
        _currentScore = 0;
        _additionTimer = 0;
        _uiManager.UpdateScoreText(_currentScore);
    }

    private void AddScore()
    {
        _currentScore += _additionScore;
        _uiManager.UpdateScoreText(_currentScore);

        if (_currentScore > _bestScore)
        {
            SetBestScore(_currentScore);
        }
    }

    private void SetBestScore(int newBest)
    {
        if (newBest < _bestScore) return;

        _bestScore = newBest;
        _uiManager.UpdateBestScoreText(_bestScore);
    }
}