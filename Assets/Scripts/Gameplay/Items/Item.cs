﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemProfile", menuName = "ScriptableObjects/Item Profile", order = 0)]
public class ItemProfile : ScriptableObject
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string itemName;
    [SerializeField]
    private string itemDescription;
    [SerializeField]
    private int itemValue;

    [SerializeField]
    private Sprite menuSprite;
    [SerializeField]
    private GameObject worldPrefab;

    // Properties
    public int ID { get { return id; } }
    public string Name { get { return itemName; } }
    public string Description { get { return itemDescription; } }
    public int Value { get { return itemValue; } }

    public Sprite MenuSprite { get { return menuSprite; } }
    public GameObject WorldPrefab { get { return worldPrefab; } }
}
