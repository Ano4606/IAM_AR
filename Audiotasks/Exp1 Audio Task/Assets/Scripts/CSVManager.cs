


using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;
using TMPro;
using UnityEngine.Android;
using System;

public class CSVManager : MonoBehaviour
{
    // Declare references to get info from our scenes
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
            File.WriteAllText(GetPath(), "subj_id,ThoughtTime,Content,\n");
        }

        if (File.Exists(GetPath()))
        {
            FileStream fileStream = new FileStream(GetPath(), FileMode.Open, FileAccess.ReadWrite);
            StreamReader read = new StreamReader(fileStream);
            file = read.ReadToEnd();
            read.Close(); // Always close the file after reading/writing
            Debug.Log("Contents of CSV file:\n" + file);
        }


        // else
        // {
        // // File does not exist, create it
        // Debug.Log("File at does not exist. Creating the file...");

        // int SelectionDD = PlayerPrefs.GetInt("DropdownValue", 0);
        // string fileName = $"P{SelectionDD +1}_RP.csv";

        // // Combine the folder path and file name
        // string filePath = Path.Combine(Application.dataPath, "CSVFolder", fileName);

        // // Create the directory if it doesn't exist
        // Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        // // Create the file
        // File.WriteAllText(filePath, "rando_id,ThoughtTime,Content,\n");

        // Debug.Log("File created at: " + filePath);
        // }
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
    string newLine = participantIndex.ToString() + "," + Time_IAMs + "," + RP_IAMsContent.text;

    // Append the new line to the end of the CSV file
    using (StreamWriter sw = File.AppendText(filePath))
    {
        sw.WriteLine(newLine);
    }
}

// public void LastCountWriteCSV()
// {
//     string filePath = GetPath();

//     // Read all lines from the CSV file into a string array
//     string[] lines = File.ReadAllLines(filePath);

//     // Get the participant index from PlayerPrefs to indicate which participant it is
//     int participantIndex = PlayerPrefs.GetInt("DropdownValue", 0) + 1;
    
//     // Get the current date and time when IAM is recorded
//     string Time_End = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

//     // Get the participant count from PlayerPrefs from the Counter Script 
//     int participantCount = PlayerPrefs.GetInt("TotalClicks", 0);
//     Debug.LogError("The final count is " + participantCount);

//     // Create a new line to append to the CSV file
//     string newLine = participantIndex.ToString() + "," + Time_End + "," + participantCount.ToString();

//     // Append the new line to the end of the CSV file
//     using (StreamWriter sw = File.AppendText(filePath))
//     {
//         sw.WriteLine(newLine);
//     }
// }


}

