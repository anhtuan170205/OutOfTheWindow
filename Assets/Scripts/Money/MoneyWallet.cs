using UnityEngine;
using System;

public class MoneyWallet : MonoBehaviour
{
    public static event Action<int> OnMoneyChanged;
    private int totalMoney = 1000;
    public int TotalMoney
    {
        get => totalMoney;
        private set
        {
            totalMoney = value;
            OnMoneyChanged?.Invoke(totalMoney);
        }
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
