using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DroppableMissingNote : MonoBehaviour, IDropHandler
{
    // Assign (via Inspector or by code) a reference to the PuzzleMechanism.
    public PuzzleMechanism puzzleMechanism;
    
    // The index of this missing note image (set this based on level configuration).
    public int missingNoteIndex;

    public void OnDrop(PointerEventData eventData)
    {
        // Check if the dragged object is a DraggableNoteButton.
        DraggableNoteButton draggable = eventData.pointerDrag.GetComponent<DraggableNoteButton>();
        if (draggable != null)
        {
            // Call PuzzleMechanism to update the missing note image with the dragged note.
            puzzleMechanism.HandleDropNote(draggable.noteIndex, missingNoteIndex);
        }
    }
}
