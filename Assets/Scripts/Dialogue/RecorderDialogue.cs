using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecorderDialogue
{
    public string npcName; 
    [TextArea(3,10)]
    public string[] sentences;
    public bool[] isPlayerSpeaking;
}
