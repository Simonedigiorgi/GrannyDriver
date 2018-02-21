using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IA : MonoBehaviour {

    public enum Directions { Horizontal, Vertical }

    private GameObject Player;                                                  // GIOCATORE
    public GameManager gameManager;                                             // GAME MANAGER
    public QuestManager questManager;                                           // QUEST MANAGER

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
    public AudioClip crashSound;                                                // Suono dell'incidente
    public AudioClip hornSound;                                                 // Suono del clacson
    public float generalVolume = 0.3f;                                          // Volume generale
    private bool hasAudioTriggered;
    public bool isHornActive;                                                   // Suona il clacson quando ha un incidente

    [Header("DEBUG")]
    public bool isActive = true;                                                // E' l'oggetto attivo?
    [TextArea]
    public string infoIsParking = "Quando isIAParking è attivo se l'auto si trova nella ParkingArea inizializza un bool nel GameManager chiamato isIAParkingTrue, se isIAParkingTrue == True allora attiva la fine della quest su QuestManager";
    public bool isIAParking;
    public bool isFirstImpact = true;

    void Start () {

        Player = GameObject.Find("Player");                                     // Cerca l'oggetto con TAG "Player"
        source = GetComponent<AudioSource>();
	}

    private void Update()
    {
        #region Directions

        if (directions == Directions.Vertical && isActive)
        {
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
            
        }

        if (directions == Directions.Horizontal && isActive)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        #endregion                                                              

        if(gameManager.isStopAllIA == true)
        {
            speed = 0;
        }
    }

    #region On Collision

    public void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Player" || collision.gameObject.tag == "Environment" || collision.gameObject.tag == "CarTraffic") && isFirstImpact == true)
        {
            StartCoroutine("Crash");
        }

        if ((collision.gameObject.tag == "Player" || collision.gameObject.tag == "Buildings" || collision.gameObject.tag == "CarTraffic") && !hasAudioTriggered)
        {
            gameManager.comboHit++;                                                 // Aumenta la combo di 1
            hasAudioTriggered = true;
            source.PlayOneShot(crashSound, generalVolume);

            if (isHornActive)                                                       // Suona il clacson?
            {
                source.PlayOneShot(hornSound, generalVolume);
            }
        }

        if (collision.gameObject.tag == "Buildings")
        {
            StartCoroutine("CrashWithoutExplosionForce");
        }

    }
    #endregion

    #region On Trigger Enter

    public void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Traffico")
        {
            if (directions == Directions.Vertical && isActive)
            {
                transform.position = new Vector3(transform.position.x - positionX, transform.position.y, positionZ);
            }

            if (directions == Directions.Horizontal && isActive)
            {
                transform.position = new Vector3(positionX - positionX, transform.position.y, transform.position.x - positionZ);
            }
        }

        if (other.gameObject.tag == "ParkingArea" && isIAParking == true)
        {
            gameManager.isIAParkingTrue = true;
        }

    }
    #endregion

    #region Coroutines

    IEnumerator Crash()
    {
        GetComponent<Rigidbody>().AddExplosionForce(explosionForce, Player.transform.position, explosionRadius, explosionJump, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        isActive = false;

        isFirstImpact = false;

        // Se attivo stacca le ruote dall'auto

        /*List<Transform> childs = new List<Transform>();

        childs.Add(transform.GetChild(0));
        childs.Add(transform.GetChild(1));
        childs.Add(transform.GetChild(2));
        childs.Add(transform.GetChild(3));


        for (int i = 0; i < childs.Count; i++)
        {
            childs[i].gameObject.AddComponent<Rigidbody>();
            childs[i].parent = null;
        }*/
    }

    IEnumerator CrashWithoutExplosionForce()
    {       
        yield return new WaitForSeconds(0.1f);
        isActive = false;
    }
    #endregion
}
