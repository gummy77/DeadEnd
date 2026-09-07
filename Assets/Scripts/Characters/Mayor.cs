using Helper;
using UnityEngine;

namespace Characters
{
    public class Mayor : MonoBehaviour
    {
        [SerializeField] private Speaker dialogueOne;
        [SerializeField] private Speaker dialogueTwo;
        
        [SerializeField] private GameObject[] barricades;

        [SerializeField] private float newHeight;
        
        
        public void Spin()
        {
            rotate().DiscardAwaitable(nameof(Spin));
            dialogueOne.enabled = false;
            dialogueTwo.enabled = true;
            foreach (var barricade in barricades)
            {
                barricade.SetActive(false);
            }
        }

        private async Awaitable rotate()
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
