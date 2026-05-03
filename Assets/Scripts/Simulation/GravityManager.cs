using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGravity
{
    public class GravityManager : MonoBehaviour
    {
        public static GravityManager Instance;

        public double metersPerUnit = 100000000;
        public double g = 6.674E-11;
        public int simulationSpeed = 86400;

        public GravityBody star;
        
        private static readonly List<GravityBody> Bodies = new();

        public static void Register(GravityBody body) => Bodies.Add(body);
        public static void Unregister(GravityBody body) => Bodies.Remove(body);

        private void Awake()
        {
            if (!Instance) Instance = this;
        }

        private void Start()
        {
            foreach (var body in Bodies)
            {
                SetInitialVelocity(body);
            }
        }

        private void Update()
        {
            var dt = Time.deltaTime * simulationSpeed;
            
            foreach (var body in Bodies)
            {
                body.Acceleration = Vector2Double.zero;

                foreach (var other in Bodies)
                {
                    if (body == other) continue;

                    var dir = other.position - body.position;
                    var r = dir.magnitude;

                    body.Acceleration += dir.normalized * (g * other.mass / (r * r));
                }
            }

            foreach (var body in Bodies)
            {
                body.Velocity += body.Acceleration * dt;
                body.position += body.Velocity * dt;

                body.transform.position = body.position / metersPerUnit;
            }
        }

        private GravityBody FindDominantBody(GravityBody body)
        {
            if (!star)
            {
                Debug.LogError("[GravityManager] Please, specify the star of system");
                return null;
            }
            
            foreach (var other in Bodies)
            {
                if (body == other) continue;

                var hill = GravityUtils.HillSphere(other, star);

                var dist = Vector2Double.Distance(body.position, other.position);

                if (dist < hill)
                {
                    return other;
                }
            }

            return star; // fallback
        }

        private void SetInitialVelocity(GravityBody body)
        {
            if (!star)
            {
                Debug.LogError("[GravityManager] Please, specify the star of system");
                return;
            }
            if (body == star) return;
            
            var dominant = FindDominantBody(body);
            if (!dominant)
            {
                Debug.LogWarning($"No dominant body found for {body.name}");
                return;
            }
            
            var dir = dominant.position - body.position;
            var r = dir.magnitude;

            var e = body.eccentricity;
            var a = r / (1.0 - e);

            var v = Math.Sqrt(g * dominant.mass * (2.0 / r - 1.0 / a));

            var perpendicular = new Vector2Double(-dir.normalized.y, dir.normalized.x);

            body.Velocity = dominant.Velocity + perpendicular * v;
        }
    }
}