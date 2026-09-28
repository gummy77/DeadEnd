using System;
using Pickup;
using Player;
using UnityEngine;

namespace Characters
{
    public class Judgement : MonoBehaviour
    {
        private static readonly int Heart = Animator.StringToHash("heart");
        private static readonly int Feather = Animator.StringToHash("feather");

        [SerializeField] private GameObject scubaMask;
        [SerializeField] private GameObject feather;
        [SerializeField] private GameObject heart;

        [SerializeField] private Item swimmingItem;
        
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlaceHeart()
        {
            heart.SetActive(true);
            _animator.SetBool(Heart, true);
            
            CheckBoth();
        }
        
        public void PlaceFeather()
        {
            feather.SetActive(true);
            _animator.SetBool(Feather, true);
            
            CheckBoth();
        }

        private void CheckBoth()
        {
            if (feather.activeSelf && heart.activeSelf)
            {
                PlayerController playerController = FindAnyObjectByType<PlayerController>();
                if (playerController)
                {
                    playerController.inventoryController.AddItem(swimmingItem, 1, new Vector2(Screen.width/2f, Screen.height - Screen.height/4f));
                    scubaMask.SetActive(false);
                }
            }
        }
    }
}
