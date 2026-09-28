using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Menu
{
    public class DriveController : MonoBehaviour
    {
        [SerializeField] private GameObject[] roadSegments;
        [SerializeField] private GameObject car;
        [SerializeField] private GameObject steeringWheel;
        [SerializeField] private GameObject camera;
        
        [SerializeField] private float carSpeed = 20f;
        [SerializeField] private float turnSpeed = 4f;

        private InputAction _pointAction;

        private float _carTargetPosition;
        
        private Vector3 _startingWheelPosition;
        private Vector3 _startingCameraPosition;
        
        private void Start()
        {
            _pointAction = InputSystem.actions.FindAction("Point");
            _startingWheelPosition = steeringWheel.transform.localPosition;
            _startingCameraPosition = camera.transform.localPosition;
        }

        private void Update()
        {
            _carTargetPosition = ((_pointAction.ReadValue<Vector2>().x - Screen.width / 2f) / Screen.width) * -4;

            float difference = car.transform.position.z - _carTargetPosition;
            
            steeringWheel.transform.localRotation = Quaternion.Euler(0, 90, -difference * turnSpeed);
            float shake = 0.005f;
            steeringWheel.transform.localPosition = _startingWheelPosition + new Vector3(Random.Range(-shake, shake), Random.Range(-shake, shake), Random.Range(-shake, shake));
            float cameraShake = 0.002f;
            camera.transform.localPosition = _startingCameraPosition + new Vector3(Random.Range(-cameraShake, cameraShake), Random.Range(-cameraShake, cameraShake), Random.Range(-cameraShake, cameraShake));

            
            
            car.transform.position = Vector3.Lerp(car.transform.position, new Vector3(0, 0, _carTargetPosition), Time.deltaTime * turnSpeed);
            
            // car.transform.position = new Vector3(carSpeed * Time.time, 0, 0);
            foreach (var roadSegment in roadSegments)
            {
                roadSegment.transform.position -= new Vector3(carSpeed * Time.deltaTime, 0, 0);

                if (roadSegment.transform.position.x < (-10f))
                {
                    roadSegment.transform.position += new Vector3(roadSegments.Length * 10, 0, 0);
                }
            }
        }
    }
}
