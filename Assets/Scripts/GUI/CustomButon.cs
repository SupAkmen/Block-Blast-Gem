using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class CustomButon : Button
{
     public AudioClip overrideClickSound;
     public RuntimeAnimatorController overriderAnimatorController;
     private bool isClicked;
     private readonly float cooldownTime = 0.5f;
     public new ButtonClickedEvent onClick;
     private new Animator animator;

     private static bool blockInput;

     protected override void OnEnable()
     {
          base.OnEnable();
          animator = GetComponent<Animator>();
          if (overriderAnimatorController != null)
          {
               animator.runtimeAnimatorController = overriderAnimatorController;
          }
          
          isClicked = false;
     }

     public override void OnPointerClick(PointerEventData eventData)
     {
          if (blockInput || isClicked)
          {
               return;
          }
     }

}
