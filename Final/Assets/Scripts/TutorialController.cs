using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public GameObject tutorial;
    public Button prevButton;
    public Button nextButton;
    public Text[] tutorialTexts;

    private int currIndex;
    private bool active;

    // Start is called before the first frame update
    void Start()
    {
        currIndex = 0;
        active = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Next()
    {
        tutorialTexts[currIndex++].gameObject.SetActive(false);
        tutorialTexts[currIndex].gameObject.SetActive(true);

        ShowButton();
    }

    public void Prev()
    {
        tutorialTexts[currIndex--].gameObject.SetActive(false);
        tutorialTexts[currIndex].gameObject.SetActive(true);

        ShowButton();
    }

    public void TurnOffOn()
    {
        tutorial.SetActive(!active);
        active = !active;
    }

    public bool IsActive()
    {
        return active;
    }

    private void ShowButton()
    {
        if (currIndex == 0) prevButton.gameObject.SetActive(false);
        else if (currIndex == tutorialTexts.Length - 1) nextButton.gameObject.SetActive(false);
        else
        {
            prevButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
        }
    }
}
