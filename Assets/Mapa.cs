using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mapa : MonoBehaviour
{
    public GameObject DepartamentDesbloqueig;
    public GameObject AtencioAlClient;
    public GameObject Atencio;
    public SpriteRenderer AtencioCercle;
    public GameObject Control;
    public SpriteRenderer ControlCercle;
    public SpriteRenderer Estrella;
    public SpriteRenderer Estrella2;
    public SpriteRenderer MapaRenderer;
    public Sprite MapaDalt;
    public Sprite MapaColor;
    public Button PlaygroundBoto;
    public Tutorial tutorial;

    void ComprovarSprites(){
        if(Global.ProvaSuperada1){
            Atencio.SetActive(false);
            AtencioAlClient.GetComponent<Button>().interactable = false;
            Control.transform.position = Atencio.transform.position;
            Estrella.color = Color.white;
        }else if(Global.EsPotFerProva1){
            if(!Global.ModeDaltonic) AtencioCercle.color = Color.yellow;
            else AtencioCercle.color = Color.white;

        }else if(Global.ModeDaltonic) AtencioCercle.color = Color.gray;
        else AtencioCercle.color = Color.red;

        if(Global.ProvaSuperada2){
            Control.SetActive(false);
            Estrella2.color = Color.white;
            DepartamentDesbloqueig.GetComponent<Button>().interactable = false;
        }
        else if(Global.EsPotFerProva2){
            if(!Global.ModeDaltonic)ControlCercle.color = Color.yellow;
            else ControlCercle.color = Color.white;
        }else if(Global.ModeDaltonic) ControlCercle.color = Color.gray;
        else ControlCercle.color = Color.red;

        if(Global.ModeDaltonic) MapaRenderer.sprite = MapaDalt;
        else MapaRenderer.sprite = MapaColor;
    }

    public void CanviarMode(){
        if(Global.tutoMapa){
            Global.ModeDaltonic = !Global.ModeDaltonic;
            ComprovarSprites();

        }
    }

    void Start(){
        if(Global.tutoMapa){
            tutorial.Pistas[0].gameObject.SetActive(false);
            Destroy(tutorial.gameObject);
        }
        else {
            AtencioAlClient.GetComponent<Button>().interactable = false;
            PlaygroundBoto.interactable = false;

        }
        DepartamentDesbloqueig.GetComponent<Button>().interactable = Global.estaDesbloquejat;
        Control.SetActive(Global.estaDesbloquejat);

        if(Global.ProvaSuperada1 && Global.ProvaSuperada2){
            SceneManager.LoadScene("Final");
        }

        ComprovarSprites();
    }

    private void Update() {
        if(!Global.tutoMapa && tutorial.desbloquejat){
            Global.tutoMapa = true;
            AtencioAlClient.GetComponent<Button>().interactable = true;
            PlaygroundBoto.interactable = true;
        }
    }

    public void EntrarDepartament(string Departament){
        if(Global.tutoMapa){
            Global.Departament = Departament;
            SceneManager.LoadScene("Editor");

        }
    }

    public void Playground(){
        if(Global.tutoMapa){
            SceneManager.LoadScene("Playground");

        }
    }
}
