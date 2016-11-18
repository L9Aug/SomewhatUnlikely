using UnityEngine;
using System.Collections;

public class Items {

    string itemName;
    int idValue;
    Sprite itemSprite;
    float itemDamage;
    float itemNoise;
    int itemValue;
    int itemWeight;
    string description;
    GameObject model;
    TypeofItem itemType;

    public enum TypeofItem
    {
        Equipable,
        Consumable,
        EquipAndConsume,
        Quest,
        misc
    }

    public Items(string name, int id, float damage, float noise, int value, int weight, string desc,TypeofItem TypeItem )
    {
        itemName = name;
        idValue = id;
        itemDamage = damage;
        itemNoise = noise;
        itemValue = value;
        itemWeight = weight;
        description = desc;
        itemType = TypeItem;
        itemSprite = Resources.Load<Sprite>("" + name); //name passed in must be sprite fileName; 
    }

    public Items() // for 0 items 
    {

    }

    
    
}
