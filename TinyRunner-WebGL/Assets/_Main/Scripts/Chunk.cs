using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private GhostSpike[] _ghostSpikes;

    public void Initialize()
    {
        if (_ghostSpikes.Length <= 0) return;

        for (int i = 0; i < _ghostSpikes.Length; i++)
        {
            _ghostSpikes[i].Initialize();
        }
    }

    public void ResetChunk()
    {
        if (_ghostSpikes.Length <= 0) return;

        for (int i = 0;  i < _ghostSpikes.Length; i++)
        {
            _ghostSpikes[i].ResetSpike();
        }
    }
}