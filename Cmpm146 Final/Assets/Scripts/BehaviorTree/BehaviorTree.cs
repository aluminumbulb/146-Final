using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BehaviorTree : MonoBehaviour
{
    //Fill this with whatever function you want to use to make a check
    //It seems like once passed in, that value is safe in the new object!
    checkDelegate testDelegator;

    // Start is called before the first frame update
    void Start()
    {
        testDelegator = somethin;
        BTPriorityQueue testQ = new BTPriorityQueue();
        testDelegator = somethin;
        BTCheck micCheck = new BTCheck(testDelegator);
        testDelegator = somethinElse;
        BTCheck ikeCheck = new BTCheck(testDelegator);

        testQ.push(micCheck);
        testQ.push(ikeCheck);

        foreach(BTNode node in testQ.getPQ())
        {
            node.execute();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool somethin()
    {
        Debug.Log("Holy Shit I worked");
        return true;
    }

    public bool somethinElse()
    {
        Debug.Log("TAKE A GOOD LOOK SUNNY ITS ABOUT TO HAPPEN AGAIN");
        return false;
    }
}
