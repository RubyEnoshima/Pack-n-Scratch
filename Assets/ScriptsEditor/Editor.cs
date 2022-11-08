using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Editor : MonoBehaviour
{
    GameObject Blocs;
    Pantalla Pantalla;
    
    public void AfegirBloc(Bloc bloc){
        
        bloc.transform.parent = Blocs.transform;

    }

    public void Compilar(){
        foreach(Transform bloc in Blocs.transform){
            bloc.GetComponent<Bloc>().Executar();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Blocs = transform.Find("Blocs").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
