using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FilteredPositions : MonoBehaviour
{
    // References for the user to change frequency in test mode
    public TextMeshProUGUI FrequencyText; 
    [SerializeField] public Slider _slider;

    
    // The filter frequency
    public float filterFrequency = 120.0f;
    // The filter for Vector3 positions
    OneEuroFilter<Vector3> vector3Filter;

    void Start()
    {
        // Initialize the filter with the provided frequency
        vector3Filter = new OneEuroFilter<Vector3>(filterFrequency);
    }

    // Method to be called by the parent script instead of using Update
    public void FilterPositions()
    {
        // Get all child objects of the current GameObject
        Transform[] children = GetComponentsInChildren<Transform>();

        // Iterate through each child and filter its position
        foreach (Transform child in children)
        {
            // Skip the current GameObject itself
            if (child == transform)
                continue;

            // Get the current position of the child
            Vector3 noisyPosition = child.position;

            // Filter the position using the OneEuroFilter
            Vector3 filteredPosition = vector3Filter.Filter(noisyPosition);

            // Update the child's position
            child.position = filteredPosition;
        }
    }

    public void UpdateFilterText(float newValue)
    {
        // change the value of filterFrequency
        filterFrequency = newValue;
        
        // Update the text on the UI
        FrequencyText.text = "Current Value: " + newValue.ToString();
    }

    public void UpdateFilterValue(int newValue)
    {
        // User choose the frequency value 
        _slider.onValueChanged.AddListener(UpdateFilterText);
    }
}
