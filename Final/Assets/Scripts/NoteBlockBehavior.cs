using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBlockBehavior : MonoBehaviour
{
    private Note note;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignNote(Note newNote)
    {
        // update the render material
        // assign the current note value
        note = newNote;
        this.GetComponent<Renderer>().material = new Material(note.getMaterial());
    }
}
