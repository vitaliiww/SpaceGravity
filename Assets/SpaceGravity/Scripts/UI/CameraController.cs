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
        
        private bool _d;
        private float _yPos = 100f;
    
        private void Start()
        {
            _target = targets[0];
            _cam = GetComponent<Camera>();
            
            var gravityManager2d = GravityManager2D.Instance;
            var gravityManager3d = GravityManager3D.Instance;
            _d = gravityManager2d;
        
            sliderZoom.value = _cam.orthographicSize;
            sliderZoom.onValueChanged.AddListener(value =>
            {
                if (_d) _cam.orthographicSize = value;
                else _yPos = value;
            });
            
            var speed = _d ? gravityManager2d.simulationSpeed : gravityManager3d.simulationSpeed;
            sliderSpeed.value = speed;
            sliderSpeed.onValueChanged.AddListener(value =>
            {
                if (_d) GravityManager2D.Instance.simulationSpeed = (int)value;
                else GravityManager3D.Instance.simulationSpeed = (int)value;
            });
        
            dropdownTarget.onValueChanged.AddListener(value => _target = targets[value]);
        }

        private void Update()
        {
            if (_d)
            {
                var pos = _target.position;
                pos.z = -10;
                transform.position = pos;
            }
            else
            {
                var pos = _target.position;
                pos.y = _yPos;
                transform.position = pos;
            }
        }
    }
}
