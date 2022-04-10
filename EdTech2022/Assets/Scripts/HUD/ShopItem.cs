﻿using HUD;
using UnityEngine;
using UnityEngine.UI;
using World;
using World.Entities;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private new Text name;
    [SerializeField] private Image image;
    private EntityPlacer placer;
    private ContentPanelController controller;

    private Item item;
    public Item Item => item;

    // Start is called before the first frame update

    private void Awake()
    {
        controller = FindObjectOfType<ContentPanelController>();
    }

    private void Start()
    {
        placer = FindObjectOfType<EntityPlacer>();
    }

    public void Setup(Item item)
    {
        this.item = item;

        // populate information views
        name.text = item.entity.Type.ToString();
        image.sprite = item.sprite;
    }

    public void HandleClick()
    {
        // spawn entity on cursor
        placer.Mode = EntityPlacerMode.BUILD;
        placer.Spawn(item.entity.Type);
        controller.InvalidateUI();
    }

    public void Interactable(bool pred)
    {
        button.interactable = pred;
    }
}