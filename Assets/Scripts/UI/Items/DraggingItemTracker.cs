using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggingItemTracker : MonoBehaviour
{
    private ItemObject dragItem;

    // Set the current drag item being tracked
    public void SetDragItem(ItemObject dragItem)
    {
        this.dragItem = dragItem;
    }

    public ItemObject DragItem
    {
        get
        {
            return dragItem;
        }
    }
}
