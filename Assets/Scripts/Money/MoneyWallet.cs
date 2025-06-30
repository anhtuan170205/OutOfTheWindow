using UnityEngine;
using System;

public class MoneyWallet : MonoBehaviour
{
    public event Action<int> OnMoneyChanged;

    public int TotalMoney
    {
        get => TotalMoney;
        private set
        {
            TotalMoney = value;
            OnMoneyChanged?.Invoke(TotalMoney);
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
