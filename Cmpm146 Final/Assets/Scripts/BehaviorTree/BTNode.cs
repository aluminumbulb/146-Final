using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A parent class that will permit for all nodes to be fit
//on whatever data structure we might use. 
public class BTNode
{
    public float priority = 0;
    public BTPriorityQueue myQ = null;
    //These floats are used to calculate the average changes for each tracked value
    float totDeltX = 0, totDeltY = 0, totDeltHealth = 0;
    float totalUses = 0;
    public float aveDeltX = 0, aveDeltY = 0, aveDeltHealth = 0;
    protected GameState gs = null;
    //Basic execute should pretty much always be available to any kind of node

    //============This is hopefully where we're gunna make the magic happen========================
    //This training function should update priority based on a sum of all weights listed above
    //It will then use this and replace priority with this function.
    //For the sake of cleanness, it may be best to keep these checks as their own functions elsewhere.
    public void trainingFunction()
    {
        //Updates this nodes priority adjusting values:
        totalUses++; //increase total uses generally
        totDeltX += gs.getDeltX();
        totDeltY += gs.getDeltY();
        totDeltHealth += gs.getDeltHealth();

        aveDeltX = totDeltX / totalUses;
        aveDeltY = totDeltY / totalUses;
        aveDeltHealth = totDeltHealth / totalUses;

        //TODO: Prove directionality and signs
        float xDiff = gs.boss.position.x - gs.hero.position.x;
        float yDiff = gs.boss.position.y - gs.hero.position.y;

        //Priority will attempt to minimize all of these values to the best of its ability
        priority = 
            Mathf.Abs(xDiff + aveDeltX)
            + Mathf.Abs(yDiff + aveDeltY) 
            + Mathf.Abs(gs.bossHealth + aveDeltHealth);
    }
    //=============================================================================================

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


}

public class BTAction : BTNode
{
    actionDelegate del;

    public BTAction(actionDelegate del, GameState state)
    {
        this.del = del;
        base.gs = state;
    }

    public override bool execute()
    {
        bool response = del();//Perform the check
        return response;
    }
}

public class BTCheck : BTNode
{
    checkDelegate del;

    public BTCheck(checkDelegate del, GameState state)
    {
        this.del = del;
        base.gs = state;
    }

    public override bool execute()
    {
        bool response = del();//Perform the check
        return response;
    }
}

public class BTSelector : BTNode
{
    BTPriorityQueue btq;
    public BTSelector(GameState state)
    {
        btq = new BTPriorityQueue();
        base.gs = state;
    }

    public override bool execute()
    {
        bool response = false;//Perform the check
        //btq.reorganize();//orders the sub-nodes before going through them
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
        base.trainingFunction();//update priority
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
    public BTSequence(GameState state)
    {
        btq = new BTPriorityQueue();
        base.gs = state;
    }

    public override bool execute()
    {
        bool response = false;//Perform the check
        //btq.reorganize();//orders the sub-nodes before going through them
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
        base.trainingFunction();//update priority
        return response;
    }

    public void pushNode(BTNode node)
    {
        btq.push(node);
    }
}
//Static Branch structures could be used in the same way they're used for leaves,
//but I'd like to avoid that unless needed. (Just fill one with all static leafs
//to achieve a similar affect

/*
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
*/

