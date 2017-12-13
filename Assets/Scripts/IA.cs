using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IA : MonoBehaviour {

    public enum Directions { horizontal, vertical }

    public Directions directions;

    public GameManager GM;

    private Rigidbody rb;
    private AudioSource source;
    private bool hasAudioTriggered;

    public bool isMoving;

    public float speed;

    // Pooling Positions
    
    public float spawnPosition;


    public AudioClip crashSound;
    private float volume = 0.3f;

    void Start () {

        isMoving = true;
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
	}

    private void Update()
    {
        if (directions == Directions.vertical && isMoving)
        {
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
            
        }

        if (directions == Directions.horizontal && isMoving)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Objects")
        {
            StartCoroutine("Crash");
        }

        if ((collision.gameObject.tag == "Player" || collision.gameObject.tag == "Objects") && !hasAudioTriggered)
        {
            GM.comboHit++; // Aumenta la combo di 1
            hasAudioTriggered = true;
            source.PlayOneShot(crashSound, volume);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Traffico")
        {

            if (directions == Directions.vertical && isMoving)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - spawnPosition);

            }

            if (directions == Directions.horizontal && isMoving)
            {
                transform.position = new Vector3(transform.position.x - spawnPosition, transform.position.y, transform.position.z);
            }

        }
    }

    IEnumerator Crash()
    {


        yield return new WaitForSeconds(0.1f);
        isMoving = false;

        List<Transform> childs = new List<Transform>();

        childs.Add(transform.GetChild(0));
        //childs.Add(transform.GetChild(1));
        //childs.Add(transform.GetChild(2));
        childs.Add(transform.GetChild(3));


        for (int i = 0; i < childs.Count; i++)
        {
            childs[i].gameObject.AddComponent<Rigidbody>();
            childs[i].parent = null;
            childs[i].GetComponent<Rigidbody>().AddExplosionForce(40, transform.position, 40, 10, ForceMode.VelocityChange);
        }
    }

}
