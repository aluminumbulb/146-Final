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

            node.priority = 
            //The underused node's grace:

            Mathf.Abs(node.aveDeltX+node.aveDeltY+node.aveDeltHealth)
            /*
            I think this works because it gives a very slight
            advantage, which is the only situation in which we would
            want such a toss-up
            */ 
            //Adding the amount of changes that need to be made with the amound we can change them
            + Mathf.Abs(gs.transDeltX() + node.aveDeltX) 
            + Mathf.Abs(gs.transDeltY() + node.aveDeltY) 
            + Mathf.Abs(10 + gs.bossHealth + node.aveDeltHealth); //10+ is cheating, gotta fix that
        }
        //Taken from: https://answers.unity.com/questions/677070/sorting-a-list-linq.html
        //pq.Sort((e1, e2) => e2.priority.CompareTo(e1.priority));//biggest to smallest
        pq.Sort((e1, e2) => e1.priority.CompareTo(e2.priority));//smallest to biggest
    }
}

