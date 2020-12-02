using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "EventsObjects/event", fileName = "newEventObject")]
public class EventObject : ScriptableObject
{
    public List<EnemyAI> listeners;
    //TODO: add AddListener Method when enemys will be generated
    private void Awake()
    {
        //TODO: change EnemyAI for Listener
        listeners = new List<EnemyAI>(FindObjectsOfType<EnemyAI>());
    }

    public void Invoke(GameObject source, float range = Mathf.Infinity)
    {
        foreach (var listener in listeners)
        {
            if (Vector3.Distance(listener.gameObject.transform.position, source.transform.position) < range)
            {
                //listeners.hear or something like thats
            }
        }
    }

    public void RemoveAllListeners()
    {
        listeners.Clear();
    }
}