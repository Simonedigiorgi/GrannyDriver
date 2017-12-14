using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IA : MonoBehaviour {

    private GameObject Player;
    private GameObject ParkingArea;

    public enum Directions { Horizontal, Vertical }
    public GameManager gameManager;
    public QuestManager questManager;
    private AudioSource source;
    private Rigidbody rb;

    [Header("Directions and Speed")]
    public Directions directions;
    public float speed;

    [Header("Respawn Positions")]
    public float positionX;                                                     // Pooling Positions
    public float positionZ;                                                     // Pooling Positions

    [Header("Explosion")]
    public float explosionForce;                                                // Forza dell'esplosione
    public float explosionRadius;                                               // Radius dell'esplosione
    public float explosionJump;                                                 // Spinta dell'esplosione (Y)

    [Header("Audio and Volume")]
    public AudioClip crashSound;
    public AudioClip hornSound;                                                 // Suono del clacson
    public float volume = 0.3f;
    private bool hasAudioTriggered;
    public bool isHornActive;

    [Header("DEBUG")]
    public bool isMoving = true;

    void Start () {

        Player = GameObject.FindGameObjectWithTag("Player");                    // Cerca l'oggetto con TAG "Player"

        source = GetComponent<AudioSource>();

	}

    private void Update()
    {


        if (directions == Directions.Vertical && isMoving)
        {
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
            
        }

        if (directions == Directions.Horizontal && isMoving)
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
            gameManager.comboHit++; // Aumenta la combo di 1
            hasAudioTriggered = true;
            source.PlayOneShot(crashSound, volume);

            if (isHornActive)       // Suona il clacson?
            {
                source.PlayOneShot(hornSound, volume);
            }
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Traffico")
        {

            if (directions == Directions.Vertical && isMoving)
            {
                transform.position = new Vector3(transform.position.x - positionX, transform.position.y, transform.position.z - positionZ);

            }

            if (directions == Directions.Horizontal && isMoving)
            {
                transform.position = new Vector3(transform.position.x - positionX, transform.position.y, transform.position.z - positionZ);
            }

        }

    }

    IEnumerator Crash()
    {

        yield return new WaitForSeconds(0.1f);
        isMoving = false;

        List<Transform> childs = new List<Transform>();

        childs.Add(transform.GetChild(0));
        childs.Add(transform.GetChild(1));
        childs.Add(transform.GetChild(2));
        childs.Add(transform.GetChild(3));


        for (int i = 0; i < childs.Count; i++)
        {
            childs[i].gameObject.AddComponent<Rigidbody>();
            childs[i].parent = null;
            childs[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, Player.transform.position, explosionRadius, explosionJump, ForceMode.Impulse);
        }
    }

}
