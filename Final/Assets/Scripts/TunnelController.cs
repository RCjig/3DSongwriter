using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelController : MonoBehaviour
{
    readonly float SPACING = 3.0f;

    public GameObject NoteGate;

    private bool hasBeenCreated;

    // Start is called before the first frame update
    void Start()
    {
        hasBeenCreated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTunnel(int gateCount)
    {
        if (hasBeenCreated) return;

        Vector3 currPosition = Vector3.zero;
        GameObject curr;
        float gateCountLoopLength = (float) gateCount * SPACING;
        hasBeenCreated = true;

        for (float i = 0.0f; i < gateCountLoopLength - 1; i += SPACING)
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

    public void DestroyTunnel()
    {
        hasBeenCreated = false;

        foreach (var noteGate in GameObject.FindGameObjectsWithTag("NoteGate"))
        {
            Destroy(noteGate);
        }
    }
}
