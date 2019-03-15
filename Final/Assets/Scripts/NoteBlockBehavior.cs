using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBlockBehavior : MonoBehaviour
{
    private Note note;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool HasNote()
    {
        if (audioSource.clip)
            return true;
        return false;
    }

    private void PlayNote()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
        else if (audioSource.time > 0.5)
            audioSource.Stop();
    }

    public void Play()
    {
        if (HasNote()) PlayNote();
    }

    private void SetAudioClip (AudioClip sound)
    {
        audioSource.clip = sound;
        if (HasNote())
        {
            //audioSource.loop = true;
            PlayNote();
        }
        else audioSource.loop = false;
    }

    public void AssignNote(Note newNote)
    {
        // update the render material
        // assign the current note value
        note = newNote;
        this.GetComponent<Renderer>().material = new Material(note.getMaterial());
        AudioClip sound = note.getSound();
        if (sound)
            SetAudioClip(sound);
    }
}
