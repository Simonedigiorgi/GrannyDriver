using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public Transform pivotMain;
    public Transform pivotChallenges;
    public Transform pivotOptions;

    [Header("Button")]
    public Button challenges;
    public Button options;
    public Button quit;

    [Header("Images")]
    public Image fade;

	void Start () {

        fade.enabled = true;
        StartCoroutine("FadeIn");

	}
	
	void Update () {
		
	}

    public void Challenges()
    {
        pivotMain.DOMoveX(-600, 0.4f);
        pivotChallenges.DOMoveX(800, 0.4f);
    }

    public void Options()
    {
        pivotMain.DOMoveX(-600, 0.4f);
        pivotOptions.DOMoveX(800, 0.4f);
    }

    public void Back()
    {
        pivotMain.DOMoveX(800, 0.4f);
        pivotChallenges.DOMoveX(2400, 0.4f);
        pivotOptions.DOMoveX(2400, 0.4f);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1);

        fade.DOFade(0, 0.5f);

        yield return new WaitForSeconds(0.5f);

        fade.enabled = false;
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0);

        fade.enabled = false;
        fade.DOFade(1, 0.5f);

        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator LoadParkingCoroutine()
    {
        yield return new WaitForSeconds(0);

        fade.enabled = true;
        fade.DOFade(1, 0.5f);

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Main");
    }

    public void LoadParkingScene()
    {
        StartCoroutine("LoadParkingCoroutine");
    }

}
