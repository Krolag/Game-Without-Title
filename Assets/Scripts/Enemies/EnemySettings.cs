using UnityEngine;


public class EnemySettings : MonoBehaviour
{
    public float HeightMultiplier;
    [Header("Movement")] 
    public GameObject[] DestinationPoints;
    public int IndexOfCurrentDestinationPoint;
    public float WanderSpeed;
    public float Acceleration;
    public float MinimumDistanceFromPoint;
    [Header("Detection")]
    public GameObject Player;
    public float FieldOfView;
    public float SightRange;
    public int RayCastCount;
}
