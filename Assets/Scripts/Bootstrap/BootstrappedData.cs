using UnityEngine;

public class BootstrappedData : SingletonMonoBehaviour<BootstrappedData>
{
    public TurnManager TurnManager { get; private set; }
    public GameManager GameManager { get; private set; }
    public AudioManager AudioManager { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        TurnManager = GetComponentInChildren<TurnManager>(true);
        GameManager = GetComponentInChildren<GameManager>(true);
        AudioManager = GetComponentInChildren<AudioManager>(true);
    }

}
