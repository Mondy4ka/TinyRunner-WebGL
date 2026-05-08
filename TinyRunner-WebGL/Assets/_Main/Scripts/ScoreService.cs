using UnityEngine;
using YG;

public class ScoreService
{
    public int CurrentScore
    {
        get => _currentScore;
        private set
        {
            _currentScore = Mathf.Max(value, 0);
            _uiManager.UpdateScoreText(_currentScore);
        }
    }

    public int BestScore
    {
        get => _bestScore;
        private set
        {
            _bestScore = Mathf.Max(value, _bestScore);
            _uiManager.UpdateBestScoreText(_bestScore);

            YG2.saves.BestScore = _bestScore;
            YG2.SaveProgress();
        }
    }

    private readonly UIManager _uiManager;
    private readonly int _additionScore;
    private readonly float _additionTime;

    private float _additionTimer;
    private int _currentScore;
    private int _bestScore;

    public ScoreService(int additionScore, float additionTime, UIManager uiManager)
    {
        _additionScore = additionScore;
        _additionTime = additionTime;
        _uiManager = uiManager;
    }

    public void LoadBestScore(int bestScore) => BestScore = bestScore;

    public void Update()
    {
        _additionTimer += Time.deltaTime;

        if (_additionTimer >= _additionTime)
        {
            _additionTimer = 0;
            AddScore();
        }
    }

    private void AddScore() => CurrentScore += _additionScore;

    public void UpdateBestScore()
    {
        if (_currentScore > BestScore)
            BestScore = _currentScore;
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        _additionTimer = 0;
    }
}