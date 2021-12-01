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
    public float totalUses = 0;
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
        totDeltX += (currHeroPos.x - prevHeroPos.x);
        totDeltY += (currHeroPos.y - prevHeroPos.y);
        totDeltHealth += (currBossHealth - prevBossHealth);

        //Average used to determine the likely effect of the action
        aveDeltX = totDeltX / totalUses;
        aveDeltY = totDeltY / totalUses;
        aveDeltHealth = totDeltHealth / totalUses;
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

    public virtual bool execute()
    {
        return true;
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
        //Given the current state, adjust priority

        bool response = del();//Perform the check
        return response;
    }
}

//An action node that will shift itself within a tree structure
public class BTDynamicAction : BTNode
{
     actionDelegate del;

    public BTDynamicAction(actionDelegate del, GameState state)
    {
        this.del = del;
        base.gs = state;
    }

    public override bool execute()
    {   
        base.setBeforeValues();
        bool response = del();//Perform the check
        base.setAfterValues();
        base.trainingFunction();
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
        btq.reorganize(base.gs);
        //Iterate through every node in order
        base.setBeforeValues();
        foreach (BTNode node in btq.getPQ())
        {
            //Debug.Log("BT Selector Executing a Node");
            //Attempt to execute the underlying node
            //return true as soon as a selected node has succeeded
            response = node.execute();
            //if (node.execute())
            if(response)
            {
                //Debug.Log("Node successfully Executed");
                //response = true;
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
        bool response = true;//Perform the check
        btq.reorganize(base.gs);
        //Iterate through every node in order
        base.setBeforeValues();
        foreach (BTNode node in btq.getPQ())
        {
            //Debug.Log("BT Sequence Executing a Node");
            //Attempt to execute the underlying node
            //return true as soon as a selected node has succeeded
            response = node.execute();
            if (!response)
            {
                //response = true;
                //Debug.Log("Sequence Encountered Failure");
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


//----------Static Versions of Branch Structures---------------
/*
These act similarly to the above, but they do not "train" themselves to shift around in a parent branch structure
*/
public class BTStaticSelector : BTNode
{
    BTPriorityQueue btq;
    public BTStaticSelector(GameState state)
    {
        btq = new BTPriorityQueue();
        base.gs = state;
    }

    public override bool execute()
    {
        bool response = false;//Perform the check
        btq.reorganize(base.gs);
        //Iterate through every node in order
        base.setBeforeValues();
        foreach (BTNode node in btq.getPQ())
        {
            //Debug.Log("BT Selector Executing a Node");
            //Attempt to execute the underlying node
            //return true as soon as a selected node has succeeded
            response = node.execute();
            //if (node.execute())
            if(response)
            {
                //Debug.Log("Node successfully Executed");
                //response = true;
                break;
            }
        }
        base.setAfterValues();
        return response;
    }

    public void pushNode(BTNode node)
    {
        btq.push(node);
    }
}

public class BTStaticSequence : BTNode
{
    BTPriorityQueue btq;
    public BTStaticSequence(GameState state)
    {
        btq = new BTPriorityQueue();
        base.gs = state;
    }

    public override bool execute()
    {
        bool response = true;//Perform the check
        btq.reorganize(base.gs);
        //Iterate through every node in order
        base.setBeforeValues();
        foreach (BTNode node in btq.getPQ())
        {
            //Debug.Log("BT Sequence Executing a Node");
            //Attempt to execute the underlying node
            //return true as soon as a selected node has succeeded
            response = node.execute();
            if (!response)
            {
                //response = true;
                //Debug.Log("Sequence Encountered Failure");
                break;
            }
        }
        base.setAfterValues();
        return response;
    }

    public void pushNode(BTNode node)
    {
        btq.push(node);
    }
}

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

