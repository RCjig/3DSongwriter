using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Tutorial Steps:
 * 0. Welcome text: welcome, please aim your ray at this text and press the right hand trigger to continue
 * 1. Create: start by creating gates, please click the down arrow and select 12
 * 2. Create P2: press create button
 * 3. Change mode: voila, you've created a tunnel of note gates! now let's change modes by pressing the change mode button
 * 4. Edit mode: this is edit mode, which allows you to select notes to "paint" onto note blocks
 *    select the correct inputs for C#4, then press enter to set your ray to the correct painting mode
 * 5. Now that you've set a note, you can start painting! Aim your ray at the black block to your right to "paint" it
 * 6. Great! Now, paint the whole column with that note.
 * 7. Next, input A3. 
 * 8. Now, paint the column to your left!
 * 9. Movement: Now, try walking through the gate. Look forward and gently press the left hand trigger until you hit the line
 *    at your feet and hear the notes of your gate. Notice that you can tell which directions the notes are coming from!
 * 10. Movement 2: Look at the column to your left and gently move toward it. Then, press the ">" button to listen to the
 *     gate again. Notice that the note you are closer to is louder!
 * 11. Free Play: Now, pick whatever notes you want and continue to paint the rest of the tunnel. When you're done, press the
 *     "Change Mode" button to enter play mode so that you can listen to your masterpiece!
 * 12. then press "Reset" to reset your position
 * 13. followed by "Play" to play your song. Remember that you can press the right hand trigger to pause! Select this text and press the right hand trigger to continue.
 * 14. That was lovely, really. Let's save your work. Press the "Save" button!
 * 15. Oh, no! Your tunnel disappeared...completely on accident. But don't worry, we saved for a reason! Press the "Load" button
 *     and watch it magically reappear.
 * 16. Your amazing composition has returned! If you want to make it go away, you can press the "Reset" button. Go ahead, try it.
 * 17. Now that you know all the controls, you are ready to spread you creative wings and write a beautiful song.
 *     Thanks for using our application!
 */

public class TeacherController : MonoBehaviour
{
    private readonly int FINAL_STEP = 17;
    private readonly string OTHER_SCENE = "MusicTunnel";

    private int currStep;

    public OVRInput.Controller rightController; // right touch controller
    public GameObject MusicBox;
    public string fileName;

    private OvrAvatar avatar; // player avatar
    private GameObject rightHand; // for ray cast position
    private ModeController modeController;
    private MenuController menuController;
    private MovementController movementController;
    private MusicBoxController musicBoxController;
    private TunnelController tunnelController;
    private TutorialController tutorialController;
    private Vector3 forward;
    private Vector3 up;
    private bool canPaint;
    private bool tutorialOn;
    private bool turnOnOnce;

    private Note currNote; // the note we are currently "painting"

    // Start is called before the first frame update
    void Start()
    {
        currStep = 0;

        rightHand = transform.parent.gameObject;
        avatar = gameObject.GetComponentInParent<OvrAvatar>();
        modeController = GameObject.Find("LogicController").GetComponent<ModeController>();
        menuController = GameObject.Find("LogicController").GetComponent<MenuController>();
        movementController = GameObject.Find("OVRPlayerController").GetComponent<MovementController>();
        musicBoxController = MusicBox.GetComponent<MusicBoxController>();
        tunnelController = GameObject.Find("Tunnel").GetComponent<TunnelController>();
        tutorialController = GameObject.Find("LogicController").GetComponent<TutorialController>();
        currNote = musicBoxController.GetNote("N♮N");
        canPaint = false;
        tutorialOn = false;
        turnOnOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        avatar.GetPointingDirection(OvrAvatar.HandType.Right, ref forward, ref up);
        if (currStep != 9)
            RayCast();
        else Step9();

    }

    private void HandleNoteBlockHit(GameObject noteBlock, bool rIndexTriggered)
    {
        if (movementController.GetIsPlaying()) return;

        NoteBlockBehavior behaviorController = noteBlock.GetComponent<NoteBlockBehavior>();
        behaviorController.Play();
        if (canPaint && rIndexTriggered)
            behaviorController.AssignNote(currNote);
    }

    private void UpdateMenuController(string updateType, string input)
    {
        canPaint = false;
        menuController.NotSet();
        menuController.ResetButtons(updateType);
        menuController.Enter(updateType, input);
    }

    private void HandleNoteButtonHit(GameObject hitObject)
    {
        string type = "";

        if (hitObject.name[0] == '♭' || hitObject.name[0] == '♮' || hitObject.name[0] == '♯') type = "NOTE_MODIFIER";
        else type = "NOTE";

        UpdateMenuController(type, hitObject.name[0].ToString());
    }

    private void HandleOctaveButtonHit(GameObject hitObject)
    {
        UpdateMenuController("OCTAVE", hitObject.name[0].ToString());
    }

    private void Proceed()
    {
        // step 13: change mode twice to get from play mode to edit mode
        // step 14: change mode twice to get from edit mode to create mode
        if (currStep == 13 || currStep == 14)
        {
            ChangeMode();
            ChangeMode();
            if (currStep == 13)
                menuController.Reset(); // reset position after playing so user can see tunnel again
        }

        else if (currStep == 16)
        {
            menuController.Hide();
        }

        currStep++;
        tutorialController.Next();
    }

