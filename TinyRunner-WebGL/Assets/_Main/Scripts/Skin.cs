using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "Skin")]
public class Skin : ScriptableObject
{
    public Sprite Sprite;
    public int Price;
    public bool IsDefaultSkin;
}