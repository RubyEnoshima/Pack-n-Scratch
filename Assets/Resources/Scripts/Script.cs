using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour
{

    public float[] Inputs = {3,6,1};
    public string[] InputsString = {"","",""};

    public bool SonInputsNombres = true;

    public List<string> ResultatsEsperats1 = new List<string>(){"6"};
    public List<string> ResultatsEsperats2 = new List<string>(){"12"};
    public List<string> ResultatsEsperats3 = new List<string>(){"2"};

    public string Descripcio = "Volem que l'script multipliqui l'entrada per 2.";
    public string Inspiracio = "L'script semblava correcte...";
    public string Client = "Agapito Dissousa";
    public string Assumpte = "Un asunto por defecto";

    public bool esDesbloqueig = false;
}
