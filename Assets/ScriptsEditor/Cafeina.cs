using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cafeina : MonoBehaviour
{
    public Sprite buit;
    public Sprite migbuit;
    public Sprite mig;
    public Sprite migple;
    public Sprite ple;
    public Image img;
    public void ActualitzarCafeina(int Cafeina){
        Debug.Log(Cafeina);
        if(Cafeina<=2) img.sprite = buit;
        else if(Cafeina==3 || Cafeina==4) img.sprite = migbuit;
        else if(Cafeina==5 || Cafeina==6) img.sprite = mig;
        else if(Cafeina==7 || Cafeina==8) img.sprite = migple;
        else img.sprite = ple;
    }
}
