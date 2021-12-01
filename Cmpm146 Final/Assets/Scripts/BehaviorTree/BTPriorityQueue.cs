using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTPriorityQueue
{
    List<BTNode> pq;
    
    /// <summary>
    /// Default constructor that sets up the list
    /// </summary>
    public BTPriorityQueue()
    {
        pq = new List<BTNode>();
    }

    /// <summary>
    /// Inserts a node to the back of the list
    /// </summary>
    /// <param name="node">
    /// Any form of bt node to be added to the list
    /// </param>
    public void push(BTNode node)
    {
        pq.Add(node);
        node.myQ = this;//Remembers reference parent queue
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
    public void reorganize(GameState gs)
    {
        foreach(BTNode node in pq)
        {
            //We're attempting to get the priority as close to zero as possible
            //So we take the absolute value, such that minimum is closer to zero, not infinitely negative

            node.priority = Mathf.Abs(gs.transDeltX() + node.aveDeltX) + Mathf.Abs(gs.transDeltY() + node.aveDeltY); //+ gs.bossHealth * node.aveDeltHealth;;
        }
        //Taken from: https://answers.unity.com/questions/677070/sorting-a-list-linq.html
        //pq.Sort((e1, e2) => e2.priority.CompareTo(e1.priority));//biggest to smallest
        pq.Sort((e1, e2) => e1.priority.CompareTo(e2.priority));//smallest to biggest
    }
}

