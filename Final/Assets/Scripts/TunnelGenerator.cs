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
            curr.name = "NoteGate_" + gateCount;
            for (int j = 0; j < curr.transform.childCount; j++)
            {
                if (curr.transform.GetChild(j).name != "TriggerLine")
                {
                    for (int k = 0; k < curr.transform.GetChild(j).transform.childCount; k++)
                    {
                        curr.transform.GetChild(j).transform.GetChild(k).name += "_Gate_" + gateCount;
                    }
                }

                curr.transform.GetChild(j).name += "_Gate_" + gateCount;
            }
            gateCount++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
