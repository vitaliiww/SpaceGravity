using System;
using UnityEngine;

namespace SpaceGravity
{
    public static class GravityUtils
    {
        public static double HillSphere(GravityBody2D body, GravityBody2D star)
        {
            var a = Vector2Double.Distance(body.position, star.position);
            var m = body.mass; 
            var M = star.mass;
            return a * Math.Pow(m / (3 * (M + m)), 1.0/3.0);
        }
        
        public static double HillSphere(GravityBody3D body, GravityBody3D star)
        {
            var a = Vector3Double.Distance(body.position, star.position);
            var m = body.mass; 
            var M = star.mass;
            return a * Math.Pow(m / (3 * (M + m)), 1.0/3.0);
        }

        public static double OrbitalPeriod(GravityBody2D body)
        {
            var a = SemiMajorAxisByEnergy(body);
            var mu = StandardGravitationalParameter();
            return 2 * Math.PI * Math.Sqrt(a * a * a / mu);
        }
        
        public static double OrbitalPeriod(GravityBody3D body)
        {
            var a = SemiMajorAxisByEnergy(body);
            var mu = StandardGravitationalParameter();
            return 2 * Math.PI * Math.Sqrt(a * a * a / mu);
        }

        public static double SemiMajorAxisByEnergy(GravityBody2D body)
        {
            if (GravityManager2D.Instance)
            {
                var r = body.position - GravityManager2D.Instance.star.position;
                var mu = StandardGravitationalParameter();
                var energy = body.Velocity.sqrMagnitude / 2.0 - mu / r.magnitude;
                var a = -mu / (2.0 * energy);
                return a;
            }
            
            Debug.LogWarning("[GravityUtils] GravityManager2D not found");
            return 0;
        }
        
        public static double SemiMajorAxisByEccentricity(GravityBody2D body)
        {
            if (GravityManager2D.Instance)
            {
                var r = (body.position - GravityManager2D.Instance.star.position).magnitude;
                var e = body.eccentricity;
                var a = r / (1 - e);
                return a;
            }
            
            Debug.LogWarning("[GravityUtils] GravityManager2D not found");
            return 0;
        }
        
        public static double SemiMajorAxisByEccentricity(GravityBody2D body, double r)
        {
            var e = body.eccentricity;
            var a = r / (1 - e);
            return a;
        }

        public static double SemiMajorAxisByEnergy(GravityBody3D body)
        {
            if (GravityManager3D.Instance)
            {
                var r = body.position - GravityManager3D.Instance.star.position;
                var mu = StandardGravitationalParameter();
                var energy = body.Velocity.sqrMagnitude / 2.0 - mu / r.magnitude;
                var a = -mu / (2.0 * energy);
                return a;
            }
            
            Debug.LogWarning("[GravityUtils] GravityManager3D not found");
            return 0;
        }
        
        public static double SemiMajorAxisByEccentricity(GravityBody3D body)
        {
            if (GravityManager3D.Instance)
            {
                var r = (body.position - GravityManager3D.Instance.star.position).magnitude;
                var e = body.eccentricity;
                var a = r / (1 - e);
                return a;
            }
            
            Debug.LogWarning("[GravityUtils] GravityManager3D not found");
            return 0;
        }
        
        public static double SemiMajorAxisByEccentricity(GravityBody3D body, double r)
        {
            var e = body.eccentricity;
            var a = r / (1 - e);
            return a;
        }
        
        public static double SemiMinorAxis(GravityBody2D body) => Math.Sqrt(body.periapsis * body.apoapsis);
        
        public static double SemiMinorAxis(GravityBody3D body) => Math.Sqrt(body.periapsis * body.apoapsis);

        public static double StandardGravitationalParameter()
        {
            if (GravityManager2D.Instance)
            {
                var manager = GravityManager2D.Instance;
                return manager.star.mass * manager.g;
            }
            if (GravityManager3D.Instance)
            {
                var manager = GravityManager3D.Instance;
                return manager.star.mass * manager.g;
            }
            
            Debug.LogWarning("[GravityUtils] GravityManager not found");
            return 0.0;
        }
    }
}