    private void SwitchScenes()
    {
        SceneManager.LoadScene(OTHER_SCENE);
    }

    private void Step0 (GameObject hitObject)
    {
        WaitingForTutorialPanelHit(hitObject);
    }

    private void WaitingForTutorialPanelHit (GameObject hitObject)
    {
        if (hitObject.name.Equals("TutorialNextButton"))
        {
            if (currStep == FINAL_STEP)
                SwitchScenes();
            else
                Proceed();
        }
    }

    private void Step1 (GameObject hitObject)
    {
        if (hitObject.name.Equals("GateDropdown"))
            menuController.ExpandOrCollapseDropdown();

        else if (hitObject.name.Contains("GateDropdown_Option_1"))
        {
            menuController.SetDropdown(hitObject.name[hitObject.name.Length - 1] - '0' - 1);
            Proceed();
        }
    }

    private void Step2 (GameObject hitObject)
    {
        if (hitObject.name.Equals("CreateButton"))
        {
            tunnelController.CreateTunnel(menuController.GetNumberOfGates());
            Proceed();
        }
    }

    private void Step3 (GameObject hitObject)
    {
        WaitingForModeChange(hitObject);
    }

    private void ChangeMode()
    {
        modeController.ChangeMode();
    }

    private void WaitingForModeChange (GameObject hitObject)
    {
        if (hitObject.name.Equals("ChangeModeButton"))
        {
            ChangeMode();
            Proceed();
        }
    }

    private void Step4 (GameObject hitObject)
    {
        WaitingForDesiredNote(hitObject, "C♯4");
    }

    private void WaitingForDesiredNote (GameObject hitObject, string desiredNoteName)
    {
        if (hitObject.name.Contains("_NoteButton"))
            HandleNoteButtonHit(hitObject);
        else if (hitObject.name.Contains("_OctaveButton"))
            HandleOctaveButtonHit(hitObject);

        else if (hitObject.name.Equals("SetButton"))
        {
            if (menuController.Set())
            {
                string selectedNote = menuController.GetSelectedNote();
                this.currNote = musicBoxController.GetNote(selectedNote);
                if (this.currNote.getName().Equals(desiredNoteName))
                {
                    canPaint = true;
                    Proceed();
                } else { canPaint = false; }
            }
        }
    }

    private void Step5 (GameObject hitObject)
    {
        if (hitObject.name.Equals("NoteBlock_10_Gate_0"))
        {
            HandleNoteBlockHit(hitObject, true);
            Proceed();
        }
    }

    private void Step6 (GameObject hitObject)
    {
        WaitingForPaintedColumn(hitObject, 7, 13, "Gate_0", "C♯4");
    }

    private void WaitingForPaintedColumn (GameObject hitObject, int startIndex, int endIndex, string gateName, string noteName)
    {
        if (hitObject.name.Contains("NoteBlock"))
        {
            int blockIndex = int.Parse(hitObject.name.Split('_')[1]);
            if (blockIndex >= startIndex && blockIndex <= endIndex && hitObject.name.Contains(gateName))
                HandleNoteBlockHit(hitObject, true);
        }

        if (ColumnIsPainted(startIndex, endIndex, gateName, noteName))
            Proceed();
    }

    private bool ColumnIsPainted (int startIndex, int endIndex, string gateName, string noteName)
    {
        NoteBlockBehavior behaviorController;
        for (int i = startIndex; i <= endIndex; i++)
        {
            behaviorController = GameObject.Find("NoteBlock_" + i + "_" + gateName).GetComponent<NoteBlockBehavior>();
            if (!(behaviorController.GetNoteName().Equals(noteName)))
                return false;
        }
        return true;
    }

    private void Step7(GameObject hitObject)
    {
        WaitingForDesiredNote(hitObject, "A♮3");
    }

    private void Step8(GameObject hitObject)
    {
        WaitingForPaintedColumn(hitObject, 0, 6, "Gate_0", "A♮3");
    }

    private void Step9()
    {
        if (GameObject.Find("NoteGate_0").GetComponent<NoteGateController>().IsInLine())
            Proceed();
    }

    private void Step10(GameObject hitObject)
    {
        if (hitObject.name == "PlayGateButton")
        {
            menuController.StartTimedButtonHighlight("PLAY_GATE");
            tunnelController.PlayGates();
            Proceed();
        }
    }

    private void HandleEditModeInputs(GameObject hitObject, bool rIndexTriggered, bool rHandTriggered)
    {
        if (hitObject.name.Contains("NoteBlock"))
            HandleNoteBlockHit(hitObject, rIndexTriggered);

        else if (rHandTriggered)
        {
            if (hitObject.name.Contains("_NoteButton"))
                HandleNoteButtonHit(hitObject);

            else if (hitObject.name.Contains("_OctaveButton"))
                HandleOctaveButtonHit(hitObject);

            else if (hitObject.name == "ClearButton")
            {
                UpdateMenuController("CLEAR", "Clear");
            }

            else if (hitObject.name == "PlayGateButton")
            {
                menuController.StartTimedButtonHighlight("PLAY_GATE");
                tunnelController.PlayGates();
            }

            else if (hitObject.name == "SetButton")
            {
                if (menuController.Set())
                {
                    string selectedNote = menuController.GetSelectedNote();
                    this.currNote = musicBoxController.GetNote(selectedNote);
                    canPaint = true;
                }
            }
        }
    }

