using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DAO{

    //0 = n'existe pas , 1 = non prise , 2 = prise
    public static bool estPrise(int id)
    {
        bool output = false;

        if (PlayerPrefs.GetInt("Piece" + System.Convert.ToString(id)) == 0)
            PlayerPrefs.SetInt("Piece" + System.Convert.ToString(id), 1);

        if (PlayerPrefs.GetInt("Piece" + System.Convert.ToString(id)) == 2)
            output = true;

        return output;
    }

    public static void setPrise(int id,bool value)
    {
        PlayerPrefs.SetInt("Piece" + System.Convert.ToString(id),(value ? 2 : 1));
    }

    public static void setPieces(int nb)
    {
        PlayerPrefs.SetInt("nbPiece", nb);
    }

    public static int getPieces()
    {
        return PlayerPrefs.GetInt("nbPiece");
    }

	public static void setSoftCap(int nb){
		PlayerPrefs.SetInt ("softCap", nb);
	}

	public static int getSoftCap(){
		if (PlayerPrefs.GetInt("softCap") <= 0)
			PlayerPrefs.SetInt("softCap",1);
		
		return PlayerPrefs.GetInt ("softCap");
	}

    public static void setZone(int nb)
    {
        PlayerPrefs.SetInt("currentZone", nb);
    }

    public static int getZone()
    {
        return PlayerPrefs.GetInt("currentZone");
    }
}
