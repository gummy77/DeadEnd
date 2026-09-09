using System;
using System.Collections.Generic;
using Helper;
using Pickup;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Player
{
    [Serializable]
    public class InventoryItem
    {
        public Item item;
        public int count;
        public ItemDisplay itemDisplay;
        public Transform FloatingItemsParent { get; private set; }
        public Transform LayoutParent { get; private set; }
        
        public InventoryItem(Item item, int count, ItemDisplay itemDisplay, Transform floatingItemsParent, Transform layoutParent)
        {
            this.item = item;
            this.count = count;
            this.itemDisplay = itemDisplay;
            this.FloatingItemsParent = floatingItemsParent;
            this.LayoutParent = layoutParent;
        }
    }
    
    public class PlayerInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform uiItemsParent;
        [SerializeField] private Transform floatingItemsParent;
        
        [SerializeField] private GameObject uiItemsPrefab;
        
        [SerializeField] private List<InventoryItem> inventory;

        [Header("Text")]
        [SerializeField] private AudioSource itemTextAudioSource;
        [SerializeField] private AudioClip itemTextAudioClip;
        [SerializeField] private TMP_Text itemText;
        [SerializeField] private float characterSpeed;
        [SerializeField] private float lineWaitTime;
        
        public List<InventoryItem> startingInventory;

        private bool _isReading;
        
        private void Start()
        {
            inventory = new List<InventoryItem>();

            foreach (var inventoryItem in startingInventory)
            {
                AddItem(inventoryItem.item, inventoryItem.count);
            }
        }

        public void AddItem(Item item, int count = 1)
        {
            bool isInInventory = false;
            
            foreach (var inventoryItem in inventory)
            {
                if (inventoryItem.item == item)
                {
                    inventoryItem.count += count;
                    inventoryItem.itemDisplay.UpdateDisplays();
                    inventoryItem.itemDisplay.PickupItem(item, Vector3.zero);
                    isInInventory = true;
                }
            }

            if (!isInInventory)
            {
                GameObject newDisplay = Instantiate(uiItemsPrefab, uiItemsParent);
                ItemDisplay itemDisplay = newDisplay.GetComponent<ItemDisplay>();
                
                if (itemDisplay)
                {
                    InventoryItem newItem = new InventoryItem(item, count, itemDisplay, floatingItemsParent, uiItemsParent);
                    inventory.Add(newItem);
                    
                    itemDisplay.SetItem(newItem, this);
                    itemDisplay.PickupItem(item, new Vector2(Screen.width/2f, Screen.height/2f));
                }
            }
        }
        
        public void RemoveItem(Item item, int count = 1)
        {
            for (var index = 0; index < inventory.Count; index++)
            {
                InventoryItem inventoryItem = inventory[index];
                if (inventoryItem.item == item)
                {
                    inventoryItem.count -= count;
                    
                    inventoryItem.itemDisplay.UpdateDisplays();
                    
                    if (inventoryItem.count <= 0)
                    {
                        Destroy(inventoryItem.itemDisplay.gameObject);
                        inventory.RemoveAt(index);
                    }

                    return;
                }
            }
        }

        public void DropItem(InventoryItem item, PointerEventData eventData, int count)
        {
            if (Camera.main)
            {
                Ray ray = Camera.main.ScreenPointToRay(eventData.position);

                if (Physics.Raycast(ray, out RaycastHit hit, 20f))
                {
                    ItemDropoff itemDrop = hit.transform.gameObject.GetComponent<ItemDropoff>();
                    if (itemDrop)
                    {
                        if (itemDrop.DropItem(item.item))
                        {
                            RemoveItem(item.item, count);
                            ReadOutText(itemDrop.successText).DiscardAwaitable(nameof(DropItem));
                        }
                        else
                        {
                            ReadOutText(itemDrop.failureText).DiscardAwaitable(nameof(DropItem));
                        }
                    }
                }
            }
        }
        
        private async Awaitable ReadOutText(string text)
        {
            if (_isReading) return;
            
            _isReading = true;
            for (int characterIndex = 0; characterIndex < text.Length; characterIndex++)
            {
                itemText.text = text.Substring(0, characterIndex + 1);
                if (text[characterIndex] != ' ')
                {
                    itemTextAudioSource.PlayOneShot(itemTextAudioClip);
                }
                await Awaitable.WaitForSecondsAsync(characterSpeed);
            }
            await Awaitable.WaitForSecondsAsync(lineWaitTime);

            itemText.text = "";
            _isReading = false;
        }
    }
}