    private void Step11(GameObject hitObject, bool rIndexTriggered, bool rHandTriggered)
    {
        HandleEditModeInputs(hitObject, rIndexTriggered, rHandTriggered);
        if (rHandTriggered) WaitingForModeChange(hitObject);
    }

    private void Step12(GameObject hitObject)
    {
        if (hitObject.name == "ResetButton")
        {
            menuController.Reset();
            Proceed();
        }
    }

    private void HandlePlayModeInputs(GameObject hitObject, bool rIndexTriggered, bool rHandTriggered)
    {
        if (rIndexTriggered)
            HandlePlayModePauseInput();
        else if (rHandTriggered)
            HandlePlayModeMenuInput(hitObject);
    }

    private void HandlePlayModePauseInput()
    {
        if (tutorialOn && turnOnOnce)
        {
            tutorialController.TurnOffOn();
            turnOnOnce = false;
        }
        menuController.Pause();
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void HandlePlayModeMenuInput(GameObject hitObject)
    {
        if (hitObject == null) return;

        else if (hitObject.name == "PlayButton")
        {
            tutorialOn = tutorialController.IsActive();
            if (tutorialOn)
            {
                tutorialController.TurnOffOn();
                turnOnOnce = true;
            }

            menuController.Play();
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (hitObject.name == "ResetButton")
        {
            menuController.Reset();
        }
    }

    private void Step13(GameObject hitObject, bool rIndexTriggered, bool rHandTriggered)
    {
        HandlePlayModeInputs(hitObject, rIndexTriggered, rHandTriggered);
        if (rHandTriggered) WaitingForTutorialPanelHit(hitObject);
    }

    private void Step14(GameObject hitObject)
    {
        if (hitObject.name == "SaveButton")
        {
            menuController.StartTimedButtonHighlight("SAVE");
            tunnelController.WriteToFile(fileName);
            tunnelController.DestroyTunnel();
            Proceed();
        }
    }

    private void Step15(GameObject hitObject)
    {
        if (hitObject.name == "LoadButton")
        {
            menuController.StartTimedButtonHighlight("LOAD");
            tunnelController.LoadFromFile(fileName);
            Proceed();
        }
    }

    private void Step16(GameObject hitObject)
    {
        if (hitObject.name == "ResetButton")
        {
            tunnelController.DestroyTunnel();
            Proceed();
        }
    }

    private void Step17(GameObject hitObject)
    {
        // show exit text and tell user to press the back button to return to the main scene
        WaitingForTutorialPanelHit(hitObject);
    }

    private void ProcessRaycast(GameObject hitObject, bool rIndexTriggered, bool rHandTriggered)
    {
        if (rHandTriggered && hitObject.name.Equals("TutorialExitButton"))
            SwitchScenes();

        // handle edit and play mode inputs as per usual: pass in rIndexTriggered and rHandTriggered
        if (currStep == 11) Step11(hitObject, rIndexTriggered, rHandTriggered);
        else if (currStep == 13) Step13(hitObject, rIndexTriggered, rHandTriggered);

        else if (rHandTriggered)
        {
            switch (currStep)
            {
                case 0: Step0(hitObject); break;
                case 1: Step1(hitObject); break;
                case 2: Step2(hitObject); break;
                case 3: Step3(hitObject); break;
                case 4: Step4(hitObject); break;
                // cases 5 and 6 are handled below
                case 7: Step7(hitObject); break;
                // cases 8 and 9 are handled in Update()
                case 10: Step10(hitObject); break;
                // case 11 is handled above
                case 12: Step12(hitObject); break;
                // case 13 is handled above
                case 14: Step14(hitObject); break;
                case 15: Step15(hitObject); break;
                case 16: Step16(hitObject); break;
                case 17: Step17(hitObject); break;
            }
        }

        else if (rIndexTriggered)
        {
            if (currStep == 5) Step5(hitObject);
            else if (currStep == 6) Step6(hitObject);
            else if (currStep == 8) Step8(hitObject);
        }
    }

    private void RayCast()
    {
        string mode = modeController.GetMode();
        bool rIndexTriggered = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
        bool rHandTriggered = OVRInput.GetDown(OVRInput.RawButton.RHandTrigger);

        Ray ray = new Ray(rightHand.transform.position, forward);
        Debug.DrawRay(rightHand.transform.position, forward, Color.green);
        RaycastHit hit;
        GameObject hitObject = null;

        if (Physics.Raycast(ray, out hit))
        {
            hitObject = hit.collider.gameObject;
            ProcessRaycast(hitObject, rIndexTriggered, rHandTriggered);
        }
        else if (currStep == 13)
            Step13(hitObject, rIndexTriggered, rHandTriggered);
    }
}