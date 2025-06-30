using UnityEngine;

public class Shop : SingletonMonoBehaviour<Shop>
{
    public bool isRifleUnlocked = false;
    public void TryPurchaseItem(ShopItemSO item)
    {
        if (!Player.Instance.GetMoneyWallet().SpendMoney(item.price))
        {
            return;
        }
        switch (item.itemType)
        {
            case ShopItemType.Heal:
                Player.Instance.GetHealth().Heal(50);
                break;
            case ShopItemType.MaxHealthUp:
                Player.Instance.GetHealth().IncreaseMaxHealth(50);
                break;
            case ShopItemType.MaxShieldUp:
                Player.Instance.GetShield().IncreaseMaxShield(50);
                break;
            case ShopItemType.UnlockRifle:
                if (isRifleUnlocked)
                {
                    Debug.Log("Rifle is already unlocked.");
                    return;
                }
                Player.Instance.GetActiveWeapon().UnlockRifle();
                isRifleUnlocked = true;
                break;
            case ShopItemType.RifleAmmo:
                Player.Instance.GetActiveWeapon().AddAmmo(60);
                break;
        }
    }
}
