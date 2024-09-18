using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapScenes : MonoBehaviour
{
    
    private ExpAudio expAudio;

    void Update()
    {

        //if (SceneManager.GetActiveScene().name == "Record_IAMs")
            ExpAudio.instance.GetComponent<AudioSource>().Pause();
            //BGmusic.instance.GetComponent<AudioSource>().Play();

        //if (SceneManager.GetActiveScene().name == "CountIAMs")
            ExpAudio.instance.GetComponent<AudioSource>().Pause();

        //if (SceneManager.GetActiveScene().name == "ScannerScene")
            ExpAudio.instance.GetComponent<AudioSource>().Pause();

        if  (SceneManager.GetActiveScene().name == "ChooseScene")
            ExpAudio.instance.GetComponent<AudioSource>().UnPause();   
    }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class SwapScenes : MonoBehaviour
// {
//     private ExpAudio expAudio;

//     void Start()
//     {
//         // Make sure ExpAudio.instance is properly initialized
//         expAudio = ExpAudio.instance;
//     }

//     void Update()
//     {
//         if (expAudio != null)
//         {
//             //if (SceneManager.GetActiveScene().name == "Record_IAMs")
//             //    expAudio.GetComponent<AudioSource>().Pause();

//             //if (SceneManager.GetActiveScene().name == "CountIAMs")
//             //    expAudio.GetComponent<AudioSource>().Pause();

//             //if (SceneManager.GetActiveScene().name == "ScannerScene")
//             //    expAudio.GetComponent<AudioSource>().Pause();

//             if (SceneManager.GetActiveScene().name == "ChooseScene")
//                 expAudio.GetComponent<AudioSource>().UnPause();
//         }
//     }
// }
