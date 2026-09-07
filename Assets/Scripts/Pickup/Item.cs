using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pickup
{
    [CreateAssetMenu(menuName = "Item")]
    public class Item : ScriptableObject
    {
        public Sprite itemSprite;
    }
}
