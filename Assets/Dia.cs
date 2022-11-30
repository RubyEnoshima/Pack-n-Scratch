using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dia : MonoBehaviour
{
    public Text ScriptsCorrectes;
    public Text ScriptsIncorrectes;
    public Text Percentatge;

    public void AcabarDia(int correctes, int incorrectes, float mitjana){
        this.gameObject.SetActive(true);

        ScriptsCorrectes.text = correctes.ToString();
        ScriptsIncorrectes.text = incorrectes.ToString();
        Percentatge.text = (mitjana*100f).ToString()+"%";

        // Guardar estadistiques
        Global.suma+=mitjana;
        Global.mitjana = Global.suma / Global.dia;
        Global.dia++;
    }
    
    public void Acabar(){
        SceneManager.LoadScene("Mapa");
    }
}
