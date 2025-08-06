using UnityEngine;

public class BootstrappedData : SingletonMonoBehaviour<BootstrappedData>
{
    public GameManager GameManager { get; private set; }
    public TurnManager TurnManager { get; private set; }
    public AudioManager AudioManager { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        GameManager = GetComponentInChildren<GameManager>(true);
        TurnManager = GetComponentInChildren<TurnManager>(true);
        AudioManager = GetComponentInChildren<AudioManager>(true);
    }

}
