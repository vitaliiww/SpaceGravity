using UnityEngine;

namespace SpaceGravity
{
    public class GravityBody2D : MonoBehaviour
    {
        public double mass;
        public Vector2Double position; // only used for calculations
    
        [Range(0, 1)]
        public double eccentricity;
    
        public Vector2Double Velocity { get; set; }
        public Vector2Double Acceleration { get; set; }
        public Vector2Double PrevAcceleration { get; set; }
    
        private void Awake()
        {
            GravityManager2D.Register(this);
        }

        private void OnDisable()
        {
            GravityManager2D.Unregister(this);
        }
    }
}
