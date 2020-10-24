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
        public static void Open<T>(WindowType type, T value)
        {
            Instance.windows.GetWindow(type).Open(value);
        }

        public static void Open(WindowType type)
        {
            Instance.windows.GetWindow(type).Open();
        }


        public static void Close(WindowType type)
        {
            Instance.windows.GetWindow(type).Close();
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

