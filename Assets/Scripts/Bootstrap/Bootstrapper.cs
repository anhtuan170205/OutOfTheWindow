using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private AudioSource mouseClickSource;
    private bool hasLoaded = false;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!hasLoaded && Input.GetKeyDown(KeyCode.Space))
        {
            hasLoaded = true;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            mouseClickSource.Play();
        }
    }
}
