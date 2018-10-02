using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sorting queue of the priority of items based on their values (lowest -> highest)
public class PriorityQueue : MonoBehaviour {

    private List<PriorityItem> pQueue = new List<PriorityItem>();

    /// <summary>
    /// Add item into the queue
    /// </summary>
    /// <param name="pItem">The item to be added to the queue</param>
    public void Add(PriorityItem pItem)
    {
        if (pQueue.Count == 0) //If the queue is empty
        {
            pQueue.Add(pItem);
            return;
        }

        // Determine where to start searching for an insertion point
        int i = 0;
        if (pItem.EstimatedTotalDistance > pQueue[pQueue.Count / 2].EstimatedTotalDistance)
            i = pQueue.Count / 2;

        // Find the insertion point for the item
        for (; i < pQueue.Count; i++)
            if (pItem.EstimatedTotalDistance <= pQueue[i].EstimatedTotalDistance)
            {
                pQueue.Insert(i, pItem);
                break;
            }
    }

    /// <summary>
    /// Removes the first item from the queue
    /// </summary>
    /// <returns>The first item in the queue.</returns>
    public PriorityItem Pop()
    {
        if (pQueue.Count == 0) //If the queue is empty
            return null;

        // Save the return value and then remove it before returning it
        PriorityItem returnValue = pQueue[0];
        pQueue.RemoveAt(0);
        return returnValue;
    }

    /// <summary>
    /// Remove a specific item from the queue
    /// </summary>
    /// <param name="pItem">The item to be removed from the queue</param>
    public void Remove(PriorityItem pItem)
    {
        pQueue.Remove(pItem);
    }

    /// <summary>
    /// Check if an item is in the queue
    /// </summary>
    /// <param name="pItem">The item to be checked for in the queue</param>
    public bool Contains(PriorityItem pItem)
    {
        return pQueue.Contains(pItem);
    }

    /// <summary>
    /// Check if the queue is empty or not
    /// </summary>
    public bool Empty()
    {
        if (pQueue.Count == 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Gets the pItem at the location given
    /// </summary>
    /// <param name="location">The location of the requested value</param>
    public PriorityItem Get(int location)
    {
        return pQueue[location];
    }

    /// <summary>
    /// Gets the index of a pItem in the queue
    /// </summary>
    /// <param name="pItem">The item to be checked for in the queue</param>
    public int IndexOf(PriorityItem pItem)
    {
        return pQueue.IndexOf(pItem);
    }

}
