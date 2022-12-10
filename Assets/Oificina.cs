using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Oificina : MonoBehaviour
{
    public GameObject Llibre;
    public SpriteRenderer fondo;
    public Sprite Dep1;
    public Sprite Dep1Dalt;
    public Sprite Dep2;
    public Sprite Dep2Dalt;
    public Pista pista;
    public Tutorial tutorial;
    public Button Estanteria;
    public Button Monitor;
    public void MostrarLlibre(){
        Llibre.SetActive(true);
    }

    public void OcultarLlibre(){
        Llibre.SetActive(false);
    }

    public void AnarEditor(){
        Global.pistaOfi = true;
        SceneManager.UnloadSceneAsync("Oficina");
    }

    // Start is called before the first frame update
    void Start()
    {
        if(Global.tutoOfi){
            tutorial.Pistas[1].gameObject.SetActive(false);
            Destroy(tutorial.gameObject);
        }else{
            Estanteria.interactable = false;
            Monitor.interactable = false;
        }
        
        if(Global.Departament=="Departament1"){
            if(Global.ModeDaltonic) fondo.sprite = Dep1Dalt;
            else fondo.sprite = Dep1;
        }else{
            if(Global.ModeDaltonic) fondo.sprite = Dep2Dalt;
            else fondo.sprite = Dep2;
        }

        if(Global.pistaOfi) pista.Tancar(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(!Global.tutoOfi && tutorial.desbloquejat) {
            Global.tutoOfi = true;
            Estanteria.interactable = true;
            Monitor.interactable = true;
        }
    }
}
