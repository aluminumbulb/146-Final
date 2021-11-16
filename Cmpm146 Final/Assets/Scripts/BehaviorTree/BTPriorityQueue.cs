using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTPriorityQueue : MonoBehaviour
{
    List<BTNode> pq;
    
    /// <summary>
    /// Default constructor that sets up the list
    /// </summary>
    public BTPriorityQueue()
    {
        pq = new List<BTNode>();
    }

    public void push(BTNode node)
    {
        pq.Insert(0, node);
        reorganize();
    }

    public List<BTNode> getPQ()
    {
        return pq;
    }
    /// <summary>
    /// Sorts the priority queue by priority
    /// Currently unknown if ascending or descending
    /// entirely untested
    /// </summary>
    public void reorganize()
    {
        //Taken from: https://answers.unity.com/questions/677070/sorting-a-list-linq.html
        pq.Sort((e1, e2) => e1.priority.CompareTo(e2.priority));
    }

    //Need a function that iterates through the queue
}

