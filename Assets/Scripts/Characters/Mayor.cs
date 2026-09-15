using Helper;
using Pickup;
using Player;
using UnityEngine;

namespace Characters
{
    public class Mayor : MonoBehaviour
    {
        [SerializeField] private GameObject[] barricades;

        [SerializeField] private float newHeight;

        [SerializeField] private Item keys;
        [SerializeField] private Item thing;

        public void GiveKeys()
        {
            PlayerController playerController = FindAnyObjectByType<PlayerController>();
            if (playerController)
            {
                playerController.inventoryController.AddItem(keys);
            }
        }

        public void GiveThing()
        {
            PlayerController playerController = FindAnyObjectByType<PlayerController>();
            if (playerController)
            {
                playerController.inventoryController.AddItem(thing);
            }
        }
        
        public void Spin()
        {
            Rotate().DiscardAwaitable(nameof(Spin));
            foreach (var barricade in barricades)
            {
                barricade.SetActive(false);
            }
        }

        private async Awaitable Rotate()
        {
            float alpha = 0;
            while (alpha <= 1)
            {
                alpha += Time.deltaTime * 0.5f;
                transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0f, 90f, 0f), Quaternion.Euler(0f, 270f, 0f), alpha);
                await Awaitable.EndOfFrameAsync();
            }

            transform.localPosition = new Vector3(transform.localPosition.x, newHeight, transform.localPosition.z);
        }
    }
}
