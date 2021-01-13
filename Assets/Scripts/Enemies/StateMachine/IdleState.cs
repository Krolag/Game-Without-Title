using UnityEngine;
using UnityEngine.AI;

// Move this to the definition file ASAP
public enum Transition
{
    IDLE,
    WANDER,
    ATTACK,
    INVESTIGATE
}

public class IdleState : StateMachineBehaviour
{
    #region Properties
    [SerializeField] private EnemySettings _settings;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    // Move this to the definition file ASAP
    public const string TransitionParameter = "State";
    #endregion

    #region Private Variables
    private Vector3 _playerPosition;
    private static readonly int State = Animator.StringToHash(TransitionParameter);
    #endregion

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_navMeshAgent == null)
            _navMeshAgent = animator.GetComponent<NavMeshAgent>();
        if (_settings == null)
            _settings = _navMeshAgent.GetComponentInParent<EnemySettings>();
        
        SetClosestPoint();

        _navMeshAgent.ResetPath();
        _navMeshAgent.speed = _settings.WanderSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get current player position
        _playerPosition = _settings.Player.transform.position;
        
        // Check if player is in move
        if (!PlayerMovement.IsPlayerInMove)
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;
            _navMeshAgent.enabled = true;
        }
        else
        {
            _navMeshAgent.isStopped = false;

            if (_navMeshAgent.hasPath)
                animator.SetInteger(State, (int) Transition.WANDER);
            else
                _navMeshAgent.SetDestination(_settings.DestinationPoints[_settings.IndexOfCurrentDestinationPoint].transform.position);
        
            // TODO: Check if Awarness bar should go up
        }
    }

    private void SetClosestPoint()
    {
        // Check for the closest point in DestinationPoints, then set the _currentDestinationPoint to it
        var distanceTMP = Vector3.Distance(_navMeshAgent.transform.position, _settings.DestinationPoints[0].transform.position);
        int indexOfPoint = 0;
        for (var i = 0; i < _settings.DestinationPoints.Length; i++)
        {
            if (!(Vector3.Distance(_navMeshAgent.transform.position, _settings.DestinationPoints[i].transform.position) < distanceTMP)) continue;;
            distanceTMP = Vector3.Distance(_navMeshAgent.transform.position, _settings.DestinationPoints[i].transform.position);
            indexOfPoint = i;
        }

        _settings.IndexOfCurrentDestinationPoint = indexOfPoint;
    }
}
