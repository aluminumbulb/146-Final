using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This script will hold all the points of contact between the tree and the player.
    Currently, it is used to move the hero around with the tree
*/
public class HeroControl : MonoBehaviour
{
    public enum directions{N,NE,E,SE,S,SW,W,NW};
    public AreaChecker[] moveZones;
    public AreaChecker[] dodgeZones; 
    public float moveDist = 1f, dodgeDist = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MoveNorth(){
        transform.position += transform.up * moveDist;
    }

    public void MoveSouth(){
        transform.position -= transform.up * moveDist;
    }
    
    public void MoveWest(){
        transform.position -= transform.right * moveDist;
    }

    public void MoveEast(){
        transform.position += transform.right * moveDist;
    }

    public void DodgeNorth(){
        transform.position += transform.up * dodgeDist;
    }
    
    public void DodgeSouth(){
        transform.position -= transform.up * dodgeDist;
    }
    
    public void DodgeWest(){
        transform.position -= transform.right * dodgeDist;
    }
    
    public void DodgeEast(){
        transform.position += transform.right * dodgeDist;
    }

    //Takes in the cardinal direction we want to check
    public bool CheckMovePoss(directions dir){
        //Indexes the proper moveZone in that direction for danger
        return moveZones[(int)dir].checkDanger();
    }

    public bool CheckDodgePoss(directions dir){
        //Indexes the proper moveZone in that direction for danger
        return dodgeZones[(int)dir].checkDanger();
    }

}
