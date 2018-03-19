using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {

    private CarController carController;

    void Start () {
        carController = GetComponentInParent<CarController>();
    }

    public void OnTriggerStay(Collider other)                                           // Il collider tocca il terreno
    {
        if (other.gameObject.tag == "Plane")
        {
            carController.isOnGround = true;
        }

        if (other.gameObject.CompareTag("Water"))
        {
            carController.isGrannyDriving = false;
            carController.isOnGround = true;
        }
    }

    public void OnTriggerExit(Collider other)                                           // Il collider NON tocca il terreno
    {
        if (other.gameObject.tag == "Plane")
        {
            carController.isOnGround = false;
        }

        if (other.gameObject.CompareTag("Water"))
        {
            carController.isOnGround = false;
            carController.isGrannyDriving = true;
        }
    }

}
