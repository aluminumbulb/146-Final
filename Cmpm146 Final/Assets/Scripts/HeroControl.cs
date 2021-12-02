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
    Vector3 heroStart;
    // Start is called before the first frame update
    void Start()
    {
        heroStart = transform.position;
    }

    //Determines result of hero taking damage
    public void heroHit(){
        Debug.Log("Hero Hit!");
        respawn();
    }

    //Places the hero back to where they started 
    //Functions as "Death"
    public void respawn(){
        transform.position = heroStart;
    }

    public void MoveNorth(){
        transform.position += transform.up * moveDist;
    }

    public void MoveSouth(){
        transform.position -= transform.up * moveDist;
    }
    
    public void MoveWest(){
        //transform.position -= transform.right * moveDist;
        Debug.Log("Moved west");
        transform.position = moveZones[(int)directions.W].transform.position;
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
        Debug.Log("Move Check "+dir+ " for danger");
        if(moveZones[(int)dir].checkDanger()){//Just a test
            Debug.Log("Move "+dir+" is clear");
        }
        return moveZones[(int)dir].checkDanger();
    }

    public bool CheckDodgePoss(directions dir){
        Debug.Log("Dodge Check "+dir+ " for danger");
        //Indexes the proper moveZone in that direction for danger
        if(dodgeZones[(int)dir].checkDanger()){//Just a test
            Debug.Log("Doodge "+dir+" is clear");
        }
        return dodgeZones[(int)dir].checkDanger();
    }

}
