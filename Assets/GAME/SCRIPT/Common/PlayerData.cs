using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Данные об игроке - настройки, рекорды, выбранный голубь
/// </summary>
[Serializable]
public class PlayerData : MonoBehaviour {
    public float Settings_SFX;          //1 - вкл, 0.0001 - выкл
    public float Settings_Music;        //1 - вкл, 0.0001 - выкл
    public int Settings_languageID;
    public int Record_Scores;
    public int Record_Meters;
    public int Coins;
    public int SelectedPigeonID;        //0 - default

    public void LoadData() {
        if (PlayerPrefs.HasKey("SFX")) Settings_SFX = PlayerPrefs.GetFloat("SFX"); else Settings_SFX = 1;         
        if (PlayerPrefs.HasKey("Music")) Settings_Music = PlayerPrefs.GetFloat("Music"); else Settings_Music = 1;
        if (PlayerPrefs.HasKey("Language")) Settings_languageID = PlayerPrefs.GetInt("Language"); else Settings_languageID = 0;

        if (PlayerPrefs.HasKey("ScoresRecord")) Record_Scores = PlayerPrefs.GetInt("ScoresRecord"); else Record_Scores = 0;
        if (PlayerPrefs.HasKey("MetersRecord")) Record_Meters = PlayerPrefs.GetInt("MetersRecord"); else Record_Meters = 0;
        if (PlayerPrefs.HasKey("Coins")) Coins = PlayerPrefs.GetInt("Coins"); else Coins = 0;
        if (PlayerPrefs.HasKey("SelectedPigeonID")) SelectedPigeonID = PlayerPrefs.GetInt("SelectedPigeonID"); else SelectedPigeonID = 0;
    }

    public void SaveData() {
        PlayerPrefs.SetFloat("SFX", Settings_SFX);
        PlayerPrefs.SetFloat("Music", Settings_Music);
        PlayerPrefs.SetInt("Language", Settings_languageID);

        PlayerPrefs.SetInt("ScoresRecord", Record_Scores);
        PlayerPrefs.SetInt("MetersRecord", Record_Meters);
        PlayerPrefs.SetInt("Coins", Coins);
        PlayerPrefs.SetInt("SelectedPigeonID", SelectedPigeonID);

        PlayerPrefs.Save();
    }

    public void ModData() {
        PlayerPrefs.SetFloat("SFX", 1);
        PlayerPrefs.SetFloat("Music", 1);
        PlayerPrefs.SetInt("Language", 0);

        PlayerPrefs.SetInt("ScoresRecord", 10000);
        PlayerPrefs.SetInt("MetersRecord", 10000);
        PlayerPrefs.SetInt("Coins", 777);
        PlayerPrefs.SetInt("SelectedPigeonID", 0);

        PlayerPrefs.Save();
    }

    public void ZeroData() {
        PlayerPrefs.SetFloat("SFX", 1);
        PlayerPrefs.SetFloat("Music", 1);
        PlayerPrefs.SetInt("Language", 0);

        PlayerPrefs.SetInt("ScoresRecord", 0);
        PlayerPrefs.SetInt("MetersRecord", 0);
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("SelectedPigeonID", 0);

        PlayerPrefs.Save();
    }

}
