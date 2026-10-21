using System;
using UnityEngine;

namespace World
{
    [ExecuteInEditMode]
    public class LineString : MonoBehaviour
    {
        [SerializeField] private Transform startTransform;
        [SerializeField] private Transform endTransform;
        
        [SerializeField] private int lineSegments;
        [SerializeField] private AnimationCurve lineCurve;
        
        private LineRenderer _lineRenderer;

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        void Update()
        {
            _lineRenderer.positionCount = lineSegments + 1;
            
            for (int lineIndex = 0; lineIndex < lineSegments + 1; lineIndex++)
            {
                float linePoint = lineIndex / (float)lineSegments;
                
                Vector3 position = Vector3.Lerp(startTransform.position, endTransform.position, linePoint);
                
                position.y += lineCurve.Evaluate(linePoint);
                
                _lineRenderer.SetPosition(lineIndex, position);
            }
        }
    }
}
