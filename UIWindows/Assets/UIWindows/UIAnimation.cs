using System;
using UIWindowsExtention;
using UnityEngine;

namespace UIWindowsManager
{
    public static class UIAnimation
    {
        #region WindowAnimation

        private const float AlphaMin = 0;
        private const float AlphaMax=0.65f;
        private const float AlphaOne = 1;

        private const float TimeInstant = 0; // momental
        private const float BackgroundAppearTime = 0.3f;
        private const float WindowAppearTime = 0.3f;
        
        private const float BackgroundHideTime = 0.2f;
        private const float WindowHIdeTime = 0.2f;

        private static readonly Vector2 WindowSmallSize = new Vector2(0.2f,0.2f);
        private static readonly Vector2 WindowNormalSize = new Vector2(1f,1f);

        
        
        
        public static void Appear(this Window window,Action onComplete)
        {
            window.gameObject.SetActive(true);
            window.Background.Appear();
            window.Body.Appear(onComplete);
        }

        public static void Hide(this Window window, Action onStart, Action onComplete)
        {
            window.Background.Hide();
            window.Body.Hide(onStart,()=>
            {
                onComplete?.Invoke();
                // may be laggy need to test
                window.gameObject.DoAfterEndOfFrame(()=>window.gameObject.SetActive(false));
                //window.gameObject.SetActive(false); //old version (onComplete not work for Close)
            });
        }

        /// <summary>
        /// Background appear tween animation, do any animation changes here
        /// </summary>
        /// <param name="background"></param>
        private static void Appear(this WindowBackground background)
        {
            //background.img.gameObject.SetActive(false);//disable on start (doublecheck)
            var imgColor = background.img.color;
            imgColor.a = AlphaMin;
            background.img.color = imgColor;
            
            background.img.gameObject.SetActive(true);
            background.img.DoFade(AlphaMax, BackgroundAppearTime);
        }

        /// <summary>
        /// Window Body appear tween animation, do any animation changes here.
        /// At end of animation fires action
        /// </summary>
        /// <param name="body"></param>
        /// <param name="onComplete"> Action that fires at the end</param>
        private static void Appear(this WindowBody body,Action onComplete)
        {
            //body.window.gameObject.SetActive(false);//disable on start (doublecheck)

           
            body.wRect.transform.localScale=WindowSmallSize;
            body.window.gameObject.SetActive(true);
            body.wRect.DoScale(WindowNormalSize, WindowAppearTime,()=>onComplete?.Invoke());
            //onComplete?.Invoke();
        }

        /// <summary>
        /// Window body hide tween animation, do any changes to hide animation here.
        /// At start fires action onStart
        /// </summary>
        /// <param name="body"></param>
        /// <param name="onStart"> Action that fires on start</param>
        private static void Hide(this WindowBody body,Action onStart,Action onComplete)
        {
            onStart?.Invoke();
            body.CGroup.DoFade(AlphaMin, WindowHIdeTime,()=>
            {
                onComplete?.Invoke();
                body.window.SetActive(false);
                body.CGroup.DoFade(AlphaOne, TimeInstant);
                
            });
        }

        /// <summary>
        /// Window background tween hide animation, do any changes to animation here
        /// </summary>
        /// <param name="background"></param>
        private static void Hide(this WindowBackground background )
        {
            background.img.DoFade(AlphaMin, BackgroundHideTime,()=>background.img.gameObject.SetActive(false));
        }
        #endregion
    }
}