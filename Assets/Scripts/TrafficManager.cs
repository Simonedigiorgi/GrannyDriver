using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TrafficManager : MonoBehaviour {

    private Rigidbody rb;

    [BoxGroup("Respawn position")] public Transform spawnPoint;
    [BoxGroup("Controls")] public float speed;

    private bool isMoving = true;
    private bool isFirstImpact = true;

    void Start () {
        rb = GetComponent<Rigidbody>();
	}

    private void Update()
    {
        if (isMoving)
            transform.Translate(Vector3.forward * -speed * Time.deltaTime);
    }

    #region On Collision
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("CarTraffic") || collision.gameObject.CompareTag("Buildings"))
        {
            isMoving = false;

            if (isFirstImpact)
            {
                FindObjectOfType<AudioManager>().Play("Crash");
                isFirstImpact = false;
                rb.isKinematic = false;
                rb.velocity = transform.TransformDirection(0, 16, -6);
            }
        }
    }
    #endregion

    #region On Trigger Enter
    public void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Traffico"))
            transform.position = spawnPoint.transform.position;
    }
    #endregion
}
