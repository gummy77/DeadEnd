using UnityEngine;

namespace World
{
    public class SunflowerEye : MonoBehaviour
    {
        [SerializeField] private Renderer eyeRenderer;

        [SerializeField] private Material healedEyeMaterial;

        public void Fertilise()
        {
            eyeRenderer.material = healedEyeMaterial;
        }
    }
}
