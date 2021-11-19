using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{
    public Transform swipeRightPoint;
    public Transform swipeLeftPoint;
    public Transform centerPoint;
    public LayerMask enemyLayers;
    public int beamLength = 8;
    public int aoeSize = 1;
    public int swipeSize = 2;
    public bool showSwipe = true;
    public bool showBeams = true;
    public bool showAOE = true;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SwipeRight();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            SwipeLeft();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Beams();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AOE();
        }
    }
    void SwipeRight()
    {
        //Play animation 

        //collect the objects hit
        Collider2D[] hitHero = Physics2D.OverlapBoxAll(swipeRightPoint.position, new Vector2(swipeSize, swipeSize+1), 0,enemyLayers);
        
        //send meesage to Hero/ hit the Hero
        foreach (Collider2D Hero in hitHero)
        {
            Debug.Log("hit " + Hero.name + " with Swipe Right");
        }
    }

    void SwipeLeft()
    {
        //Play animation 

        //collect the objects hit
        Collider2D[] hitHero = Physics2D.OverlapBoxAll(swipeLeftPoint.position, new Vector2(swipeSize, swipeSize+1), 0, enemyLayers);

        //send meesage to Hero/ hit the Hero
        foreach (Collider2D Hero in hitHero)
        {
            Debug.Log("hit " + Hero.name + " with Swipe Left");
        }
    }

    void AOE()
    {
        //Play animation 

        //collect the objects hit
        Collider2D[] hitHero = Physics2D.OverlapBoxAll(centerPoint.position, new Vector2(aoeSize, aoeSize), 0, enemyLayers);

        //send meesage to Hero/ hit the Hero
        foreach (Collider2D Hero in hitHero)
        {
            Debug.Log("hit " + Hero.name + " with AOE");
        }
    }

    void Beams()
    {
        //Play animation 

        //collect the objects hit
        Collider2D[] hitHero = Physics2D.OverlapBoxAll(centerPoint.position, new Vector2(beamLength, 1), 0, enemyLayers);
        //send meesage to Hero/ hit the Hero
        foreach (Collider2D Hero in hitHero)
        {
            Debug.Log("hit " + Hero.name + " with Beams horizontal");
        }

        hitHero = Physics2D.OverlapBoxAll(centerPoint.position, new Vector2(1, beamLength), 0, enemyLayers);

        foreach (Collider2D Hero in hitHero)
        {
            Debug.Log("hit " + Hero.name + " with Beams vertical");
        }
    }

    //This is called to draw attack boxes when the boss is selected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if(swipeRightPoint != null && showSwipe)
        {
            
            Gizmos.DrawWireCube(swipeRightPoint.position, new Vector3(swipeSize+1, swipeSize+2, 1));
        }

        if (swipeLeftPoint != null && showSwipe)
        {
            Gizmos.DrawWireCube(swipeLeftPoint.position, new Vector3(swipeSize+1, swipeSize+2, 1));
        }

        if (centerPoint != null && showAOE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(centerPoint.position, new Vector3(aoeSize + 1, aoeSize + 1, 1));
        }

        if (centerPoint != null && showBeams)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(centerPoint.position, new Vector3(2, beamLength, 1));
            Gizmos.DrawWireCube(centerPoint.position, new Vector3(beamLength, 2, 1));
        }
        

    }
}
