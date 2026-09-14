using UnityEngine;

namespace World
{
    public class Bridge : MonoBehaviour
    {
        [SerializeField] private Collider interactCollider;
        [SerializeField] private GameObject plank;

        public void PlacePlank()
        {
            plank.SetActive(true);
            interactCollider.enabled = false;
            // TODO play sounds
        }
    }
}
