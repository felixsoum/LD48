using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [SerializeField] TMP_Text coinText;
    [SerializeField] PlayerCamera playerCamera;
    public int CoinAmount { get; set; } = 0;
    public UnitBuyButton SelectedShopUnit { get; internal set; }

    private void Awake()
    {
        AddCoin(2);
    }

    private void Update()
    {
        float horizontalDelta = Input.GetAxisRaw("Horizontal");
        float verticalDelta = Input.GetAxisRaw("Vertical");

        if (horizontalDelta != 0 || verticalDelta != 0)
        {
            playerCamera.Move(horizontalDelta, verticalDelta);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(ray.origin, ray.direction, Color.red, 10);
            if (Physics.Raycast(ray, out hit))
            {
                var tile = hit.collider.gameObject.GetComponent<Tile>();
                if (tile != null && SelectedShopUnit != null)
                {
                    if (SelectedShopUnit.cost <= CoinAmount && tile.OnClick(SelectedShopUnit))
                    {
                        AddCoin(-SelectedShopUnit.cost);
                    }
                }
            }
        }
    }
    internal void AddCoin(int amount)
    {
        CoinAmount += amount;
        coinText.text = $"x{CoinAmount}";
    }
}
