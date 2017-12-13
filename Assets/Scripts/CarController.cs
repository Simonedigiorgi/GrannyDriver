using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;


public class CarController : MonoBehaviour
{
    //[HideInInspector]
    public GameObject camera;
    public GameManager GM;
    private AudioSource source;

    [Header("Car Stats")]
    [Space(10)]
    public float MaxSpeed = 15.0f;                                          // Velocità Massima
    public float MaxSteer = 4.0f;                                           // Sterzata Massima
    public float Breaks = 0.2f;                                             // Freni

    [SerializeField]
    public float Acceleration = 0.0f;                                       // Accellerazione
    public float Steer = 1.0f;                                              // Sterzata

    public float accSpeed = 0.4f;                                           // Velocità di Accellerazione
    public float decSpeed = 0.4f;                                           // Velocità Retromarcia
    public float steerSpeed = 0.06f;                                        // Velocità di sterzata
    public float accBrake = 0.4f;                                           // Velocità Freni

    bool AccelFwd, AccelBwd;
    bool SteerLeft, SteerRight;

    [HideInInspector]
    public bool isActive;                                                   // Il Player è attivo                                      
    bool isMad;                                                             // E' in guida spericolata                
    int randomNumber;                                                       // Direzioni della guida spericolata

    [Header("(DEBUG)")]
    public bool isGrannyDriving = true;                                     // Lasciare attivo per attivare la guida spericolata (DEBUG)

    //[Header("Audio")]
    //[Space(10)]
    //public AudioClip Crash;
    private bool isCrashsnd;                                                // Has sound crash triggered? (bool that trigger only ONE time)
    public bool isOnGround;                                                 // Is the Car colliding the Ground? (Both frontal wheels)

    public AudioClip VoiceCrash;
    public AudioClip VoiceCrash2;
    public AudioClip Brakes;

    float volume = 0.3f;                                                    // Modifica il volume dell'audio



    void Start()
    {

        //isGrannyDriving = true;
        
        source = GetComponent<AudioSource>();
        isActive = true;

        InvokeRepeating("RandomDirection", 0, 0.3f);

    }

    private void Update()
    {
        // PREMI "R" PER LA RETROMARCIA

        if (Input.GetKeyDown(KeyCode.R) && GM.reverseText.enabled == false)
        {
            GM.reverseText.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.R) && GM.reverseText.enabled == true)
        {
            GM.reverseText.enabled = false;
        }

        // Granny Driving

        if (Acceleration > MaxSpeed && isGrannyDriving == true)
        {
            isMad = true;

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
            
            isMad = false;

        }

    }

    void FixedUpdate()
    {


        // COMANDI DI MOVIMENTO

        if (isActive == true && isOnGround)
        {
            if (Input.GetKey(KeyCode.W) && GM.reverseText.enabled == false)
            {
                Accel(1);                                                   //Accelerate in forward direction

            }

            else if (Input.GetKey(KeyCode.S) && GM.reverseText.enabled == true)
            {
                Accel(-1);                                                  //Accelerate in backward direction
            }

            else if (Input.GetKey(KeyCode.Space))
            {
                if (AccelFwd)
                {
                    StopAccel(1, Breaks);                                   //Breaks while in forward direction
                }

                else if (AccelBwd)
                {
                    StopAccel(-1, Breaks);                                  //Breaks while in backward direction
                }
            }
            else
            {
                if (AccelFwd)
                {
                    StopAccel(1, accBrake);                                     //Applies breaks slowly if no key is pressed while in forward direction
                }

                else if (AccelBwd)
                {
                    StopAccel(-1, accBrake);                                    //Applies breaks slowly if no key is pressed while in backward direction
                }

            }


        }

    }

    // ACCELLERATION
    #region Acceleration

    public void Accel(int Direction)
    {
        if (Direction == 1)
        {
            AccelFwd = true;

            if (Acceleration <= MaxSpeed)
            {
                Acceleration += accSpeed;
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
            AccelBwd = true;

            if ((-1 * MaxSpeed) <= Acceleration)
            {
                Acceleration -= decSpeed;
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

    // STOP ACCELLERATION
    #region Stop Acceleration

    public void StopAccel(int Direction, float BreakingFactor)
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
                AccelFwd = false;
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
                AccelBwd = false;
            }

        }

        if (Steer >= 1.0f)
        {
            Steer -= steerSpeed;
        }

        transform.Translate(Vector3.back * Acceleration * Time.deltaTime);
    }
    #endregion


    // ON COLLISION

    public void OnCollisionEnter(Collision collision)
    {
        // stacca e applica forza alle ruote

        if(collision.gameObject.tag == "Objects" && !isCrashsnd)
        {

            // Avvicina la telecamera **********************************************************

            //camera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 60, this.transform.position.z);


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

            source.PlayOneShot(VoiceCrash);
            StartCoroutine(GM.LOSER());
            isCrashsnd = true;
        }
    }

    public void RandomDirection()
    {
        randomNumber = Random.Range(0, 2);
    }



}
