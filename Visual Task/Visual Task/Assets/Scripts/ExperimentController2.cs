// This script is our experiment controller, it holds several coroutines for : 
// - Saving participant data in CSV, running a block (holding 16 trials), changing scenes



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
using System.Linq;

public class ExperimentController2 : MonoBehaviour {

// REFERENCES PART : 
    

/// I. Block and Trials references 
    public int i = 0; // stimuli index 



/// II. Realism task and Self Reference task references 
    [SerializeField] public TextMeshProUGUI TimerText; 
    [SerializeField] public float TotalTrialTime = 10f;
    public float ElapsedTime = 0f;
    public int RNote;    
    public int SRNote;
    public ToggleGroup toggleGroup;
    public ToggleGroup toggleGroupR;


/// III. CAMERA
    [SerializeField] public GameObject GoNARcam;
    [SerializeField] public Camera NARcam;
    [SerializeField] public GameObject GoARcam;
    [SerializeField] public Transform[] NARstimuli;
    [SerializeField] public float distanceToTarget = 700;


/// IV. CANVAS references 
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

/// V. CSV Methods references
    public string ParticipantPath; 
    public SaveParticipant saveParticipant;
    public string file, myfolder, CSVfolder;




/// I. Block and Trials

    // Start Method 
    public void Start() 
    {
    StartCoroutine(RequestStoragePermission());
    GoARcam.SetActive(true);
    }

    // ONE TRIAL : WHEN SCANNING A TARTGET
    int numTrials = 16; //Number of trial in total

