using UnityEngine;

/// <summary>
/// He thong cac setting co ban cua game
/// </summary>
[CreateAssetMenu(fileName = "GameSetting", menuName = "Scriptable Objects/GameSettings")]
public class GameSettings : SettingBase
{
     [Header("On Start Game")]
     public int startCoins;

     [Header("Monetization")]
     public int coinForAd = 25;
     public bool enableAds = true;
     public bool enableInApps = true;
     public bool enableLuckySpin = true;
     public bool enablePreFailPopUp = true;

     [Header("GDPR Settings")] 
     public string privacyPolicyUrl;
     public bool skipConsentPopup;

     [Header("Timed mode")]
     public bool enableTimeMode = false;
     public int globalTimeModeSeconds = 60; // Default time value for timed mode
     public int continueTimerBonus = 30;

     [Header("GamePlay")]
     public int scorePerLine = 10;
     public bool enablePool;
     public int resetComboAfterMoves = 3;

     public int continuePrice = 15;
     public int failTimerStart = 5;

     [Header("Map Settings")] 
     public EMapType mapType = EMapType.Tiled;

     public int maxLevelRows = 8;
     public int maxLevelColumns = 8;

}
