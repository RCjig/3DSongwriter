using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note
{
    private AudioClip sound;
    private string name;
    private Material mat;

    public Note (AudioClip inSound, string inName, Material inMaterial)
    {
        this.sound = inSound;
        this.name = inName;
        this.mat = inMaterial;
    }

    public AudioClip getSound ()
    {
        return this.sound;
    }

    public string getName ()
    {
        return this.name;
    }

    public Material getMaterial ()
    {
        return this.mat;
    }
}
