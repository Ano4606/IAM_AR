using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;
using TMPro;
using UnityEngine.Android;
using System;

public class CSVManagerTS : MonoBehaviour
{

    // Declare references to get info from our scenes
    // Ref for participant selection
    
    public string ParticipantPath;
    public SaveParticipant saveParticipant;
    public string file, myfolder, CSVfolder;

    private void Start()
    {
        StartCoroutine(RequestStoragePermissionTS());
    }

    IEnumerator RequestStoragePermissionTS()
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
            Debug.Log("File at " + csvFolder + " EXISTE");
        #endif

        // Construct the path to the CSV file
        int selectionDD = PlayerPrefs.GetInt("DropdownValue", 0);
        string fileName = $"P{selectionDD + 1}_RP_TS.csv";
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
public string GetPathTS()
{
    int selectionDD = PlayerPrefs.GetInt("DropdownValue", 0);
    string fileName = $"P{selectionDD + 1}_RP_TS.csv";
    string csvFolder;

    #if UNITY_EDITOR
        csvFolder = Application.dataPath + "/CSVFolder/";
        Debug.Log("File at " + csvFolder + " EXISTE");

    #elif UNITY_ANDROID
        csvFolder = Application.persistentDataPath + "/CSVFolder";
        Debug.Log("File at " + csvFolder + " EXISTE");
    #else
        csvFolder = Application.dataPath + "/";
    #endif

    string participantPath = Path.Combine(csvFolder, fileName);
    return participantPath;
}

// Read the CSV file at the selected path and return its contents
public void ReadTSCSV()
{
    file = "";
    
    if (File.Exists(GetPathTS()))
    {
        FileStream fileStream = new FileStream(GetPathTS(), FileMode.Open, FileAccess.ReadWrite);
        StreamReader read = new StreamReader(fileStream);
        file = read.ReadToEnd();
        read.Close(); // Always close the file after reading/writing
        Debug.Log("Contents of CSV file:\n" + file);
    }
    else
    {
        // File does not exist, create it
        Debug.LogWarning("File does not exist. Creating the file...");

        // Combine the folder path and file name
        string filePath = GetPathTS();

        // Create the directory if it doesn't exist
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        // Create the file
        File.WriteAllText(filePath, "rando_id,Last_TimeScan\n");

        Debug.Log("File created at: " + filePath);
    }
}

public void TScanWriter()
{
    string filePath = GetPathTS();

    // Read all lines from the CSV file into a string array
    string[] lines = File.ReadAllLines(filePath);

    // Get the participant index from PlayerPrefs to indicate which participant it is
    int participantIndex = PlayerPrefs.GetInt("DropdownValue", 0) + 1;
    
    // Get the current date and time when IAM is recorded
    string timeScanRP = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    // Create a new line to append to the CSV file
    string newLine = participantIndex.ToString() + "," + timeScanRP;

    // Append the new line to the end of the CSV file
    using (StreamWriter sw = File.AppendText(filePath))
    {
        sw.WriteLine(newLine);
    }
}


//     IEnumerator RequestStoragePermissionTS()
//     {
//         // Check if the user has already granted permission
//         if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) &&
//             Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
//         {
//             // Permission already granted, proceed with your code
//             yield break;
//         }

//         // Request permission from the user
//         Permission.RequestUserPermission(Permission.ExternalStorageRead);
//         Permission.RequestUserPermission(Permission.ExternalStorageWrite);

//         // Check if permission was granted
//         if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) &&
//             Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
//         {
//             // Permission granted, proceed with your code
//             string csvFolder = "";
//             #if UNITY_EDITOR
//                 csvFolder = Application.dataPath + "/CSVFolder/";
//                 Debug.Log("File at " + csvFolder + " EXISTE");

//             #elif UNITY_ANDROID
//                 csvFolder = GetPathAndroid() + "/CSVFolder/";
//             #else
//                 csvFolder = Application.dataPath + "/";
//                 Debug.Log("File at " + csvFolder + " EXISTE");
//             #endif

//             // Construct the path to the CSV file
//             int SelectionDD = PlayerPrefs.GetInt("DropdownValue", 0);
//             string fileName = $"P{SelectionDD + 1}_RP_TS.csv";
//             ParticipantPath = Path.Combine(csvFolder, fileName);
//         }
//         else
//         {
//             // Permission denied, handle the error
//             Debug.LogError("External Storage permission denied");
//         }
//     }

//     public static string GetPathAndroid()
//     {
//         string path = "";

//         // Get the default external storage directory
//         AndroidJavaClass environment = new AndroidJavaClass("android.os.Environment");
//         AndroidJavaObject directory = environment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory");

//         // Get the Downloads directory path
//         AndroidJavaObject downloadsDir = directory.Call<AndroidJavaObject>("getAbsolutePath");
//         path = downloadsDir.Call<string>("concat", "/Download");

//         return path;
//     }

  

//     // Function to get the path to the CSV file
//     public string GetPathTS()
//     {
//     int SelectionDD = PlayerPrefs.GetInt("DropdownValue", 0);
//     string fileName = $"P{SelectionDD + 1}_RP_TS.csv";
//     string csvFolder;

//     #if UNITY_EDITOR
//         csvFolder = Application.dataPath + "/CSVFolder/";
//         Debug.Log("File at " + csvFolder + " EXISTE");

//     #elif UNITY_ANDROID
//         csvFolder = Application.persistentDataPath + "/CSVFolder";
//         Debug.Log("File at " + csvFolder + " EXISTE");
//     #else
//         csvFolder = Application.dataPath + "/";
//     #endif

//     string ParticipantPath = Path.Combine(csvFolder, fileName);
//     return ParticipantPath;
//     }
//     // Read the CSV file at the selected path and return its contents
//     public void ReadTSCSV()
//     {
//         file = "";
        
//         if (File.Exists(GetPathTS()))
//         {
//             FileStream fileStream = new FileStream(GetPathTS(), FileMode.Open, FileAccess.ReadWrite);
//             StreamReader read = new StreamReader(fileStream);
//             file = read.ReadToEnd();
//             read.Close(); // Always close the file after reading/writing
//             Debug.Log("Contents of CSV file:\n" + file);
//         }
//         else
//         {
//         // File does not exist, create it
//         Debug.LogWarning("File at does not exist. Creating the file...");

//         int SelectionDD = PlayerPrefs.GetInt("DropdownValue", 0);
//         string fileName = $"P{SelectionDD +1}_RP_TS.csv";

//         // Combine the folder path and file name
//         string filePath = Path.Combine(Application.dataPath, "CSVFolder", fileName);

//         // Create the directory if it doesn't exist
//         Directory.CreateDirectory(Path.GetDirectoryName(filePath));

//         // Create the file
//         File.WriteAllText(filePath, "rando_id,Last_TimeScan\n");

//         Debug.Log("File created at: " + filePath);
//         }
//         }


// public void TScanWriter()
// {
//     string filePath = GetPathTS();

//     // Read all lines from the CSV file into a string array
//     string[] lines = File.ReadAllLines(filePath);

//     // Get the participant index from PlayerPrefs to indicate which participant it is
//     int participantIndex = PlayerPrefs.GetInt("DropdownValue", 0) + 1;
    
//     // Get the current date and time when IAM is recorded
//     string Time_ScanRP = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

//     // Create a new line to append to the CSV file
//     string newLine = participantIndex.ToString() + "," + Time_ScanRP;

//     // Append the new line to the end of the CSV file
//     using (StreamWriter sw = File.AppendText(filePath))
//     {
//         sw.WriteLine(newLine);
//     }
// }

}

