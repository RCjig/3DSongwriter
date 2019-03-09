using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastController : MonoBehaviour
{
    public OVRInput.Controller rightController; // right touch controller

    private OvrAvatar avatar; // player avatar
    private GameObject rightHand; // for ray cast position
    private ModeController modeController;
    private Vector3 forward;
    private Vector3 up;

    // Start is called before the first frame update
    void Start()
    {
        rightHand = transform.parent.gameObject;
        avatar = gameObject.GetComponentInParent<OvrAvatar>();
        modeController = GameObject.Find("LogicController").GetComponent<ModeController>();
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
                if (modeController.GetMode().Equals("EDIT") && OVRInput.Get(OVRInput.RawButton.RHandTrigger))
                {
                    // assign the current mode's note to the note block
                    hitObject.GetComponent<NoteBlockBehavior>().AssignNote();
                }
            }

            else if (hitObject.name == "ChangeModeButton")
            {
                if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                {
                    modeController.ChangeMode();
                }
            }
        }
    }
}
