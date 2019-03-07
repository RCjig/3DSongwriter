using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeController : MonoBehaviour
{
    private string mode; // string cus idk how to define enums outside this script

    // Start is called before the first frame update
    void Start()
    {
        mode = "EDIT";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMode()
    {
        if (mode == "EDIT") mode = "TEST";
        else if (mode == "TEST") mode = "PLAY";
        else mode = "EDIT";
    }

    public string GetMode()
    {
        return mode;
    }
}
