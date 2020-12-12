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

        _navMeshAgent.SetDestination(_settings.Player.transform.position);
    }
    
    private void PlayerInSight(Animator animator)
    {
        var origin = _navMeshAgent.transform.position + Vector3.up * _settings.HeightMultiplier;
        var direction = (_navMeshAgent.transform.forward - _navMeshAgent.transform.right).normalized;
        var angleStep = Quaternion.AngleAxis(_settings.FieldOfView / _settings.RayCastCount, Vector3.up);
        
        for (var i = 0; i < _settings.RayCastCount; i++)
        {
            Debug.DrawRay(origin, direction * _settings.SightRange, Color.yellow);

            if (Physics.Raycast(origin, direction, out var hit, _settings.SightRange))
            {
                if (hit.collider.CompareTag("Player"))
                    Debug.Log("ATTACK!");
            }

            direction = angleStep * direction;
        }
    }
}
