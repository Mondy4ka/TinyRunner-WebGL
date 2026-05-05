using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelService
{
    private readonly float _chunkLength;

    private readonly List<Chunk> _activeChunks = new();
    private readonly List<Chunk> _deactiveChunks = new();

    public LevelService(float chunkLength) => _chunkLength = chunkLength;

    public void Initialize(Chunk[] chunks, int chunkRepeat, out List<SpriteRenderer> renderers)
    {
        renderers = new();

        for (int i = 0; i < chunks.Length; i++)
        {
            for (int j = 0; j < chunkRepeat; j++)
            {
                Chunk chunk = Object.Instantiate(chunks[i]);
                chunk.Initialize();
                renderers.AddRange(chunk.GetComponentsInChildren<SpriteRenderer>());
                chunk.gameObject.SetActive(false);
                _deactiveChunks.Add(chunk);
            }
        }
    }

    public void SpawnChunk()
    {
        Vector2 chunkPosition;

        if (_activeChunks.Count > 0)
        {
            chunkPosition = _activeChunks.Last().transform.position;
            chunkPosition.x += _chunkLength;
        }
        else
        {
            chunkPosition = new(_chunkLength, 0);
        }

        var chunk = GetRandomChunk();
        chunk.ResetChunk();
        chunk.transform.position = chunkPosition;

        if (_activeChunks.Count < 5) return;

        DeleteLastChunk();
    }

    public Chunk GetRandomChunk()
    {
        var chunk = _deactiveChunks[Random.Range(0, _deactiveChunks.Count)];
        chunk.gameObject.SetActive(true);

        _deactiveChunks.Remove(chunk);
        _activeChunks.Add(chunk);

        return chunk;
    }

    public void DeleteLastChunk()
    {
        var chunk = _activeChunks[0];
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