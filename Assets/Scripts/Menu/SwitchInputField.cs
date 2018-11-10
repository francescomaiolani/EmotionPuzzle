using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchInputField : MonoBehaviour {

    public InputField[] inputFields;

    void Update()
    {
        SwitchWithTab();
    }

    //Metodo che permete di andare da un InputField all'altro con il tasto TAB
    private void SwitchWithTab()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inputFields[0].isFocused)
            {
                inputFields[1].Select();
                return;
            }

            if (inputFields[1].GetComponent<InputField>().isFocused)
            {
                if (inputFields[2].IsActive())
                    inputFields[2].Select();
                else
                    inputFields[0].Select();
                return;
            }
            if (inputFields[2].isFocused && inputFields[2].IsActive())
            {
                inputFields[0].Select();
                return;
            }

        }
    }
}
