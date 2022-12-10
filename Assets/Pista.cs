using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pista : MonoBehaviour
{
    public bool QualsevolLloc = false;
    public void Tancar(){
        Destroy(gameObject);
    }

    void Update() {
        if(QualsevolLloc && Input.GetMouseButtonDown(0)){
            Tancar();
        }
    }
}
