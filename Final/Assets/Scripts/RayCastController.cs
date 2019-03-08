using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastController : MonoBehaviour
{
    public OVRInput.Controller rightController; // right touch controller

    private OvrAvatar avatar; // player avatar
    private GameObject rightHand; // for ray cast position
    private LineRenderer rayCastLine; // line from hand
    private ModeController modeController;
    private Vector3 forward;
    private Vector3 up;

    // Start is called before the first frame update
    void Start()
    {
        rightHand = transform.parent.gameObject;
        avatar = gameObject.GetComponentInParent<OvrAvatar>();
        rayCastLine = gameObject.GetComponentInChildren<LineRenderer>();
        modeController = GameObject.Find("LogicController").GetComponent<ModeController>();
    }

    // Update is called once per frame
    void Update()
    {
        avatar.GetPointingDirection(OvrAvatar.HandType.Right, ref forward, ref up);

        // should we keep a ray visible at all times (dunno how you would change back to like edit mode if ray isnt visible in play mode
        //rayCastLine.transform.parent.gameObject.SetActive((modeController.GetMode() != "PLAY"));

        if (rayCastLine.transform.parent.gameObject.activeSelf == true)
        {
            RayCast();
        }
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
                //Debug.Log(hitObject.name); // working
            }

            else if (hitObject.name == "ChangeModeButton")
            {
                // this changes too fast
                if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
                {
                    modeController.ChangeMode();
                }
            }
        }
    }
}
