using UnityEngine;

[CreateAssetMenu(menuName = "ThrowableBehaviours/ActivateBehaviour", fileName = "newActivateBehaviour")]
public class ThrowableActivateBehaviourObject : ThrowableBehaviourObjectBase
{
    //potentialy some activation time
    public override void Action(GameObject source, Collision target)
    {
        Activatable component;
        if(target.gameObject.TryGetComponent<Activatable>(out component))
        {
            component.Activate();
            Destroy(source.gameObject);
        }
    }
}