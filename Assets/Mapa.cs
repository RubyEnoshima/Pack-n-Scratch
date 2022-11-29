using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mapa : MonoBehaviour
{
    public GameObject DepartamentDesbloqueig;
    

    void Start(){
        DepartamentDesbloqueig.SetActive(Global.estaDesbloquejat);
    }
    public void EntrarDepartament(string Departament){
        Global.Departament = Departament;
        SceneManager.LoadScene("Editor");
    }
}
