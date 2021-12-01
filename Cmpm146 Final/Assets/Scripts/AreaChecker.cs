using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaChecker : MonoBehaviour
{
    private CircleCollider2D myCollider;
    float radius;

    private void Start() {
        myCollider = GetComponent<CircleCollider2D>();
        radius = myCollider.radius;
    }

    //Returns true if space is safe
    public bool checkDanger(){
        //Is anything in the space I occupy?
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, Vector3.zero, Mathf.Infinity ,~LayerMask.GetMask("Hero")); 
        
        if(hit.collider != null && hit.collider!=myCollider){
            return false;
        }

        return true;
    }

}
