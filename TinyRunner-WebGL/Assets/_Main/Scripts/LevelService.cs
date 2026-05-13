using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelService
{
    private readonly int _activeChunksCount;
    private readonly float _chunkLength;

    private readonly List<Chunk> _activeChunks = new();
    private readonly List<Chunk> _deactiveChunks = new();

    public LevelService(float chunkLength, int activeChunksCount)
    {
        _chunkLength = chunkLength;
        _activeChunksCount = activeChunksCount;
    }

    public void Initialize(Chunk[] chunks, int chunkRepeat)
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            for (int j = 0; j < chunkRepeat; j++)
            {
                Chunk chunk = Object.Instantiate(chunks[i]);

                chunk.Initialize();
                chunk.gameObject.SetActive(false);

                _deactiveChunks.Add(chunk);
            }
        }
    }

    public void SpawnChunk(bool isFirstChunk = false)
    {
        Vector2 chunkPosition = new(_chunkLength, 0);

        if (isFirstChunk == false)
        {
            chunkPosition = _activeChunks.Last().transform.position;
            chunkPosition.x += _chunkLength;
        }

        Chunk chunk = GetRandomChunk();
        chunk.ResetChunk();
        chunk.MoveTo(chunkPosition);

        if (_activeChunks.Count <= _activeChunksCount) return;

        DeleteLastChunk();
    }

    public Chunk GetRandomChunk()
    {
        Chunk chunk = _deactiveChunks[Random.Range(0, _deactiveChunks.Count)];
        chunk.gameObject.SetActive(true);

        _deactiveChunks.Remove(chunk);
        _activeChunks.Add(chunk);

        return chunk;
    }

    public void DeleteLastChunk()
    {
        Chunk chunk = _activeChunks[0];
        chunk.gameObject.SetActive(false);

        _activeChunks.Remove(chunk);
        _deactiveChunks.Add(chunk);
    }

    public void ClearLevel()
    {
        while (_activeChunks.Count > 0)
            DeleteLastChunk();
    }
}