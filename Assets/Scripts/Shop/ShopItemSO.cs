using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemSO", menuName = "Scriptable Objects/ShopItemSO")]
public class ShopItemSO : ScriptableObject
{
    public ShopItemType itemType;
    public string itemName;
    public int price;
    public Sprite icon;
}
