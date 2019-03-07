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

        for (float i = 0.0f; i < (200.0f * 1.1f); i += 1.2f)
        {
            curr = Instantiate(NoteGate);
            curr.transform.position = new Vector3(0.0f, 3.6f, i);
            curr.name = "NoteGate_" + gateCount++;
        }

        /*
        while (currPosition.z < 200.0f)
        {

            for (float i = 0.0f; i < 7.0f; i += 1.1f)
            {
                curr = Instantiate(NoteBlock);
                curr.transform.position = new Vector3(-6.0f, i, currPosition.z);
                curr.name = "NoteBlock_" + blockCount;
                blockCount++;
            }

            for (float j = 0.0f; j < 7.0f; j += 1.1f)
            {
                curr = Instantiate(NoteBlock);
                curr.transform.position = new Vector3(6.0f, j, currPosition.z);
                curr.name = "NoteBlock_" + blockCount;
                blockCount++;
            }

            for (float k = 0.0f; k < 7.0f*1.6f; k += 1.6f)
            {
                curr = Instantiate(NoteBlock);
                curr.transform.position = new Vector3(k - 4.8f, 7.0f*1.1f, currPosition.z);
                curr.name = "NoteBlock_" + blockCount;
                blockCount++;
            }

            currPosition.z += 1.1f;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
