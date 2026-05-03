using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public double mass;
    public Vector2Double position; // only used for calculations
    public GravityBody parent;
    
    [HideInInspector] public Vector2Double velocity;
    [HideInInspector] public double hillSphere;
    
    private void Awake()
    {
        GravityManager.Register(this);
    }
}
