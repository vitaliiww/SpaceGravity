using System;
using UnityEngine;

namespace SpaceGravity
{
    [Serializable]
    public struct Vector3Double
    {
        public double x;
        public double y;
        public double z;
    
        public static Vector3Double zero = new(0, 0, 0);
        public static Vector3Double up => new(0, 1, 0);
        public static Vector3Double right => new(1, 0, 0);
        public static Vector3Double forward => new(0, 0, 1);
    
        public Vector3Double(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector3Double operator +(Vector3Double a, Vector3Double b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vector3Double operator -(Vector3Double a, Vector3Double b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vector3Double operator -(Vector3Double a) => new(-a.x, -a.y, -a.z);
        public static Vector3Double operator *(Vector3Double a, Vector3Double b) => new(a.x * b.x, a.y * b.y, a.z * b.z);
        public static Vector3Double operator *(Vector3Double a, double b) => new(a.x * b, a.y * b, a.z * b);
        public static Vector3Double operator /(Vector3Double a, double b) => new(a.x / b, a.y / b, a.z / b);
        public static bool operator ==(Vector3Double a, Vector3Double b) => a.x == b.x && a.y == b.y && a.z == b.z;
        public static bool operator !=(Vector3Double a, Vector3Double b) => !(a == b);

        public static implicit operator Vector3Double(Vector2 a) => new(a.x, a.y, 0);
        public static implicit operator Vector3Double(Vector3 a) => new(a.x, a.y, a.z);
        public static implicit operator Vector2(Vector3Double a) => new((float)a.x, (float)a.y);
        public static implicit operator Vector3(Vector3Double a) => new((float)a.x, (float)a.y, (float)a.z);
        
        public static implicit operator Vector2Double(Vector3Double a) => new(a.x, a.y);
        public static implicit operator Vector3Double(Vector2Double a) => new(a.x, a.y, 0);
    
        public double magnitude => Math.Sqrt(x * x + y * y + z * z);
        public double sqrMagnitude => x * x + y * y + z * z;
        public Vector3Double normalized => magnitude > 0 ? new(x / magnitude, y / magnitude, z / magnitude) : zero;
    
        public static double Distance(Vector3Double a, Vector3Double b)
        {
            var dx = a.x - b.x;
            var dy = a.y - b.y;
            var dz = a.z - b.z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        public static double Dot(Vector3Double a, Vector3Double b) => a.x * b.x + a.y * b.y + a.z * b.z;
        public static Vector3Double Cross(Vector3Double a, Vector3Double b) => new(
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x
        );

        public override string ToString() => $"({x:e}, {y:e}, {z:e})";
        public override bool Equals(object obj) => obj is Vector3Double other && this == other;
        public override int GetHashCode() => HashCode.Combine(x, y, z);
    }
}