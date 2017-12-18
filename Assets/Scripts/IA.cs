using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IA : MonoBehaviour {

    public enum Directions { Horizontal, Vertical }

    private GameObject Player;
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
    public bool isActive = true;                                                      // E' l'oggetto attivo?
    [TextArea]
    public string infoIsParking = "Quando isParking è attivo se l'auto si trova nella ParkingArea inizializza un int nel GameManager chiamato ParkingTime, se ParkingTime è >= 5 allora attiva la fine della quest su QuestManager";
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

        /*#region Parking                                                         

        if(gameManager.IAParkingTime >= 5)
        {
            Debug.Log("Limo su Piscina");
        }
        #endregion  */                                                               

        if(gameManager.isStopAllIA == true)
        {
            StopCars();
        }
    }

    #region On Collision

    public void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Player" || collision.gameObject.tag == "Environment" || collision.gameObject.tag == "CarTraffic") && isFirstImpact == true)
        {
            StartCoroutine("Crash");
        }

        if ((collision.gameObject.tag == "Player" || collision.gameObject.tag == "Objects" || collision.gameObject.tag == "CarTraffic") && !hasAudioTriggered)
        {
            gameManager.comboHit++; // Aumenta la combo di 1
            hasAudioTriggered = true;
            source.PlayOneShot(crashSound, volume);

            if (isHornActive)       // Suona il clacson?
            {
                source.PlayOneShot(hornSound, volume);
            }
        }

        if (collision.gameObject.tag == "Objects")
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
                transform.position = new Vector3(transform.position.x - positionX, transform.position.y, transform.position.z - positionZ);

            }

            if (directions == Directions.Horizontal && isActive)
            {
                transform.position = new Vector3(transform.position.x - positionX, transform.position.y, transform.position.z - positionZ);
            }

        }

        if (other.gameObject.tag == "ParkingArea" && isIAParking == true)
        {
            StartCoroutine(IAParkingCoroutine());
        }

    }
    #endregion

    #region On Trigger Exit

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ParkingArea" && isIAParking == true)
        {
            StopCoroutine(IAParkingCoroutine());
            gameManager.IAParkingTime = 0;
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
        //GetComponent<Rigidbody>().AddExplosionForce(explosionForce, Player.transform.position, explosionRadius, explosionJump, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        isActive = false;

    }

    IEnumerator IAParkingCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            gameManager.IAParkingTime++;
        }
    }
    #endregion

    public void StopCars()
    {
        speed = 0;
    }
}
