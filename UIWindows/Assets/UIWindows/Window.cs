﻿using System;
using System.Collections;
using UIWindowsExtention;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



namespace UIWindowsManager
{
    public class Window : MonoBehaviour
    {
        #region Editor
        [Serializable]
        private class WindowComponents
        {
            [SerializeField] private WindowBackground _background;
            [SerializeField] private WindowBody _body;
            [SerializeField] private Button _closeButton;

            public WindowBackground Background => _background;
            public WindowBody Body => _body;
            public Button CloseButton
            {
                get => _closeButton;
                set => _closeButton = value;
            }


            public Image BackgroundImage
            {
                get => _background.img;
                set => _background.img = value;
            }

            public GameObject BodyWindow
            {
                get => _body.window;
                set => _body.window = value;
            }
        }
        #endregion

        #region ConstData
        private const string WindowBackgroundTag = "WindowBackground";
        private const string WindowBodyTag = "WindowBody";
        private const string WindowCloseButtonTag = "WindowCloseButton";
        

        #endregion
        
        public WindowType type;
        [SerializeField] private WindowComponents _components=new WindowComponents();
        public UnityAction OnCloseClickAdditional;
        
        public WindowBackground Background => _components.Background;
        public WindowBody Body => _components.Body;
        public Button CloseButton => _components.CloseButton;


        public delegate void WindowAction(WindowType type);
        
        public static event WindowAction OnOpened;
        public static event WindowAction OnClosed;


       
        protected virtual void OnEnable()
        {
            if (CloseButton != null)
            {
                CloseButton.onClick.AddListener(Close);
                if (OnCloseClickAdditional != null)
                {
                    CloseButton.onClick.AddListener(OnCloseClickAdditional);
                }
            }
        }

        protected virtual void OnDisable()
        {
            if (CloseButton != null)
            {
                CloseButton.onClick.RemoveListener(Close);
                if (OnCloseClickAdditional != null)
                {
                    CloseButton.onClick.RemoveListener(OnCloseClickAdditional);
                }
            }
        }

        public void AutoAssign()
        {
            _components.BackgroundImage = gameObject.FindComponentInChildWithTag<Image>(WindowBackgroundTag);
            _components.BodyWindow = gameObject.FindComponentInChildWithTag<RectTransform>(WindowBodyTag).gameObject;
            _components.CloseButton = _components.BodyWindow.FindComponentInChildWithTag<Button>(WindowCloseButtonTag);
        }

        [SerializeField] private bool isAnimated;

        private bool IsAnimated
        {
            get
            {
                return isAnimated;
            }
            set
            {
                if (onCompleteRoutine != null && value)
                {
                    StopCoroutine(onCompleteRoutine);
                }

                isAnimated = value;
            }
        }

        private Coroutine onCompleteRoutine;
        
        public virtual Window Open()
        {
            IsAnimated = true;
            this.Appear(()=>
            {
                OnOpened?.Invoke(type);
                IsAnimated = false;
            });
            return this;
        }

        public virtual Window Open<T>(T value)
        {
            IsAnimated = true;
            this.Appear(()=>
            {
                OnOpened?.Invoke(type);
                IsAnimated = false;
            });
            return this;
        }
        
        public virtual void Close()
        {
            IsAnimated = true;
            this.Hide(()=>
            {
                OnClosed?.Invoke(type);
            }, () =>
            {
                IsAnimated = false;
            });
        }

        public virtual Window OnComplete(UnityAction onComplete=null)
        {
            if(gameObject.activeSelf) onCompleteRoutine=StartCoroutine(WaitRoutine(onComplete));
            return this;
        }

        private IEnumerator WaitRoutine(UnityAction onComplete)
        {
            yield return new WaitUntil(()=>!IsAnimated);
            onComplete?.Invoke();
            onCompleteRoutine = null;
            
        }
    }

    #region Structs

    [Serializable]
    public struct WindowBackground
    {
        public Image img;
    }

    [Serializable]
    public struct WindowBody
    {
        public GameObject window;
        private RectTransform rect;
        public RectTransform wRect
        {
            get
            {
                if (rect == null)
                {
                    rect =window.GetComponent<RectTransform>();
                }
                return rect;
            }
        }

        private CanvasGroup canvasGroup;
        public CanvasGroup CGroup
        {
            get
            {
                if (canvasGroup == null)
                {
                    canvasGroup = window.GetComponent<CanvasGroup>();
                }
                return canvasGroup;
            }
        }
    }

    #endregion
}