using UnityEngine;

public class ScoreService
{
    public int CurrentScore
    {
        get => _currentScore;
        private set
        {
            _currentScore = Mathf.Max(value, 0);
            _uiManager.UpdateScoreText(_currentScore);

            if (_currentScore > BestScore)
                BestScore = _currentScore;
        }
    }

    public int BestScore
    {
        get => _bestScore;
        private set
        {
            _bestScore = Mathf.Max(value, _bestScore);
            _uiManager.UpdateBestScoreText(_bestScore);
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

    public void ResetScore()
    {
        CurrentScore = 0;
        _additionTimer = 0;
    }
}