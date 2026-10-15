using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Debugging
{
    public enum EGizmoShape
    {
        Ray = 0,
        Line = 1,
        Cube = 2,
        WireCube = 3,
        Sphere = 4,
        WireSphere = 5
    }

    public enum ETransformDirection
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
        Forward = 4,
        Back = 5
    }

    public class DrawGizmo : MonoBehaviour
    {
        [SerializeField]
        private bool isGizmoEnabled = true;

        [SerializeField]
        private EGizmoShape gizmoShape = EGizmoShape.Ray;

        [SerializeField]
        private ETransformDirection rayDirection = ETransformDirection.Up;

        [SerializeField]
        private bool useLocalSpace = false;

        [SerializeField]
        private Color gizmoColor = Color.white;

        [SerializeField]
        private Vector3 gizmoOffset = Vector3.zero;

        [SerializeField]
        private bool matchObjectLocalScale = false;

        [SerializeField]
        private float gizmoScale = 1f;

        [SerializeField]
        private Vector3 gizmoScale3D = Vector3.one;

        private Vector3 _direction = Vector3.zero;

        public void SetIsDrawingGizmo(bool isDrawingGizmoIn)
        {
            isGizmoEnabled = isDrawingGizmoIn;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;

            _direction = rayDirection switch
            {
                ETransformDirection.Up => useLocalSpace ? gameObject.transform.up : Vector3.up,
                ETransformDirection.Down => useLocalSpace ? -gameObject.transform.up : Vector3.down,
                ETransformDirection.Right => useLocalSpace ? gameObject.transform.right : Vector3.right,
                ETransformDirection.Left => useLocalSpace ? -gameObject.transform.right : Vector3.left,
                ETransformDirection.Forward => useLocalSpace ? gameObject.transform.forward : Vector3.forward,
                ETransformDirection.Back => useLocalSpace ? -gameObject.transform.forward : Vector3.back,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (isGizmoEnabled)
            {
                switch (gizmoShape)
                {
                    case EGizmoShape.Ray:
                        Gizmos.DrawRay(transform.position + gizmoOffset, matchObjectLocalScale ? _direction * transform.localScale.z : _direction * gizmoScale);
                        break;
                    case EGizmoShape.Line:
                        Gizmos.DrawLine(transform.position + gizmoOffset, new Vector3(0, 0, 0));
                        break;
                    case EGizmoShape.Cube:
                        Gizmos.DrawCube(transform.position + gizmoOffset, matchObjectLocalScale ? transform.localScale : gizmoScale3D);
                        break;
                    case EGizmoShape.WireCube:
                        Gizmos.DrawWireCube(transform.position + gizmoOffset, matchObjectLocalScale ? transform.localScale : gizmoScale3D);
                        break;
                    case EGizmoShape.Sphere:
                        Gizmos.DrawSphere(transform.position + gizmoOffset, matchObjectLocalScale ? transform.localScale.y : gizmoScale);
                        break;
                    case EGizmoShape.WireSphere:
                        Gizmos.DrawWireSphere(transform.position + gizmoOffset, matchObjectLocalScale ? transform.localScale.y : gizmoScale);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(GetType().Name);
                }
            }
        }
    }
}