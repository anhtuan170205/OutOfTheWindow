using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] private ShopItemSO itemDetails;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI itemNameText;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClicked);
        priceText.text = itemDetails.price.ToString() + " $";
        itemNameText.text = itemDetails.itemName;
    }

    private void OnButtonClicked()
    {
        Shop.Instance.TryPurchaseItem(itemDetails);
    }
}
