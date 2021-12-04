using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform[] targets;
    private Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        transform.position = GetCenterPoint();
        cam.orthographicSize = 2 + GetGreatestDistance();
    }

    float GetGreatestDistance(){
         Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
       
        foreach(Transform target in targets){
            bounds.Encapsulate(target.position);
        }

        if(bounds.size.x>bounds.size.y){
            return bounds.size.x;
        }
        //else
        return bounds.size.y;
    }

    Vector3 GetCenterPoint(){
        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
       
        foreach(Transform target in targets){
            bounds.Encapsulate(target.position);
        }

        return bounds.center + new Vector3(0,0,-10);
    }
}
