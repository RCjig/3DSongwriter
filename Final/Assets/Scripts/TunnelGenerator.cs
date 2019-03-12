using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelGenerator : MonoBehaviour
{
    public GameObject NoteGate;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 currPosition = Vector3.zero;
        GameObject curr;
        int gateCount = 0;

        for (float i = 0.0f; i < (200.0f * 1.2f) - 1; i += 1.2f)
        {
            curr = Instantiate(NoteGate);
            curr.transform.position = new Vector3(0.0f, 3.6f, i);
            curr.name = "NoteGate_" + gateCount++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