    IEnumerator RunTrials(int numTrials)
    {
            Debug.Log("Coroutine Lancée");
            ScannerToFeedBack();
            yield return new WaitForSeconds(1);
            Debug.Log("Trial" + i.ToString());

            // Active the i stimulus pre-defined in the inspector 
            stimuli[i].SetActive(true);
        

            // Check if the current stimulus has the "NAR" tag ou check si dans la liste de NARStimuli
            if (stimuli[i].CompareTag("NAR"))
            {
                
                TimerGO.SetActive(true);
                // Item Visualization
                ScannertoVisu();
                NARTrialCanvas.SetActive(true);
                GoARcam.SetActive(false);
                GoNARcam.SetActive(true);
                StartCoroutine(CamMovement());
                yield return new WaitForSeconds(10);
                VisuCanvas.SetActive(false);
                FeedBackCanvas.SetActive(true);
                yield return new WaitForSeconds(1);
                FeedBackCanvas.SetActive(false);


                // Self reference task 
                VisutoSR();
                SRCanvas.SetActive(true);
                NARTrialCanvas.SetActive(true);
                yield return new WaitForSeconds(10);
                SRCanvas.SetActive(false);
                FeedBackCanvas.SetActive(true);
                yield return new WaitForSeconds(1);
                FeedBackCanvas.SetActive(false);

                // Realism Task
                SRtoRealism();
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
                ScannertoARTrialCanvas();
                // Wait for slider input
                ScannertoVisu(); 
                yield return new WaitForSeconds(10);
                VisuCanvas.SetActive(false);
                VisutoSR();
                SRCanvas.SetActive(true);
                yield return new WaitForSeconds(10);
                SRCanvas.SetActive(false);
                SRtoRealism();
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
    public void Block()
    {
        // Increment the number of completed trials
        i++;
        
        // If there are still trials remaining, start the next one
            if (i <= numTrials)
            {   
                StartCoroutine(RunTrials(1));
            }
    }

/// II. Realism task and Self Reference task references 
 
public IEnumerator Timer(){
    TimerText.gameObject.SetActive(true);
    ElapsedTime = 0f;

    while (ElapsedTime < TotalTrialTime)
    {
    ElapsedTime += Time.deltaTime;
    TimerText.text = (TotalTrialTime - ElapsedTime).ToString("0");
    yield return null;
    }
}

public void ResetDefaultValueSR()
{
   foreach (Toggle toggle in toggleGroup.ActiveToggles())
   {

               toggle.isOn = false;

   }

}

public void ResetDefaultValueR()
{
      foreach (Toggle toggleR in toggleGroupR.ActiveToggles())
   {
               toggleR.isOn = false;
   }

}

public IEnumerator WaitAndSaveSelectedValueSR()
{
   SRNote = 0;

   yield return new WaitForSeconds(10f);

    Toggle toggle = toggleGroup.ActiveToggles().First();
   if (toggle != null)
   {
       if (int.TryParse(toggle.GetComponentInChildren<Text>().text, out int parsedSRNote))
       {
           SRNote = parsedSRNote;
           Debug.Log("Saved SRNote as an integer after 10 seconds: " + SRNote);
           // Deselect the radio button
           toggle.isOn = false;
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

public IEnumerator WaitForRadioButtonInputSR()
{
    ResetDefaultValueSR();
    StartCoroutine(WaitAndSaveSelectedValueSR());

    float startTime = Time.time;
    float elapsedTime = 0f;

    while (elapsedTime < 10f)
    {
        elapsedTime = Time.time - startTime;
        yield return null;
    }

    if (elapsedTime >= 10f)
    {
        Debug.LogError(SRNote);
    }
}

public IEnumerator WaitAndSaveSelectedValueR()
{
    RNote = 0;
    
    
    yield return new WaitForSeconds(10f);

   Toggle toggleR = toggleGroupR.ActiveToggles().First();
    if (toggleR != null)
    {
        if (int.TryParse(toggleR.GetComponentInChildren<Text>().text, out int parsedRNote))
        {
            RNote = parsedRNote;
            Debug.Log("Saved SRNote as an integer after 10 seconds: " + RNote);
            toggleR.isOn = false;

        }
        else
        {
            Debug.Log("Failed to parse RNote as an integer after 10 seconds.");
        }
    }
    else
    {
        Debug.Log("No toggle selected after 10 seconds.");
    }

}
  
public IEnumerator WaitForRadioButtonInputR()
{
    ResetDefaultValueR();
    StartCoroutine(WaitAndSaveSelectedValueR());

    float startTime = Time.time;
    float elapsedTime = 0f;

    while (elapsedTime < 10f)
    {
        elapsedTime = Time.time - startTime;
        yield return null;
    }

    if (elapsedTime >= 10f)
    {
        Debug.LogError(RNote);
        RnoteWriteCSV(); 
    }
}



/// III. Camera 
 public IEnumerator CamMovement()
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



/// IV. Canvas and Scenes Controller  
    public void ChooseScene()
    {
        // Get the selected ID from PlayerPrefs
        int selectedID = PlayerPrefs.GetInt("DropdownValue", 0) +1;

        // The randomized list of 1s and 0s
        int[] randomList = { 1, 1, 1, 1, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 1};

        // Check if the selected ID is within the valid range
        if (selectedID > 0 && selectedID <= randomList.Length)
        {
            // Check the corresponding value in the randomized list
            int sceneType = randomList[selectedID - 1];

            // Load the scene based on the value in the list
            if (sceneType == 1)
            {
                // Load Odd_scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            }
            else
            {
                // Load Paired_scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        else
        {
            // Handle invalid selected ID
            Debug.LogError("Invalid selected ID: " + selectedID);
        }
    }
  
    public void ActiveSRTask()
    {
        SRCanvas.SetActive(true);
    }

    public void DesactiveSRTask()
    {
        SRCanvas.SetActive(false);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ScannerToFeedBack()
    {   
        ScannerCanvas.SetActive(false);
        FeedBackCanvas.SetActive(true);
    }

        public void ScannertoVisu() 
    {
        FeedBackCanvas.SetActive(false);
        VisuCanvas.SetActive(true);
        StartCoroutine(Timer());

        Debug.Log("Visualisation du S");
    }

    public void VisutoSR()
    {
        StartCoroutine(Timer());
        StartCoroutine(WaitForRadioButtonInputSR());
        Debug.Log("Coroutine SR");
    }

    public void SRtoRealism()
    {
        StartCoroutine(Timer());
        StartCoroutine(WaitForRadioButtonInputR());
        Debug.Log("Coroutine Realism");
    }


    public void ScannertoARTrialCanvas()
    {
        FeedBackCanvas.SetActive(false);
        ARTrialCanvas.SetActive(true);
    }

    public void NewScanToScanner()
    {   
        NewScanCanvas.SetActive(false);
        ScannerCanvas.SetActive(true);

    }

   public void PlusOne()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
    public void PlusThree()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +3);
    }
    public void MinusThree()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +3);
    }

    public void PlusFour()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +4);
    }
    public void MinusFour()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +4);
    }


// V. CSV Controller 
    //// Coroutine to request user permission to use storage 
    IEnumerator RequestStoragePermission()
    {
        // Check if the user has already granted permission
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) &&
            Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            // Permission already granted, proceed with your code
            yield break;
        }

        // Request permission from the user
        Permission.RequestUserPermission(Permission.ExternalStorageRead);
        Permission.RequestUserPermission(Permission.ExternalStorageWrite);

