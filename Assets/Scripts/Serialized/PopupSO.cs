using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupType
{
    WidescreenVideo,
    SquareVideo,
}

[CreateAssetMenu(menuName = "PopupSO", order = 2)]
public class PopupSO : ScriptableObject
{
    public PopupType type;
    public Texture texture;
    public float scale = 1;
}
