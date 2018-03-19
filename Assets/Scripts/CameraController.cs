using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    private Vector3 offset;

    public bool isFollowing = true;

	void Start () {
        offset = transform.position - player.transform.position;
    }
	
	// Update is called once per frame
	void LateUpdate () {

        if (isFollowing)
        {
            transform.position = player.transform.position + offset;
        }

    }
}
