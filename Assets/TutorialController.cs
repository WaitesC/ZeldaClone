using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public List<GameObject> tutorialPages;

    GameManager gameManager;

    int currentPageNum, totalPageNum;

    bool pressedButtonDown;

    // Start is called before the first frame update
    void Start()
    {
        pressedButtonDown = false;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        currentPageNum = 0;
        totalPageNum = tutorialPages.Count;

        foreach (GameObject tutorialPage in tutorialPages)
            tutorialPage.SetActive(false);

        tutorialPages[currentPageNum].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //set pages visible and not visible
        tutorialPages[currentPageNum].SetActive(true);
        if (currentPageNum != tutorialPages.Count - 1)
            tutorialPages[currentPageNum + 1].SetActive(false);
        if (currentPageNum != 0)
            tutorialPages[currentPageNum - 1].SetActive(false);

        //reset button
        if (gameManager.tutorialTime && Input.GetAxisRaw("Horizontal") == 0)
        {
            pressedButtonDown = false;
        }

        //go up a page
        if (gameManager.tutorialTime && Input.GetAxisRaw("Horizontal") > 0)
        {
            if (currentPageNum < tutorialPages.Count - 1)
            {
                if (pressedButtonDown == false)
                {
                    currentPageNum += 1;

                    pressedButtonDown = true;

                }
            }
        }

        //go down a page
        if (gameManager.tutorialTime && Input.GetAxisRaw("Horizontal") < 0)
        {
            if (currentPageNum > 0)
            {
                if (pressedButtonDown == false)
                {
                    currentPageNum -= 1;

                    pressedButtonDown = true;

                }
            }

        }

        //end tutorial
        if (currentPageNum == tutorialPages.Count - 1 && Input.GetAxisRaw("Horizontal") > 0)
        {

            if (pressedButtonDown == false)
            {
                gameManager.tutorialTime = false;

                pressedButtonDown = true;
            }
        }

        if (gameManager.tutorialTime == false)
        {
            //foreach (GameObject tutorialPage in tutorialPages)
            //    tutorialPage.SetActive(false);

            tutorialPages[currentPageNum].SetActive(false);
            EndTutorial();
        }



        if (currentPageNum == totalPageNum)
        {

        }
    }

    void EndTutorial()
    {
        gameManager.tutorialTime = false;
    }
}
