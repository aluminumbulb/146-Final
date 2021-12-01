using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaChecker : MonoBehaviour
{
    private CircleCollider2D myCollider;

    private void Start() {
        myCollider = GetComponent<CircleCollider2D>();
    }

    //Returns true if space is safe
    public bool checkDanger(){
        //Is anything in the space I occupy?

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, myCollider.radius, Vector3.zero); 
        Debug.Log("Cast with a result of: "+hit.collider);
        if(hit.collider!=null && hit.collider != myCollider){
            Debug.Log("Collision Detected!");
            return false;
        }

        return true;
    }

}
