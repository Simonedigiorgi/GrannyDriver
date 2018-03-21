using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuestManager : MonoBehaviour
{
    public enum MainMission { None, Main1, Main2 }
    public MainMission mainMissions;

    private GameManager gameManager;
    private CarController carController;

    [Header("Main Mission")]
    public Text mainDescription;
    public Image descriptionImage;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        carController = FindObjectOfType<CarController>();
    }

    void Update()
    {
        // MAIN MISSION CONDITIONS

        if (mainMissions == MainMission.None)
        {
            mainDescription.text = ("Go to the Auto Repair");
        }

        if (mainMissions == MainMission.Main1)
        {
            Mission1();
        }

        if (mainMissions == MainMission.Main2)
        {
            Mission2();
        }
    }

    // MAIN MISSIONS

    public void Mission1()                                                                  // Mission (1)
    {
        bool isCompleted;

        mainDescription.text = ("Collect all back pain pills " + gameManager.collectables + "/5");

        if (gameManager.collectables == 5)
        {
            isCompleted = true;
        }
        else
        {
            isCompleted = false;
        }

        if (isCompleted)
        {
            mainDescription.text = ("Completed");
            mainDescription.color = Color.red;
        }
    }

    public void Mission2()
    {
        bool isCompleted;

        descriptionImage.transform.DOMoveX(10, 1);
        mainDescription.text = "Go to the Auto Repair";

        if (gameManager.collectables == 5)
        {
            isCompleted = true;
        }
        else
        {
            isCompleted = false;
        }

        if (isCompleted)
        {
            mainDescription.text = ("Completed");
            mainDescription.color = Color.red;
        }
    }
}
