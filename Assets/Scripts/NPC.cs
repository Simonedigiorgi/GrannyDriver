using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {

    private AudioSource source;
    private Animator anim;
    private Camera mainCamera;

    public bool startScene;
    public bool isFirstTime = true;

    public AudioClip dieSound;

	void Start () {
        mainCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
	}
	
	void Update () {
        if (startScene)
        {
            mainCamera.orthographicSize = mainCamera.orthographicSize - 8 * Time.deltaTime;
            if (mainCamera.orthographicSize < 4)
            {
                mainCamera.orthographicSize = 4;
            }
        }
        /*else if(startScene == false)
        {
            // Distanza della Main Camera (FOV) Ritorno

            mainCamera.orthographicSize = mainCamera.orthographicSize + 8 * Time.deltaTime;
            if (mainCamera.orthographicSize > 16)
            {
                mainCamera.orthographicSize = 16;
            }

        }*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        source.PlayOneShot(dieSound);

        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("die", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isFirstTime)
            {
                StartCoroutine(CutScene());
                isFirstTime = false;
            }

        }
    }

    public IEnumerator CutScene()
    {
        FindObjectOfType<CarController>().Acceleration = 0;
        FindObjectOfType<CarController>().isActive = false;
        startScene = true;
        yield return new WaitForSeconds(5);
        startScene = false;
        FindObjectOfType<CarController>().isActive = true;
    }
}
