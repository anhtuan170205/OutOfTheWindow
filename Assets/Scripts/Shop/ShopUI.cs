using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject shopUI;

    private void OnEnable()
    {
        inputReader.ShopEvent += ToggleShop;
    }

    private void OnDisable()
    {
        inputReader.ShopEvent -= ToggleShop;
    }

    private void Start()
    {
        ToggleShop(false);
    }

    private void ToggleShop(bool isOpen)
    {
        if (DayNightManager.Instance.CurrentState == DayNightState.Night)
        {
            return;
        }
        if (!isOpen)
        {
            shopUI.SetActive(false);
            inputReader.cursorLocked = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            shopUI.SetActive(true);
            inputReader.cursorLocked = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
