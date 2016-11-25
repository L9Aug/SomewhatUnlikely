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
   Button itemButton;


	// Use this for initialization
	void Start() {

       maxWeight = 60;
       WeaponImagePosition = new Vector3(-12, -114, 0);
       HealthImagePosition = new Vector3(-12, -114, 0);
       DistractionImagePosition = new Vector3(-12, -114, 0);
       MiscImagePosition = new Vector3(-12, -114, 0);
       QuestImagePosition = new Vector3(-12, -114, 0);

	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (weight < maxWeight)
            {
                AddItem(9);
            }

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (weight < maxWeight)
            {
                AddItem(8);
            }

        }


    }

    void AddItem(int id)
    {  
      

        if (ItemDataBase.InventoryDataBase.itemList[id].currentStack < ItemDataBase.InventoryDataBase.itemList[id].maxItemStack) 
        {
            if (ItemDataBase.InventoryDataBase.itemList[id].itemType == Items.TypeofItem.Equipable)
            {
                InstantiateItem(Weapon, WeaponImagePosition, id);
                WeaponImagePosition = MoveItemImage(WeaponImagePosition);
            }

            else if (ItemDataBase.InventoryDataBase.itemList[id].itemType == Items.TypeofItem.Consumable)
            {
                if (ItemDataBase.InventoryDataBase.itemList[id].currentStack < 1)
                {
                    InstantiateItem(Health, HealthImagePosition, id);
                    HealthImagePosition = MoveItemImage(HealthImagePosition);
                    itemButton.GetComponentInChildren<Text>().text = ItemDataBase.InventoryDataBase.itemList[id].currentStack.ToString();
                }
                else
                {
                    ItemDataBase.InventoryDataBase.itemList[id].currentStack++;
                    weight += ItemDataBase.InventoryDataBase.itemList[id].itemWeight;
                    itemButton.GetComponentInChildren<Text>().text = ItemDataBase.InventoryDataBase.itemList[id].currentStack.ToString();
                    
                    
                }
            }

            else if(ItemDataBase.InventoryDataBase.itemList[id].itemType == Items.TypeofItem.EquipAndConsume)
            {
                if (ItemDataBase.InventoryDataBase.itemList[id].currentStack < 1)
                {
                    InstantiateItem(Distraction, DistractionImagePosition, id);
                    DistractionImagePosition = MoveItemImage(DistractionImagePosition);
                    itemButton.GetComponentInChildren<Text>().text = ItemDataBase.InventoryDataBase.itemList[id].currentStack.ToString();
                }
                else
                {
                    ItemDataBase.InventoryDataBase.itemList[id].currentStack++;
                    weight += ItemDataBase.InventoryDataBase.itemList[id].itemWeight;
                    itemButton.GetComponentInChildren<Text>().text = ItemDataBase.InventoryDataBase.itemList[id].currentStack.ToString();
                }
            }

            else if(ItemDataBase.InventoryDataBase.itemList[id].itemType == Items.TypeofItem.misc)
            {
                if (ItemDataBase.InventoryDataBase.itemList[id].currentStack < 1)
                {
                    InstantiateItem(Misc, MiscImagePosition, id);
                    MiscImagePosition = MoveItemImage(MiscImagePosition);
                }
                else
                {
                    ItemDataBase.InventoryDataBase.itemList[id].currentStack++;
                    weight += ItemDataBase.InventoryDataBase.itemList[id].itemWeight;
                    itemButton.GetComponentInChildren<Text>().text = ItemDataBase.InventoryDataBase.itemList[id].currentStack.ToString();
                }

            }

            else
            {
                if (ItemDataBase.InventoryDataBase.itemList[id].currentStack < 1)
                {
                    InstantiateItem(Quest, QuestImagePosition, id);
                    QuestImagePosition = MoveItemImage(QuestImagePosition);
                }
                else
                {
                    ItemDataBase.InventoryDataBase.itemList[id].currentStack++;
                    weight += ItemDataBase.InventoryDataBase.itemList[id].itemWeight;
                    itemButton.GetComponentInChildren<Text>().text = ItemDataBase.InventoryDataBase.itemList[id].currentStack.ToString();
                }

            }

          
            
        }
    }


    Vector3 MoveItemImage(Vector3 ItemImagePos)
    {
        ItemImagePos = ItemImagePos + new Vector3(165, 0, 0);
        return ItemImagePos;
    }

  

    void InstantiateItem(RawImage ImageType,Vector3 ImagePosition, int id)
    {
        ItemImage.GetComponent<Image>().sprite = ItemDataBase.InventoryDataBase.itemList[id].itemSprite;
        itemButton = Instantiate(ItemImage, ImageType.transform, false) as Button;
        itemButton.GetComponent<RectTransform>().localPosition = ImagePosition;
        ItemDataBase.InventoryDataBase.itemList[id].currentStack++;
        weight += ItemDataBase.InventoryDataBase.itemList[id].itemWeight;

        itemButton.GetComponent<ItemEnum>().thisItem = (ItemEnum.Item)id;
        itemButton.onClick.AddListener(delegate { UseItem((int)itemButton.GetComponent<ItemEnum>().thisItem); });
       
    }

    public void UseItem(int id)
    {
         
    }
    
    

}
