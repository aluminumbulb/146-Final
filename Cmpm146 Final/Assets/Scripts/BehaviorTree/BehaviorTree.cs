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

        /*
        BTSequence dodgeBranch = new BTSequence(state);
        //dodgeBranch.pushNode(new BTCheck(bossAttacked, state));
        dodgeBranch.pushNode(new BTCheck(dodgeCheck, state));
        dodgeBranch.pushNode(new BTAction(moveToSafety, state));
        root.pushNode(dodgeBranch);
        */
        
        //----Directional Movement Branches----
        BTSequence moveNorthBranch = new BTSequence(state);
        moveNorthBranch.pushNode(new BTCheck(nMovCheck, state));//Can I move there?
        moveNorthBranch.pushNode(new BTAction(nMove, state));//Try to move there.

        BTSequence moveNorthEastBranch = new BTSequence(state);
        moveNorthEastBranch.pushNode(new BTCheck(neMovCheck, state));
        moveNorthEastBranch.pushNode(new BTAction(neMove, state));

        BTSequence moveEastBranch = new BTSequence(state);
        moveEastBranch.pushNode(new BTCheck(eMovCheck, state));
        moveEastBranch.pushNode(new BTAction(eMove, state));

        BTSequence moveSouthEastBranch = new BTSequence(state);
        moveSouthEastBranch.pushNode(new BTCheck(seMovCheck, state));
        moveSouthEastBranch.pushNode(new BTAction(seMove, state));

        BTSequence moveSouthBranch = new BTSequence(state);
        moveSouthBranch.pushNode(new BTCheck(sMovCheck, state));
        moveSouthBranch.pushNode(new BTAction(sMove, state));

        BTSequence moveSouthWestBranch = new BTSequence(state);
        moveSouthWestBranch.pushNode(new BTCheck(swMovCheck, state));
        moveSouthWestBranch.pushNode(new BTAction(swMove, state));

        BTSequence moveWestBranch = new BTSequence(state);
        moveWestBranch.pushNode(new BTCheck(wMovCheck, state));
        moveWestBranch.pushNode(new BTAction(wMove, state));

        BTSequence moveNorthWestBranch = new BTSequence(state);
        moveNorthWestBranch.pushNode(new BTCheck(nwMovCheck, state));
        moveNorthWestBranch.pushNode(new BTAction(nwMove, state));

        BTSelector moveDirSelect = new BTSelector(state);
        moveDirSelect.pushNode(moveNorthBranch);
        moveDirSelect.pushNode(moveNorthEastBranch);
        moveDirSelect.pushNode(moveEastBranch);
        moveDirSelect.pushNode(moveSouthEastBranch);
        moveDirSelect.pushNode(moveSouthBranch);
        moveDirSelect.pushNode(moveSouthWestBranch);
        moveDirSelect.pushNode(moveWestBranch);
        moveDirSelect.pushNode(moveNorthWestBranch);

        //---Dodge Branches---
        BTSequence dodgeNorthBranch = new BTSequence(state);
        dodgeNorthBranch.pushNode(new BTCheck(nDodgeCheck, state));//Can I move there?
        dodgeNorthBranch.pushNode(new BTAction(nDodge, state));//Try to move there.

        BTSequence dodgeNorthEastBranch = new BTSequence(state);
        dodgeNorthEastBranch.pushNode(new BTCheck(neDodgeCheck, state));
        dodgeNorthEastBranch.pushNode(new BTAction(neDodge, state));

        BTSequence dodgeEastBranch = new BTSequence(state);
        dodgeEastBranch.pushNode(new BTCheck(eDodgeCheck, state));
        dodgeEastBranch.pushNode(new BTAction(eDodge, state));

        BTSequence dodgeSouthEastBranch = new BTSequence(state);
        dodgeSouthEastBranch.pushNode(new BTCheck(seDodgeCheck, state));
        dodgeSouthEastBranch.pushNode(new BTAction(seDodge, state));
        
        BTSequence dodgeSouthBranch = new BTSequence(state);
        dodgeSouthBranch.pushNode(new BTCheck(sDodgeCheck, state));
        dodgeSouthBranch.pushNode(new BTAction(sDodge, state));

        BTSequence dodgeSouthWestBranch = new BTSequence(state);
        dodgeSouthWestBranch.pushNode(new BTCheck(swDodgeCheck, state));
        dodgeSouthWestBranch.pushNode(new BTAction(swDodge, state));

        BTSequence dodgeWestBranch = new BTSequence(state);
        dodgeWestBranch.pushNode(new BTCheck(wDodgeCheck, state));
        dodgeWestBranch.pushNode(new BTAction(wDodge, state));

        BTSequence dodgeNorthWestBranch = new BTSequence(state);
        dodgeNorthWestBranch.pushNode(new BTCheck(nwDodgeCheck, state));
        dodgeNorthWestBranch.pushNode(new BTAction(nwDodge, state));

        BTSelector dodgeDirSelect = new BTSelector(state);
        dodgeDirSelect.pushNode(dodgeNorthBranch);
        dodgeDirSelect.pushNode(dodgeNorthEastBranch);
        dodgeDirSelect.pushNode(dodgeEastBranch);
        dodgeDirSelect.pushNode(dodgeSouthEastBranch);
        dodgeDirSelect.pushNode(dodgeSouthBranch);
        dodgeDirSelect.pushNode(dodgeSouthWestBranch);
        dodgeDirSelect.pushNode(dodgeWestBranch);
        dodgeDirSelect.pushNode(dodgeNorthWestBranch);
       
        //---------Adding branches to root----------
        root.pushNode(moveDirSelect);
        root.pushNode(dodgeDirSelect);
    }

    public void execute()
    {
        ready = false;
        root.execute();
        ready = true;
    }

    /*
     ******************CHECKS***********************
     */

    // attackCheck to see if hero in range for attack
    public bool attackCheck()
    {
        state.distBtwn = Mathf.Abs(Vector2.Distance(state.hero.position, state.boss.position));
        //Debug.Log("AttackCheck, dist between = "+ state.distBtwn);
        return state.distBtwn <= 3;
    }

    // check to see if hero's square is in danger
    public bool dodgeCheck()
    {
        // get square player is on
        // is square about to be covered by bullet?
        //Debug.Log("DodgeCheck");
        return state.BossAtkCheck("Hero", state.bossAtk.currAttack);
    }

    //-----Move Checks-----
    //Check in a particular direction (in this case, north)
    bool nMovCheck(){
        return heroControl.CheckMovePoss(HeroControl.directions.N);
    }

    bool neMovCheck(){
        return heroControl.CheckMovePoss(HeroControl.directions.NE);
    }

    bool eMovCheck(){
        return heroControl.CheckMovePoss(HeroControl.directions.E);
    }

    bool seMovCheck(){
        return heroControl.CheckMovePoss(HeroControl.directions.SE);
    }

    bool sMovCheck(){
        return heroControl.CheckMovePoss(HeroControl.directions.S);
    }

    bool swMovCheck(){
        return heroControl.CheckMovePoss(HeroControl.directions.SW);
    }

    bool wMovCheck(){
        return heroControl.CheckMovePoss(HeroControl.directions.W);
    }

    bool nwMovCheck(){
        return heroControl.CheckMovePoss(HeroControl.directions.NW);
    }

    //----Dodge Checks----
    bool nDodgeCheck(){
        return heroControl.CheckDodgePoss(HeroControl.directions.N);
    }

    bool neDodgeCheck(){
        return heroControl.CheckDodgePoss(HeroControl.directions.NE);
    }

    bool eDodgeCheck(){
        return heroControl.CheckDodgePoss(HeroControl.directions.E);
    }

    bool seDodgeCheck(){
        return heroControl.CheckDodgePoss(HeroControl.directions.SE);
    }

    bool sDodgeCheck(){
        return heroControl.CheckDodgePoss(HeroControl.directions.S);
    }

    bool swDodgeCheck(){
        return heroControl.CheckDodgePoss(HeroControl.directions.SW);
    }

    bool wDodgeCheck(){
        return heroControl.CheckDodgePoss(HeroControl.directions.W);
    }

    bool nwDodgeCheck(){
        return heroControl.CheckDodgePoss(HeroControl.directions.NW);
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


    //----Move Actions----
    bool nMove(){
        heroControl.MoveDirection(HeroControl.directions.N);
        return true;
    }

    bool neMove(){
        heroControl.MoveDirection(HeroControl.directions.NE);
        return true;
    }

    bool eMove(){
        heroControl.MoveDirection(HeroControl.directions.E);
        return true;
    }

    bool seMove(){
        heroControl.MoveDirection(HeroControl.directions.SE);
        return true;
    }

    bool sMove(){
        heroControl.MoveDirection(HeroControl.directions.S);
        return true;
    }

    bool swMove(){
        heroControl.MoveDirection(HeroControl.directions.SW);
        return true;
    }

    bool wMove(){
        heroControl.MoveDirection(HeroControl.directions.W);
        return true;
    }
    
    bool nwMove(){
        heroControl.MoveDirection(HeroControl.directions.NW);
        return true;
    }

    //----Dodge Actions
    bool nDodge(){
        heroControl.DodgeDirection(HeroControl.directions.N);
        return true;
    }

    bool neDodge(){
        heroControl.DodgeDirection(HeroControl.directions.NE);
        return true;
    }

    bool eDodge(){
        heroControl.DodgeDirection(HeroControl.directions.E);
        return true;
    }

    bool seDodge(){
        heroControl.DodgeDirection(HeroControl.directions.SE);
        return true;
    }

    bool sDodge(){
        heroControl.DodgeDirection(HeroControl.directions.S);
        return true;
    }

    bool swDodge(){
        heroControl.DodgeDirection(HeroControl.directions.SW);
        return true;
    }

    bool wDodge(){
        heroControl.DodgeDirection(HeroControl.directions.W);
        return true;
    }
    
    bool nwDodge(){
        heroControl.DodgeDirection(HeroControl.directions.NW);
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

    /*
    This happens automatically as a form of the tree
    // move hero out of danger
    public bool moveToSafety()
    {
        Debug.Log("Dodge");
        // find closest square that is not under attack
        // move player to that square
        state.heroMove.movePoint.position += new Vector3(0f, 0f, 0f);
        return true;
    }
*/