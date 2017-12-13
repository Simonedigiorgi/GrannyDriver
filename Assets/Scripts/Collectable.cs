using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    //[HideInInspector]
    public GameManager GM;

    private AudioSource source;
    public AudioClip gotIt;

	void Start () {
        source = GetComponent<AudioSource>();
    }
	
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            source.PlayOneShot(gotIt);
            GM.collectables++;
            GM.timeLeft += GM.bonusTime;
            transform.position = new Vector3(0, 0, 100);
        }
    }

}
