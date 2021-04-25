using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [SerializeField] TMP_Text coinText;
    [SerializeField] TMP_Text depthText;
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] GameObject backButton;
    [SerializeField] Level startLevel;
    public int CoinAmount { get; set; } = 0;
    int currentDepth = 0;
    public UnitBuyButton SelectedShopUnit { get; internal set; }

    Stack<Level> levelStack = new Stack<Level>();

    private void Awake()
    {
        startLevel.Depth = currentDepth;
        levelStack.Push(startLevel);
        AddCoin(2);
        UpdateCameraTarget();
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
                if (tile != null)
                {
                    if (tile.HasInnerLevel())
                    {
                        tile.OnClick();
                    }
                    else if (SelectedShopUnit != null 
                        && SelectedShopUnit.cost <= CoinAmount
                        && tile.OnClick(SelectedShopUnit))
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

    internal void OnLevelZoom(Level innerLevel)
    {
        levelStack.Peek().DisableColliders();
        levelStack.Push(innerLevel);
        currentDepth++;
        UpdateDepthText();
        UpdateCameraTarget();
        backButton.SetActive(true);
    }

    private void UpdateDepthText()
    {
        depthText.text = $"Depth: {currentDepth}";
    }

    public void OnLevelBack()
    {
        currentDepth--;
        levelStack.Pop();
        levelStack.Peek().EnableColliders();
        UpdateCameraTarget();
        UpdateDepthText();
        if (currentDepth == 0)
        {
            backButton.SetActive(false);
        }
    }

    private void UpdateCameraTarget()
    {
        playerCamera.TargetTransform = levelStack.Peek().cameraPosition;
    }
}
