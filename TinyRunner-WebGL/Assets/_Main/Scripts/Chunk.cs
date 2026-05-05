using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private Collectable[] _collectables;

    public void Initialize()
    {
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