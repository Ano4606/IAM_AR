// Ce script renvoie la valeur du dropdown comme playerpref pour être utilisée plus tard
// Encoding Phase 

using UnityEngine;
using System.IO;
using UnityEditor;
using TMPro;

public class SaveParticipant : MonoBehaviour 
{
    public TMP_Dropdown SelectionID;
    public string ParticipantPath;

    public void clickSaveParticipant() 
    {
        PlayerPrefs.SetInt("DropdownValue", SelectionID.value);
        int dropdownValue = SelectionID.value;
        Debug.Log("Dropdown value saved to player preferences: " + dropdownValue);

    }

    

    
}
