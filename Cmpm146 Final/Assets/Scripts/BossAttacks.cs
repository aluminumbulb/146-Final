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
    public string currAttack;
    public bool inputGiven;
    private HeroControl heroControl;
    public Renderer AOERenderer;
    public Renderer swipeLeftRenderer;
    public Renderer swipeRightRenderer;
    public Renderer beamsRenderer;
    public Renderer beamsRenderer2;
    public int aoe_cooldown = 3;
    public int sl_cooldown = 3;
    public int sr_cooldown = 3;
    public int beams_cooldown = 3;
    int aoeCurrCool;
    int slCurrCool;
    int srCurrCool;
    int beamsCurrCool;

    private void Start() {
        heroControl = FindObjectOfType<HeroControl>();
        aoeCurrCool = aoe_cooldown;
        slCurrCool = sl_cooldown;
        srCurrCool = sr_cooldown;
        beamsCurrCool = beams_cooldown;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (sr_cooldown == srCurrCool)
            {
                currAttack = "SwipeRight";
                inputGiven = true;
                srCurrCool = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (sl_cooldown == slCurrCool)
            {
                currAttack = "SwipeLeft";
                inputGiven = true;
                slCurrCool = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (beams_cooldown == beamsCurrCool)
            {
                currAttack = "Beams";
                inputGiven = true;
                beamsCurrCool = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (aoe_cooldown == aoeCurrCool)
            {
                currAttack = "AOE";
                inputGiven = true;
                aoeCurrCool = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currAttack = "Nothing";
            inputGiven = true;
        }
    }

    public void tickdown()
    {
        if (aoe_cooldown > aoeCurrCool) { aoeCurrCool += 1; }
        if (sl_cooldown > slCurrCool) { slCurrCool += 1; }
        if (sr_cooldown > srCurrCool) { srCurrCool += 1; }
        if (beams_cooldown > beamsCurrCool) { beamsCurrCool += 1; }
    }

    public bool Nothing(string obj)
    {
        return false;
    }

    public bool SwipeRight(string obj)
    {
        //Play animation 
        swipeRightRenderer.enabled = true;

        //collect the objects hit
        Collider2D[] hitHero = Physics2D.OverlapBoxAll(swipeRightPoint.position, new Vector2(swipeSize, swipeSize+1), 0,enemyLayers);

        //send meesage to Hero/ hit the Hero
        foreach (Collider2D Hero in hitHero)
        {
            if (obj == "ShowAllHit")
            {
                Debug.Log("hit " + Hero.name + " with Swipe Right");
            }
            if (Hero.name == obj)
            {
                return true;
            }
        }
        return false;
    }

    public bool SwipeLeft(string obj)
    {
        //Play animation 
        swipeLeftRenderer.enabled = true;

        //collect the objects hit
        Collider2D[] hitHero = Physics2D.OverlapBoxAll(swipeLeftPoint.position, new Vector2(swipeSize, swipeSize+1), 0, enemyLayers);

        //send meesage to Hero/ hit the Hero
        foreach (Collider2D Hero in hitHero)
        {
            if (obj == "ShowAllHit")
            {
                Debug.Log("hit " + Hero.name + " with Swipe Left");
            }
            if (Hero.name == obj)
            {
                return true;
            }
        }
        return false;
    }

    public bool AOE(string obj)
    {
        //Play animation 
        AOERenderer.enabled = true;

        //collect the objects hit
        Collider2D[] hitHero = Physics2D.OverlapBoxAll(centerPoint.position, new Vector2(aoeSize, aoeSize), 0, enemyLayers);

        //send meesage to Hero/ hit the Hero
        foreach (Collider2D Hero in hitHero)
        {
            if (obj == "ShowAllHit")
            {
                Debug.Log("hit " + Hero.name + " with AOE");
            }
            if (Hero.name == obj)
            {
                return true;
            }
        }
        return false;
    }

    public bool Beams(string obj)
    {
        //Play animation 
        beamsRenderer.enabled = true;
        beamsRenderer2.enabled = true;

        //collect the objects hit
        Collider2D[] hitHero = Physics2D.OverlapBoxAll(centerPoint.position, new Vector2(beamLength, 1), 0, enemyLayers);

        //send meesage to Hero/ hit the Hero
        foreach (Collider2D Hero in hitHero)
        {
            if (obj == "ShowAllHit")
            {
                Debug.Log("hit " + Hero.name + " with Beams horizontal");
            }
            if (Hero.name == obj)
            {
                return true;
            }
        }

        hitHero = Physics2D.OverlapBoxAll(centerPoint.position, new Vector2(1, beamLength), 0, enemyLayers);

        foreach (Collider2D Hero in hitHero)
        {
            if (obj == "ShowAllHit")
            {
                Debug.Log("hit " + Hero.name + " with Beams vertical");
            }
            if (Hero.name == obj)
            {
                return true;
            }
        }
        return false;
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

    public void clearRends()
    {
        swipeRightRenderer.enabled = false;
        swipeLeftRenderer.enabled = false;
        AOERenderer.enabled = false;
        beamsRenderer.enabled = false;
        beamsRenderer2.enabled = false;
    }
}
