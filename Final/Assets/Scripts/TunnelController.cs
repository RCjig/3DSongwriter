using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TunnelController : MonoBehaviour
{
    readonly float SPACING = 3.0f;

    public GameObject NoteGate;
    public string fileName;
    public GameObject MusicBox;

    private bool hasBeenCreated;
    private GameObject[] noteGates;

    // Start is called before the first frame update
    void Start()
    {
        hasBeenCreated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // need to figure out how we want to split gates if new line doesn't work
    private string GateToString (GameObject gate)
    {
        string gateString = "";
        NoteBlockBehavior[] noteBlockControllers = gate.GetComponentsInChildren<NoteBlockBehavior>();

        foreach (NoteBlockBehavior currController in noteBlockControllers)
            gateString = gateString + currController.GetNoteName() + " ";

        return gateString;
    }


    // this does not work yet and we need to write the number of lines if we can't make the newline character work
    public void LoadFromFile ()
    {
        if (hasBeenCreated) return;

        string filePath = Application.dataPath.ToString() + @"/Saved/" + fileName;
        StreamReader reader = new StreamReader(filePath);
        int numGates = int.Parse(reader.ReadLine());
        CreateTunnel(numGates);
        for (int i = 0; i < numGates; i++)
        {
            GameObject currGate = noteGates[i];
            NoteBlockBehavior[] currNoteBlocks = currGate.GetComponentsInChildren<NoteBlockBehavior>();
            MusicBoxController musicBoxController = MusicBox.GetComponent<MusicBoxController>();

            string[] notes = reader.ReadLine().Split(' ');
            for (int j = 0; j < notes.Length && j < currNoteBlocks.Length; j++)
            {
                string currName = currNoteBlocks[j].name;
                if (!currNoteBlocks[j].IsInitialized()) currNoteBlocks[j].InitVars();
                string noteName = notes[int.Parse((currName.Split('_')[1]))];
                currNoteBlocks[j].AssignNote(musicBoxController.GetNote(noteName));
            }
        }
    }

    public void WriteToFile ()
    {
        // Write to disk
        string path = Application.dataPath.ToString() + @"/Saved/Save.txt";
        StreamWriter writer = new StreamWriter(path, false);
        writer.AutoFlush = true;

        writer.WriteLine(noteGates.Length + "");

        foreach (GameObject gate in noteGates)
            writer.WriteLine(GateToString(gate));
    }

    public void CreateTunnel(int gateCount)
    {
        if (hasBeenCreated) return;

        Vector3 currPosition = Vector3.zero;
        GameObject curr;
        float gateCountLoopLength = (float) gateCount * SPACING;
        hasBeenCreated = true;

        noteGates = new GameObject[gateCount];
        int gateIndex = 0;

        for (float i = 0.0f; i < gateCountLoopLength - 1; i += SPACING)
        {
            curr = Instantiate(NoteGate);
            curr.transform.position = new Vector3(0.0f, 3.6f, i);
            curr.name = "NoteGate_" + gateIndex;
            for (int j = 0; j < curr.transform.childCount; j++)
            {
                if (curr.transform.GetChild(j).name != "TriggerLine")
                {
                    for (int k = 0; k < curr.transform.GetChild(j).transform.childCount; k++)
                    {
                        curr.transform.GetChild(j).transform.GetChild(k).name += "_Gate_" + gateIndex;
                    }
                }

                curr.transform.GetChild(j).name += "_Gate_" + gateIndex;
            }
            noteGates[gateIndex] = curr;
            gateIndex++;
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

    public void PlayGates()
    {
        foreach (GameObject noteGate in noteGates)
            noteGate.GetComponent<NoteGateController>().PlayOnce();
    }
}
