using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDataBase : MonoBehaviour {

   public  List<Items> itemList = new List<Items>();
	// Use this for initialization
	void Start () {
        

        //Weapons
        itemList.Add(new Items("Pistol", 0, 30f, 10f, 1, 1, "A pistol", Items.TypeofItem.Equipable));
        itemList.Add(new Items("Silenced_Pistol", 1, 25f, 0f, 1, 1, "A pistol that creates less noise", Items.TypeofItem.Equipable));
        itemList.Add(new Items("Sniper_Rifle", 2, 100f, 50f, 1, 1, "A scoped long range rifle", Items.TypeofItem.Equipable));
        itemList.Add(new Items("Assault_Rifle", 3, 40f, 30f, 1, 1, "A medium range automatic rifle", Items.TypeofItem.Equipable));
        itemList.Add(new Items("Explosive", 4, 100f, 75f, 1, 1, "A timed explosive", Items.TypeofItem.Equipable));
        itemList.Add(new Items("Tranq_Pistol", 5, 0f, 0f, 1, 1, "A non letahl pistol that puts the target to sleep", Items.TypeofItem.Equipable));

        //Consumables
        itemList.Add(new Items("Body_Armour", 6, -50f, 0f, 1, 1, "Provides a small amount of protection from incoming bullets", Items.TypeofItem.Consumable));
        itemList.Add(new Items("Med_Kit", 7, -30f, 0f, 1, 1, "Regain a small amount of lost health", Items.TypeofItem.Consumable));

        //Equip and consume
        itemList.Add(new Items("Bottle", 8, 5f, 10f, 1, 1, "A glass bottle that can be thrown to distract enemies", Items.TypeofItem.EquipAndConsume));
        itemList.Add(new Items("Rock", 9, 10f, 5f, 1, 1, "A rock that can be thrown to distract enemies", Items.TypeofItem.EquipAndConsume));
        itemList.Add(new Items("Distraction Box", 10, 0f, 15f, 1, 1, "A box that can be used to distract enemies", Items.TypeofItem.EquipAndConsume));

        //Misc
        itemList.Add(new Items("Pistol_Ammo", 11, 0f, 0f, 1, 1, "The amount of unloaded pistol Ammo you are carrying", Items.TypeofItem.misc));
        itemList.Add(new Items("Sniper_Ammo", 12, 0f, 0f, 1, 1, "The amount of unloaded sniper ammo you are carrying", Items.TypeofItem.misc));
        itemList.Add(new Items("Assualt_Ammo", 13, 0f, 0f, 1, 1, "The amount of unloaded assualt rifle ammo you are carrying", Items.TypeofItem.misc));
        itemList.Add(new Items("Tranq_Ammo", 14, 0f, 0f, 1, 1, "The amount of unloaded tranq ammo you are carrying", Items.TypeofItem.misc));
        itemList.Add(new Items("Key", 15, 0f, 0f, 0, 0, "A key to unlock a door", Items.TypeofItem.misc));
        itemList.Add(new Items("KeyCard",16,0f,0f,0,0,"A keycard to open a door",Items.TypeofItem.misc));
        itemList.Add(new Items("Intel", 17, 0f, 0f, 0, 0, "Some intel for your current mission", Items.TypeofItem.misc));

        //Quest
        itemList.Add(new Items("Quest_Item", 18, 0f, 0f, 0, 0, "Hand in this item to complete a quest", Items.TypeofItem.Quest));



	}
	

}
