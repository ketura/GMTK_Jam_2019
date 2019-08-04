using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void GotoMainMenu
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GotoControlDescription()
    {
        SceneManager.LoadScene("ControlsExplanation");
    }
	
	public void GotoBackstory()
    {
        SceneManager.LoadScene("Backstory");
    }
	
	public void GotoFirstLevel()
    {
        SceneManager.LoadScene("Tutorial Level");
    }
	
	public void ExitGame()
    {
        //SceneManager.LoadScene("Tutorial level");
    }
}


