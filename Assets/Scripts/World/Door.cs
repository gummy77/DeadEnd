using Helper;
using UnityEngine;

namespace World
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private float closedAngle;
        [SerializeField] private float openAngle;
        
        public void OpenDoor()
        {
            DoSwing().DiscardAwaitable(nameof(OpenDoor));
        }

        private async Awaitable DoSwing()
        {
            float alpha = 0;
            while (alpha < 1)
            {
                alpha += Time.deltaTime;
                transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0f, closedAngle, 0f), Quaternion.Euler(0f, openAngle, 0f), alpha);
                await Awaitable.EndOfFrameAsync();
            }
        }
    }
}
