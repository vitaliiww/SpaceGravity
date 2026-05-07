using SpaceGravity.Attributes;
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
        
        [Header("Info")]
        [ReadOnly] public double periapsis;
        [ReadOnly] public double apoapsis;
        [ReadOnly] public double orbitalPeriod;
        [ReadOnly] public double semiMajorAxis;
        [ReadOnly] public double semiMinorAxis;
        
        private void Awake()
        {
            GravityManager3D.Register(this);
            GravityManager3D.OnInitialize += Initialized;
        }

        private void OnDisable()
        {
            GravityManager3D.Unregister(this);
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
