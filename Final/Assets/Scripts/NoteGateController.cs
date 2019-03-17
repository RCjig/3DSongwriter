using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class NoteGateController : MonoBehaviour
{
    readonly float TRIGGER_BUFFER = 0.25f; // idk how else to check when a player crosses a line

    private GameObject playerController;
    private LineRenderer triggerLine;
    private bool canPlay;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("OVRPlayerController");
        triggerLine = this.GetComponentInChildren<LineRenderer>();
        canPlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLineTriggered() && canPlay) OnLineTrigger();
    }

    public void PlayOnce()
    {
        if (IsInLine()) PlayGate();
    }

    private void PlayGate()
    {
        foreach (Transform layer in transform)
        {
            if (layer.tag != "GateLayer") continue;
            foreach (Transform noteBlock in layer.transform)
            {
                noteBlock.gameObject.GetComponent<NoteBlockBehavior>().Play();
            }
        }
    }

    private bool IsLineTriggered()
    {
        bool triggered = IsInLine();
        if (!triggered)
            canPlay = true;
        return triggered;
    }

    private void OnLineTrigger()
    {
        canPlay = false;
        // play notes
        PlayGate();
    }

    private bool IsInLine()
    {
        return Mathf.Abs(playerController.transform.position.z - triggerLine.transform.position.z) < TRIGGER_BUFFER;
    }
}
