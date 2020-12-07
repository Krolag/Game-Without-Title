using UnityEngine;

[CreateAssetMenu(menuName = "ThrowableBehaviours/KillBehaviour", fileName = "newKillBehaviour")]
public class ThrowableKillBehaviourObject : ThrowableBehaviourObjectBase
{
    //potentialy some dealt damage, or continued damage from fe. poison
    public override void Action(GameObject source, Collision target)
    {
        Killable component;
        if(target.gameObject.TryGetComponent<Killable>(out component))
        {
            component.Die();
            Destroy(source.gameObject);
        }
    }
}
