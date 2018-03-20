using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficManager : MonoBehaviour {

    private GameManager gameManager;                                             // GAME MANAGER
    private QuestManager questManager;                                           // QUEST MANAGER
    private Rigidbody rb;

    private AudioSource source;                                    

    public float speed;

    [Header("Explosion")]
    public float explosionForce;                                                // Forza dell'esplosione
    public float explosionRadius;                                               // Radius dell'esplosione
    public float explosionJump;                                                 // Spinta dell'esplosione (Y)

    [Header("Audio and Volume")]
    public AudioClip crashSound;                                                // Suono dell'incidente
    public AudioClip hornSound;                                                 // Suono del clacson
    public float generalVolume = 0.3f;                                          // Volume generale
    private bool hasAudioTriggered;
    public bool isHornActive;                                                   // Suona il clacson quando ha un incidente

    [Header("DEBUG")]
    public bool isActive = true;                                                // E' l'oggetto attivo?
    [TextArea]
    public string infoIsParking = "Quando isIAParking è attivo se l'auto si trova nella ParkingArea inizializza un bool nel GameManager chiamato isIAParkingTrue, se isIAParkingTrue == True allora attiva la fine della quest su QuestManager";
    public bool hasExplosionForce;
    public bool isIAParking;
    public bool isFirstImpact = true;

    void Start () {

        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
        questManager = FindObjectOfType<QuestManager>();
        source = GetComponent<AudioSource>();
	}

    private void Update()
    {
        if (isActive)
        {
            transform.Translate(Vector3.forward * -speed * Time.deltaTime);
            //rb.velocity = (Vector3.forward * -speed * Time.deltaTime);
        }
    }

    #region On Collision
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isActive = false;
            gameManager.comboHit++;
            source.PlayOneShot(crashSound, generalVolume);
            //GetComponent<Rigidbody>().AddExplosionForce(12, FindObjectOfType<CarController>().transform.position, 12, 0, ForceMode.Impulse);
            rb.isKinematic = false;
        }

        if (collision.gameObject.CompareTag("CarTraffic"))
        {
            isActive = false;
            gameManager.comboHit++;
            source.PlayOneShot(crashSound, generalVolume);
            GetComponent<Rigidbody>().AddExplosionForce(12, FindObjectOfType<CarController>().transform.position, 12, 0, ForceMode.Impulse);
            rb.isKinematic = false;
        }

        if ((/*collision.gameObject.CompareTag("Player") || */collision.gameObject.CompareTag("Environment") 
            /*|| collision.gameObject.CompareTag("CarTraffic")*/ && isFirstImpact))
        {
            isActive = false;
            isFirstImpact = false;
            rb.isKinematic = false;

            if (hasExplosionForce)
                GetComponent<Rigidbody>().AddExplosionForce(explosionForce, FindObjectOfType<CarController>().transform.position, explosionRadius, explosionJump, ForceMode.Impulse);
        }

        if ((/*collision.gameObject.CompareTag("Player") || */collision.gameObject.CompareTag("Buildings") 
            /*|| collision.gameObject.CompareTag("CarTraffic")*/ && !hasAudioTriggered))
        {
            source.PlayOneShot(crashSound, generalVolume);
            gameManager.comboHit++;
            hasAudioTriggered = true;
            rb.isKinematic = false;

            if (isHornActive)
            {
                source.PlayOneShot(hornSound, generalVolume);
            }
        }

        if (collision.gameObject.CompareTag("Buildings"))
        {
            isActive = false;
        }
    }
    #endregion

    #region On Trigger Enter
    public void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Traffico"))
        {
            transform.position = transform.parent.GetChild(1).transform.position;
        }

        if (other.gameObject.CompareTag("ParkingArea") && isIAParking)
        {
            gameManager.isIAParkingTrue = true;
        }
    }
    #endregion
}
