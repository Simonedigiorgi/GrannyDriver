using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

    private CarController CC;

    void Start () {
        CC = GetComponentInParent<CarController>();
    }
	
    public void OnTriggerStay(Collider other)                                           // Il collider tocca il terreno
    {
        if (other.gameObject.tag == "Plane")
        {
            CC.isOnGround = true;
        }
    }

    public void OnTriggerExit(Collider other)                                           // Il collider NON tocca il terreno
    {
        if (other.gameObject.tag == "Plane")
        {
            CC.isOnGround = false;
        }
    }

    
}
