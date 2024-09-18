using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class RadioButtonScriptSR : MonoBehaviour
{
    ToggleGroup toggleGroup;
    public int SRNote;

    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        Debug.Log("L'echelle a été trouvée");

        // Start the coroutine to save the selected value after 10 seconds
    }

    // Coroutine to wait for 10 seconds and then save the selected value
   public IEnumerator WaitAndSaveSelectedValue()
    {
        yield return new WaitForSeconds(10f);

        // Get the selected toggle and try to parse its text value to an int
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (toggle != null)
        {
            string textValue = toggle.GetComponentInChildren<Text>().text;

            if (int.TryParse(textValue, out int parsedSRNote))
            {
                SRNote = parsedSRNote;
                Debug.Log("Saved SRNote as an integer after 10 seconds: " + SRNote);
            }
            else
            {
                Debug.Log("Failed to parse SRNote as an integer after 10 seconds.");
            }
        }
        else
        {
            Debug.Log("No toggle selected after 10 seconds.");
        }
    }
}
