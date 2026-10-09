using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace World
{
    public class Interactor : MonoBehaviour
    {
        
        [SerializeField] protected bool canInteract = true;
        [SerializeField] private UnityEvent onInteract;
        
        private InteractController _interactController;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!canInteract) return;
            
            if (other.CompareTag("Player"))
            {
                _interactController = other.transform.parent.GetComponent<InteractController>();
        
                if (_interactController != null)
                {
                    _interactController.SetInteractor(this);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (!canInteract) return;
            
            if (other.CompareTag("Player"))
            {
                _interactController = other.transform.parent.GetComponent<InteractController>();
        
                if (_interactController != null)
                {
                    _interactController.SetInteractor(null);
                }
            }
        }

        public void OnInteract()
        {
            onInteract.Invoke();
        }
    }
}