using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private Transform _openPosition;
    [SerializeField] private Transform _closePosition;
    [SerializeField] private float _step;

    private Coroutine _currentlyUsedCoroutine;

    [ContextMenu("open")]
    public void Open()
    {
        if (gameObject.transform.position == _openPosition.position)
            return;
        if (_currentlyUsedCoroutine != null)
            StopCoroutine(_currentlyUsedCoroutine);

        _currentlyUsedCoroutine = StartCoroutine(Move(_openPosition.position, _step));
    }
    [ContextMenu("close")]
    public void Close()
    {
        if (gameObject.transform.position == _closePosition.position)
            return;
        if (_currentlyUsedCoroutine != null)
            StopCoroutine(_currentlyUsedCoroutine);

        _currentlyUsedCoroutine = StartCoroutine(Move(_closePosition.position, _step));
    }

    private IEnumerator Move(Vector3 position, float step)
    {
        while (gameObject.transform.position != position)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, position, step * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}