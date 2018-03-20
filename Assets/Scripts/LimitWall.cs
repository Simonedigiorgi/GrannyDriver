using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitWall : MonoBehaviour {


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("CarTraffic"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }

}
