using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Basically a template for us to pass around functions
public delegate bool checkDelegate(); //sets up the form of the delegate
public delegate bool actionDelegate();

/// <summary>
/// This is the script that will hold the primary behavior tree implementations
/// So basiclly make changes here
/// </summary>
public class BehaviorTree : MonoBehaviour
{
    GameState state;
    BTSelector root;
    // Start is called before the first frame update
    void Start()
    {
        state = FindObjectOfType<GameState>();
        root = new BTSelector();

        root.pushNode(new BTCheck(somethin));
        root.pushNode(new BTAction(somethinElse));
        root.execute();
    }

    /// <summary>
    /// Example function that follows format of delegates
    /// </summary>
    /// <returns>
    /// True/False depending on the result of the function
    /// </returns>
    public bool somethin()
    {
        Debug.Log("Holy Shit I worked");
        return false;
    }

    public bool somethinElse()
    {
        Debug.Log("TAKE A GOOD LOOK SUNNY ITS ABOUT TO HAPPEN AGAIN");
        return false;
    }
}
