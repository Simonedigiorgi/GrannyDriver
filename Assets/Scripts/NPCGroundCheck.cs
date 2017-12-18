using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGroundCheck : MonoBehaviour {

    private IA IA;

    void Start()
    {
        IA = GetComponentInParent<IA>();
    }

    public void OnTriggerExit(Collider other)                                           // Il collider NON tocca il terreno
    {
        if (other.gameObject.tag == "Plane")
        {
            IA.isActive = false;
        }
    }
}
