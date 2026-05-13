using PrimeTween;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Camera Settings")]
    public float CameraOffsetX;

    [Header("Line Switch Settings")]
    public float TopYPosition;
    public float DownYPosition;
    public float SwitchAnimationDuration;
    public Ease SwitchAnimationEase;

    [Header("Jump Settings")]
    public AnimationCurve JumpCurve;
    public float JumpDuration;

    [Header("Speed Settings")]
    public float StartSpeed;
    public float Acceleration;
    public float AccelerationTime;

    [Header("Level Settings")]
    public float ChunkLength;
    public int ActiveChunksCount;
    public int ChunksRepeat;
    public Chunk[] Chunks;

    [Header("Score Settings")]
    public int AdditionScore;
    public float AdditionScoreTime;

    [Header("Shop Settings")]
    public List<Skin> Skins;
    public SkinCell SkinCellPrefab;

    [Header("Sound Settings")]
    public AudioClip SwitchLineSound;
    public AudioClip JumpSound;
    public AudioClip CoinSound;
    public AudioClip DeathSound;
    public AudioClip PurchaseSound;

    [Header("Adv Settings")]
    public int RestartCountAdv;
    public int CoinsRewardAdv;
}