using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CredentialsManager : MonoBehaviour {

    public Text loginButtonText;
    public Text signupButtonText;
    public Text mainButtonText;
    public Text errorMessage;
    public GameObject errorPanel;
    public InputField usernameField;
    public InputField passwordField;
    public InputField confirmPasswordField;

 	public void SignupButtonClick()
    {
        errorPanel.SetActive(false);
        ClearForm();
        confirmPasswordField.gameObject.SetActive(true);
        mainButtonText.text = "REGISTRATI";
    }

    public void LoginButtonClick()
    {
        errorPanel.SetActive(false);
        ClearForm();
        confirmPasswordField.gameObject.SetActive(false);
        mainButtonText.text = "ENTRA";
    }

    public void MainButtonClick()
    {
        //Controllo sul riempimento completo del form
        if (!CheckFormValidity())
        {
            errorMessage.text = "Controlla di aver riempito tutti i campi";
            errorPanel.SetActive(true);
            return;
        }
        
        if (mainButtonText.text == "ENTRA")
        {
            if (DatabaseManager.GetTherapist(usernameField.text, passwordField.text))
            {
                SceneManager.LoadScene("MinigameSelection");
            } else
            {
                errorMessage.text = "Lo username e/o la password sono sbagliati";
                errorPanel.SetActive(true);
                return;
            }
        }
        else
        {
            if (!PasswordConfirmCheck())
            {
                errorMessage.text = "Controlla di aver inserito correttamente entrambe le password";
                errorPanel.SetActive(true);
                return;
            }

            //Se l'inserimento è andato a buon fine allora andiamo nella selezione dei minigame altrimenti mandiamo un messaggio di errore
            if (DatabaseManager.InsertTherapist(usernameField.text, passwordField.text))
            {
                SceneManager.LoadScene("MinigameSelection");
            }
            else
            {
                errorMessage.text = "Non è stato possibile effetuare la registrazione.";
                errorPanel.SetActive(true);
                return;
            }
        }
    }

    //Metodo utilizzato per verificare che il form Password e Confirm password combacino
    private bool PasswordConfirmCheck()
    {
        if (passwordField.text != confirmPasswordField.text)
            return false;
        return true;
    }

    //Metodo utilizzato per verificare che tutti i campi del form sono stati riempiti
    private bool CheckFormValidity()
    {
        if (usernameField.text == "")
            return false;
        if (passwordField.text == "")
            return false;
        if (mainButtonText.text == "REGISTRATI")
            if (confirmPasswordField.text == "")
                return false;      
        return true;
    }

    private void ClearForm()
    {
        usernameField.text = "";
        passwordField.text = "";
        confirmPasswordField.text = "";
    }
}
