using UnityEngine;

[System.Serializable]
public class PuzzleLevelConfig
{
    public int missingNotesCount;
    public float timeLimit;
    public int[] correctSequence;  // The answer sequence
    public AudioClip musicSegment; // The music for the level
}
