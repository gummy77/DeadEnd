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
        
        public string interactText;
        
        private InteractController _interactController;

        public void SetInteractable(bool canNowInteract)
        {
            canInteract = canNowInteract;
            if (!canInteract)
            {
                _interactController.SetInteractor(null);
            }
        }
        
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