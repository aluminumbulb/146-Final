using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basically a template for us to pass around functions
public delegate bool checkDelegate(); //sets up the form of the delegate
public delegate bool actionDelegate();


//A parent class that will permit for all nodes to be fit
//on whatever data structure we might use. 
public class BTNode
{
    public float priority = 0;

    //Basic execute should pretty much always be available to any kind of node
    public virtual bool execute()
    {
        return true;
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
        return del();
    }
}

public class BTSelector : BTNode
{

}

public class BTSequence : BTNode
{

}
