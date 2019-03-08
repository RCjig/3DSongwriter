using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class NoteGateController : MonoBehaviour
{
    readonly float TRIGGER_BUFFER = 0.25f; // idk how else to check when a player crosses a line

    private GameObject playerController;
    private LineRenderer triggerLine;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("OVRPlayerController");
        triggerLine = this.GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLineTriggered()) OnLineTrigger();
    }

    private bool IsLineTriggered()
    {
        return (Mathf.Abs(playerController.transform.position.z - triggerLine.transform.position.z) < TRIGGER_BUFFER) ? true : false;
    }

    private void OnLineTrigger()
    {
        // play notes
        // Debug.Log(this.name + "crossed\n"); // this works
    }
}
