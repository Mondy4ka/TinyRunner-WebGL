using UnityEngine;

public class CameraFollow
{
    private readonly Transform _targetTransform;
    private readonly Transform _cameraTransform;
    private readonly float _offsetX;

    public CameraFollow(Transform targetTransform, Transform cameraTransform, float offsetX)
    {
        _targetTransform = targetTransform;
        _cameraTransform = cameraTransform;
        _offsetX = offsetX;
    }

    public void Follow()
    {
        Vector3 newPosition = _cameraTransform.position;
        newPosition.x = _targetTransform.position.x + _offsetX;

        _cameraTransform.position = newPosition;
    }
}