using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InvestigateState : StateMachineBehaviour
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
        PlayerInSight(animator);

        // Check if enemy has path or if the destination is not set as player position
        if (!_navMeshAgent.hasPath || _navMeshAgent.destination != _settings.PositionToInvestigate)
            _navMeshAgent.SetDestination(_settings.PositionToInvestigate);
        
        //this should only work for nosie event, if positinToInvestigate is player he shold be killed before this condition is met
        if(_navMeshAgent.remainingDistance < 3.0f)
            animator.SetInteger(State, (int)Transition.WANDER);


        // Check for current Awarness
        if (_settings.CurrentAwarness >= 100f)
        {
            _settings.CurrentAwarness = 100f;
            animator.SetInteger(State, (int)Transition.ATTACK);
        }
        _settings.AwarenessBar.setAwareness(_settings.CurrentAwarness);
    }

    private void PlayerInSight(Animator animator)
    {
        //var origin = _navMeshAgent.transform.position + Vector3.up * _settings.HeightMultiplier;
        //var direction = (_navMeshAgent.transform.forward - _navMeshAgent.transform.right).normalized;
        //var angleStep = Quaternion.AngleAxis(_settings.FieldOfView / _settings.RayCastsCount, Vector3.up);
        
        //for (var i = 0; i < _settings.RayCastsCount; i++)
        //{
        //    Debug.DrawRay(origin, direction * _settings.SightRange, Color.yellow);

        //    if (Physics.Raycast(origin, direction, out var hit, _settings.SightRange))
        //    {
        //        if (hit.collider.CompareTag("Player"))
        //        {
        //            _settings.PositionToInvestigate = _settings.Player.transform.position;
        //            _settings.EnemyAwareness(_navMeshAgent);
        //        }
        //    }

        //    direction = angleStep * direction;
        //}

        if (!_settings.PlayerInSight)
            return;

        var position = _navMeshAgent.transform.position;
        var origin = position + Vector3.up * _settings.HeightMultiplier;
        var direction = (_settings.Player.transform.position - position).normalized;

        Debug.DrawRay(origin, direction * _settings.SightRange, Color.yellow);
        if (Physics.Raycast(origin, direction, out var hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.CompareTag("Player"))
            {
                _settings.PositionToInvestigate = _settings.Player.transform.position;
                _settings.EnemyAwareness(_navMeshAgent);
            }
        }
    }
}
