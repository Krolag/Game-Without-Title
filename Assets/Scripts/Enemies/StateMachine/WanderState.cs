using UnityEngine;
using UnityEngine.AI;

public class WanderState : StateMachineBehaviour
{
    #region Properties
    [SerializeField] private EnemySettings _settings;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public const string TransitionParameter = "State";
    #endregion
    
    private static readonly int State = Animator.StringToHash(TransitionParameter);

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_navMeshAgent == null)
            _navMeshAgent = animator.GetComponent<NavMeshAgent>();
        if (_settings == null)
            _settings = _navMeshAgent.GetComponentInParent<EnemySettings>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if player is in move
        if (!PlayerMovement2.IsPlayerInMove)
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;
            _navMeshAgent.enabled = true;
        }
        else
        {
            _navMeshAgent.isStopped = false;   
            // If destination point is reached, go back to idle state
            if (_navMeshAgent.hasPath)
            {
                if (Vector3.Distance(_navMeshAgent.transform.position,
                        _settings.DestinationPoints[_settings.IndexOfCurrentDestinationPoint].transform.position) <
                    _settings.MinimumDistanceFromPoint)
                {
                    _settings.IndexOfCurrentDestinationPoint = (int) Random.Range(0, _settings.DestinationPoints.Length);
                    _navMeshAgent.SetDestination(_settings.DestinationPoints[_settings.IndexOfCurrentDestinationPoint]
                        .transform.position);
                }
            }
            else
                _navMeshAgent.SetDestination(_settings.DestinationPoints[_settings.IndexOfCurrentDestinationPoint]
                    .transform.position);
        }

        // Check if player in sight
        PlayerInSight(animator);
    }
    
    private void PlayerInSight(Animator animator)
    {
        var origin = _navMeshAgent.transform.position + Vector3.up * _settings.HeightMultiplier;
        var direction = (_navMeshAgent.transform.forward - _navMeshAgent.transform.right).normalized;
        var angleStep = Quaternion.AngleAxis(_settings.FieldOfView / _settings.RayCastCount, Vector3.up);
        
        for (var i = 0; i < _settings.RayCastCount; i++)
        {
            Debug.DrawRay(origin, direction * _settings.SightRange, Color.white);

            if (Physics.Raycast(origin, direction, out var hit, _settings.SightRange))
            {
                if (hit.collider.name == "Player") //dont search gameobjects by name
                    animator.SetInteger(State, (int)Transition.INVESTIGATE);
            }

            direction = angleStep * direction;
        }
    }
}
