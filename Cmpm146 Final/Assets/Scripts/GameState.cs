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
    public Transform hero, boss;
    private Vector3 currHeroPos, prevHeroPos;
    public float distBtwn;
    public float bossHealth;
    BossAttacks bossAtk;
    private float prevBossHealth, currBossHealth;
    public float lightDmg = 10;
    public float heavyDmg = 20;
    private BehaviorTree bt;

    //Just shows what state we're in with a little more readability
    enum turn {BOSS_DECISION, HERO_DECISION, ACTION };
    turn currTurn = turn.BOSS_DECISION;

    public bool inputPlaced = false;
    void Start()
    {
        //Initializng values
        bossAtk = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossAttacks>();
        currHeroPos = prevHeroPos = hero.position;
        currBossHealth = prevBossHealth = bossHealth;

        distBtwn = Vector3.Distance(hero.position, boss.position);

        bt = FindObjectOfType<BehaviorTree>();

        StartCoroutine(turnOrder());
        Debug.Log("Hero was hit by Beams: " + BossAtkCheck("Hero", "Beams"));
        Debug.Log("Hero was hit by AOE: " + BossAtkCheck("Hero", "AOE"));
    }

    //This should basically enforce turn order
    IEnumerator turnOrder()
    {
        while(!gameOver())
        {
            if (currTurn == turn.BOSS_DECISION)
            {
                Debug.Log("Boss Decision Turn");
                //Wait for user to confirm their input
                while (!inputPlaced) //This can't be a key down due to timing problems
                {
                    inputPlaced = true;//Have some other function do this
                   yield return null;
                }
                currTurn = turn.HERO_DECISION;
            }

            if(currTurn == turn.HERO_DECISION)
            {
                Debug.Log("Hero Decision Turn");
                bt.execute();
                currTurn = turn.ACTION;
            }

            if(currTurn == turn.ACTION)
            {
                Debug.Log("Time for Action");
                //At this phase, we should check the result of the boss' move on the hero
                currTurn = turn.BOSS_DECISION;
            }
            yield return null;
        }
        Debug.Log("Exited pipe");
    }

    bool BossAtkCheck(string obj, string atk)
    {
        //check which atk is being used and get the hitObjs from it
        if (atk == "SwipeRight") 
        {
            return bossAtk.SwipeRight(obj);
        } 
        else if (atk == "SwipeLeft") 
        {
            return bossAtk.SwipeLeft(obj);
        }
        else if(atk == "AOE") 
        {
            return bossAtk.AOE(obj);
        } 
        else if (atk == "Beams") 
        {
            return bossAtk.Beams(obj);
        }
        //return false if atk doesnt exist
        return false;
    }

    //A function to hold all of our "game over" conditions
    bool gameOver()
    {
        if(bossHealth <= 0)
        {
            return true;
        }

        return false;
    }

    //We need to be certain this only gets called once per tree check
    void UpdateStateValues()
    {
        prevHeroPos = currHeroPos;
        currHeroPos = hero.position;

        prevBossHealth = currBossHealth;
        currBossHealth = bossHealth;

        distBtwn = Vector3.Distance(hero.position, boss.position);
    }

    public float getDeltX()
    {
        return currHeroPos.x - prevHeroPos.y;
    }

    public float getDeltY()
    {
        return currHeroPos.x - prevHeroPos.y;
    }

    public float getDeltHealth()
    {
        return currBossHealth - prevBossHealth;
    }
}
