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

        public bool IsOpened => windows.IsAnyOpened();

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

}

