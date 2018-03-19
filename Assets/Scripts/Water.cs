using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

    private Camera mainCamera;
    private bool onWater;                                                                           // E' in acqua

	void Start () {
        mainCamera = FindObjectOfType<Camera>();
	}

    private void Update()
    {
        // Riporta la telecamera alla sua grandezza normale

        if (onWater)
        {
            mainCamera.orthographicSize = mainCamera.orthographicSize + 4 * Time.deltaTime;
            if (mainCamera.orthographicSize > 16)
            {
                mainCamera.orthographicSize = 16;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<CameraController>().isFollowing = false;
            FindObjectOfType<CarController>().isActive = false;
            onWater = true;

            //waterSplash.transform.position = FindObjectOfType<CarController>().transform.position;
            //FindObjectOfType<CarController>().transform.GetChild(6).GetComponent<ParticleSystem>().Play();
        }
    }
}
