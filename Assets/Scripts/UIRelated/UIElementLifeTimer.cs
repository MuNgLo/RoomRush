using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(TextMeshProUGUI))]
public class UIElementLifeTimer : MonoBehaviour
{
    private TextMeshProUGUI _sign = null;
    // Start is called before the first frame update
    void Start()
    {
        _sign = GetComponent<TextMeshProUGUI>();
        _sign.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        _sign.text = Core.Instance.Runs.TimerLife.ToString();
    }
}
