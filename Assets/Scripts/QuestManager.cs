using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuestManager : MonoBehaviour
{

    public enum MainMission { None, Main1, Main2, Main3 }
    public enum SecondMission { None, Second1, Second2, Second3 }
    public enum ThirdMission { None, Third1, Third2, Third3 }

    public GameManager gameManager;


    public MainMission mainMissions;
    public SecondMission secondMission;
    public ThirdMission thirdMission;

    [Header("Main Mission")]
    public Text mainTitle;
    public Text mainDescription;

    [Header("Second Mission")]
    public Text secondTitle;
    public Text secondDescription;

    [Header("Third Mission")]
    public Text thirdTitle;
    public Text thirdDescription;


    void Start()
    {

    }

    void Update()
    {

        // MAIN MISSION CONDITIONS

        if (mainMissions == MainMission.None)
        {
            mainDescription.text = ("");
        }

        if (mainMissions == MainMission.Main1)
        {
            Mission1();
        }

        // SECOND MISSION CONDITIONS

        if (secondMission == SecondMission.None)
        {
            secondDescription.text = ("");
        }

        if (secondMission == SecondMission.Second1)
        {
            Secondary1();
        }

        // THIRD MISSION CONDITIONS

        if (thirdMission == ThirdMission.None)
        {
            thirdDescription.text = ("");
        }

        if (thirdMission == ThirdMission.Third1)
        {
            Third1();
        }

    }

    // MAIN MISSIONS

    public void Mission1()                                                                  // Mission (1)
    {
        bool isCompleted;

        //mainTitle.text = ("");
        mainDescription.text = ("Raccogli tutti i collezionabili");

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
            mainDescription.text = ("Raccogli tutti i collezionabili");
            mainDescription.color = Color.red;
        }

    }

    // SECONDARY MISSIONS

    public void Secondary1()                                                                // Secondary (1)
    {
        bool isCompleted;

        //secondaryTitle.text = ("");
        secondDescription.text = ("Ottieni una combo da 7");

        if (gameManager.comboHit >= 7)
        {
            isCompleted = true;
        }
        else
        {
            isCompleted = false;
        }

        if (isCompleted)
        {
            secondDescription.text = ("Ottieni una combo da 7");
            secondDescription.color = Color.red;
        }

    }

    // THIRD MISSIONS

    public void Third1()                                                                    // Third (1)
    {
        //bool isCompleted;

        //thirdTitle.text = ("");
        thirdDescription.text = ("Parcheggia la Limousine sul tetto");

        /*if (gameManager.comboHit >= 7)
        {
            isCompleted = true;
        }
        else
        {
            isCompleted = false;
        }

        if (isCompleted)
        {
            thirdDescription.text = ("Parcheggia la Limousine sul tetto");
            thirdDescription.color = Color.red;
        }*/

    }

}
