using UnityEngine;

public class UnitShop : MonoBehaviour
{
    [SerializeField] GameDirector gameDirector;
    UnitBuyButton[] buyButtons;

    private void Awake()
    {
        buyButtons = GetComponentsInChildren<UnitBuyButton>();

        foreach (var buyButton in buyButtons)
        {
            buyButton.OnBuyClick += OnBuyClick;
        }
    }

    private void OnBuyClick(UnitBuyButton clickedButton)
    {
        foreach (var buyButton in buyButtons)
        {
            if (buyButton != clickedButton)
            {
                buyButton.Deselect();
            }
            else
            {
                if (clickedButton.cost > gameDirector.CoinAmount)
                {
                    gameDirector.SelectedShopUnit = null;
                    clickedButton.Deselect();
                }
                else
                {
                    gameDirector.SelectedShopUnit = clickedButton;
                    clickedButton.Select();
                }
            }
        }
    }
}
