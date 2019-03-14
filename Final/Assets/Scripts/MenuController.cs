using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    readonly int IS_SET_INDEX = 0;
    readonly int NOTE_INDEX = 1;
    readonly int OCTAVE_INDEX = 2;
    readonly int NOTES_END_INDEX = 7;
    readonly int NOTE_MODIFIERS_END_INDEX = 10;

    public Text[] statusText;
    public Button[] keypadButtons;

    private bool isSet;

    // Start is called before the first frame update
    void Start()
    {
        isSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set()
    {
        statusText[IS_SET_INDEX].text = "SET";
        statusText[IS_SET_INDEX].color = Color.green;
        isSet = true;

        foreach (Button button in keypadButtons)
        {
            if (button.colors.normalColor == Color.red)
            {
                ColorBlock cb = button.colors;
                cb.normalColor = Color.green;
                button.colors = cb;
            }
        }
    }

    public void NotSet()
    {
        statusText[IS_SET_INDEX].text = "NOT SET";
        statusText[IS_SET_INDEX].color = Color.red;

        if (!isSet) return;

        isSet = false;

        foreach (Button button in keypadButtons)
        {
            ColorBlock cb = button.colors;
            cb.normalColor = Color.white;
            button.colors = cb;
        }
    }

    public void Enter(string type, string text)
    {
        int start = 0;
        int end = 0;
        SetStartAndEnd(type, ref start, ref end);

        if (type == "NOTE_MODIFIER" && !IsNoteSet()) return;

        for (int i = start; i < end; i++)
        {
            if (keypadButtons[i].GetComponentInChildren<Text>().text.Equals(text))
            {
                ColorBlock cb = keypadButtons[i].colors;
                cb.normalColor = Color.red;
                keypadButtons[i].colors = cb;
                break;
            }
        }
    }

    public void ResetButtons(string type)
    {
        int start = 0;
        int end = 0;
        SetStartAndEnd(type, ref start, ref end);

        for (int i = start; i < end; i++)
        {
            ColorBlock cb = keypadButtons[i].colors;
            cb.normalColor = Color.white;
            keypadButtons[i].colors = cb;
        }

        if (type == "NOTE")
        {
            SetStartAndEnd("NOTE_MODIFIER", ref start, ref end);

            for (int i = start; i < end; i++)
            {
                ColorBlock cb = keypadButtons[i].colors;
                cb.normalColor = Color.white;
                keypadButtons[i].colors = cb;
            }
        }
    }

    public string GetSelectedNote()
    {
        string noteName = "N";
        string noteModifier = "♮";
        string octave = "N";

        /*
        if (statusText[NOTE_INDEX].text.Length >= 7)
            noteName = statusText[NOTE_INDEX].text.Substring(6, 1);
        if (statusText[NOTE_INDEX].text.Length == 8)
            noteModifier = statusText[NOTE_INDEX].text.Substring(7, 1);
        if (statusText[OCTAVE_INDEX].text.Length == 9)
            octave = statusText[OCTAVE_INDEX].text.Substring(8, 1);
            */


        for (int i = 0; i < keypadButtons.Length; i++)
        {
            if (keypadButtons[i].colors.normalColor == Color.green)
            {
                if (i < NOTES_END_INDEX) noteName = keypadButtons[i].GetComponentInChildren<Text>().text;
                else if (i < NOTE_MODIFIERS_END_INDEX) noteModifier = keypadButtons[i].GetComponentInChildren<Text>().text;
                else octave = keypadButtons[i].GetComponentInChildren<Text>().text;
            }
        }

        // TODO: Fix this
        Debug.Log(noteName + noteModifier + octave + "\n");
        return noteName + noteModifier + octave;
    }

    private bool IsNoteSet()
    {
        bool isNoteSet = false;

        for (int i = 0; i < NOTES_END_INDEX; i++)
        {
            if (keypadButtons[i].colors.normalColor == Color.red)
            {
                isNoteSet = true;
                break;
            }
        }

        return isNoteSet;
    }

    private void SetStartAndEnd(string type, ref int start, ref int end)
    {
        if (type == "NOTE")
        {
            start = 0;
            end = NOTES_END_INDEX;
        }
        else if (type == "NOTE_MODIFIER")
        {
            start = NOTES_END_INDEX;
            end = NOTE_MODIFIERS_END_INDEX;
        }
        else
        {
            start = NOTE_MODIFIERS_END_INDEX;
            end = keypadButtons.Length;
        }
    }
}
