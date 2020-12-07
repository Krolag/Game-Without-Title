using UnityEngine;
using System;

[CreateAssetMenu(menuName = "ThrowableBehaviours/NoiseBehaviour", fileName = "newNoiseBehaviour")]
public class ThrowableNoiseBehaviourObject : ThrowableBehaviourObjectBase
{
    public float noiseRange;
    public EventObject eventObject;
    public override void Action(GameObject source, Collision target)
    {
        eventObject?.Invoke(source, noiseRange);
    }
}
