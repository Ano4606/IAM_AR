// This script is our experiment controller, it holds a coroutine corresponding to a trial
// For now it can only display a specific stimulus in AR or NAR depending on the csv participant 


using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExperimentController3 : MonoBehaviour {

// REFERENCES PART : 

/// I. CSV Methods references
    public string ParticipantPath; 
    public SaveParticipant saveParticipant;
    public string file, myfolder, CSVfolder;
    

/// II. DISPLAY Methods references 
    public int i = 9;

/// III. CANVAS references 
    [SerializeField] public GameObject ScannerCanvas; 
    [SerializeField] public GameObject FeedBackCanvas;
    [SerializeField] public GameObject NewScanCanvas;
    [SerializeField] public GameObject NARTrialCanvas;
    [SerializeField] public GameObject ARTrialCanvas;
    [SerializeField] public GameObject VisuCanvas;
    [SerializeField] public GameObject RealismTCanvas;
    [SerializeField] public GameObject SRCanvas;
    [SerializeField] public GameObject TimerGO;
    [SerializeField] public List<GameObject > stimuli = new List<GameObject >(); // List all the NAR stimuli

/////// AUDIO 


/// IV. REALISM TASK  references 
    [SerializeField] public Slider _slider;
    [SerializeField] public TextMeshProUGUI RNote;
    [SerializeField] public TextMeshProUGUI TimerText; 
    [SerializeField] public float TotalTrialTime = 10f;
    public float ElapsedTime = 0f;
    int Rnote = 0;

/// IV. SELF REFERENCE TASK  references 
    [SerializeField] public Slider _sliderSR;
    [SerializeField] public TextMeshProUGUI SRNote;
    int SRnote = 0;


// SCENE ASSOCIATION 
    [SerializeField] public GameObject GoNARcam;
    [SerializeField] public Camera NARcam;
    [SerializeField] public GameObject GoARcam;
    [SerializeField] public Transform[] NARstimuli;
    [SerializeField] public float distanceToTarget = 700;



// TRIAL PART PAIRED PARTICIPANT : 
    /// Start Method 
    public void Start() 
    {
    //StartCoroutine(RequestStoragePermission());
    GoARcam.SetActive(true);
    }

    /// ONE TRIAL : WHEN SCANNING A TARTGET
    int numTrials = 2; //Number of trial in total

    IEnumerator RunTrialsDemo(int numTrials)
    {
            Debug.Log("Coroutine Lancée");
            ScannerToFeedBackDemo();
            yield return new WaitForSeconds(1);
            Debug.Log("Trial" + i.ToString());

            // Active the i stimulus pre-defined in the inspector 
            stimuli[i].SetActive(true);
            
            // Reset the value of the slider to 3
            _slider.value = 3;
            _sliderSR.value = 3;

            // Check if the current stimulus has the "NAR" tag ou check si dans la liste de NARStimuli
            if (stimuli[i].CompareTag("NAR"))
            {
                
                TimerGO.SetActive(true);
                // Item Visualization
                ScannertoVisuDemo();
                NARTrialCanvas.SetActive(true);
                GoARcam.SetActive(false);
                GoNARcam.SetActive(true);
                StartCoroutine(CamMovementDemo());
                yield return new WaitForSeconds(10);
                VisuCanvas.SetActive(false);

                // Self reference task 
                VisutoSRDemo();
                SRCanvas.SetActive(true);
                NARTrialCanvas.SetActive(true);
                yield return new WaitForSeconds(10);
                SRCanvas.SetActive(false);

                // Realism Task
                SRtoRealismDemo();
                RealismTCanvas.SetActive(true);
                NARTrialCanvas.SetActive(true);
                yield return new WaitForSeconds(10);
                RealismTCanvas.SetActive(false);

                // Reset Scene
                NARTrialCanvas.SetActive(false);
                GoNARcam.SetActive(false); 
                TimerGO.SetActive(false);
                GoARcam.SetActive(true);
            }

            else // For AR Condition, active the UI showing the timer & the slider
            {
               
                TimerGO.SetActive(true);
                ScannertoARTrialCanvasDemo();
                // Wait for slider input
                ScannertoVisuDemo(); 
                yield return new WaitForSeconds(10);
                VisuCanvas.SetActive(false);
                VisutoSRDemo();
                SRCanvas.SetActive(true);
                yield return new WaitForSeconds(10);
                SRCanvas.SetActive(false);
                SRtoRealismDemo();
                RealismTCanvas.SetActive(true);
                yield return new WaitForSeconds(10);
                RealismTCanvas.SetActive(false);
                ARTrialCanvas.SetActive(false);
                TimerGO.SetActive(false);
            }

            stimuli[i].SetActive(false);
            
            
            NewScanCanvas.SetActive(true);

    }

    /// BLOCK 
    public void BlockDemo()
    {
        // Increment the number of completed trials
        i++;
        
        // If there are still trials remaining, start the next one
            if (i <= numTrials)
            {   
                StartCoroutine(RunTrialsDemo(1));
            }
    }

// METHODS PART : 
    public IEnumerator TimerDemo(){
            TimerText.gameObject.SetActive(true);
            ElapsedTime = 0f;

            while (ElapsedTime < TotalTrialTime)
            {
            ElapsedTime += Time.deltaTime;
            TimerText.text = (TotalTrialTime - ElapsedTime).ToString("0");
            yield return null;
            }
    }

/// Realism task 

    public void UpdateSliderTextDemo(float value)
    {
        RNote.text = value.ToString("0");
    }

    IEnumerator WaitForSliderInputRealismDemo()
    {   
        // ActiveRealismTask();
        float startTime = Time.time;
        float elapsedTime = 0f;

        _slider.onValueChanged.AddListener(UpdateSliderTextDemo);
        while (elapsedTime < 10f && _slider.value != 0f)
        {
            elapsedTime = Time.time - startTime;
            yield return null;
        }

        if (elapsedTime >= 10f)
        {
            Rnote = Mathf.RoundToInt(_slider.value);
            Debug.Log("Voici la note" + Rnote);
            //RnoteWriteCSV();
            // DesactiveRealismTask();
        }
        _slider.onValueChanged.RemoveListener(UpdateSliderTextDemo);

    }

/// Self Reference task 

    public void UpdateSliderTextSRDemo(float value)
    {
        SRNote.text = value.ToString("0");
    }

    IEnumerator WaitForSliderInputSRDemo()
    {   
        // ActiveRealismTask();
        float startTime = Time.time;
        float elapsedTime = 0f;

        _sliderSR.onValueChanged.AddListener(UpdateSliderTextSRDemo);
        while (elapsedTime < 10f && _sliderSR.value != 0f)
        {
            elapsedTime = Time.time - startTime;
            yield return null;
        }

        if (elapsedTime >= 10f)
        {
            SRnote = Mathf.RoundToInt(_sliderSR.value);
            Debug.Log("Voici la note" + SRnote);
            // RnoteWriteCSV();
            // DesactiveRealismTask();
        }
        _sliderSR.onValueChanged.RemoveListener(UpdateSliderTextSRDemo);

    }

// Camera 
 public IEnumerator CamMovementDemo()
{
    // Set the starting position of the camera
    NARcam.transform.position = NARstimuli[i].position;
    NARcam.transform.rotation = Quaternion.Euler(0, 0, 0);

    // Move the camera closer to the gameobject
    NARcam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

    // Set the initial distance between two touches for zoom control
    float initialPinchDistance = 0f;

 // Set the initial zoom factor
    float zoomFactor = 1f;

    // Set the minimum and maximum zoom factors
    float minZoomFactor = 0.5f;
    float maxZoomFactor = 1f;

    // Set the previous position to the current mouse position
    Vector2[] previousPositions = new Vector2[2];

    while (true)
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                // Store the initial distance between two touches for zoom control
                initialPinchDistance = Vector2.Distance(touch0.position, touch1.position);
            }
            else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                // Calculate the current distance between two touches
                float currentPinchDistance = Vector2.Distance(touch0.position, touch1.position);

                // Calculate the pinch difference to control zoom
                float pinchDifference = currentPinchDistance - initialPinchDistance;

                // Calculate the zoom factor based on pinch difference
                zoomFactor -= pinchDifference * 0.01f; // Adjust the multiplier based on your preference

                // Clamp the zoom factor to the specified range
                zoomFactor = Mathf.Clamp(zoomFactor, minZoomFactor, maxZoomFactor);

                // If the zoom factor is at the minimum and trying to dezoom, set it back to the minimum
                if (zoomFactor == minZoomFactor && pinchDifference < 0)
                {
                    zoomFactor = minZoomFactor;
                }

                // Update the camera position based on the zoom factor
                NARcam.transform.position = NARstimuli[i].position - NARcam.transform.forward * distanceToTarget * zoomFactor;

                // Update the initial pinch distance for the next frame
                initialPinchDistance = currentPinchDistance;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            previousPositions[0] = (Vector2)NARcam.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Player clicked on UI element
                // Do nothing
            }
            else
            {
                Vector3 newPosition = NARcam.ScreenToViewportPoint(Input.mousePosition);
                Vector3 direction = (Vector2)previousPositions[0] - (Vector2)newPosition;

                float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
                float rotationAroundXAxis = direction.y * 180; // camera moves vertically

                NARcam.transform.position = NARstimuli[i].position;

                NARcam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
                NARcam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World);

                // Move the camera closer to the gameobject
                NARcam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

                previousPositions[0] = (Vector2)newPosition;
            }
        }

        yield return null;
    }
}



