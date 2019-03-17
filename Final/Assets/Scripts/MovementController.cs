using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    readonly float MOVEMENT_SCALE = 0.025f;
    private ModeController modeController;

    // Start is called before the first frame update
    void Start()
    {
        modeController = GameObject.Find("LogicController").GetComponent<ModeController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (modeController.GetMode().Equals("EDIT"))
        {
            float moveSpeed = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);
            //Debug.Log(moveSpeed.ToString());
            Vector3 movementVector = transform.forward * moveSpeed * MOVEMENT_SCALE;
            movementVector.y = 0.0f;
            GetComponent<CharacterController>().Move(movementVector);
            //transform.position += new Vector3(0.0f, 0.0f, 0.1f);
            //Debug.Log(movementVector.magnitude.ToString());
            //Debug.Log(GetComponent<CharacterController>().velocity.ToString());
        }
    }
}
