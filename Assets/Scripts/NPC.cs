using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

public class NPC : MonoBehaviour {

    private AudioSource source;
    private Animator anim;
    private Camera mainCamera;
    public Image popup;

    public bool isCutScene;
    public bool startScene;
    public bool isFirstTime = true;

    public AudioClip dieSound;

	void Start () {
        mainCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
	}
	
	void Update () {

        if (isCutScene)
        {
            if (startScene)
            {
                mainCamera.orthographicSize = mainCamera.orthographicSize - 8 * Time.deltaTime;
                if (mainCamera.orthographicSize < 4)
                {
                    mainCamera.orthographicSize = 4;
                }
            }
            else if (startScene == false)
            {
                // Distanza della Main Camera (FOV) Ritorno

                mainCamera.orthographicSize = mainCamera.orthographicSize + 8 * Time.deltaTime;
                if (mainCamera.orthographicSize > 16)
                {
                    mainCamera.orthographicSize = 16;
                }

            }
        }


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
                isCutScene = true;
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
        yield return new WaitForSeconds(1);
        popup.GetComponent<RectTransform>().DOScale(1, 0.2f);
        yield return new WaitForSeconds(8);
        startScene = false;

        FindObjectOfType<QuestManager>().Mission2();
        FindObjectOfType<CarController>().isActive = true;
        popup.GetComponent<RectTransform>().DOScale(0, 0.2f);
    }
}