        // Check if permission was granted
        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) &&
            Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            // Permission granted, proceed with your code
            string csvFolder = "";
            #if UNITY_EDITOR
                csvFolder = Application.dataPath + "/CSVFolder/";
                Debug.Log("File at " + csvFolder + " EXISTE");

            #elif UNITY_ANDROID
                csvFolder = GetPathAndroid() + "/CSVFolder/";
            #else
                csvFolder = Application.dataPath + "/";
            #endif

            // Construct the path to the CSV file
            int SelectionDD = PlayerPrefs.GetInt("DropdownValue", 0);
            string fileName = $"P{SelectionDD +1}_EP.csv";
            ParticipantPath = Path.Combine(csvFolder, fileName);
        }
        else
        {
            // Permission denied, handle the error
            Debug.LogError("External Storage permission denied");
        }
    }

    //// Methods for reading through a specific CSV File
    public static string GetPathAndroid() // A voir si on peut delete
    {
        string path = "";

        // Get the default external storage directory
        AndroidJavaClass environment = new AndroidJavaClass("android.os.Environment");
        AndroidJavaObject directory = environment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory");

        // Get the Downloads directory path
        AndroidJavaObject downloadsDir = directory.Call<AndroidJavaObject>("getAbsolutePath");
        path = downloadsDir.Call<string>("concat", "/Download");

        return path;
    } 

    public string GetPath() // Returns a participant path (string) used in the READ & WRITE Methods
    {
        int SelectionDD = PlayerPrefs.GetInt("DropdownValue", 0);
        string fileName = $"P{SelectionDD +1}_EP.csv";
        string csvFolder;

        #if UNITY_EDITOR
            csvFolder = Application.dataPath + "/CsvFolder/";
            Debug.Log("File at " + csvFolder + " EXISTE");
        #elif UNITY_ANDROID
            csvFolder = Application.persistentDataPath + "/CsvFolder/";
            Debug.Log("File at " + csvFolder + " EXISTE");
        #else
            csvFolder = Application.dataPath + "/";
        #endif

        string ParticipantPath = Path.Combine(csvFolder, fileName);
        Debug.Log("Valeur selectionnée" + SelectionDD);
        return ParticipantPath;
    }

    // Method to READ the csv file 
    public void ReadCSV() 
    {
        file = "";
        
        string folderPath = Path.GetDirectoryName(GetPath());
        if (!Directory.Exists(folderPath))
        {
            // Directory doesn't exist, create it
            Directory.CreateDirectory(folderPath);
        }

        if (!File.Exists(GetPath()))
        {
            // File doesn't exist, create it
            File.WriteAllText(GetPath(), "subj_id,SRnote,RNote\n");
        }
        else
    {
        // File does not exist, create it
        Debug.LogWarning("File at does not exist. Creating the file...");

        int SelectionDD = PlayerPrefs.GetInt("DropdownValue", 0);
        string fileName = $"P{SelectionDD +1}_EP.csv";

        // Combine the folder path and file name
        string filePath = Path.Combine(Application.dataPath, "CsvFolder", fileName);

        // Create the directory if it doesn't exist
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        // Create the file
        File.WriteAllText(filePath, "subj_id,SRnote,RNote\n");

        Debug.Log("File created at: " + filePath);

        
    }
    }

    // string with the content of the csv file 
    private string ReadCSV2(string ParticipantPath)
    {
        if (File.Exists(ParticipantPath))
        {
            return File.ReadAllText(ParticipantPath);
        }
        else
        {
            Debug.LogError("File at " + ParticipantPath + " does not exist");
            return "";
        }
    }


    // method to record the Realism and self-references notes on the csv file 
    public void RnoteWriteCSV()
    {
        string filePath = GetPath();

        // Get the participant index from PlayerPrefs to indicate which participant it is
        int participantIndex = PlayerPrefs.GetInt("DropdownValue", 0) +1;

        // Create new lines to append to the CSV file
        string newLine = participantIndex.ToString() + "," + SRNote + "," + RNote;

        // Append the new lines to the end of the CSV file
        using (StreamWriter sw = new StreamWriter(filePath, true))
        {
            // Check if the file is empty, and if so, write the header
            if (sw.BaseStream.Length == 0)
            {
                sw.WriteLine("subj_id,SRnote,RNote");
            }

            sw.WriteLine(newLine);
            Debug.Log("Ecriture de" + newLine);
        }
    }
 
}   