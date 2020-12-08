using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIElementRunStats : MonoBehaviour
{
    private TextMeshProUGUI _sign = null;
    void Start()
    {
        _sign = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        _sign.text = Core.Instance.Runs.Stats.ToString();
    }
}
