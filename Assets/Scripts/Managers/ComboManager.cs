using System;
using UnityEngine;

public class ComboManager : SingletonBehaviour<ComboManager>
{
     public Action<int> OnComboChanged;

     [SerializeField] private float comboTimeout = 2.0f;
     
     private int currentCombo = 0;
     private float comboTimer = 0f;
     
     public int CurrentCombo => currentCombo;

     public override void Awake()
     {
          base.Awake();
     }

     private void Update()
     {
          if (comboTimer > 0)
          {
               comboTimer -= Time.deltaTime;
               if (comboTimer <= 0)
               {
                    ResetCombo();
               }
          }
     }

     public void AddCombo()
     {
          currentCombo++;
          comboTimer = comboTimeout;
          OnComboChanged?.Invoke(currentCombo);
     }

     public void ResetComboTimer()
     {
          if (currentCombo > 0)
          {
               comboTimer = comboTimeout;
          }
     }

     public void ResetCombo()
     {
          if (currentCombo > 0)
          {
               currentCombo = 0;
               comboTimer = 0f;
               OnComboChanged?.Invoke(currentCombo);
          }
     }
}
