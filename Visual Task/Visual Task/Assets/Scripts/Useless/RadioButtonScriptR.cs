using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class RadioButtonScriptR : MonoBehaviour
{

    ToggleGroup toggleGroup;

    public int RNote;
 
    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }
 
    public void Submit()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        Debug.Log(toggle.name + " _ " + toggle.GetComponentInChildren<Text>().text);

        // Attempt to parse the text as an integer
        if (int.TryParse(toggle.GetComponentInChildren<Text>().text, out int parsedRNote))
        {
            // Parsing successful, assign the integer value to RNote
            RNote = parsedRNote;
        }
        else
        {
            // Parsing failed, handle the error or provide a default value
            Debug.LogError("Failed to parse RNote as an integer.");
        }
    }
   
   
   /*  private Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();

        // Add a listener to handle toggle changes
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            // Notify the ExperimentController2 script about the change
            ExperimentController2.OnRadioButtonSelected(toggle.name);
        }
    } */

}
