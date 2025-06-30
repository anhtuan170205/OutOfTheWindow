using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] private ShopItemSO itemDetails;
    [SerializeField] private Button button;

    private void Awake()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        Shop.Instance.TryPurchaseItem(itemDetails);
    }
}
