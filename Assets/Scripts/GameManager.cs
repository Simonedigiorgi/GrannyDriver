using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private AudioSource source;

    private GameObject Player;                                              // GIOCATORE
    private QuestManager questManager;                                      // QUEST MANAGER

    [Space(10)]
    public Text timerText;                                                  // Testo del Countdown
    public Text accidentText;
    public int timeLeft = 30;                                               // Tempo rimanente
    public int bonusTime = 0;                                               // Tempo Bonus
    public int accidentTime = 20;                                           // Tempo per gli incidenti

    [Space(10)]
    public Text collectableText;                                            // Testo degli oggetti collezionati
    public int collectables = 0;                                            // Collezionabili

    [Space(10)]
    public Text comboText;                                                  // Testo degli incidenti
    //public Text comboNumberText;                                          // Testo del numero della combo
    public int comboHit = 0;                                                // Ammontare della combo

    [Space(10)]
    public Text reverseText;                                                // Testo della Retromarcia

    [Space(10)]
    public Text resultsTitleText;                                           // Testo della Vittoria
    public Text results;

    [Space(10)]    
    public Image fadeImage;                                                 // Immagine FADEIN/FADEOUT   

    [Space(10)]
    public Button quitGame;
    public Button restartGame;
    public Button mainMenu;

    [Space(10)]
    public AudioClip carEngineSound;
    public AudioClip musicSound;
    public float volume = 0.3f;
    [HideInInspector] public int IAParkingTime = 0;                         // L'IA ha parcheggiato
    public bool isIAParkingTrue;


    [Header("DEBUG")]
    public bool isMusicActive;                                              // Attiva la Musica (DEBUG)
    public bool isStopAllIA;                                                // Setta la speed dell'IA su 0


    void Start () {

        Player = GameObject.Find("Player");                                 // Cerca l'oggetto con TAG "Player"
        Player.GetComponent<CarController>().enabled = false;               // Disabilita il Giocatore
        source = GetComponent<AudioSource>();
        questManager = FindObjectOfType<QuestManager>();

        fadeImage.enabled = true;                                           // Attiva l'immagine del fade
        StartCoroutine("STARTGAME");                                        // Coroutine "STARTGAME", inizializza anche la couroutine del Timer


    }
	
	void Update () {

        #region Activate Music

        if (isMusicActive)
        {
            source.enabled = true;
        }
        else
        {
            source.enabled = false;
        }
        #endregion

        #region Combo System

        if (comboHit >= 2)
        {
            comboText.transform.DOMoveX(94, 0.3f);
            comboText.text = (" x" + comboHit);
        }
        #endregion

        #region Countdown

        timerText.text = ("" + timeLeft);

        if (timeLeft <= 10)
        {
            timerText.color = Color.red;
        }

        if (timeLeft <= 0)
        {
            StopCoroutine("Countdown");
            timerText.text = "" + timeLeft;
            StartCoroutine("LOSER");
        }
        #endregion

        #region Accidents

        accidentText.text = ("" + accidentTime);

        if (accidentTime <= 0)
        {
            StopCoroutine("AccidentCountdown");
            accidentText.text = "0";
            StartCoroutine("LOSER");
        }
        #endregion

        // Mostra il testo dei Collezionabili

        collectableText.text = ("Collectables = " + collectables);

        // Condizione di Vittoria

        if (collectables == 5)
        {
            StopCoroutine("Countdown");
            StartCoroutine("VICTORY");
        }

        // Risultati Partita

        results.text = ("Time: " + timeLeft + "  Collectables: " + collectables + "  Combo: " + comboHit);
    }

    #region Coroutines

    IEnumerator Countdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }

    public IEnumerator AccidentCountdown()
    {
        yield return new WaitForSeconds(0.1f);
        Player.GetComponent<CarController>().isActive = false;                          // Disattiva il Player
        Player.GetComponent<CarController>().Acceleration = 0f;                         // Ferma l'accellerazione del Player
        

        StopCoroutine("Countdown");

        yield return new WaitForSeconds(2);

        accidentText.transform.DOMoveY(150, 0.3f);

        while (true)
        {
            yield return new WaitForSeconds(1);
            accidentTime--;
        }
    }

    public IEnumerator STARTGAME()
    {

        yield return new WaitForSeconds(2);
        fadeImage.DOFade(0, 1);
        yield return new WaitForSeconds(1);
        fadeImage.enabled = false;                                                       // Disabilita il FadeImage
        source.PlayOneShot(carEngineSound);
        yield return new WaitForSeconds(2.5f);
        source.PlayOneShot(musicSound, volume);
        StartCoroutine("Countdown");
        Player.GetComponent<CarController>().enabled = true;
    }

    public IEnumerator VICTORY()
    {
        resultsTitleText.text = ("Well Done!!");
        resultsTitleText.transform.DOLocalMoveX(0, 1);
        Player.GetComponent<CarController>().enabled = false;                            // Disabilita il Player
        yield return new WaitForSeconds(2);
        fadeImage.enabled = true;
        fadeImage.DOFade(1, 1);

    }

    public IEnumerator LOSER()
    {
        yield return new WaitForSeconds(2);
        fadeImage.enabled = true;
        fadeImage.DOFade(1, 0.5f);
        isStopAllIA = true;                                                             // Ferma l'IA
        yield return new WaitForSeconds(1);

        resultsTitleText.text = ("");
        resultsTitleText.transform.DOLocalMoveX(0, 1);
        yield return new WaitForSeconds(1);
    }

    public IEnumerator RestartGame()
    {
        fadeImage.enabled = true;
        results.DOFade(0, 0.5f);
        quitGame.image.DOFade(0, 0.5f);
        restartGame.image.DOFade(0, 0.5f);
        mainMenu.image.DOFade(0, 0.5f);
        fadeImage.DOFade(1, 0.5f);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main");
    }

    public IEnumerator MainMenu()
    {
        fadeImage.enabled = true;
        results.DOFade(0, 0.5f);
        quitGame.image.DOFade(0, 0.5f);
        restartGame.image.DOFade(0, 0.5f);
        mainMenu.image.DOFade(0, 0.5f);
        fadeImage.DOFade(1, 0.5f);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
    }
    #endregion

    #region HUD OnClick

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        StartCoroutine(RestartGame());
    }

    public void Menu()
    {
        StartCoroutine(MainMenu());
    }
    #endregion

}
