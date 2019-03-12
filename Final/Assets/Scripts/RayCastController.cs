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
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.name.Contains("NoteBlock"))
            {
                // is this supposed to be rhandtrigger?
                if (modeController.GetMode().Equals("EDIT") && OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
                {
                    // assign the current note to the note block
                    hitObject.GetComponent<NoteBlockBehavior>().AssignNote(currNote);
                }
            }

            else if (hitObject.name == "ChangeModeButton")
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
                {
                    modeController.ChangeMode();
                }
            }

            else if (hitObject.name.Contains("_NoteButton"))
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
                {
                    menuController.NotSet();

                    if (hitObject.name[0] == '♭' || hitObject.name[0] == '♮' || hitObject.name[0] == '#')
                        menuController.AppendModifier(hitObject.name[0].ToString());
                    else
                        menuController.EnterNote(hitObject.name[0].ToString());
                }
            }

            else if (hitObject.name.Contains("_OctaveButton"))
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
                {
                    menuController.NotSet();
                    menuController.EnterOctave(hitObject.name[0].ToString());
                }
            }

            else if (hitObject.name == "SetButton")
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
                {
                    string selectedNote = menuController.GetSelectedNote();
                    Debug.Log("Looking for: " + selectedNote);
                    this.currNote = musicBoxController.GetNote(selectedNote);
                    Debug.Log("Setting: " + this.currNote.getName());
                    menuController.Set();
                }
            }
        }
    }
}
