using System;
using System.Collections.Generic;
using UIWindowsExtention;

using UnityEngine;

namespace UIWindowsManager
{
    public class UIWindows : MonoBehaviour
    {
        [Serializable]
        public struct WindowEntity
        {
            public WindowType type;
            public Window entity;

            public WindowEntity(WindowType type, Window entity)
            {
                this.type = type;
                this.entity = entity;
            }

        }

        public static UIWindows Instance;
        public List<WindowEntity> windows=new List<WindowEntity>();

        

        private void Awake()
        {
            Singleton();
        }

        /// <summary>
        /// Open window with special type 
        /// </summary>
        /// <param name="type">window type</param>
        /// <param name="value">given value</param>
        /// <typeparam name="T">any type if you overwrite it in window</typeparam>
        public static Window Open<T>(WindowType type, T value)
        {
            var window = Instance.windows.GetWindow(type);
            window.Open(value);
            return window;
        }

        public static Window Open(WindowType type)
        {
            var window = Instance.windows.GetWindow(type);
            window.Open();
            return window;
        }


        public static Window Close(WindowType type)
        {
            var window = Instance.windows.GetWindow(type);
            window.Close();
            return window;
        }

        public static bool IsAnyOpened(out List<Window> openedWindows)
        {
            return Instance.windows.IsAnyOpened(out openedWindows);
        }

        public static bool IsOpened(WindowType type)
        {
            return Instance.windows.IsOpened(type);
        }
        
        
        private void Singleton()
        { 
            if (Instance == null){
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            } else {
                Destroy(this.gameObject);
            }
        }

    }


    public static class WindowExtension
    {
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
        
        public static bool IsAnyOpened(this List<UIWindows.WindowEntity> list,out List<Window> openedWindows)
        {
            openedWindows=new List<Window>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].entity.gameObject.activeSelf)
                {
                    openedWindows.Add(list[i].entity);
                }
            }

            return openedWindows.Count > 0;
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
    }
}

