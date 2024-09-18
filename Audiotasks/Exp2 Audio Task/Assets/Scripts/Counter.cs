using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Counter : MonoBehaviour 
{
    public TextMeshProUGUI ClicksView;

    static int TotalClicks;

    private void Start()
    {
        UpdateClicksView();
        Debug.LogError("The count is " + TotalClicks);
    }

    public void AddClicks()
    {
        TotalClicks++;
        UpdateClicksView();
    }

    public void SaveCount()
    {
        // Save the TotalClicks value to PlayerPrefs
        PlayerPrefs.SetInt("TotalClicks", TotalClicks);
        PlayerPrefs.Save();
    }

    private void UpdateClicksView()
    {
        ClicksView.text = TotalClicks.ToString("0");
    }
}
