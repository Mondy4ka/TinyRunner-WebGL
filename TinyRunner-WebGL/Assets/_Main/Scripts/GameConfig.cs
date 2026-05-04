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
    public Chunk[] Chunks;
    public int ChunksRepeat;

    [Header("Score Settings")]
    public int AdditionScore;
    public float AdditionScoreTime;

    [Header("Color Settings")]
    public List<Color> Colors;
    public float ColorSwitchDelay;
    public float ColorTransitionDuration;
}