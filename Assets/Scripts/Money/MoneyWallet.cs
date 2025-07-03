using UnityEngine;
using System;

public class MoneyWallet : MonoBehaviour
{
    public event Action<int> OnMoneyChanged;
    private int totalMoney = 0;
    public int TotalMoney
    {
        get => totalMoney;
        private set
        {
            totalMoney = value;
            OnMoneyChanged?.Invoke(totalMoney);
        }
    }

    private void Start()
    {
        TotalMoney = 0;
    }

    public void AddMoney(int amount)
    {
        TotalMoney += amount;
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= TotalMoney)
        {
            TotalMoney -= amount;
            return true;
        }
        return false;
    }
}
