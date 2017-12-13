using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private AudioSource source;
    public GameObject Player;                                               // GIOCATORE

    [Space(10)]
    public Text countdownText;                                              // Testo del Countdown
    public int timeLeft = 30;                                               // Tempo rimanente
    public int bonusTime = 0;                                               // Tempo Bonus

    [Space(10)]
    public Text collectableText;                                            // Testo degli oggetti collezionati
    public int collectables = 0;                                            // Collezionabili

    [Space(10)]
    public Text comboText;                                                  // Testo degli incidenti
    public Text comboNumberText;                                            // Testo del numero della combo
    public int comboHit = 0;                                                // Ammontare della combo


    [Space(10)]
    public Text reverseText;                                                // Testo della Retromarcia
    public Text resultsTitleText;                                           // Testo della Vittoria
    public Text results;

    [Space(10)]    
    public SpriteRenderer fadeImage;                                        // Immagine FADEIN/FADEOUT       

    // AUDIO

    private float volume = 0.3f;

    public AudioClip carEngineSound;
    public AudioClip musicSound;

    // Booleane per Coroutines

    [Header("(DEBUG")]
    public bool isStartGame;
    public bool isLoser;
    public bool isYouWin;

    void Start () {

        source = GetComponent<AudioSource>();

        // Abilita/Disabilita Sprites e Testi

        HUDhide();
        fadeImage.enabled = true;

        // Disabilita il Giocatore

        Player.GetComponent<CarController>().enabled = false;

        // Inizializza la coroutine,
        //STARTGAME inizializza anche la couroutine del Timer

        StartCoroutine("STARTGAME");
    }
	
	void Update () {

        // COMBO SYSTEM

        if (comboHit == 2)
        {
            comboText.text = ("Double Combo!!");
            comboNumberText.text = ("" + comboHit);
            comboNumberText.fontSize = 64;
        }

        if (comboHit == 3)
        {
            comboText.text = ("Triple Combo!!");
            comboNumberText.text = ("" + comboHit);
            comboNumberText.fontSize = 68;
        }

        if (comboHit == 4)
        {
            comboText.text = ("Quaternion Combo!!");
            comboNumberText.text = ("" + comboHit);
            comboNumberText.fontSize = 72;
        }

        if (comboHit == 5)
        {
            comboText.text = ("Destruction Combo!!");
            comboNumberText.text = ("" + comboHit);
            comboNumberText.fontSize = 76;
        }

        if (comboHit >= 6)
        {
            comboText.text = ("Beastly Combo!!");
            comboNumberText.text = ("" + comboHit);
            comboNumberText.fontSize = 80;
        }

        // Mostra il testo del Timer ed il tempo rimanente

        countdownText.text = ("Time Left = " + timeLeft);

        results.text = ("Time: " + timeLeft + " - Collectables: " + collectables + " - Combo: " + comboHit);

        if (timeLeft <= 0)
        {
            StopCoroutine("Countdown");
            countdownText.text = "Your time: " + timeLeft;
            StartCoroutine("LOSER");
        }

        // Mostra il testo dei Collezionabili

        collectableText.text = ("Collectables = " + collectables);

        // Condizione di Vittoria

        if (collectables == 5)
        {
            StopCoroutine("Countdown");
            StartCoroutine("VICTORY");
        }

    }

    // COUNTDOWN

    IEnumerator Countdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }

    // HUD ELEMENTS

    public void HUDhide()
    {
        resultsTitleText.enabled = false;
        collectableText.enabled = false;
        comboText.enabled = false;
        countdownText.enabled = false;
    }

    // START GAME

    public IEnumerator STARTGAME()
    {
        // Inizializza le animazioni dell'HUD

        yield return new WaitForSeconds(2);
        fadeImage.DOFade(0, 1);
        collectableText.enabled = true;
        comboText.enabled = true;
        countdownText.enabled = true;

        // Inizializza l'audio

        yield return new WaitForSeconds(1);
        source.PlayOneShot(carEngineSound);
        yield return new WaitForSeconds(2.5f);
        source.PlayOneShot(musicSound);

        // Inizializza la Coroutine del Timer

        StartCoroutine("Countdown");

        // Abilita il Player

        Player.GetComponent<CarController>().enabled = true;
    }

    // YOU WIN

    public IEnumerator VICTORY()
    {
        // Mostra il testo della vittoria

        resultsTitleText.text = ("Well Done!!");
        resultsTitleText.enabled = true;
        resultsTitleText.transform.DOLocalMoveX(0, 1);

        // disabilita il Player

        Player.GetComponent<CarController>().enabled = false;

        yield return new WaitForSeconds(2);

        // Inizializza le animazioni dell'HUD

        fadeImage.DOFade(1, 1);
        countdownText.DOFade(0, 1);
        collectableText.DOFade(0, 1);
        comboText.DOFade(0, 1);
        reverseText.DOFade(0, 1);

        yield return new WaitForSeconds(3);

        resultsTitleText.DOFade(0, 1);
        results.DOFade(0, 1);

        yield return new WaitForSeconds(5);

        // Carica la scena

        SceneManager.LoadScene("Main");
    }

    // YOU LOSE
    public IEnumerator LOSER()
    {
        yield return new WaitForSeconds(0.1f);

        // Disattiva il Player

        Player.GetComponent<CarController>().isActive = false;

        // Ferma l'accellerazione del Player

        Player.GetComponent<CarController>().Acceleration = 0f;

        // Ferma la Coroutine del Timer

        StopCoroutine("Countdown");

        yield return new WaitForSeconds(8);                                             // Tempo da dare agli incidenti

        fadeImage.DOFade(1, 0.5f);
        countdownText.DOFade(0, 0.5f);
        comboText.DOFade(0, 0.5f);
        collectableText.DOFade(0, 0.5f);
        reverseText.DOFade(0, 0.5f);

        yield return new WaitForSeconds(1);

        // Carica la scena

        SceneManager.LoadScene("Main");

    }
}
