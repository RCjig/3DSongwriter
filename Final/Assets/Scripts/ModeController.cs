using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeController : MonoBehaviour
{
    enum MODE { CREATE, EDIT, PLAY };

    public GameObject createMenu;
    public GameObject editMenu;
    public Text modeText;

    private MODE mode;

    // Start is called before the first frame update
    void Start()
    {
        mode = MODE.CREATE;
        modeText.text = mode.ToString();
        modeText.color = Color.white;
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
        }
        else
        {
            mode = MODE.CREATE;
            modeText.color = Color.white;
            createMenu.SetActive(true);
        }

        modeText.text = mode.ToString();
    }

    public string GetMode()
    {
        return mode.ToString();
    }
}
