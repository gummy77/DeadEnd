using System;
using Helper;
using Pickup;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class ItemDisplay : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text countText;

        [SerializeField] private GameObject dragItem;
        [SerializeField] private Image dragImage;
        [SerializeField] private TMP_Text dragCountText;
        
        [SerializeField] private GameObject pickupPrefab;
        
        private PlayerInventoryController _inventoryController;
        
        private InventoryItem _item;
        private float _dragCount;
        private float _dragCountMult;
        private bool _isDragging;
        private bool _isPickingUp;

        private InputAction _mouseinput;

        private void Start()
        {
            _mouseinput = InputSystem.actions.FindAction("Point");
            countText.text = "";
        }

        public void SetItem(InventoryItem item, PlayerInventoryController inventoryController)
        {
            _inventoryController = inventoryController;
            _item = item;
            
            UpdateDisplays();
        }

        public void PickupItem(Item item, Vector2 spawnPoint)
        {
            if(_mouseinput == null) _mouseinput = InputSystem.actions.FindAction("Point");
            
            GameObject pickup = Instantiate(pickupPrefab, transform);
            ItemPickupDisplay pickupDisplay = pickup.GetComponent<ItemPickupDisplay>();
            if (pickupDisplay)
            {
                pickupDisplay.Initialize((spawnPoint == Vector2.zero) ? _mouseinput.ReadValue<Vector2>() : spawnPoint, transform, item);
            }
            else
            {
                Debug.LogWarning("Pickup prefab does not have an ItemPickupDisplay");
            }
        }

        public void UpdateDisplays()
        {
            image.sprite = _item.item.itemSprite;
            if (_item.count > 1)
            {
                countText.text = "x" + _item.count;
            }
            else
            {
                countText.text = "";
            }
            dragImage.sprite = _item.item.itemSprite;
        }

        private void Update()
        {
            if (_isDragging)
            {
                // Vector3 distance = dragItem.transform.position - transform.position;
                // if (distance.magnitude < 150f)
                // {
                //     if (_dragCount <= _item.count)
                //     {
                //         _dragCount += Time.deltaTime * _dragCountMult;
                //         _dragCountMult += Time.deltaTime;
                //         countText.text = "x" + (_item.count - (int)_dragCount);
                //         dragCountText.text = "x" + (int)_dragCount;
                //     }
                // }
            }
        }

        private void OnDestroy()
        {
            Destroy(dragItem);
        }

        public void OnDrag(PointerEventData eventData)
        {
            dragItem.transform.position = eventData.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            dragItem.SetActive(true);
            countText.gameObject.SetActive(false);
            _isDragging = true;
            dragItem.transform.SetParent(_item.FloatingItemsParent);
            dragItem.transform.localScale = Vector3.one;
            _dragCount = 1;
            _dragCountMult = 1f;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            BounceDropItem().DiscardAwaitable(nameof(BounceDropItem));
            _inventoryController.DropItem(_item, eventData, (int)_dragCount);
            UpdateDisplays();
        }

        private async Awaitable BounceDropItem()
        {
            float alpha = 0;
            while (alpha < 1)
            {
                alpha += Time.deltaTime * 4;
                if (dragItem)
                {
                    dragItem.transform.localScale = Vector3.one * (1 - ((alpha * alpha) * 0.5f));
                }
                else
                {
                    return;
                }
                await Awaitable.NextFrameAsync();
            }

            if (dragItem && countText)
            {
                dragItem.SetActive(false);
                countText.gameObject.SetActive(true);
                _isDragging = false;
            }
        }
    }
}
