using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Inventory : MonoBehaviour {

   public Button ItemImage;
   public RawImage Weapon;
   public RawImage Health;
   public RawImage Distraction;
   public RawImage Misc;
   public RawImage Quest;

   Vector3 WeaponImagePosition           = Vector3.zero;
   Vector3 HealthImagePosition           = Vector3.zero;
   Vector3 MiscImagePosition             = Vector3.zero;
   Vector3 QuestImagePosition            = Vector3.zero;
   Vector3 DistractionImagePosition      = Vector3.zero;

   int weight;
   int maxWeight;
    
    
	// Use this for initialization
	void Start() {
       maxWeight = 60;
       WeaponImagePosition = new Vector3(-12, -114, 0);
       HealthImagePosition = new Vector3(-12, -114, 0);
       ItemDataBase.InventoryDataBase.itemList.ToArray();
        
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Space))
        {
            if (weight < maxWeight)
            {
                AddItem(6);
                
                
            }
          
        }
	}

    void AddItem(int id)
    {
        if (ItemDataBase.InventoryDataBase.itemList[id].currentStack < ItemDataBase.InventoryDataBase.itemList[id].maxItemStack)
        {
            if (ItemDataBase.InventoryDataBase.itemList[id].itemType == Items.TypeofItem.Equipable)
            {

                Instantiate(ItemImage, Weapon.transform, false);
                ItemImage.GetComponent<RectTransform>().localPosition = WeaponImagePosition;
                ItemImage.GetComponent<Image>().sprite = ItemDataBase.InventoryDataBase.itemList[id].itemSprite;
                weight += ItemDataBase.InventoryDataBase.itemList[id].itemWeight;
                ItemDataBase.InventoryDataBase.itemList[id].currentStack++;
                WeaponImagePosition = UpdatePosition(WeaponImagePosition);
            }

            else if (ItemDataBase.InventoryDataBase.itemList[id].itemType == Items.TypeofItem.Consumable)
            {
                if (ItemDataBase.InventoryDataBase.itemList[id].currentStack < 1)
                {
                    Instantiate(ItemImage, Health.transform, false);
                    ItemImage.GetComponent<RectTransform>().localPosition = HealthImagePosition;
                    ItemImage.GetComponent<Image>().sprite = ItemDataBase.InventoryDataBase.itemList[id].itemSprite;
                    weight += ItemDataBase.InventoryDataBase.itemList[id].itemWeight;
                    ItemDataBase.InventoryDataBase.itemList[id].currentStack++;
                    HealthImagePosition = UpdatePosition(HealthImagePosition);
                }
                else
                {
                    ItemDataBase.InventoryDataBase.itemList[id].currentStack++;
                    weight += ItemDataBase.InventoryDataBase.itemList[id].itemWeight;
                    ItemImage.GetComponentInChildren<Text>().text = ItemDataBase.InventoryDataBase.itemList[id].currentStack.ToString();
                    
                }
            }
        }
    }




    Vector3 UpdatePosition(Vector3 ImagePos)
    {
        ImagePos = ImagePos + new Vector3(165, 0, 0);
        return ImagePos;
    }

    void InstantiateItem(RawImage ImageType,Vector3 ImagePosition, int id)
    {
        Instantiate(ItemImage, ImageType.transform, false);
        ItemImage.GetComponent<RectTransform>().localPosition = ImagePosition;
        ItemImage.GetComponent<Image>().sprite = ItemDataBase.InventoryDataBase.itemList[id].itemSprite;
        weight += ItemDataBase.InventoryDataBase.itemList[id].itemWeight;
        ImagePosition = ImagePosition + new Vector3(165, 0, 0);
    }
    

}