/// I. Canvas Controller 

    public void ActiveSRTaskDemo()
    {
        SRCanvas.SetActive(true);
    }

    public void DesactiveSRTaskDemo()
    {
        SRCanvas.SetActive(false);
    }

    public void ReloadSceneDemo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ScannerToFeedBackDemo()
    {   
        ScannerCanvas.SetActive(false);
        FeedBackCanvas.SetActive(true);
    }

        public void ScannertoVisuDemo() 
    {
        FeedBackCanvas.SetActive(false);
        VisuCanvas.SetActive(true);
        StartCoroutine(TimerDemo());

        Debug.Log("Visualisation du S");
    }

    public void VisutoSRDemo()
    {
        StartCoroutine(TimerDemo());
        StartCoroutine(WaitForSliderInputSRDemo());
    }

    public void SRtoRealismDemo()
    {
        StartCoroutine(TimerDemo());
        StartCoroutine(WaitForSliderInputRealismDemo());
        Debug.Log("je suis lancé");
    }


    public void ScannertoARTrialCanvasDemo()
    {
        FeedBackCanvas.SetActive(false);
        ARTrialCanvas.SetActive(true);
    }

    public void NewScanToScannerDemo()
    {   
        NewScanCanvas.SetActive(false);
        ScannerCanvas.SetActive(true);

    }
}