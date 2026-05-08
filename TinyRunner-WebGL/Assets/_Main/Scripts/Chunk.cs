using UnityEngine;

public class Chunk : MonoBehaviour
{
    private Collectable[] _collectables;

    public void Initialize()
    {
        _collectables = GetComponentsInChildren<Collectable>();

        if (_collectables.Length <= 0) return;

        for (int i = 0; i < _collectables.Length; i++)
            _collectables[i].Initialize();
    }

    public void ResetChunk()
    {
        if (_collectables.Length <= 0) return;

        for (int i = 0; i < _collectables.Length; i++)
            _collectables[i].Revert();
    }
}