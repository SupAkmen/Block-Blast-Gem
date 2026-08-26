using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
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

          if (transition != Transition.Animation)
          {
               Pressed();
          }

          isClicked = true;
          
          //Start cooldown
          if (gameObject.activeInHierarchy)
          {
               StartCoroutine(Cooldown());
          }
          
          base.OnPointerClick(eventData);
     }

     public void Pressed()
     {
          if (blockInput)
          {
               return;
          }
          
          ExecutedEvent();
     }

     private void ExecutedEvent()
     {
          onClick?.Invoke();
          base.onClick?.Invoke();
     }

     IEnumerator Cooldown()
     {
          yield return new WaitForSeconds(cooldownTime);
          isClicked = false;
     }

}
