using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accappottamento : MonoBehaviour {


    public GameManager GM;                                                      // Get the Game Manager Component

    private CarController CC;                                                   // Get the CarController Script


    void Start () {

        CC = GetComponentInParent<CarController>();
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Plane")
        {
            CC.isActive = false;                                                
            StartCoroutine(GM.LOSER());                                         
        }
    }
}
