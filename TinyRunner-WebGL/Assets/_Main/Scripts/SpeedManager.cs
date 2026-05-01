using UnityEngine;

public class SpeedManager
{
    private readonly Player _player;
    private readonly float _startSpeed;
    private readonly float _acceleration;
    private readonly float _accelerationTime;

    private float _accelerationTimer;
    private float _currentSpeed;

    public SpeedManager(Player player, float startSpeed, float acceleration, float accelerationTime)
    {
        _player = player;
        _startSpeed = startSpeed;
        _acceleration = acceleration;
        _accelerationTime = accelerationTime;

        _currentSpeed = _startSpeed;
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

    public void ResetSpeed()
    {
        _currentSpeed = _startSpeed;
        _player.SetMoveSpeed(_currentSpeed);
    }

    private void UpdateSpeed()
    {
        _currentSpeed += _acceleration;
        _player.SetMoveSpeed(_currentSpeed);
    }
}