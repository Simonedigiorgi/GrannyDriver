using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGroundCheck : MonoBehaviour {


    private IA traffic;

    void Start()
    {
        traffic = GetComponentInParent<IA>();
    }

    /*public void OnTriggerStay(Collider other)                                           // Il collider tocca il terreno
    {
        if (other.gameObject.tag == "Plane")
        {
            traffic.isMoving = true;
        }
    }*/

    public void OnTriggerExit(Collider other)                                           // Il collider NON tocca il terreno
    {
        if (other.gameObject.tag == "Plane")
        {
            traffic.isMoving = false;
        }
    }
}
