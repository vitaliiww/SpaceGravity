using System;
using UnityEngine;

namespace SpaceGravity
{
    [Serializable]
    public struct Vector2Double
    {
        public double x;
        public double y;
    
        public static Vector2Double zero = new(0, 0);
    
        public Vector2Double(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2Double operator +(Vector2Double a, Vector2Double b) => new(a.x + b.x, a.y + b.y);
        public static Vector2Double operator -(Vector2Double a, Vector2Double b) => new(a.x - b.x, a.y - b.y);
        public static Vector2Double operator -(Vector2Double a) => new(-a.x, -a.y);
        public static Vector2Double operator *(Vector2Double a, Vector2Double b) => new(a.x * b.x, a.y * b.y);
        public static Vector2Double operator *(Vector2Double a, double b) => new(a.x * b, a.y * b);
        public static Vector2Double operator /(Vector2Double a, double b) => new(a.x / b, a.y / b);
        public static bool operator ==(Vector2Double a, Vector2Double b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Vector2Double a, Vector2Double b) => !(a == b);

        public static implicit operator Vector2Double(Vector2 a) => new(a.x, a.y);
        public static implicit operator Vector2Double(Vector3 a) => new(a.x, a.y);
        public static implicit operator Vector2(Vector2Double a) => new((float)a.x, (float)a.y);
        public static implicit operator Vector3(Vector2Double a) => new((float)a.x, (float)a.y, 0);
    
        public double magnitude => Math.Sqrt(x * x + y * y);
        public double sqrMagnitude => x * x + y * y;
        public Vector2Double normalized => magnitude > 0 ? new(x / magnitude, y / magnitude) : zero;
    
        public static double Distance(Vector2Double a, Vector2Double b)
        {
            var dx = a.x - b.x;
            var dy = a.y - b.y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public override string ToString() => $"({x:e}, {y:e})";
        public override bool Equals(object obj) => obj is Vector2Double other && this == other;
        public override int GetHashCode() => HashCode.Combine(x, y);
    }
}