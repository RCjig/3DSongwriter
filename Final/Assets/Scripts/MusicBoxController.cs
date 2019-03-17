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
    public Color[] noteColors;
    private Note[] notes;

    private readonly char flat = '♭';
    private readonly char sharp = '♯';
    private readonly char natural = '♮';

    private readonly int numBaseNotes = 12;

    // Start is called before the first frame update
    void Start()
    {
        if ((noteSounds.Length <= 0) || !(noteSounds.Length % numBaseNotes == 0))
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
            notes[i] = new Note(noteSounds[i], noteSounds[i].name, currMat);
        }
        // adding the empty note to the array (no note modifier and no octave number)
        notes[noteSounds.Length] = new Note(null, "N" + natural + "N", sourceMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool SameNote (string musicBoxNote, string inputNote)
    {
        string compareTo = inputNote;
        char octave = inputNote[2];

        // handling flat inputs
        if (inputNote.Equals("A" + flat + octave)) compareTo = "G" + sharp + octave;
        else if (inputNote.Equals("B" + flat + octave)) compareTo = "A" + sharp + octave;
        else if (inputNote.Equals("D" + flat + octave)) compareTo = "C" + sharp + octave;
        else if (inputNote.Equals("E" + flat + octave)) compareTo = "D" + sharp + octave;
        else if (inputNote.Equals("G" + flat + octave)) compareTo = "F" + sharp + octave;
        // handling unusual flats
        else if (inputNote.Equals("C" + flat + octave)) compareTo = "B" + natural + octave;
        else if (inputNote.Equals("F" + flat + octave)) compareTo = "E" + natural + octave;
        // handling unusual sharps
        else if (inputNote.Equals("B" + sharp + octave)) compareTo = "C" + natural + octave;
        else if (inputNote.Equals("E" + sharp + octave)) compareTo = "F" + natural + octave;

        return (musicBoxNote.CompareTo(compareTo) == 0);

    }

    public Note GetNote(string lookingFor)
    {
        for (int i = 0; i < notes.Length; i++)
        {
            string musicBoxNote = notes[i].getName().Trim();
            if (SameNote(musicBoxNote, lookingFor))
                return notes[i];
        }
        // return the default note
        return notes[notes.Length - 1];
    }
}
