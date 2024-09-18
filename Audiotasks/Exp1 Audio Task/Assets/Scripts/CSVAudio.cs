using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;
using TMPro;
using UnityEngine.Android;
using System;

public class CSVAudio : MonoBehaviour
{

    // Declare references to get info from our scenes
    // Ref for participant selection
    
    public string ParticipantPath;
    public SaveParticipant saveParticipant;
    public string file, myfolder, CSVfolder;
    public ExpAudio expAudio;
    public int i;
    float audioTiming;

    private void Start()
    {
        StartCoroutine(RequestStoragePermissionAudio());
        expAudio = ExpAudio.instance;
        audioTiming = expAudio.audioSource.time;
    }

    IEnumerator RequestStoragePermissionAudio()
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
                csvFolder = Application.dataPath + "/CSVAudio/";
                Debug.Log("File at " + csvFolder + " EXISTE");

            #elif UNITY_ANDROID
                csvFolder = GetPathAndroid() + "/CSVFolder/";
            #else
                csvFolder = Application.dataPath + "/";
            #endif

            // Construct the path to the CSV file
            string fileName = $"AudioTime.csv";
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
    public string GetPathAudio()
    {
    int SelectionDD = PlayerPrefs.GetInt("DropdownValue", 0);
    string fileName = $"AudioTime.csv";
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
    // Read the CSV file at the selected path and return its contents
    
    public void ReadAudioSCSV()
    {
        file = "";

        if (!File.Exists(GetPathAudio()))
        {
            // File doesn't exist, create it
            File.WriteAllText(GetPathAudio(), "subj_id,AudioTime,Count\n");
        }

        if (File.Exists(GetPathAudio()))
        {
            FileStream fileStream = new FileStream(GetPathAudio(), FileMode.Open, FileAccess.ReadWrite);
            StreamReader read = new StreamReader(fileStream);
            file = read.ReadToEnd();
            read.Close(); // Always close the file after reading/writing
            Debug.Log("Contents of CSV file:\n" + file);
        }
    }



public void AudioWriter()
{
    string filePath = GetPathAudio();

    // Read all lines from the CSV file into a string array
    string[] lines = File.ReadAllLines(filePath);

    int participantIndex = PlayerPrefs.GetInt("DropdownValue", 0) + 1;

    int participantCount = PlayerPrefs.GetInt("TotalClicks", 0);
    Debug.LogError("The final count is " + participantCount);

    // Create a new line to append to the CSV file
    string newLine = participantIndex.ToString() + "," + audioTiming + "," +  participantCount.ToString();

    file = "";

        if (!File.Exists(GetPathAudio()))
        {
            // File doesn't exist, create it
            File.WriteAllText(GetPathAudio(), "subj_id,AudioTime,Count\n");
        }
        
    // Append the new line to the end of the CSV file
    using (StreamWriter sw = File.AppendText(filePath))
    {
    // Check if the file is empty, and if so, write the header
        if (sw.BaseStream.Length == 0)
        {
            sw.WriteLine("subj_id,AudioTime,Count");
        }

        sw.WriteLine(newLine);
        Debug.Log("Ecriture de" + newLine);    
    }
}

public void RecordAudioTime()
    {
        i++;
        if (i == 16) 
        {
            // Print the current audio time
            Debug.LogError("Audio Time: " + audioTiming);
            ReadAudioSCSV();
            AudioWriter();

            // Print the marker information
            Debug.LogError("This is the " + i + " marker");
        }

        else
        {
            // Print the marker number
            Debug.LogError("This is the " + i + " marker");
        }
    }
}