using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TEMPODEVBB : MonoBehaviour
{
    TMP_Dropdown _Dropdown;
    private void Start()
    {
        _Dropdown = GetComponent<TMP_Dropdown>();    

        _Dropdown.options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i< 60; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            if (i==0)
            {
                option.text = $"{i + 1} Minute";
            }
            else
            {
                option.text = $"{i + 1} Minutes";
            }
           _Dropdown.options.Add(option);
        }

        _Dropdown.value = 5;
    }
}
