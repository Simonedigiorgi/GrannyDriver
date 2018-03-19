using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    private AudioSource source;
    private Animator anim;

    public AudioClip dieSound;

	void Start () {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
	}
	
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        source.PlayOneShot(dieSound);

        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("die", true);
        }
    }
}
