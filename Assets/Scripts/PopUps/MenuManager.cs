using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PopUps
{
    public class MenuManager : SingletonBehaviour<MenuManager>
    {
        public Fader fader;
        public List<PopUp> popUpStack = new List<PopUp>();

        [SerializeField] private Canvas canvas;

        private static bool _isAdShowing = false;

        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            PopUp.OnClosePopup += ClosePopUp;
            PopUp.OnBeforeCloseAction += OnBeforeCloseAction;
            SceneManager.activeSceneChanged += OnSceneLoaded;
        }

        private void OnBeforeCloseAction(PopUp popUp)
        {
            if (fader != null && popUpStack.Count == 0)
            {
                fader.FadeOut();
            }
        }

        private void OnSceneLoaded(Scene scene, Scene scene1)
        {
            if (canvas == null && this != null)
            {
                canvas= GetComponent<Canvas>();
            }
            
            canvas.worldCamera = Camera.main;
        }
        
        private void OnDisable()
        {
            PopUp.OnClosePopup -= ClosePopUp;
            PopUp.OnBeforeCloseAction -= OnBeforeCloseAction;
            SceneManager.activeSceneChanged -= OnSceneLoaded;
        }
        
        

        public T ShowPopUp<T>(Action onShow = null, Action<EPopUpResult> onClose = null) where T : PopUp
        {
            // Ktra xem popup nay co dang mo hay ko
            if (popUpStack.OfType<T>().Any())
            {
                return popUpStack.OfType<T>().First();
            }
            
            return (T)ShowPopUp("PopUps/" +  typeof(T).Name, onShow, onClose);
        }

        public PopUp ShowPopUp(string pathWithType, Action onShow = null, Action<EPopUpResult> onClose = null)
        {

            if (popUpStack.Any(p => p.GetType().Name == pathWithType.Split('/').Last()))
            {
                return popUpStack.First(p => p.GetType().Name == pathWithType.Split('/').Last());
            }
            
            var popUpPrefab = Resources.Load<PopUp>(pathWithType);
            if (popUpPrefab == null)
            {
                return null;
            }
            
            return ShowPopUp(popUpPrefab, onShow, onClose);
        }

        public PopUp ShowPopUp(PopUp popUpPrefab, Action onShow = null, Action<EPopUpResult> onClose = null)
        {
            var popUp = Instantiate(popUpPrefab, transform);

            if (popUpStack.Count > 0)
            {
                popUpStack.Last().Hide();
            }
            
            popUpStack.Add(popUp);

            if (fader != null && popUpStack.Count > 0 && popUp.fade)
            {
                fader.FadeIn(.997f,popUp.FadeInDuration);
            }
            
            popUp.InitDelayedShow(onShow, onClose);
            
            return popUp;
        }

        public PopUp ShowPopUpDelayed(PopUp popUpPrefab, Action onShow = null, Action<EPopUpResult> onClose = null)
        {
            float delay = popUpPrefab.apperanceDelay;

            if (fader != null && popUpPrefab.fade)
            {
                fader.FadeIn(0.997f,popUpPrefab.FadeInDuration);
            }
            
            StartCoroutine(ShowPopUpDelayed(popUpPrefab,delay, onShow, onClose));

            return null;
        }

        private IEnumerator ShowPopUpDelayed(PopUp popUpPrefab, float delay, Action onShow = null, Action<EPopUpResult> onClose = null)
        {
            yield return new WaitForSeconds(delay);
            
            var popUp = Instantiate(popUpPrefab, transform);

            if (popUpStack.Count > 0)
            {
                popUpStack.Last().Hide();
            }
            
            popUpStack.Add(popUp);
            
            popUp.InitDelayedShow(onShow, onClose);
        }

        private void ClosePopUp(PopUp closePopUp)
        {
            if (popUpStack.Count > 0)
            {
                popUpStack.Remove(closePopUp);

                if (popUpStack.Count > 0)
                {
                    var popUp = popUpStack.Last();
                    popUp.Show();
                }
            }

            if (fader != null && popUpStack.Count == 0 && fader.IsFaded())
            {
                fader.FadeOut();
            }
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    }
}