using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Pickup
{
    public class ItemDropoff : MonoBehaviour
    {
        
        [SerializeField] private Item requiredItem;
        [SerializeField] private bool consumesItem;
        
        public string successText;
        public string failureText;
        
        [SerializeField] private UnityEvent onSuccess;
        
        [SerializeField] private bool repeatable;

        private bool _completed;
        
        public bool DropItem(Item item)
        {
            if (_completed && !repeatable) return false;
            
            if (item == requiredItem)
            {
                onSuccess.Invoke();
                // display success text
                _completed = true;
                if (consumesItem)
                {
                    return true;
                }
            }
            else
            {
                // display failure text
            }

            return false;
        }
    }
}
