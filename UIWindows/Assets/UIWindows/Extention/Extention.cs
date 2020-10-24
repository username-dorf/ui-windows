using System;
using System.Collections;
using System.Collections.Generic;
using UIWindowsManager;
using UnityEngine;
using UnityEngine.UI;

namespace UIWindowsExtention
{
    
    public static class Extention 
    {
        #region Fade
        public static void DoFade(this Image image,float alpha, float time,Action onComplete=null)
        {
            ExtensionMethodHelper.Instance.StartCoroutine(DoFadeRoutine(image, alpha, time, onComplete));
        }
        public static void DoFade(this CanvasGroup canvasGroup,float alpha, float time,Action onComplete=null)
        {
            ExtensionMethodHelper.Instance.StartCoroutine(DoFadeRoutine(canvasGroup, alpha, time, onComplete));
        }
        
      

        private static IEnumerator DoFadeRoutine(Image image, float alpha, float time,Action onComplete=null)
        {
            var imageColor = image.color;
            var currentAlphaValue = imageColor.a;
            var elapsedTime = 0f;
            
            while (elapsedTime<time)
            {
                imageColor.a = Mathf.Lerp(currentAlphaValue,alpha,(elapsedTime/time));
                image.color = imageColor;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            imageColor.a = alpha;
            image.color = imageColor;
            onComplete?.Invoke();
        }
        
        private static IEnumerator DoFadeRoutine(CanvasGroup canvasGroup, float alpha, float time,Action onComplete=null)
        {
            var currentAlphaValue = canvasGroup.alpha;
            var elapsedTime = 0f;
            
            while (elapsedTime<time)
            {
                canvasGroup.alpha = Mathf.Lerp(currentAlphaValue,alpha,(elapsedTime/time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = alpha;
            onComplete?.Invoke();
        }

        
        #endregion

        #region Scale

        public static void DoScale(this RectTransform rectTransform,Vector2 value, float time,Action onComplete=null)
        {
            ExtensionMethodHelper.Instance.StartCoroutine(DoScaleRoutine(rectTransform, value, time, onComplete));
        }
        
        private static IEnumerator DoScaleRoutine(RectTransform rectTransform,Vector2 value,float time,Action onComplete=null )
        {
            var currentSize = rectTransform.transform.localScale;
            var elapsedTime = 0f;
            while (elapsedTime<time)
            {
                rectTransform.transform.localScale = Vector2.Lerp(currentSize,value,(elapsedTime/time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            rectTransform.transform.localScale = value;
            onComplete?.Invoke();
        }
        #endregion

        #region GetComponentsWithTag

        public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag)where T:Component{
            Transform t = parent.transform;
            foreach(Transform tr in t)
            {
                if(tr.CompareTag(tag))
                {
                    return tr.GetComponent<T>();
                }
            }
            return null;
        }

        #endregion
        
        #region Window

        public static Window GetWindow(this List<UIWindows.WindowEntity> list, WindowType type)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].type == type)
                {
                    if (list[i].entity.type != type)
                    {
                        Debug.LogError($"Finded window with type {type}. But actual window not match given type. " +
                                       $"Window type is {list[i].entity.type}",list[i].entity.gameObject);
                    }
                    return list[i].entity;
                }
            }

            Debug.LogError($"No window with type {type} in list");
            return null;
        }

        public static bool IsAnyOpened(this List<UIWindows.WindowEntity> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].entity.gameObject.activeSelf)
                {
                    return true;
                }
            }

            return false;
        }
        public static bool IsAnyOpened(this List<UIWindows.WindowEntity> list,out Window opened)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].entity.gameObject.activeSelf)
                {
                    opened = list[i].entity;
                    return true;
                }
            }
            opened = null;
            return false;
        }
    
        public static bool IsOpened(this List<UIWindows.WindowEntity> list,WindowType type)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].entity.gameObject.activeSelf && list[i].type==type)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
    public class ExtensionMethodHelper : MonoBehaviour
    {
        private static ExtensionMethodHelper _Instance;

        public static ExtensionMethodHelper Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new GameObject("ExtensionMethodHelper").AddComponent<ExtensionMethodHelper>();
                }

                return _Instance;
            }
        }

        public static void Clear()
        {
            var ob = GameObject.Find("ExtensionMethodHelper");
            Destroy(ob);
        }
        
        
    }

   
}
