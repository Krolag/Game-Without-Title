using UnityEngine;

public abstract class ThrowableBehaviourObjectBase : ScriptableObject
{
    public abstract void Action(GameObject source, Collision target);
}
