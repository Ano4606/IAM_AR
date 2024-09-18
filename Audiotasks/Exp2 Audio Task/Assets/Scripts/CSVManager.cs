


using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Android;
using System;
using System.Linq;

public class CSVManager : MonoBehaviour
{
    // Declare references to get info from our scenes

    // Realism task and Self Reference task references 
    [HideInInspector]
    public int LSNote;
    public ToggleGroup toggleGroup;

    // Ref for participant selection
    [SerializeField] public float Time_ScanRP;
    [SerializeField] public float Time_IAMs;
    [SerializeField] public float Time_End;
    [SerializeField] public TMP_InputField RP_IAMsContent;
    [SerializeField] public TextMeshProUGUI ClicksView;
    [SerializeField] public string ParticipantPath;
    [SerializeField] public SaveParticipant saveParticipant;
    [SerializeField] public Counter counter;
    [SerializeField] public string file, myfolder, CSVfolder;
    [SerializeField] public GameObject LikertScale;
    [SerializeField] public TMP_InputField RP_Stimulus;
    public ExpAudio expAudio;

    private void Start()
    {
        StartCoroutine(RequestStoragePermission());
        expAudio = ExpAudio.instance;
    }

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
            string fileName = $"P{SelectionDD + 1}_RP.csv";
            ParticipantPath = Path.Combine(csvFolder, fileName);
        }
        else
        {
            // Permission denied, handle the error
            Debug.LogError("External Storage permission denied");
        }
    }

    public static string GetPathAndroid()
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

    // Function to get the path to the CSV file
    public string GetPath()
    {
    int SelectionDD = PlayerPrefs.GetInt("DropdownValue", 0);
    string fileName = $"P{SelectionDD + 1}_RP.csv";
    string csvFolder;

    #if UNITY_EDITOR
        csvFolder = Application.dataPath + "/CSVFolder/";
        Debug.Log("File at " + csvFolder + " EXISTE");

    #elif UNITY_ANDROID
        csvFolder = Application.persistentDataPath + "/CSVFolder/";
        Debug.Log("File at " + csvFolder + " EXISTE");
    #else
        csvFolder = Application.dataPath + "/";
    #endif

    string ParticipantPath = Path.Combine(csvFolder, fileName);
    return ParticipantPath;
    }

    public void ReadCSV()
    {
        file = "";

        if (!File.Exists(GetPath()))
        {
            // File doesn't exist, create it
            File.WriteAllText(GetPath(), "subj_id,ThoughtTime,Content,Item,voluntariness\n");
        }

        if (File.Exists(GetPath()))
        {
            FileStream fileStream = new FileStream(GetPath(), FileMode.Open, FileAccess.ReadWrite);
            StreamReader read = new StreamReader(fileStream);
            file = read.ReadToEnd();
            read.Close(); // Always close the file after reading/writing
            Debug.Log("Contents of CSV file:\n" + file);
        }
    }


/// IAMs DROPDOWN (NOT USED): 

    //public void recordIAM() 
    //{
    //stimulus = dropdownStimulus.options[dropdownStimulus.value].text;
    //Debug.Log("Stimulus recorded: " + stimulus);
    //}


/// LIKERT SCALE : Methods handling the value of the likert scale corresponding to the voluntariness of the thought

public void ResetDefaultValueLS()
{
   foreach (Toggle toggle in toggleGroup.ActiveToggles())
   {
               toggle.isOn = false;
   }

}

public void WaitAndSaveSelectedValueLS()
{

    Toggle toggle = toggleGroup.ActiveToggles().First();
   if (toggle != null)
   {
       if (int.TryParse(toggle.GetComponentInChildren<Text>().text, out int parsedLSNote))
       {
           LSNote = parsedLSNote;
           Debug.Log("Saved LSNote as an integer :" + LSNote);
           // Deselect the radio button
           toggle.isOn = false;
               Debug.Log("C'est la ParsedNote" + parsedLSNote);
       }
       else
       {
           Debug.Log("Failed to parse LSNote as an integer ");
       }
   }
   else
   {
       Debug.Log("No toggle selected after 10 seconds.");
   }
}

public void AfficherLS()
{
    Debug.Log("C'est la LSNOTE" + LSNote);

    Debug.Log("Content" + RP_IAMsContent.text);

    Debug.Log("Stmulus" + RP_Stimulus.text);
}

public void IAMsWriteCSV()
{
    string filePath = GetPath();

    // Read all lines from the CSV file into a string array
    string[] lines = File.ReadAllLines(filePath);

    // Get the participant index from PlayerPrefs to indicate which participant it is
    int participantIndex = PlayerPrefs.GetInt("DropdownValue", 0) + 1;
    
    // Get the current date and time when IAM is recorded
    string Time_IAMs = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    // Create a new line to append to the CSV file
    string newLine = participantIndex.ToString() + "," + Time_IAMs + "," + RP_IAMsContent.text + "," +  RP_Stimulus.text + "," + LSNote;

    // Append the new line to the end of the CSV file
    using (StreamWriter sw = File.AppendText(filePath))
    {
        sw.WriteLine(newLine);
    }
}



}

