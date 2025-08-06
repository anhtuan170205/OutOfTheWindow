using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    private bool hasLoaded = false;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!hasLoaded && Input.anyKeyDown)
        {
            hasLoaded = true;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
