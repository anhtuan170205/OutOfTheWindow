using UnityEngine;

public class Coin
{
    public int Value { get; private set; }

    public Coin(int value)
    {
        Value = value;
    }

    public void Collect()
    {
        Debug.Log($"Coin collected with value: {Value}");
    }
}
