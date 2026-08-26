using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup), typeof(Animator))]
public class PopUp : MonoBehaviour
{
     [SerializeField] private float fadeInDuration = 0.1f;
     [SerializeField] public float apperanceDelay = 0.01f;
     
     private const float MINIUM_DELAY = 0.01f;
     public bool fade = true;
     public bool instantClose = false;
     public float FadeInDuration => fadeInDuration;
     
     private Animator animator;
     private CanvasGroup canvasGroup;
     public CustomButon closeButton;

     public Action OnShowAction;
     public Action<EPopUpResult> OnCloseAction;
     protected EPopUpResult result;
     private Action pendingOnShow;
     private Action<EPopUpResult> pendingOnClose;

     public delegate void PopUpEvents(PopUp popUp);

     public static event PopUpEvents OnOpenPopup;
     public static event PopUpEvents OnClosePopup;
     public static event PopUpEvents OnBeforeCloseAction;

     protected virtual void Awake()
     {
          animator = GetComponent<Animator>();
          canvasGroup = GetComponent<CanvasGroup>();

          if (closeButton != null)
          {
               closeButton.onClick.AddListener(Close);
          }
     }

     public void DelayedShow()
     {
          Show<PopUp>(pendingOnShow, pendingOnClose);
     }

     public void InitDelayedShow(Action onShow = null, Action<EPopUpResult> onClose = null)
     {
          pendingOnShow = onShow;
          pendingOnClose = onClose;

          bool hasSignificantDelay = apperanceDelay > MINIUM_DELAY;

          if (hasSignificantDelay)
          {
               if (canvasGroup != null)
               {
                    canvasGroup.alpha = 0;
                    canvasGroup.interactable = false;
               }
               
               Invoke("DelayedShow", apperanceDelay);
          }
          else
          {
               DelayedShow();
          }
     }

     public void Show<T>(Action onShow = null, Action<EPopUpResult> onClose = null)
     {
          if (onShow != null)
          {
               OnShowAction = onShow;
          }

          if (onClose != null)
          {
               OnCloseAction = onClose;
          }

          if (canvasGroup != null)
          {
               canvasGroup.alpha = 1;
               canvasGroup.interactable = true;
          }
          
          OnOpenPopup?.Invoke(this);
          PlayShowAnimation();
     }

     private void PlayShowAnimation()
     {
          if (animator != null)
          {
               animator.Play("popup_show");
          }
     }

     public virtual void ShowAnimationSound()
     {
          SoundBase.instance.PlaySound(SoundBase.instance.swish[0]);
     }

     public virtual void AfterShowAnimation()
     {
          OnShowAction?.Invoke();
     }

     public virtual void CloseAnimationSound()
     {
          SoundBase.instance.PlayDelayed(SoundBase.instance.swish[1],0.01f);
     }

     public virtual void Close()
     {
          if (this == null) return;

          if (instantClose)
          {
               CloseInStant();
               return;
          }
          
          CancelInvoke();

          if (closeButton)
          {
               closeButton.interactable = false;
          }

          if (canvasGroup != null)
          {
               canvasGroup.interactable = false;
          }
          
          OnBeforeCloseAction?.Invoke(this);

          if (animator != null)
          {
               animator.Play("popup_hide");
          }
     }

     public virtual void AfterHideAnimation()
     {
          OnClosePopup?.Invoke(this);
          OnCloseAction?.Invoke(result);
          Destroy(gameObject,0.5f);
     }

     public void Show()
     {
          if (canvasGroup != null)
          {
               canvasGroup.alpha = 1;
               canvasGroup.interactable = true;
          }
     }

     public virtual void Hide()
     {
          if(canvasGroup != null)
          {
              canvasGroup.interactable = false;
              canvasGroup.DOFade(0, 0.5f);
          }
     }

     public void CloseDelay()
     {
          Invoke(nameof(Close), 0.5f);
          if (canvasGroup != null)
          {
               canvasGroup.interactable = false;
               canvasGroup.DOFade(0, 0.5f);
          }
     }

     protected void StopInteration()
     {
          if (canvasGroup != null)
          {
               canvasGroup.interactable = false;
          }
     }

     public virtual void CloseInStant()
     {
          if (this == null) return;
          
          CancelInvoke();
          DOTween.Kill(gameObject);

          if (closeButton)
          {
               closeButton.interactable = false;
          }

          if (canvasGroup != null)
          {
               canvasGroup.interactable = false;
               canvasGroup.alpha = 0;
          }
          
          OnBeforeCloseAction?.Invoke(this);
          OnClosePopup?.Invoke(this);
          OnCloseAction?.Invoke(result);
          Destroy(gameObject);
     }

     private void OnDisable()
     {
          DOTween.Kill(gameObject);
          CancelInvoke();
     }
}
