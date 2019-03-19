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
    readonly int OCTAVE_END_INDEX = 14;
    readonly int CLEAR_END_INDEX = 15;
    readonly string ARROW = " ▼";

    public GameObject menu;
    public Button[] keypadButtons;
    public Button[] dropdownButtonOptions;
    public Button dropdownButton;
    public Button playGateButton;
    public Button playButton;
    public Button resetButton;
    public Button saveButton;
    public Button loadButton;
    public Button tutorialButton;

    private MovementController movementController;
    private ModeController modeController;
    private TutorialController tutorialController;
    private bool isSet;
    private bool isExpanded;
    private bool isHighlightTimerOn;
    private float highlightTimer;

    // Start is called before the first frame update
    void Start()
    {
        movementController = GameObject.Find("OVRPlayerController").GetComponent<MovementController>();
        modeController = GameObject.Find("LogicController").GetComponent<ModeController>();
        tutorialController = GameObject.Find("LogicController").GetComponent<TutorialController>();
        isSet = false;
        isExpanded = false;
        isHighlightTimerOn = false;
        highlightTimer = 0.0f;

        foreach (var button in dropdownButtonOptions)
        {
            button.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isHighlightTimerOn) TimedButtonHighlight();
    }

    public bool Set()
    {
        isSet = true;

        int valid = 0;
        int start = 0;
        int end = 0;

        if (keypadButtons[CLEAR_END_INDEX-1].colors.normalColor == Color.red)
        {
            ColorBlock cb = keypadButtons[CLEAR_END_INDEX - 1].colors;
            cb.normalColor = Color.green;
            keypadButtons[CLEAR_END_INDEX - 1].colors = cb;
            return true;
        }

        SetStartAndEnd("NOTE", ref start, ref end);
        for (int i = start; i < end; i++)
        {
            if (keypadButtons[i].colors.normalColor == Color.red)
            {
                valid++;
                break;
            }
        }

        SetStartAndEnd("OCTAVE", ref start, ref end);
        for (int i = start; i < end; i++)
        {
            if (keypadButtons[i].colors.normalColor == Color.red)
            {
                valid++;
                break;
            }
        }

        if (valid < 2) return false;

        foreach (Button button in keypadButtons)
        {
            if (button.colors.normalColor == Color.red)
            {
                ColorBlock cb = button.colors;
                cb.normalColor = Color.green;
                button.colors = cb;
            }
        }

        return true;
    }

    public void NotSet()
    {
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
        if (type == "CLEAR")
        {
            for (int i = 0; i < keypadButtons.Length; i++)
            {
                ColorBlock cb = keypadButtons[i].colors;
                cb.normalColor = Color.white;
                keypadButtons[i].colors = cb;
            }
            return;
        }

        ColorBlock clearCB = keypadButtons[CLEAR_END_INDEX - 1].colors;
        clearCB.normalColor = Color.white;
        keypadButtons[CLEAR_END_INDEX - 1].colors = clearCB;

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

    public void StartTimedButtonHighlight(string type)
    {
        Button button = playGateButton;
        if (type == "PLAY_GATE")
            button = playGateButton;
        else if (type == "SAVE")
            button = saveButton;
        else if (type == "LOAD")
            button = loadButton;

        ColorBlock cb = button.colors;
        cb.normalColor = Color.green;
        button.colors = cb;
        isHighlightTimerOn = true;
        highlightTimer = Time.time;
    }

    public string GetSelectedNote()
    {
        string noteName = "N";
        string noteModifier = "♮";
        string octave = "N";

        if (keypadButtons[CLEAR_END_INDEX - 1].colors.normalColor == Color.green) return "CLR";

        for (int i = 0; i < keypadButtons.Length; i++)
        {
            if (keypadButtons[i].colors.normalColor == Color.green)
            {
                if (i < NOTES_END_INDEX) noteName = keypadButtons[i].GetComponentInChildren<Text>().text;
                else if (i < NOTE_MODIFIERS_END_INDEX) noteModifier = keypadButtons[i].GetComponentInChildren<Text>().text;
                else octave = keypadButtons[i].GetComponentInChildren<Text>().text;
            }
        }

        return noteName + noteModifier + octave;
    }

    public int GetNumberOfGates()
    {
        return System.Int32.Parse(dropdownButton.GetComponentInChildren<Text>().text.Substring(0, 3));
    }

    public void ExpandOrCollapseDropdown()
    {
        isExpanded = !isExpanded;

        foreach (var button in dropdownButtonOptions)
        {
            button.gameObject.SetActive(isExpanded);
        }
    }

    public void SetDropdown(int index)
    {
        dropdownButton.GetComponentInChildren<Text>().text = dropdownButtonOptions[index].GetComponentInChildren<Text>().text + ARROW;
        ExpandOrCollapseDropdown();
    }

    public void TurnOnOffTutorial()
    {
        ColorBlock cb = tutorialButton.colors;
        cb.normalColor = tutorialController.IsActive() ? Color.white : Color.green;
        tutorialButton.colors = cb;

        tutorialController.TurnOffOn();
    }

    public void Play()
    {
        if (modeController.IsFirstPlay())
        {
            modeController.SetFirstPlay();
            movementController.ResetPosition();
        }

        menu.SetActive(false);
        movementController.SetIsPlaying(true);
    }

    public void Reset()
    {
        movementController.ResetPosition();
        movementController.SetIsPlaying(false);
    }

    public void Pause()
    {
        menu.SetActive(true);
        movementController.SetIsPlaying(false);
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
        else if (type == "OCTAVE")
        {
            start = NOTE_MODIFIERS_END_INDEX;
            end = OCTAVE_END_INDEX;
        }
        else
        {
            start = OCTAVE_END_INDEX;
            end = CLEAR_END_INDEX;
        }
    }

    private void TimedButtonHighlight()
    {
        Button button;
        ColorBlock cb;

        if (Time.time - highlightTimer > 3.0f)
        {
            button = playGateButton;
            cb = button.colors;
            cb.normalColor = Color.white;
            button.colors = cb;

            button = saveButton;
            cb = button.colors;
            cb.normalColor = Color.white;
            button.colors = cb;

            button = loadButton;
            cb = button.colors;
            cb.normalColor = Color.white;
            button.colors = cb;

            isHighlightTimerOn = false;
        }
    }
}
