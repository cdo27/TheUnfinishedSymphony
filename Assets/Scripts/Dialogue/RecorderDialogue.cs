using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderDialogue
{
    public string npc1Name; 
    public string npc2Name; 
    public string npc3Name;
    [TextArea(3,10)]
    public string[] sentences;
    public bool[] isAldricSpeaking;
    public bool[] isPlayerSpeaking;
    public bool[] isNPC2Speaking;
}