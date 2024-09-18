using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExpAudio : MonoBehaviour
{
    public static ExpAudio instance;
    public int i;

    // Add an AudioSource variable to reference the attached AudioSource
    public AudioSource audioSource;
    public CSVAudio csvAudio;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            // Get the AudioSource component attached to this GameObject
            audioSource = GetComponent<AudioSource>();
        }
    }

//     public void RecordAudioTime()
//     {
//         i++;

//         if (i == 16)
//         {
//             // Print the current audio time
//             Debug.LogError("Audio Time: " + audioSource.time);
//             csvAudio.ReadAudioSCSV();
//             csvAudio.AudioWriter();

//             // Print the marker information
//             Debug.LogError("This is the " + i + " marker");
//         }
//         else
//         {
//             // Print the marker number
//             Debug.LogError("This is the " + i + " marker");
//         }
//     }
}
