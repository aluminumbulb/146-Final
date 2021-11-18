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

        BTSequence lightAttackBranch = new BTSequence();
        lightAttackBranch.pushNode(new BTCheck(attackCheck));
        lightAttackBranch.pushNode(new BTAction(lightAttack));
        root.pushNode(lightAttackBranch);

        BTSequence heavyAttackBranch = new BTSequence();
        heavyAttackBranch.pushNode(new BTCheck(attackCheck));
        heavyAttackBranch.pushNode(new BTAction(heavyAttack));
        root.pushNode(heavyAttackBranch);

        BTSequence dodgeBranch = new BTSequence();
        dodgeBranch.pushNode(new BTCheck(bossAttacked));
        dodgeBranch.pushNode(new BTCheck(dodgeCheck));
        dodgeBranch.pushNode(new BTAction(moveToSafety));
        root.pushNode(dodgeBranch);

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
        Debug.Log("I only tell truths");
        return true;
    }

    public bool somethinElse()
    {
        Debug.Log("I only tell falsehoods");
        return false;
    }

    /*
     ******************CHECKS***********************
     */

    // attackCheck to see if hero in range for attack
    public bool attackCheck()
    {
        state.distBtwn = Mathf.Abs(Vector2.Distance(state.heroPos.position, state.bossPos.position));
        return state.distBtwn == 1f;
    }

    // check to see if boss just attacked
    public bool bossAttacked()
    {
        // did boss just launch an attack?
        return false;
    }

    // check to see if hero's square is in danger
    public bool dodgeCheck()
    {
        // get square player is on
        // is square about to be covered by bullet?
        return false;
    }

    /*
     ******************ACTIONS**********************
     */

    // lightAttack hero stab boss action
    public bool lightAttack()
    {
        // play light swing anim
        state.bossHealth -= state.lightDmg;
        return true;
    }

    // heavyAttack hero big swing boss action
    public bool heavyAttack()
    {
        // play heavy swing anim
        state.bossHealth -= state.heavyDmg;
        return true;
    }

    // move hero out of danger
    public bool moveToSafety()
    {
        // find closest square that is not under attack
        // move player to that square
        return true;
    }
}
