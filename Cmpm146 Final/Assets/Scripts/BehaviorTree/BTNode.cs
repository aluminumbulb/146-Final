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
    void updatePriority(float priDelt)
    {
        priority += priDelt;
        if (myQ != null)
        {
            myQ.reorganize();
        }
    }

    //============This is hopefully where we're gunna make the magic happen=======================
    protected void trainingFunction(bool success)
    {
        if (success)
        {
            updatePriority(1);
        }
        else
        {
            updatePriority(-1);
        }
    }
    //=============================================================================================
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
        bool response = del();//Perform the check
        base.trainingFunction(response);//update priority
        return response;
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
        bool response = del();//Perform the check
        base.trainingFunction(response);//update priority
        return response;
    }
}

/// <summary>
/// Similar to actions but they don't change their priority
/// The idea is to be grouped together in branch structures
/// </summary>
public class BTStaticAction : BTNode
{
    actionDelegate del;

    public BTStaticAction(actionDelegate del)
    {
        this.del = del;
    }

    public override bool execute()
    {
        return del();
    }
}

public class BTStaticCheck : BTNode
{
    checkDelegate del;

    public BTStaticCheck(checkDelegate del)
    {
        this.del = del;
    }

    public override bool execute()
    {
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
        bool response = false;//Perform the check
      
        //Iterate through every node in order
        foreach (BTNode node in btq.getPQ())
        {
            //Attempt to execute the underlying node
            //return true as soon as a selected node has succeeded
            if (node.execute())
            {
                response = true;
                break;
            }
        }
        base.trainingFunction(response);//update priority
        return response;
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
        bool response = false;//Perform the check
        //Iterate through every node in order
        foreach (BTNode node in btq.getPQ())
        {
            //Attempt to execute the underlying node
            //return true as soon as a selected node has succeeded
            if (!node.execute())
            {
                response = true;
                break;
            }
        }
        base.trainingFunction(response);//update priority
        return response;
    }

    public void pushNode(BTNode node)
    {
        btq.push(node);
    }

    //Static Branch structures could be used in the same way they're used for leafs,
    //but I'd like to avoid that unless needed. (Just fill one with all static leafs
    //to achieve a similar affect
}
