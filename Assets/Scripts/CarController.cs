using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class CarController : MonoBehaviour
{
    public GameManager gameManager;
    private AudioSource source;

    [Header("Car Stats")]
    [Space(10)]
    public float accellerationSpeed = 0.4f;                                 // Velocità di Accellerazione
    public float decellerationSpeed = 0.4f;                                 // Accellerazione Retromarcia
    public float MaxSpeed = 15.0f;                                          // Velocità Massima
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
                                  
    bool isMad;                                                             // E' in guida spericolata                
    int randomNumber;                                                       // Direzioni della guida spericolata

    private bool isCrashsnd;                                                // Has sound crash triggered? (bool that trigger only ONE time)
    [HideInInspector]  public bool isOnGround;                              // Is the Car colliding the Ground? (Both frontal wheels)

    [Header("Audio")]
    public AudioClip grannyAudio1;
    public AudioClip grannyAudio2;
    public AudioClip BrakesAudio;

    [Space(10)]
    public float volume = 0.3f;                                             // Modifica il volume dell'audio

    [Header("(DEBUG)")]
    public bool isActive;                                                   // Il Player è attivo  
    public bool isGrannyDriving = true;                                     // Lasciare attivo per attivare la guida spericolata (DEBUG)
    public bool isWearingGlasses = true;                                    // Mostra i testi nella sua grandezza reale

    [SerializeField] public float Acceleration = 0.0f;                      // Accellerazione

    void Start()
    {

        source = GetComponent<AudioSource>();
        isActive = true;                                                    // Attiva il Player
        InvokeRepeating("RandomDirection", 0, 0.3f);                        // Richiama la RandomDirection e sceglie una direzione casuale
    }

    private void Update()
    {
        #region Retromarcia

        if (Input.GetKeyDown(KeyCode.R) && gameManager.reverseText.enabled == false)
        {
            gameManager.reverseText.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.R) && gameManager.reverseText.enabled == true)
        {
            gameManager.reverseText.enabled = false;
        }
        #endregion

        #region Granny Driving

        if (Acceleration > MaxSpeed && isGrannyDriving == true)
        {
            //isMad = true;

            if(randomNumber == 0)
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
            
            //isMad = false;

        }
        #endregion

    }

    void FixedUpdate()
    {      
        #region Comani di Movimento

        if (isActive == true && isOnGround)
        {
            if (Input.GetKey(KeyCode.W) && gameManager.reverseText.enabled == false)
            {
                StartAccelleration(1);              //Accelerate in forward direction

            }

            else if (Input.GetKey(KeyCode.S) && gameManager.reverseText.enabled == true)
            {
                StartAccelleration(-1);             //Accelerate in backward direction
            }

            else if (Input.GetKey(KeyCode.Space))
            {
                if (AccellerationForward)
                {
                    StopAccelleration(1, Break);    //Breaks while in forward direction
                }

                else if (AccellerationBackwards)
                {
                    StopAccelleration(-1, Break);   //Breaks while in backward direction
                }
            }
            else
            {
                if (AccellerationForward)
                {
                    StopAccelleration(1, accellerationBrakes);                      //Applies breaks slowly if no key is pressed while in forward direction
                }

                else if (AccellerationBackwards)
                {
                    StopAccelleration(-1, accellerationBrakes);                     //Applies breaks slowly if no key is pressed while in backward direction
                }

            }


        }
        #endregion

    }

    #region Start Acceleration

    public void StartAccelleration(int Direction)
    {
        if (Direction == 1)
        {
            AccellerationForward = true;

            if (Acceleration <= MaxSpeed)
            {
                Acceleration += accellerationSpeed;
            }


            if (Input.GetKey(KeyCode.A) && !isMad)
            {
                transform.Rotate(Vector3.down * Steer);              //Steer left
            }


            if (Input.GetKey(KeyCode.D)  && !isMad)
            {
                transform.Rotate(Vector3.up * Steer);                 //steer right
            }

        }

        else if (Direction == -1)
        {
            AccellerationBackwards = true;

            if ((-1 * MaxSpeed) <= Acceleration)
            {
                Acceleration -= decellerationSpeed;
            }


            if (Input.GetKey(KeyCode.A) && !isMad)
            {
                transform.Rotate(Vector3.up * Steer);                 //Steer left (while in reverse direction)
            }


            if (Input.GetKey(KeyCode.D) && !isMad)
            {
                transform.Rotate(Vector3.down * Steer);              //Steer left (while in reverse direction)
            }

       
        }

        if (Steer <= MaxSteer)
        {
            Steer += steerSpeed;
        }

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

                if (Input.GetKey(KeyCode.A) && !isMad)
                {
                    transform.Rotate(Vector3.down * Steer);
                }


                if (Input.GetKey(KeyCode.D) && !isMad)
                {
                    transform.Rotate(Vector3.up * Steer);
                }

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

                if (Input.GetKey(KeyCode.A) && !isMad)
                {
                    transform.Rotate(Vector3.up * Steer);

                }

                if (Input.GetKey(KeyCode.D) && !isMad)
                {
                    transform.Rotate(Vector3.down * Steer);

                }

            }
            else
            {
                AccellerationBackwards = false;
            }

        }

        if (Steer >= 1.0f)
        {
            Steer -= steerSpeed;
        }

        transform.Translate(Vector3.back * Acceleration * Time.deltaTime);
    }
    #endregion

    #region On Collision Enter

    public void OnCollisionEnter(Collision collision)
    {
        // stacca e applica forza alle ruote

        if(collision.gameObject.tag == "Objects" && !isCrashsnd)
        {
            List<Transform> childs = new List<Transform>();

            childs.Add(transform.GetChild(0));
            childs.Add(transform.GetChild(1));
            childs.Add(transform.GetChild(2));
            childs.Add(transform.GetChild(3));


            for (int i = 0; i < childs.Count; i++)
            {
                childs[i].gameObject.AddComponent<Rigidbody>();
                childs[i].parent = null;
                childs[i].GetComponent<Rigidbody>().AddExplosionForce(40, transform.position, 40, 10 ,ForceMode.VelocityChange);
            }                                                        

            // Inizializza coroutine

            source.PlayOneShot(grannyAudio1);
            StartCoroutine(gameManager.LOSER());
            isCrashsnd = true;
        }



    }
    #endregion

    public void RandomDirection()
    {
        randomNumber = Random.Range(0, 2);
    }

}
