using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocElse : Bloc
{
    bool error = false;
    protected override void Start(){
        base.Start();
        Funcio = "Else";
    }

    public override bool TeErrors()
    {
        return error;
    }

    public void PosarError(){
        error = true;
    }

    public override string ObtenirError()
    {
        return "No hi ha cap if abans d'aquest else...";
    }
}
