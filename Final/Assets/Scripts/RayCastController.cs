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

    private Note currNote; // the note we are currently "painting"

    // Start is called before the first frame update
    void Start()
    {
        rightHand = transform.parent.gameObject;
        avatar = gameObject.GetComponentInParent<OvrAvatar>();
        modeController = GameObject.Find("LogicController").GetComponent<ModeController>();
        menuController = GameObject.Find("LogicController").GetComponent<MenuController>();
        musicBoxController = MusicBox.GetComponent<MusicBoxController>();
        currNote = musicBoxController.GetNote("N♮N");
    }

    // Update is called once per frame
    void Update()
    {
        avatar.GetPointingDirection(OvrAvatar.HandType.Right, ref forward, ref up);
        RayCast();
    }

    private void RayCast()
    {
        Ray ray = new Ray(rightHand.transform.position, forward);
        Debug.DrawRay(rightHand.transform.position, forward, Color.green);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (modeController.GetMode().Equals("EDIT") && OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
            {
                if (hitObject.name.Contains("NoteBlock"))
                {
                    // assign the current note to the note block
                    hitObject.GetComponent<NoteBlockBehavior>().AssignNote(currNote);
                }
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
            { 
                if (hitObject.name == "ChangeModeButton")
                {
                    modeController.ChangeMode();
                }

                else if (hitObject.name.Contains("_NoteButton"))
                {
                    string type = "";

                    if (hitObject.name[0] == '♭' || hitObject.name[0] == '♮' || hitObject.name[0] == '♯') type = "NOTE_MODIFIER";
                    else type = "NOTE";

                    menuController.NotSet();
                    menuController.ResetButtons(type);
                    menuController.Enter(type, hitObject.name[0].ToString());
                }

                else if (hitObject.name.Contains("_OctaveButton"))
                {
                    menuController.NotSet();
                    menuController.ResetButtons("OCTAVE");
                    menuController.Enter("OCTAVE", hitObject.name[0].ToString());
                }

                else if (hitObject.name == "SetButton")
                {
                    if (menuController.Set())
                    {
                        string selectedNote = menuController.GetSelectedNote();
                        Debug.Log("Looking for: " + selectedNote);
                        this.currNote = musicBoxController.GetNote(selectedNote);
                        Debug.Log("Setting: " + this.currNote.getName());
                    }
                }

                else if (hitObject.name == "GateDropdown")
                {
                    menuController.ExpandOrCollapseDropdown();
                }

                else if (hitObject.name.Contains("GateDropdown_Option_"))
                {
                    menuController.SetDropdown(hitObject.name[hitObject.name.Length-1] - '0' - 1);
                }

                else if (hitObject.name == "CreateButton")
                {
                    GameObject.Find("Tunnel").GetComponent<TunnelGenerator>().CreateTunnel(menuController.GetNumberOfGates());
                }

                else if (hitObject.name == "ResetButton")
                {
                    GameObject.Find("Tunnel").GetComponent<TunnelGenerator>().DestroyTunnel();
                }
            }
        }
    }
}
