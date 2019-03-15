using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMenuController : MonoBehaviour
{
    readonly string ARROW = " ▼";

    public Button dropdownButton;
    public Button[] dropdownButtonOptions;

    private bool expanded;

    // Start is called before the first frame update
    void Start()
    {
        expanded = false;

        foreach (var button in dropdownButtonOptions)
        {
            button.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExpandOrCollapseDropdown()
    {
        expanded = !expanded;

        foreach (var button in dropdownButtonOptions)
        {
            button.gameObject.SetActive(expanded);
        }
    }
}
