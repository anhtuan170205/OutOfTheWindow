using UnityEngine;

public class BootstrappedData : SingletonMonoBehaviour<BootstrappedData>
{
    public TurnManager TurnManager;
    public GameManager GameManager;
    public AudioManager AudioManager;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
