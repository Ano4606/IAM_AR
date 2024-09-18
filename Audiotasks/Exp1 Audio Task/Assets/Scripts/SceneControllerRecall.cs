using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vuforia;

public class SceneControllerRecall : MonoBehaviour {

    
    // Changing scenes methods

    public void LoadStartingScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlusOne()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
        public void CTScannerScene()
    {
        SceneManager.LoadScene("ScannerScene");
    }
    
    public void MinusOne()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }
    
    
    public void PlusTwo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +2);
    }
    
    
    public void MinusTwo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -2);
    }
    
     public void PlusThree()
    {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +3);
    }
    
    public void MinusThree()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -3);
    }
    
    
    public void PlusFour()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +4);
    }
    
    
    public void MinusFour()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -4);
    }
    
    public void PlusFive()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +5);
    }
    
    public void MinusFive()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -5);
    }
    
    public void MinusSix()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -6);
    }

    public void PlusSix()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +6);
    }
    
    
    public void MinusSeven()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -7);
    }

    public void PlusSeven()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +7);
    }
    
    

    
    public void Exit()
    {
        Debug.Log("You have exited the app");
        Application.Quit();
    }
    

}




