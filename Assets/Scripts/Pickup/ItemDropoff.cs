using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Pickup
{
    public class ItemDropoff : MonoBehaviour
    {
        
        public Item requiredItem;
        [SerializeField] private bool consumesItem;
        
        public string successText;
        public string failureText;
        
        [SerializeField] private UnityEvent onSuccess;
        
        [SerializeField] private bool repeatable;

        [SerializeField] private bool isUsable = true;
        
        private bool _completed;
        
        public bool DropItem(Item item)
        {
            if (_completed && !repeatable && !isUsable) return false;
            
            if (item == requiredItem)
            {
                onSuccess.Invoke();
                _completed = true;
                if (consumesItem)
                {
                    return true;
                }
            }

            return false;
        }

        public void SetUsable(bool usable)
        {
            isUsable = usable;
        }
    }
}
