using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This script will hold all the points of contact between the tree and the player.
    Currently, it is used to move the hero around with the tree
*/
public class TreeMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MoveUp(){
        transform.position += transform.up;
    }

    
    public void MoveDown(){
        transform.position -= transform.up;
    }

    
    public void MoveLeft(){
        transform.position -= transform.right;
    }

    
    public void MoveRight(){
        transform.position += transform.right;
    }

}
