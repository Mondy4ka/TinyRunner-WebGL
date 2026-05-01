using PrimeTween;
using System.Collections;
using UnityEngine;

public class Player
{
    public float JumpTimer
    {
        get => _jumpTimer;
        set
        {
            _jumpTimer = Mathf.Clamp(value, 0, _jumpDuration);

            if (_jumpTimer >= _jumpDuration)
                _isJumping = false;
        }
    }

    private readonly InputArea _inputArea;
    private readonly Transform _transform;

    private readonly float _topYPosition;
    private readonly float _downYPosition;

    private readonly float _switchAnimationDuration;
    private readonly Ease _switchAnimationEase;
    private readonly AnimationCurve _animationCurve;
    private readonly float _jumpDuration;

    private Vector2 _startPosition;
    private float _jumpTimer;
    private float _currentSpeed;
    private bool _isTopPosition = true;
    private bool _isBlocked = false;
    private bool _isJumping;

    public Player(InputArea inputArea, Transform transform, float topYPosition, float downYPosition, float switchAnimationDuration, Ease switchAnimationEase, AnimationCurve animationCurve, float jumpDuration)
    {
        _inputArea = inputArea;
        _transform = transform;
        _topYPosition = topYPosition;
        _downYPosition = downYPosition;
        _switchAnimationDuration = switchAnimationDuration;
        _switchAnimationEase = switchAnimationEase;
        _animationCurve = animationCurve;
        _jumpDuration = jumpDuration;
    }

    public void Initialize()
    {
        _inputArea.OnClick += SwitchLine;

        _startPosition = _transform.position;
    }

    public void Deinitialize() => _inputArea.OnClick -= SwitchLine;

    public void SetMoveSpeed(float newSpeed)
    {
        if (newSpeed < 0) return;

        _currentSpeed = newSpeed;
    }

    public void Move(out float offset)
    {
        offset = _currentSpeed * Time.deltaTime;

        Vector2 newPosition = _transform.position;
        newPosition.x += offset;

        _transform.position = newPosition;
    }

    public void MoveToStartPoint()
    {
        _transform.position = _startPosition;
        _isTopPosition = true;
    }

    public void SwitchLine()
    {
        if (_isBlocked || _isJumping) return;

        _isBlocked = true;

        float newYPosition = _isTopPosition ? _downYPosition : _topYPosition;
        _isTopPosition = !_isTopPosition;

        Tween.PositionY(_transform, newYPosition, _switchAnimationDuration, _switchAnimationEase)
            .OnComplete(() => _isBlocked = false);
    }

    public IEnumerator Jump()
    {
        if (_isBlocked || _isJumping) yield return null;

        _isJumping = true;
        JumpTimer = 0.0f;

        while (_isJumping)
        {
            JumpTimer += Time.deltaTime;

            float newPositionY = _animationCurve.Evaluate(JumpTimer);

            Vector2 newPos = _transform.position;
            newPos.y = _isTopPosition ? newPositionY : -newPositionY;

            _transform.position = newPos;

            yield return null;
        }
    }
}