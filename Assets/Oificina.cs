using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Oificina : MonoBehaviour
{
    public GameObject Llibre;
    public void MostrarLlibre(){
        Llibre.SetActive(true);
    }

    public void OcultarLlibre(){
        Llibre.SetActive(false);
    }

    public void AnarEditor(){
        SceneManager.LoadScene("Editor");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
