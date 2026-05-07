using SpaceGravity.Attributes;
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
        
        [Header("Info")]
        [ReadOnly] public double periapsis;
        [ReadOnly] public double apoapsis;
        [ReadOnly] public double orbitalPeriod;
        [ReadOnly] public double semiMajorAxis;
        [ReadOnly] public double semiMinorAxis;
    
        private void Awake()
        {
            GravityManager2D.Register(this);
            GravityManager2D.OnInitialize += Initialized;
        }

        private void OnDisable()
        {
            GravityManager2D.Unregister(this);
        }

        private void Initialized()
        {
            orbitalPeriod = GravityUtils.OrbitalPeriod(this);
            semiMajorAxis = GravityUtils.SemiMajorAxisByEnergy(this);
            
            periapsis = semiMajorAxis * (1 - eccentricity);
            apoapsis = semiMajorAxis * (1 + eccentricity);
            
            semiMinorAxis = GravityUtils.SemiMinorAxis(this);
        }
    }
}
