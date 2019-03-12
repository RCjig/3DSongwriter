using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxController : MonoBehaviour
{
    // an array of sounds that the "music box" can make
    // possible notes: G, G#/Ab, A, A#/Bb, B, C, C#/Db, D, D#/Eb, E, F, F#/Gb
    // size of array = # notes * # octaves we want (so 12 * 1, for now)
    public Material sourceMaterial;
    public AudioClip[] noteSounds;
    public string[] noteNames;
    public Color[] noteColors;
    private Note[] notes;

    private readonly int numBaseNotes = 12;

    // Start is called before the first frame update
    void Start()
    {
        if ((noteSounds.Length <= 0) || !((noteSounds.Length == noteNames.Length) && (noteSounds.Length % numBaseNotes == 0)))
        {
            Debug.Log("MUSIC BOX NOT CONFIGURED CORRECTLY");
            return;
        }

        notes = new Note[noteSounds.Length + 1];
        Material currMat;
        for (int i = 0; i < noteSounds.Length; i++)
        {
            currMat = new Material(sourceMaterial);
            currMat.SetColor("_Color", noteColors[i % numBaseNotes]);
            notes[i] = new Note(noteSounds[i], noteNames[i], currMat);
        }
        // adding the empty note to the array, where NNN means no note, no note modifier, and no octave number
        notes[noteSounds.Length] = new Note(null, "N♮N", sourceMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Note GetNote(string lookingFor)
    {
        for (int i = 0; i < notes.Length; i++)
        {
            Debug.Log(notes[i].getName().Trim());
            if (notes[i].getName().Trim().CompareTo(lookingFor) == 0)
                return notes[i];
        }
        // return the default note
        Debug.Log("RETURNING DEFAULT NOTE");
        return notes[notes.Length - 1];
    }
}
