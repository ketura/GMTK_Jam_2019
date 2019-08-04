using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void GotoMainMenu()
    {
        SceneManager.LoadScene("mainmenu");
    }

    public void GotoControlDescription()
    {
        SceneManager.LoadScene("controlsexplanation");
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
        Application.Quit();//SceneManager.LoadScene("Tutorial level");
    }
}


