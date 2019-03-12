using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    readonly int IS_SET_INDEX = 0;
    readonly int NOTE_INDEX = 1;
    readonly int OCTAVE_INDEX = 2;

    public Text[] statusText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set()
    {
        statusText[IS_SET_INDEX].text = "SET";
        statusText[IS_SET_INDEX].color = Color.green;
    }

    public void NotSet()
    {
        statusText[IS_SET_INDEX].text = "NOT SET";
        statusText[IS_SET_INDEX].color = Color.red;
    }

    public void EnterNote(string note)
    {
        statusText[NOTE_INDEX].text = "Note: " + note;
    }

    public void EnterOctave(string octave)
    {
        statusText[OCTAVE_INDEX].text = "Octave: " + octave;
    }

    public void AppendModifier(string modifier)
    {
        if (statusText[NOTE_INDEX].text.Length == 7) statusText[NOTE_INDEX].text += modifier;
        else statusText[NOTE_INDEX].text = statusText[NOTE_INDEX].text.Substring(0, 7) + modifier;
    }
}
