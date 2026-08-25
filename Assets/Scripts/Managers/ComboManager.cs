using System;
using UnityEngine;

public class ComboManager : SingletonBehaviour<ComboManager>
{
     public Action<int> OnComboChanged;

     [SerializeField] private float resetComboAfterMoves = 3;
     
     private int currentCombo = 0;
     private int movesSinceLastCombo = 0;
     
     public int CurrentCombo => currentCombo;

     public override void Awake()
     {
          base.Awake();
     }

     
     public void OnShapePlaced(bool wasLineCleared)
     {
          movesSinceLastCombo++;
          
          if (wasLineCleared)
          {
               movesSinceLastCombo = 0;
          }
          else
          {
               movesSinceLastCombo++;

               if (movesSinceLastCombo >= resetComboAfterMoves)
               {
                    ResetCombo();
               }
          }
     }
   

     public void ResetCombo()
     {
          if (currentCombo > 0)
          {
               currentCombo = 0;
               movesSinceLastCombo = 0;
               
               OnComboChanged?.Invoke(currentCombo);
          }
     }
}
