using UnityEngine;

namespace SpaceGravity
{
    public class GravityBody : MonoBehaviour
    {
        public double mass;
        public Vector2Double position; // only used for calculations
    
        [Range(0, 1)]
        public double eccentricity;
    
        public Vector2Double Velocity { get; set; }
        public Vector2Double Acceleration { get; set; }
    
        private void Awake()
        {
            GravityManager.Register(this);
        }

        private void OnDisable()
        {
            GravityManager.Unregister(this);
        }
    }
}
