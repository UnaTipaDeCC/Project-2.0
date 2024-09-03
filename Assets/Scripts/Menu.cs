using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("SampleScene");        
    }
    public void CreateCard()
    {
        SceneManager.LoadScene("CreateCards");        
    }
    public void BackMenu()
    {
        SceneManager.LoadScene("Menu Principal");
    }
    
}
