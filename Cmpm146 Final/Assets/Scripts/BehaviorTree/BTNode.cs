using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A parent class that will permit for all nodes to be fit
//on whatever data structure we might use. 
public class BTNode
{
    public float priority = 0;
    public BTPriorityQueue myQ = null;

    //Basic execute should pretty much always be available to any kind of node
    public virtual bool execute()
    {
        return true;
    }

    /// <summary>
    /// Changes the priority of a node 
    /// </summary>
    /// <param name="priDelt">
    /// The amount the node's priority will change,
    /// note that it is Addititve and that it can go negative
    /// </param>
    public void updatePriority(float priDelt)
    {
        priority += priDelt;
        if (myQ != null)
        {
            myQ.reorganize();
        }
    }
}

public class BTAction : BTNode
{
    actionDelegate del;

    public BTAction(actionDelegate del)
    {
        this.del = del;
    }

    public override bool execute()
    {
        //By my best estimate, this is where updating dependancies should go.
        return del();
    }
}

public class BTCheck : BTNode
{
    checkDelegate del;

    public BTCheck(checkDelegate del)
    {
        this.del = del;
    }

    public override bool execute()
    {
        //By my best estimate, this is where updating dependancies should go.
        return del();
    }
}

public class BTSelector : BTNode
{
    BTPriorityQueue btq;
    public BTSelector()
    {
        btq = new BTPriorityQueue();
    }

    public override bool execute()
    {
        //Iterate through every node in order
        foreach(BTNode node in btq.getPQ())
        {
            //Attempt to execute the underlying node
            //return true as soon as a selected node has succeeded
            if (node.execute())
            {
                return true;
            }
        }
        return false;
    }

    public void pushNode(BTNode node)
    {
        btq.push(node);
    }
}

public class BTSequence : BTNode
{
    BTPriorityQueue btq;
    public BTSequence()
    {
        btq = new BTPriorityQueue();
    }

    public override bool execute()
    {
        //Iterate through every node in order
        foreach (BTNode node in btq.getPQ())
        {
            //Attempt to execute the underlying node
            //return true as soon as a selected node has succeeded
            if (!node.execute())
            {
                return false;
            }
        }
        return false;
    }
}
