using UnityEngine;

public class Chunk : MonoBehaviour
{
    public bool IsCollectables => _collectables.Length <= 0;

    private Collectable[] _collectables;

    public void Initialize()
    {
        _collectables = GetComponentsInChildren<Collectable>();

        if (IsCollectables) return;

        for (int i = 0; i < _collectables.Length; i++)
            _collectables[i].Initialize();
    }

    public void ResetChunk()
    {
        if (IsCollectables) return;

        for (int i = 0; i < _collectables.Length; i++)
            _collectables[i].Revert();
    }

    public void MoveTo(Vector2 newPosition) => transform.position = newPosition;
}