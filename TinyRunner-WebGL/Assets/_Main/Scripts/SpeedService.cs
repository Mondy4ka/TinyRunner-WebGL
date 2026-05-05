using UnityEngine;

public class SpeedService
{
    public float CurrentSpeed
    {
        get => _currentSpeed;
        private set
        {
            _currentSpeed = value;
            _player.SetMoveSpeed(_currentSpeed);
            Debug.Log($"Speed changed from {_currentSpeed} to {value} at Time: {Time.time}");
        }
    }

    private readonly Player _player;
    private readonly float _startSpeed;
    private readonly float _acceleration;
    private readonly float _accelerationTime;

    private float _accelerationTimer;
    private float _currentSpeed;

    public SpeedService(Player player, float startSpeed, float acceleration, float accelerationTime)
    {
        _player = player;
        _startSpeed = startSpeed;
        _acceleration = acceleration;
        _accelerationTime = accelerationTime;

        ResetSpeed();
    }

    public void Update()
    {
        _accelerationTimer += Time.deltaTime;

        if (_accelerationTimer >= _accelerationTime)
        {
            _accelerationTimer = 0;
            UpdateSpeed();
        }
    }

    public void ResetSpeed() => CurrentSpeed = _startSpeed;

    private void UpdateSpeed() => CurrentSpeed += _acceleration;
}