using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/Item Database", order = 1)]
public class ItemDatabase : ScriptableObject
{
    public ItemProfile[] items;
}
