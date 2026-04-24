using UnityEngine;
using TMPro;

public class AlunoUI : MonoBehaviour
{
    public TMP_InputField inputNome;    
    public TMP_InputField inputEmail;
    public TMP_InputField inputSenha;

    public SQLiteManager dbManager;

    public void OnClickAdicionar()
    {
        dbManager.AdicionarAluno(inputNome.text, inputEmail.text, inputSenha.text);
    }

    public void OnClickRemover()
    {
        dbManager.RemoverAluno(inputEmail.text);
    }

    public void OnClickListar()
    {
        dbManager.ListarAlunos();
    }
}