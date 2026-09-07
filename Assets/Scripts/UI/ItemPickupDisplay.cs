using System;
using Pickup;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemPickupDisplay : MonoBehaviour
    {
        [SerializeField] private Image _image;
        
        [SerializeField] private float travelTime = 0.5f;
        [SerializeField] private AnimationCurve _movementCurve;
        [SerializeField] private AnimationCurve _ScaleCurve;
        [SerializeField] private float startAngle;
        
        private Vector3 _startPosition;
        private Transform _target;
        private float _travelAlpha;

        public void Initialize(Vector3 position, Transform target, Item item)
        {
            _startPosition = position;
            _target = target;
            
            transform.position = _startPosition;
            
            _image.sprite = item.itemSprite;
        }

        private void Update()
        {
            _travelAlpha += Time.deltaTime / travelTime;
            
            transform.position = Vector3.Lerp(_startPosition, _target.position, _movementCurve.Evaluate(_travelAlpha));
            transform.rotation = Quaternion.Euler(0, 0, (_travelAlpha * startAngle) - startAngle);
            transform.localScale = _ScaleCurve.Evaluate(_travelAlpha) * Vector3.one;
            
            if (_travelAlpha >= 1)
            {
                Destroy(gameObject);
            }
        }
    }
}