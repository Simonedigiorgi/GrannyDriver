using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitWall : MonoBehaviour {


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CarTraffic"))
        {
            Debug.Log("Collision");
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
