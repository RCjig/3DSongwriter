using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastController : MonoBehaviour
{
    public OVRInput.Controller rightController; // right touch controller
    public GameObject MusicBox;

    private OvrAvatar avatar; // player avatar
    private GameObject rightHand; // for ray cast position
    private ModeController modeController;
    private MenuController menuController;
    private MusicBoxController musicBoxController;
    private Vector3 forward;
    private Vector3 up;
    private bool canPaint;

    private Note currNote; // the note we are currently "painting"

    /* Control Flow Outline
     * 
     * CREATE mode: might need to edit display based on desired inputs (BPM?), controls should be done
     *  - point at dropdown arrow or buttons and use rhand trigger
     * 
     * EDIT mode: display and controls mostly done
     *  - use rindex trigger to paint blocks
     *  - use rhand trigger to get menu input
     *  - use ray to determine selected menu items/blocks
     *  * TODO: add a clear button to allow us to erase notes
     * 
     * PLAY mode: when we toggle to this mode, I want the menu to display a start button and a BPM slider, intialized to the BPM they originally chose
     *  - press play using the usual rhand trigger menu interaction
     *  - once we press play, menu disappears
     *  - legend on the side could show possible inputs
     *      - ex: use normal movement trigger to speed up, use normal selectin trigger to pause
     *          - on pause, the menu comes back up, with options "RESUME" and "EXIT"
     *          - exit returns to EDIT mode
     * 
     */

    // Start is called before the first frame update
    void Start()
    {
        rightHand = transform.parent.gameObject;
        avatar = gameObject.GetComponentInParent<OvrAvatar>();
        modeController = GameObject.Find("LogicController").GetComponent<ModeController>();
        menuController = GameObject.Find("LogicController").GetComponent<MenuController>();
        musicBoxController = MusicBox.GetComponent<MusicBoxController>();
        currNote = musicBoxController.GetNote("N♮N");
        canPaint = false;
    }

    // Update is called once per frame
    void Update()
    {
        avatar.GetPointingDirection(OvrAvatar.HandType.Right, ref forward, ref up);
        RayCast();
    }

    private void HandleCreateModeInputs(GameObject hitObject, bool rIndexTriggered, bool rHandTriggered)
    {
        if (rHandTriggered)
        {
            if (hitObject.name == "GateDropdown")
            {
                menuController.ExpandOrCollapseDropdown();
            }

            else if (hitObject.name.Contains("GateDropdown_Option_"))
            {
                menuController.SetDropdown(hitObject.name[hitObject.name.Length - 1] - '0' - 1);
            }

            else if (hitObject.name == "CreateButton")
            {
                GameObject.Find("Tunnel").GetComponent<TunnelController>().CreateTunnel(menuController.GetNumberOfGates());
            }

            else if (hitObject.name == "ResetButton")
            {
                GameObject.Find("Tunnel").GetComponent<TunnelController>().DestroyTunnel();
            }
        }
    }

    private void HandleNoteBlockHit (GameObject noteBlock, bool rIndexTriggered)
    {
        NoteBlockBehavior behaviorController = noteBlock.GetComponent<NoteBlockBehavior>();
        behaviorController.Play();
        if (canPaint && rIndexTriggered)
            behaviorController.AssignNote(currNote);
    }

    private void ChangeMode()
    {
        modeController.ChangeMode();
    }

    private void UpdateMenuController (string updateType, string input)
    {
        canPaint = false;
        menuController.NotSet();
        menuController.ResetButtons(updateType);
        menuController.Enter(updateType, input);
    }

    private void HandleNoteButtonHit(GameObject hitObject)
    {
        string type = "";

        if (hitObject.name[0] == '♭' || hitObject.name[0] == '♮' || hitObject.name[0] == '♯') type = "NOTE_MODIFIER";
        else type = "NOTE";

        UpdateMenuController(type, hitObject.name[0].ToString());
    }

    private void HandleOctaveButtonHit(GameObject hitObject)
    {
        UpdateMenuController("OCTAVE", hitObject.name[0].ToString());
    }

    private void HandleEditModeInputs(GameObject hitObject, bool rIndexTriggered, bool rHandTriggered)
    {
        if (hitObject.name.Contains("NoteBlock"))
            HandleNoteBlockHit(hitObject, rIndexTriggered);

        else if (rHandTriggered)
        {
            if (hitObject.name.Contains("_NoteButton"))
                HandleNoteButtonHit(hitObject);

            else if (hitObject.name.Contains("_OctaveButton"))
                HandleOctaveButtonHit(hitObject);

            else if (hitObject.name == "ClearButton")
            {
                UpdateMenuController("CLEAR", "Clear");
            }

            else if (hitObject.name == "PlayGateButton")
            {

            }

            else if (hitObject.name == "SetButton")
            {
                if (menuController.Set())
                {
                    string selectedNote = menuController.GetSelectedNote();
                    Debug.Log("Looking for: " + selectedNote);
                    this.currNote = musicBoxController.GetNote(selectedNote);
                    Debug.Log("Setting: " + this.currNote.getName());
                    canPaint = true;
                }
            }
        }
    }

    private bool ShouldChangeMode(GameObject hitObject, bool rHandTriggered)
    {
        return (hitObject.name.Equals("ChangeModeButton") && rHandTriggered);
    }

    private void RayCast()
    {
        Ray ray = new Ray(rightHand.transform.position, forward);
        Debug.DrawRay(rightHand.transform.position, forward, Color.green);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            string mode = modeController.GetMode();
            GameObject hitObject = hit.collider.gameObject;
            bool rIndexTriggered = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
            bool rHandTriggered = OVRInput.GetDown(OVRInput.RawButton.RHandTrigger);

            if (ShouldChangeMode(hitObject, rHandTriggered))
                ChangeMode();
            else if (mode.Equals("EDIT"))
                HandleEditModeInputs(hitObject, rIndexTriggered, rHandTriggered);
            else if (mode.Equals("CREATE"))
                HandleCreateModeInputs(hitObject, rIndexTriggered, rHandTriggered);
        }
    }
}
