using System;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance;

    public double metersPerUnit;
    public double g;
    public double simulationSpeed = 1.0;

    public GravityBody star;
    
    private static readonly List<GravityBody> Bodies = new();

    public static void Register(GravityBody body) => Bodies.Add(body);
    public static void Unregister(GravityBody body) => Bodies.Remove(body);

    private void Init(GravityBody body)
    {
        body.hillSphere = HillSphere(body);
        if (!body.parent) body.parent = GetParent(body);
    }

    private void Awake()
    {
        if (!Instance) Instance = this;
    }

    private void Start()
    {
        foreach (var body in Bodies)
        {
            Init(body);
            SetInitialVelocity(body);
        }
    }

    private void Update()
    {
        var dt = Time.deltaTime * simulationSpeed;
        
        foreach (var body in Bodies)
        {
            if (!body.parent) continue;
            
            var dir = body.parent.position - body.position;
            var r = dir.magnitude;
            
            var m1m2 = g * body.mass * body.parent.mass;
            var f = m1m2 / (r * r);

            var acceleration = dir.normalized * (f / body.mass);
            body.velocity += acceleration * dt;
            body.position += body.velocity * dt;
            
            body.transform.position = body.position / metersPerUnit;
        }
    }

    private void SetInitialVelocity(GravityBody body)
    {
        if (!body.parent) return;
    
        var dir = body.parent.position - body.position;
        var v = Math.Sqrt(g * body.parent.mass / dir.magnitude); // √(GM/r)

        var perpendicular = new Vector2Double(-dir.normalized.y, dir.normalized.x);
        body.velocity = perpendicular * v;
    }

    public GravityBody GetParent(GravityBody body)
    {
        var r = body.hillSphere;

        return null;
    }

    public double HillSphere(GravityBody body)
    {
        var a = Vector2Double.Distance(body.position, star.position);
        var m = body.mass;
        var M = star.mass;
        return a * Math.Pow(m / (3 * (M + m)), 1.0/3.0);
    }
}