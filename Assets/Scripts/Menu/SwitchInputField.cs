using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchInputField : MonoBehaviour {

    public GameObject[] inputFields;

    void Update()
    {
        SwitchWithTab();
    }

    private void SwitchWithTab()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inputFields[0].GetComponent<InputField>().isFocused)
            {
                inputFields[1].GetComponent<InputField>().Select();
                return;
            }

            if (inputFields[1].GetComponent<InputField>().isFocused)
            {
                if (inputFields[2].activeInHierarchy)
                    inputFields[2].GetComponent<InputField>().Select();
                else
                    inputFields[0].GetComponent<InputField>().Select();
                return;
            }
            if (inputFields[2].GetComponent<InputField>().isFocused && inputFields[2].activeInHierarchy)
            {
                inputFields[0].GetComponent<InputField>().Select();
                return;
            }

        }
    }
}
