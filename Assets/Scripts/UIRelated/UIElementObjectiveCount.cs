using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIElementObjectiveCount : MonoBehaviour
{
    private TextMeshProUGUI _sign = null;
    // Start is called before the first frame update
    void Start()
    {
        _sign = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _sign.text = $"{Core.Instance.Rooms.CurrentRoom.Condition.Clears} / {Core.Instance.Rooms.CurrentRoom.Definition.ObjectiveCount}"; 

    }
}
