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
    private Vector3 currHeroPos, prevHeroPos;
    private float prevBossHealth, currBossHealth;
    //Basic execute should pretty much always be available to any kind of node

    //============This is hopefully where we're gunna make the magic happen========================
    //This training function should update priority based on a sum of all weights listed above
    //It will then use this and replace priority with this function.
    //For the sake of cleanness, it may be best to keep these checks as their own functions elsewhere.
    public void trainingFunction()
    {
        //Updates this nodes priority adjusting values:
        totalUses++; //increase total uses generally
        totDeltX += currHeroPos.x - prevHeroPos.x;
        totDeltY += currHeroPos.y - prevHeroPos.y;
        totDeltHealth += currBossHealth - prevBossHealth;

        //Average used to determine the likely effect of the action
        aveDeltX = totDeltX / totalUses;
        aveDeltY = totDeltY / totalUses;
        aveDeltHealth = totDeltHealth / totalUses;

        //unsure about these signs

        float xDiff = gs.boss.position.x - gs.hero.position.x;
        float yDiff = gs.boss.position.y - gs.hero.position.y;

        //Priority queue will attempt to maximize all these values
        priority = transDeltX(xDiff, aveDeltX) + transDeltY(yDiff, aveDeltY) + transDeltHealth(gs.bossHealth,aveDeltHealth);
    }
    //=============================================================================================

    //This and its sister below set up variables which will be compared,
    //They do this in the case that actions add more than one attribute to the mix
    //The whole subtree can be evaluated on the changes it brings
    protected void setBeforeValues()
    {
        prevHeroPos = gs.hero.position;
        prevBossHealth = gs.bossHealth;
    }

    protected void setAfterValues()
    {
        currHeroPos = gs.hero.position;
        currBossHealth = gs.bossHealth;
    }

    float transDeltX(float xDiff, float aveDeltx)
    {
        //The amount that needs to be done, constrained, times the amound it might help
        //Same signs should maximize the value, while opposing signs will minimize it
        //(i.e. preferred direction should arise)
        return sigmoid(xDiff) * aveDeltX;
    }

    float transDeltY(float yDiff, float aveDeltY)
    {
        return sigmoid(yDiff) * aveDeltY;
    }

    float transDeltHealth(float bossHealth, float aveDeltHealth)
    {
        //boss health should pretty much always be priority 1
        return 1 * aveDeltHealth;
    }

    float sigmoid(float x)
    {
        return 1 / (1 + Mathf.Exp(-x));
    }

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
        base.setBeforeValues();
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
        base.setAfterValues();
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
        base.setBeforeValues();
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
        base.setAfterValues();
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

