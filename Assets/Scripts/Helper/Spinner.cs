using UnityEngine;

namespace Helper
{
    [ExecuteInEditMode]
    public class Spinner : MonoBehaviour
    {
        [SerializeField] private bool spinning;
        [SerializeField] private float spinSpeed;
        [SerializeField] private float defaultRotation;
        
        void Update()
        {
            if (spinning)
            {
                transform.rotation = Quaternion.Euler(0, Time.time * spinSpeed, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, defaultRotation, 0);
            }
        }
    }
}
