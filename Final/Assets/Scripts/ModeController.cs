using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeController : MonoBehaviour
{
    enum MODE { CREATE, EDIT, PLAY };

    public GameObject createMenu;
    public GameObject editMenu;
    public GameObject playMenu;
    public Text modeText;

    private MODE mode;
    private bool firstPlay;

    // Start is called before the first frame update
    void Start()
    {
        mode = MODE.CREATE;
        modeText.text = mode.ToString();
        modeText.color = Color.white;
        firstPlay = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeMode()
    {
        if (mode == MODE.CREATE)
        {
            mode = MODE.EDIT;
            modeText.color = Color.green;
            createMenu.SetActive(false);
            editMenu.SetActive(true);
        }
        else if (mode == MODE.EDIT)
        {
            mode = MODE.PLAY;
            modeText.color = Color.red;
            editMenu.SetActive(false);
            playMenu.SetActive(true);
            firstPlay = true;
        }
        else
        {
            mode = MODE.CREATE;
            modeText.color = Color.white;
            playMenu.SetActive(false);
            createMenu.SetActive(true);
            firstPlay = false;
        }

        modeText.text = mode.ToString();
    }

    public string GetMode()
    {
        return mode.ToString();
    }

    public bool IsFirstPlay()
    {
        return firstPlay;
    }
    
    public void SetFirstPlay()
    {
        firstPlay = false;
    }
}
