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
    Vector3 heroStart;
    // Start is called before the first frame update
    void Start()
    {
        heroStart = transform.position;
        Debug.Log("Hero start set to: "+heroStart);
    }

    //Determines result of hero taking damage
    public void heroHit(){
        respawn();
    }

    //Places the hero back to where they started 
    //Functions as "Death"
    public void respawn(){
        transform.position = heroStart;
    }

    //Uses the movement checkers for a more accurate guide as to where it's moving
    public void MoveDirection(directions dir){
        transform.position = moveZones[(int)dir].transform.position;
    }

    public void DodgeDirection(directions dir){
        transform.position = dodgeZones[(int)dir].transform.position;
    }

    //Takes in the cardinal direction we want to check
    public bool CheckMovePoss(directions dir){
        //Indexes the proper moveZone in that direction for danger
        return moveZones[(int)dir].checkDanger();
    }

    public bool CheckDodgePoss(directions dir){
        Debug.Log("Dodge Check "+dir+ " for danger");
        //Indexes the proper moveZone in that direction for danger
        return dodgeZones[(int)dir].checkDanger();
    }

}
