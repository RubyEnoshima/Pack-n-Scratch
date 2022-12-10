using UnityEngine;
using UnityEngine.Video;
public static class Global{
    public static int dia = 1;
    public static float suma = 0f;
    public static float mitjana = 0f;
    public static string Departament = "Departament1";
    public static bool estaDesbloquejat = false;

    // ---- DEP 1 
    public static int nScriptsDep1 = 3;
    public static bool[] Fets1 = {false,false,false};
    public static int Restants1 = nScriptsDep1;
    public static bool EsPotFerProva1 = false;
    public static bool ProvaSuperada1 = false;

    // ---- DEP 2
    public static int nScriptsDep2 = 3;
    public static bool[] Fets2 = {false,false,false};
    public static int Restants2 = nScriptsDep2;
    public static bool EsPotFerProva2 = false;
    public static bool ProvaSuperada2 = false;

    public static bool ModeDaltonic = false;
    public static bool pistaOfi = false;

    public static bool tutoMapa = false;
    public static bool tutoOfi = false;
    public static bool tutoEditor = false;

    public static void EsPotDesbloquejar(){
        estaDesbloquejat = Restants1 <= nScriptsDep1/2;
    }

    public static int ScriptsRestants(){
        if(Departament=="Departament1"){
            return Restants1;
        }else{
            return Restants2;
        }
    }

    public static string ScriptAleatori(){
        int i;
        if(Departament=="Departament1"){
            i = UnityEngine.Random.Range(1,nScriptsDep1+1);
            while(Fets1[i-1]) i = UnityEngine.Random.Range(1,nScriptsDep1+1);
            return "Script"+i.ToString();

        }
        i = UnityEngine.Random.Range(1,nScriptsDep2+1);
        while(Fets2[i-1]) i = UnityEngine.Random.Range(1,nScriptsDep2+1);
        return "Script"+i.ToString();
    }

    public static void Fet(int n){
        if(Departament=="Departament1"){
            if(n==-1){
                ProvaSuperada1 = true;
            }
            else{
                Fets1[n-1] = true;
                Restants1--;

            }
        }else{
            if(n==-1){
                ProvaSuperada2 = true;
            }
            else{
                Fets2[n-1] = true;
                Restants2--;

            }
        }
    }

    public static void ComprovarProva(){
        if(Departament=="Departament1"){
            EsPotFerProva1 = Restants1 == 0;
        }else{
            EsPotFerProva2 = Restants2 == 0;

        }
    }

    public static bool EsPotFerProva(){
        if(Departament=="Departament1"){
            return EsPotFerProva1;
        }else{
            return EsPotFerProva2;


        }
    }

    public static VideoClip ObtenirGag(bool correcte){
        if(correcte){
            return Resources.Load<VideoClip>("Gags/"+Departament+"/Correcto");
        }else{
            return Resources.Load<VideoClip>("Gags/"+Departament+"/Err"+UnityEngine.Random.Range(1,3).ToString());
        }
    }
}