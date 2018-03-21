using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    private GameManager gameManager;
    private AudioSource source;
    private Camera mainCamera;
    private Rigidbody rb;

    [Header("Car Stats")]
    [Space(10)]
    public float accellerationSpeed = 0.4f;                                 // Velocità di Accellerazionens
    public float decellerationSpeed = 0.4f;                                 // Accellerazione Retromarcia
    public float MaxSpeed = 12.0f;                                          // Velocità Massima
    [Space(10)]
    public float steerSpeed = 0.06f;                                        // Velocità di sterzata
    public float Steer = 1.0f;                                              // Sterzata
    public float MaxSteer = 4.0f;                                           // Sterzata Massima
    [Space(10)]
    public float accellerationBrakes = 0.4f;                                // Reazione dei Freni
    public float Break = 0.2f;                                              // Freni

    bool AccellerationForward;
    bool AccellerationBackwards;
    bool SteerLeft;
    bool SteerRight;
           
    int randomNumber;                                                       // Direzioni della guida spericolata

    [HideInInspector]  public bool isOnGround;                              // Is the Car colliding the Ground? (Both frontal wheels)

    [Header("Audio")]
    public AudioClip grannyAudio1;
    public AudioClip grannyAudio2;
    public AudioClip BrakesAudio;
    public AudioClip CrashAudio;

    [Space(10)]
    public float generalVolume = 0.3f;                                      // Modifica il volume generale

    [Header("(DEBUG)")]
    public bool isActive;                                                   // Il Player è attivo  
    [SerializeField] public float Acceleration = 0.0f;                      // Mostra l'accellerazione

    public bool isGrannyDriving = true;                                     // Lasciare attivo per attivare la guida spericolata (DEBUG)
    private bool isFirstImpact = true;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
        source = GetComponent<AudioSource>();
        isActive = true;                                                    // Attiva il Player
        InvokeRepeating("RandomDirection", 0, 0.3f);                        // Richiama il metodo RandomDirection e sceglie una direzione casuale
    }

    private void Update()
    {
        /*#region Retromarcia

        if (Input.GetKeyUp(KeyCode.R))
        {
            gameManager.reverseText.enabled = !gameManager.reverseText.enabled;
        }

        #endregion*/

        #region Granny Driving

        if (Acceleration > MaxSpeed && isGrannyDriving == true)
        {
            // Distanza della Main Camera (FOV) Andata

            if (FindObjectOfType<NPC>().isCutScene == false)
            {
                mainCamera.orthographicSize = mainCamera.orthographicSize - 4 * Time.deltaTime;
                if (mainCamera.orthographicSize < 8)
                {
                    mainCamera.orthographicSize = 8;
                }
            }

            if (randomNumber == 0)
            {               
                transform.Rotate(Vector3.down * 5);
            }
            else
            {
                transform.Rotate(Vector3.up * 5);
            }
        }
        else
        {
            // Distanza della Main Camera (FOV) Ritorno

            if (FindObjectOfType<NPC>().isCutScene == false)
            {
                mainCamera.orthographicSize = mainCamera.orthographicSize + 4 * Time.deltaTime;
                if (mainCamera.orthographicSize > 16)
                {
                    mainCamera.orthographicSize = 16;
                }
            }

            // Particles

            transform.GetChild(2).GetComponentInChildren<ParticleSystem>().Play();
            transform.GetChild(3).GetComponentInChildren<ParticleSystem>().Play();           
        }
        #endregion
    }

    void FixedUpdate()
    {      
        #region Comani di Movimento

        if (isActive == true && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
                StartAccelleration(1);                                      // Accelera in direzione Forward

            else if (Input.GetKey(KeyCode.S))
                StartAccelleration(-1);                                     // Accelera in direzione Backward

            else if (Input.GetKey(KeyCode.Space))
            {
                if (AccellerationForward)
                    StopAccelleration(1, Break);                            // Frena nella direzione Forward

                else if (AccellerationBackwards)
                    StopAccelleration(-1, Break);                           // Frena nella direzione Backward
            }
            else
            {
                if (AccellerationForward)
                    StopAccelleration(1, accellerationBrakes);              //Applies breaks slowly if no key is pressed while in forward direction

                else if (AccellerationBackwards)
                    StopAccelleration(-1, accellerationBrakes);             //Applies breaks slowly if no key is pressed while in backward direction
            }
        }
        #endregion
    }

    #region Start Acceleration

    public void StartAccelleration(int Direction)
    {
        if (Direction == 1)                                                 // Direzione Forward
        {
            AccellerationForward = true;

            if (Acceleration <= MaxSpeed)
                Acceleration += accellerationSpeed;

            if (Input.GetKey(KeyCode.A))
                transform.Rotate(Vector3.down * Steer);                     // Sterza a Sinistra

            if (Input.GetKey(KeyCode.D))
                transform.Rotate(Vector3.up * Steer);                       // Sterza a Destra
        }

        else if (Direction == -1)                                           // Direzione Backward
        {
            AccellerationBackwards = true;

            if ((-1 * MaxSpeed) <= Acceleration)
                Acceleration -= decellerationSpeed;

            if (Input.GetKey(KeyCode.A))
                transform.Rotate(Vector3.up * Steer);                       // Sterza a sinistra mentre sei in retromarcia

            if (Input.GetKey(KeyCode.D))
                transform.Rotate(Vector3.down * Steer);                     // Sterza a destra mentre sei in retromarcia
        }

        if (Steer <= MaxSteer)
            Steer += steerSpeed;

        transform.Translate(Vector3.back * Acceleration * Time.deltaTime);
    }
    #endregion

    #region Stop Acceleration

    public void StopAccelleration(int Direction, float BreakingFactor)
    {
        if (Direction == 1)
        {
            if (Acceleration >= 0.0f)
            {
                Acceleration -= BreakingFactor;

                if (Input.GetKey(KeyCode.A))
                    transform.Rotate(Vector3.down * Steer);

                if (Input.GetKey(KeyCode.D))
                    transform.Rotate(Vector3.up * Steer);
            }
            else
            {
                AccellerationForward = false;
            }

        }
        else if (Direction == -1)
        {
            if (Acceleration <= 0.0f)
            {
                Acceleration += BreakingFactor;

                if (Input.GetKey(KeyCode.A))
                    transform.Rotate(Vector3.up * Steer);

                if (Input.GetKey(KeyCode.D))
                    transform.Rotate(Vector3.down * Steer);
            }
            else
            {
                AccellerationBackwards = false;
            }

        }

        if (Steer >= 1.0f)
            Steer -= steerSpeed;

        transform.Translate(Vector3.back * Acceleration * Time.deltaTime);
    }
    #endregion

    #region On Collision Enter

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CarTraffic"))
        {
            isActive = false;
            Acceleration = 0;

            if (isFirstImpact)
            {
                source.PlayOneShot(grannyAudio1, 0.3f);
                FindObjectOfType<AudioManager>().Play("Crash");
                isFirstImpact = false;

                StartCoroutine(gameManager.AccidentCountdown());
                rb.velocity = transform.TransformDirection(0, 12, -6);
            }
        }

        if (collision.gameObject.CompareTag("Buildings"))
        {
            isActive = false;
            Acceleration = 0;

            if (isFirstImpact)
            {
                source.PlayOneShot(grannyAudio1, 0.3f);
                FindObjectOfType<AudioManager>().Play("Crash");
                isFirstImpact = false;

                StartCoroutine(gameManager.AccidentCountdown());
                rb.velocity = transform.TransformDirection(0, 4, -6);
            }
        }
    }
    #endregion

    #region Methods

    public void RandomDirection()
    {
        randomNumber = Random.Range(0, 2);
    }
    #endregion

}
