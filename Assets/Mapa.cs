using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mapa : MonoBehaviour
{
    public void EntrarDepartament(string Departament){
        SceneManager.LoadScene("Oficina");
    }
}
