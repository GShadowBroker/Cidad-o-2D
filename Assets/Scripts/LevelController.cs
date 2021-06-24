using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static int currentLevel = 1;
    public static int maxLevel = 2;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void PassLevel()
    {
        string levelName;
        if (currentLevel + 1 > maxLevel)
        {
            levelName = "Level-1";
            currentLevel = 1;
        }
        else
        {
            ++currentLevel;
            levelName = "Level-" + currentLevel;
            Debug.Log("LevelName: " + levelName);
        }

        SceneManager.LoadScene(levelName);
    }
}
