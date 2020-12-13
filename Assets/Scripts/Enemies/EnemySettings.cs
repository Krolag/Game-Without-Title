using System;
using UnityEngine;
using UnityEngine.AI;


public class EnemySettings : MonoBehaviour
{
    [Header("Movement")]
    public GameObject[] DestinationPoints;
    public int IndexOfCurrentDestinationPoint;
    public float MinimumDistanceFromPoint;

    [Header("RayCasts")]
    public GameObject Player;
    public float HeightMultiplier;
    public float FieldOfView;
    public float SightRange;
    public int RayCastsCount;

    [Header("Awarness")]
    public AwarenessBar AwarenessBar;
    public float CurrentAwarness;
    public float MaxAwarness;
    public float AwarenessTime = 5f;

    [Header("Wander state")]
    public float WanderSpeed;
    public float WanderAcceleration;

    [Header("Investigate state")]
    public float InvestigateSpeed;
    public float InvestigateAcceleration;
    public EventObject noiseEvent;

    public Vector3 PositionToInvestigate { get; set; } = Vector3.zero;

    private void Awake()
    {
        noiseEvent.RegisterListener(this);
        AwarenessBar.SetMaxAwareness(MaxAwarness);
        PositionToInvestigate = Player.transform.position;
    }

    private void OnEnable()
    {
        GetComponent<Killable>().Death += OnDeath;
    }

    private void OnDisable()
    {
        GetComponent<Killable>().Death -= OnDeath;
    }

    private void OnDeath()
    {
        noiseEvent.RemoveListener(this);
        Destroy(this.gameObject);
        temporary.EnemiesLeft--;//xd

    }

    private void OnApplicationQuit()
    {
        noiseEvent.RemoveListener(this);
    }

    public void HeardNoise(Vector3 position)
    {
        Debug.Log(gameObject.name + "Heard something!!!!!!!");
        PositionToInvestigate = position;
        //HACK HACK HACK as fuck
        GetComponent<Animator>().SetInteger(Animator.StringToHash("State"), (int)Transition.INVESTIGATE);
    }
    public void EnemyAwareness(NavMeshAgent navMeshAgent)
    {
        if (PlayerMovement.isCrouching == true)
            CurrentAwarness += 25f * Time.deltaTime;
        else if (PlayerMovement.isSprinting == true)
            CurrentAwarness += 50f * Time.deltaTime;
        else
            CurrentAwarness += 40f * Time.deltaTime;

        if (CurrentAwarness >= 30f)
        {
            SightRange = 40f;
            RayCastsCount = 40;
        }
        if (CurrentAwarness >= 50f)
        {
            navMeshAgent.speed = 2f;
            navMeshAgent.acceleration = 10f;
        }
        if (CurrentAwarness >= 90f)
        {
            navMeshAgent.speed = 9f;
            navMeshAgent.acceleration = 12f;
        }

        AwarenessBar.setAwareness(CurrentAwarness);
    }
}
