using UnityEngine;

[CreateAssetMenu(fileName = "New Puzzle Level Config", menuName = "Puzzle Level Config")]
public class PuzzleLevelConfig : ScriptableObject
{
    public int missingNotesCount;
    public float timeLimit;
    public int[] correctSequence;
    public AudioClip musicSegment;
}

