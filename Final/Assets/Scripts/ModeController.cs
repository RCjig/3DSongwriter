using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeController : MonoBehaviour
{
    enum MODE { EDIT, TEST, PLAY };

    public Text modeText;

    private MODE mode;

    // Start is called before the first frame update
    void Start()
    {
        mode = MODE.EDIT;
        modeText.text = mode.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeMode()
    {
        if (mode == MODE.EDIT) mode = MODE.TEST;
        else if (mode == MODE.TEST) mode = MODE.PLAY;
        else mode = MODE.EDIT;

        modeText.text = mode.ToString();
    }

    public string GetMode()
    {
        return mode.ToString();
    }
}
