using UnityEngine;

namespace SpaceGravity
{
    public class GravityBody3D : MonoBehaviour
    {
        public double mass;
        public Vector3Double position; // only used for calculations
    
        [Range(0, 1)]
        public double eccentricity;
    
        public Vector3Double Velocity { get; set; }
        public Vector3Double Acceleration { get; set; }
        public Vector3Double PrevAcceleration { get; set; }
    
        private void Awake()
        {
            GravityManager3D.Register(this);
        }

        private void OnDisable()
        {
            GravityManager3D.Unregister(this);
        }
    }
}
