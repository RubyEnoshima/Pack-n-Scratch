using UnityEngine;
public static class Global{
    public static int dificultat = 2;
    public static int dia = 1;
    public static float suma = 0f;
    public static float mitjana = 0f;
    public static string Departament = "Departament1";
    public static bool estaDesbloquejat = false;
    public static int nScriptsDep1 = 3;
    public static bool[] Fets1 = {false,false,false};
    public static int nScriptsDep2 = 4;
    public static bool[] Fets2 = {false,false,false,false};

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
        }else{
            Fets2[n-1] = true;
        }
    }
}