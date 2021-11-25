using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroZones : MonoBehaviour
{
    public Transform AtkPoint;
    public LayerMask enemyLayers;
    public bool ShowAtk;

    public bool IsAoe;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool Attack(string obj)
    {
        //Play animation 
        float width = (float)1.5;
        float length = (float)0.5;

        if (IsAoe)
        {
            width += (float)0.5;
            length += (float)1.5;
        }

        //collect the objects hit
        Collider2D[] hitBoss = Physics2D.OverlapBoxAll(AtkPoint.position, new Vector2(width, length), 0, enemyLayers);

        //send meesage to Hero/ hit the Hero
        foreach (Collider2D Enemy in hitBoss)
        {
            Debug.Log("Hero hit " + Enemy.name);
        }

        foreach (Collider2D Hero in hitBoss)
        {
            if (Hero.name == obj)
            {
                return true;
            }
        }

        hitBoss = Physics2D.OverlapBoxAll(AtkPoint.position, new Vector2(length, width), 0, enemyLayers);

        //send meesage to Hero/ hit the Hero
        foreach (Collider2D Enemy in hitBoss)
        {
            Debug.Log("Hero hit " + Enemy.name);
        }

        foreach (Collider2D Hero in hitBoss)
        {
            if (Hero.name == obj)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        float width = (float)1.5;
        float length = (float)0.5;

        if (IsAoe)
        {
            width += (float)0.5;
            length += (float)1.5;
        }

        Gizmos.color = Color.red;
        if (AtkPoint != null && ShowAtk)
        {
            Gizmos.DrawWireCube(AtkPoint.position, new Vector3(width, length, 1));
            Gizmos.DrawWireCube(AtkPoint.position, new Vector3(length, width, 1));
        }
    }
}
