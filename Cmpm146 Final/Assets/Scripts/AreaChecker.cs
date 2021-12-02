using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaChecker : MonoBehaviour
{
    private GameState gs;
    private CircleCollider2D myCollider;
    private BossAttacks bossAtk;
    float radius;

    private void Start() {
        gs = FindObjectOfType<GameState>();
        myCollider = GetComponent<CircleCollider2D>();
        bossAtk = FindObjectOfType<BossAttacks>();
        radius = myCollider.radius;
    }

    //Returns true if space is unobstructed and safe
    public bool checkDanger(){
        //Debug.Log("Checking Danger");
        //Is anything in the space I occupy?
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, Vector3.zero, Mathf.Infinity, ~LayerMask.GetMask("Hero")); 
        
        if(hit.collider != null && hit.collider!=myCollider){
            return false;
        }

        //Checks to determine if the current space could fall under attack
        if(gs.BossAtkCheck(gameObject.name, bossAtk.currAttack)){
            return false;
        }
        return true;
    }

}
