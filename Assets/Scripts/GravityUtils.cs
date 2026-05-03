using System;

namespace SpaceGravity
{
    public static class GravityUtils
    {
        public static double HillSphere(GravityBody body, GravityBody star)
        {
            var a = Vector2Double.Distance(body.position, star.position);
            var m = body.mass; 
            var M = star.mass;
            return a * Math.Pow(m / (3 * (M + m)), 1.0/3.0);
        }
    }
}