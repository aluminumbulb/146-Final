using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This should function largely as a manager,
/// holding any and all variables or functions we wish to execute
/// within the context of the tree
/// </summary>
public class GameState : MonoBehaviour
{
    public Transform heroPos;
    public Transform bossPos;
    public float distBtwn;
    public float bossHealth;
    public float lightDmg = 10;
    public float heavyDmg = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
