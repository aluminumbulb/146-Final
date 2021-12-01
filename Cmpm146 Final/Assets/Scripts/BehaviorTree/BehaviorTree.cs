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
    HeroControl heroControl;
    public bool ready = false;//indicates if a tree has made a decision

    // Start is called before the first frame update
    void Start()
    {
        heroControl = FindObjectOfType<HeroControl>();
        state = FindObjectOfType<GameState>();
        root = new BTSelector(state);

        BTSequence lightAttackBranch = new BTSequence(state);
        lightAttackBranch.pushNode(new BTCheck(attackCheck, state));
        lightAttackBranch.pushNode(new BTAction(lightAttack, state));
        root.pushNode(lightAttackBranch);

        BTSequence heavyAttackBranch = new BTSequence(state);
        heavyAttackBranch.pushNode(new BTCheck(attackCheck, state));
        heavyAttackBranch.pushNode(new BTAction(heavyAttack, state));
        root.pushNode(heavyAttackBranch);

        BTSequence dodgeBranch = new BTSequence(state);
        //dodgeBranch.pushNode(new BTCheck(bossAttacked, state));
        dodgeBranch.pushNode(new BTCheck(dodgeCheck, state));
        dodgeBranch.pushNode(new BTAction(moveToSafety, state));
        root.pushNode(dodgeBranch);

        //----Directional Movement Branches----
        BTSequence moveNorthBranch = new BTSequence(state);
        moveNorthBranch.pushNode(new BTCheck(nMovCheck, state));//Can I move there?
        moveNorthBranch.pushNode(new BTAction(moveNorth, state));//Try to move there.

      
        BTSelector moveDirSelect = new BTSelector(state);
        moveDirSelect.pushNode(moveNorthBranch);
        //moveDirSelect.pushNode(new BTDynamicAction(moveLeft, state));
        //moveDirSelect.pushNode(new BTDynamicAction(moveRight, state));
        //moveDirSelect.pushNode(new BTDynamicAction(moveDown, state));
       
        //---------Adding branches to root----------
        root.pushNode(moveDirSelect);

    }

    public void execute()
    {
        ready = false;
        root.execute();
        ready = true;
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
       
        state.distBtwn = Mathf.Abs(Vector2.Distance(state.hero.position, state.boss.position));
        //Debug.Log("AttackCheck, dist between = "+ state.distBtwn);
        return state.distBtwn == 1f;
    }

    // check to see if hero's square is in danger
    public bool dodgeCheck()
    {
        // get square player is on
        // is square about to be covered by bullet?
        //Debug.Log("DodgeCheck");
        return state.BossAtkCheck("Hero", state.bossAtk.currAttack);
    }

    bool nMovCheck(){
        return heroControl.CheckMovePoss(HeroControl.directions.N);
    }

    

    /*
     ******************ACTIONS**********************
     */

    // lightAttack hero stab boss action
    public bool lightAttack()
    {
        Debug.Log("Light Attack");
        // play light swing anim
        state.bossHealth -= state.lightDmg;
        return true;
    }

    // heavyAttack hero big swing boss action
    public bool heavyAttack()
    {
        Debug.Log("Heavy Attack");
        // play heavy swing anim
        state.bossHealth -= state.heavyDmg;
        return true;
    }

    // move hero out of danger
    public bool moveToSafety()
    {
        Debug.Log("Dodge");
        // find closest square that is not under attack
        // move player to that square
        state.heroMove.movePoint.position += new Vector3(0f, 0f, 0f);
        return true;
    }

    bool moveNorth(){
        //Add ray check here
        heroControl.MoveNorth();
        return true;
    }

    bool moveSouth(){
        //Add ray check here
        heroControl.MoveSouth();
        return true;
    }

    bool moveEast(){
        //Add ray check here
        heroControl.MoveEast();
        return true;
    }

    bool moveWest(){
        //Add ray check here
        heroControl.MoveWest();
        return true;
    }



}
 //Graveyard
    //This I think is a never true for the time being
    /*
    // check to see if boss just attacked
    public bool bossAttacked()
    {
        // did boss just launch an attack?
        Debug.Log("BossAttack?: " + state.currTurn);
        return state.currTurn == GameState.turn.BOSS_DECISION;
    }
    */