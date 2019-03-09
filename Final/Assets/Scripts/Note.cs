using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note
{
    private AudioClip sound;
    private string name;
    private Material mat;

    public Note (AudioClip inSound, string inName)
    {
        this.sound = inSound;
        this.name = inName;
    }
}
