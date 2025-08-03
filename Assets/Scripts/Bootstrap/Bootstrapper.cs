using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    public void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
}
