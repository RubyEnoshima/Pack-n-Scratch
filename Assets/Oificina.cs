using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Oificina : MonoBehaviour
{
    public GameObject Llibre;
    public SpriteRenderer fondo;
    public Sprite Dep1;
    public Sprite Dep2;
    public void MostrarLlibre(){
        Llibre.SetActive(true);
    }

    public void OcultarLlibre(){
        Llibre.SetActive(false);
    }

    public void AnarEditor(){
        SceneManager.UnloadSceneAsync("Oficina");
    }

    // Start is called before the first frame update
    void Start()
    {
        if(Global.Departament=="Departament1"){
            fondo.sprite = Dep1;
        }else{
            fondo.sprite = Dep2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
