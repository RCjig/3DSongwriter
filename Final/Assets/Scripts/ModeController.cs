using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeController : MonoBehaviour
{
    enum MODE { CREATE, EDIT, PLAY };

    public Text modeText;

    private MODE mode;

    // Start is called before the first frame update
    void Start()
    {
        mode = MODE.EDIT; // change to CREATE later
        modeText.text = mode.ToString();
        modeText.color = Color.green;
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
        }
        else if (mode == MODE.EDIT)
        {
            mode = MODE.PLAY;
            modeText.color = Color.red;
        }
        else
        {
            mode = MODE.CREATE;
            modeText.color = Color.white;
        }

        modeText.text = mode.ToString();
    }

    public string GetMode()
    {
        return mode.ToString();
    }
}
