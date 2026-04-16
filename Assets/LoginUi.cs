using UnityEngine;
using TMPro;

public class LoginUI : MonoBehaviour
{
    public TMP_InputField inputEmail;
    public TMP_InputField inputSenha;

    public SQLiteManager dbManager;

    public void OnClickLogin()
    {
        string email = inputEmail.text;
        string senha = inputSenha.text;

        dbManager.ValidarLogin(email, senha);
    }
}