using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxController : MonoBehaviour
{
    // an array of sounds that the "music box" can make
    // possible notes: G, G#/Ab, A, A#/Bb, B, C, C#/Db, D, D#/Eb, E, F, F#/Gb
    // size of array = # notes * # octaves we want (so 12 * 1, for now)
    public AudioClip[] noteSounds;
    public string[] noteNames;
    private Note[] notes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
