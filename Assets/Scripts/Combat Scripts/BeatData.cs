using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatData : MonoBehaviour
{
    public float beatTime;
    public int noteType; // 0: red, 1: green, 2: purple

    public BeatData(float beatTime, int noteType)
    {
        this.beatTime = beatTime;
        this.noteType = noteType;
    }
}
