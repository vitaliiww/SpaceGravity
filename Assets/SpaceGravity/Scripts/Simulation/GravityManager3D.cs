using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGravity
{
    public class GravityManager3D : MonoBehaviour
    {
        public static GravityManager3D Instance;
        
        public double metersPerUnit = 100000000;
        public double g = 6.674E-11;
        public int simulationSpeed = 86400;
        [Tooltip("High simulation speed leads to position miscalculation. Substeps help to deal with that problem.")]
        public int subSteps = 1;

        public GravityBody3D star;
        
        private static readonly List<GravityBody3D> Bodies = new();

        public static void Register(GravityBody3D body) => Bodies.Add(body);
        public static void Unregister(GravityBody3D body) => Bodies.Remove(body);
        
        public static event Action OnInitialize;

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
            
            OnInitialize?.Invoke();
        }

        private void FixedUpdate()
        {
            var dt = Time.fixedDeltaTime * simulationSpeed;
            var subDt = dt / subSteps;

            for (var i = 0; i < subSteps; i++)
            {
                Step(subDt);
            }

            foreach (var body in Bodies)
            {
                body.transform.position = body.position / metersPerUnit;
            }
        }

        private void Step(double dt)
        {
            foreach (var body in Bodies)
            {
                body.position += body.Velocity * dt + body.Acceleration * (0.5 * dt * dt);
            }

            foreach (var body in Bodies)
            {
                body.PrevAcceleration = body.Acceleration;
                body.Acceleration = Vector3Double.zero;
            }

            foreach (var body in Bodies)
            {
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
                body.Velocity += (body.PrevAcceleration + body.Acceleration) * (0.5 * dt);
            }
        }

        private GravityBody3D FindDominantBody(GravityBody3D body)
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

                var dist = Vector3Double.Distance(body.position, other.position);

                if (dist < hill)
                {
                    return other;
                }
            }

            return star; // fallback
        }

        private void SetInitialVelocity(GravityBody3D body)
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

            var a = GravityUtils.SemiMajorAxisByEccentricity(body, r);
            var v = Math.Sqrt(g * dominant.mass * (2.0 / r - 1.0 / a));

            var radial = dir.normalized;

            var up = Vector3Double.up;
            if (Math.Abs(Vector3Double.Dot(radial, up)) > 0.99) up = new Vector3Double(1, 0, 0);

            var perpendicular = Vector3Double.Cross(radial, up).normalized;

            body.Velocity = dominant.Velocity + perpendicular * v;
        }
    }
}