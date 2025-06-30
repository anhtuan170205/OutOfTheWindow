using UnityEngine;

public class Shop : SingletonMonoBehaviour<Shop>
{

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
                Player.Instance.GetActiveWeapon().UnlockRifle();
                break;
            case ShopItemType.RifleAmmo:
                Player.Instance.GetActiveWeapon().AddAmmo(300);
                break;
        }
    }
}
