using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Text _interactionText;
    
    void Update()
    {
        _interactionText.text = "";
        Ray ray = new Ray(Camera.main.gameObject.transform.position, Camera.main.gameObject.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, 4f);

        foreach (var hit in hits)
        {
            Interactable interactable;
            if (hit.collider.TryGetComponent<Interactable>(out interactable))
                _interactionText.text = "[ E ]";
        }
        if (GetComponentInParent<Player>().objectToThrow != null)
            _interactionText.text = "[ RMB ]";
    }
}
