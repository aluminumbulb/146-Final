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
    
    public float distBtwn;
    public float bossHealth;
    public BossAttacks bossAtk;
    HeroZones HeroZone;
    public Movement heroMove;
    private float prevBossHealth, currBossHealth;
    private Vector2 prevHeroPos, currHeroPos;
    public float lightDmg = 10;
    public float heavyDmg = 20;
    private BehaviorTree bt;
    //Just shows what state we're in with a little more readability
    public enum turn {BOSS_DECISION, HERO_DECISION, ACTION};
    public turn currTurn = turn.BOSS_DECISION;

    public float secPerTurn = 1f;//Largely used for testing

    void Start()
    {
        //Initializng values
        bossAtk = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossAttacks>();
        HeroZone = FindObjectOfType<HeroZones>();
        heroMove = GameObject.FindObjectOfType<Movement>();

        currHeroPos = prevHeroPos = hero.position;
        currBossHealth = prevBossHealth = bossHealth;

        distBtwn = Vector3.Distance(hero.position, boss.position);

        bt = FindObjectOfType<BehaviorTree>();

        StartCoroutine(turnOrder());
        
    }

    // Update is called once per frame
    void Update()
    {
        //ive been using this in update to check that attack boxes are correct - David
        //Debug.Log("HeroMoveE was hit by LSwipe: " + BossAtkCheck("HeroMoveE", "SwipeLeft"));
        //this shows all hit move points in debug
        //BossAtkCheck("ShowAllHit", "Beams");
        //this one doesnt need a debug since the debug 
        //HeroAtkCheck("Boss")
    }

    //This should basically enforce turn order
    IEnumerator turnOrder()
    {
        while(!gameOver())
        {
            if (currTurn == turn.BOSS_DECISION)
            {
                //Debug.Log("Boss Decision Turn");
                // Wait for user to confirm their input
                // this infinite loops, whoops
                /*
                bool done = false;
                while (!done)
                {
                    if (bossAtk.inputGiven)
                    {
                        done = true;
                        currTurn = turn.HERO_DECISION;
                    }
                }
                yield return null;
                */
                currTurn = turn.HERO_DECISION;
            }

            if(currTurn == turn.HERO_DECISION)
            {
                distBtwn = Vector3.Distance(hero.position, boss.position);
                //Debug.Log("Hero Decision Turn");
                bt.execute();
                currTurn = turn.ACTION;
            }

            if(currTurn == turn.ACTION)
            {
                //Debug.Log("Time for Action");
                //At this phase, we should check the result of the boss' move on the hero
                currTurn = turn.BOSS_DECISION;
            }
            //Test to slow everything down for better analysis:
            yield return new WaitForSeconds(secPerTurn);
        }
        Debug.Log("Exited pipe");
        StopAllCoroutines();
    }

    public bool HeroAtkCheck(string obj)
    {
        return HeroZone.Attack(obj);
    }

    public bool BossAtkCheck(string obj, string atk)
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


    /*
        Returns X difference between hero and boss
    */
    public float transDeltX()
    {
        //The amount that needs to be done, constrained, times the amound it might help
        //Same signs should maximize the value, while opposing signs will minimize it
        //(i.e. preferred direction should arise)
        float xDiff = hero.position.x - boss.position.x;
        return xDiff;
    }

    public float transDeltY()
    {
        float yDiff = hero.position.y - boss.position.y;
        return yDiff;
    }

    
}
