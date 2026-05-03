using UnityEngine;
using UnityEngine.UI;

namespace SpaceGravity.Demo
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Slider sliderZoom;
        [SerializeField] private Slider sliderSpeed;
        [SerializeField] private Dropdown dropdownTarget;
    
        [SerializeField] private Transform[] targets;

        private Camera _cam;
    
        private Transform _target;
    
        private void Start()
        {
            _target = targets[0];
            _cam = GetComponent<Camera>();
        
            sliderZoom.value = _cam.orthographicSize;
            sliderZoom.onValueChanged.AddListener(value => _cam.orthographicSize = value);
        
            sliderSpeed.value = GravityManager.Instance.simulationSpeed;
            sliderSpeed.onValueChanged.AddListener(value => GravityManager.Instance.simulationSpeed = (int)value);
        
            dropdownTarget.onValueChanged.AddListener(value => _target = targets[value]);
        }

        private void Update()
        {
            var pos = _target.position;
            pos.z = -10;
            transform.position = pos;
        }
    }
}
