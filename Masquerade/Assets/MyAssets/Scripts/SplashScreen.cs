using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashScreen : MonoBehaviour
{
public void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
}
