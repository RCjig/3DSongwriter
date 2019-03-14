using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    readonly float MOVEMENT_SCALE = 0.1f;
    private ModeController modeController;

    // Start is called before the first frame update
    void Start()
    {
        modeController = GameObject.Find("LogicController").GetComponent<ModeController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (modeController.GetMode().Equals("EDIT"))
        {
            float moveSpeed = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);
            Vector3 movementVector = -1 * transform.forward * moveSpeed * MOVEMENT_SCALE;
            movementVector.y = 0.0f;
            transform.position -= movementVector; // dont ask why its negative
        }
    }
}
