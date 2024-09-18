using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vuforia;

public class CanvasController : MonoBehaviour {

    // Changing Scene Scanner to feedback

    [SerializeField] public GameObject CanvasScanner; 
    [SerializeField] public GameObject CanvasFeedBack;
    public SceneControllerRecall SceneControllerRecall;
    public ThoughtManager ThoughtManager;

    // By default setting CanvasScanner true and CanvasFeedBack false    
    private void Start()
    {
        CanvasScanner.SetActive(true);
        CanvasFeedBack.SetActive(false);
    }
    // Method to switch between Canvas


    public void ScannerToFeedBack()
    {   
        CanvasScanner.SetActive(false);
        CanvasFeedBack.SetActive(true);
    }

    public void QRScanned()
    {
        StartCoroutine(WaitBeforeShow());
    }

    // Method to wait before changing scene
    
    IEnumerator WaitBeforeShow()
    {
        yield return new WaitForSeconds(1);
        ScannerToFeedBack();
        yield return new WaitForSeconds(1);
        SceneControllerRecall.MinusOne();
    }

    IEnumerator LastSceneTrigger()
    {
        yield return new WaitForSeconds(1);
        ScannerToFeedBack();
        yield return new WaitForSeconds(1);
        ThoughtManager.isComingFromScene1 = true;
        SceneControllerRecall.PlusTwo();
    }



    public void CanvasScene()
    {
        StartCoroutine(WaitBeforeShow());
    }

    public void LastPageScene()
    {
        StartCoroutine(LastSceneTrigger());
    }
  
    
}




