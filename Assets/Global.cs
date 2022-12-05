using UnityEngine;
using UnityEngine.Video;
public static class Global{
    public static int dificultat = 2;
    public static int dia = 1;
    public static float suma = 0f;
    public static float mitjana = 0f;
    public static string Departament = "Departament2";
    public static bool estaDesbloquejat = false;
    public static int nScriptsDep1 = 3;
    public static bool[] Fets1 = {false,false,false};
    public static int Restants1 = nScriptsDep1;
    public static int nScriptsDep2 = 1;
    public static bool[] Fets2 = {false,false,false,false};
    public static int Restants2 = nScriptsDep2;

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
            Fets1[n-1] = true;
            Restants1--;
        }else{
            Fets2[n-1] = true;
            Restants2--;
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