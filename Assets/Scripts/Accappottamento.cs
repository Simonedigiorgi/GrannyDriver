using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accappottamento : MonoBehaviour {

    public GameManager gameManager;                                                                 // Get the Game Manager Component
    private CarController carController;                                                            // Get the CarController Script

    void Start () {

        carController = GetComponentInParent<CarController>();
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Plane")
        {
            carController.isActive = false;
            //StartCoroutine(gameManager.AccidentCountdown());                                         
        }
    }
}
