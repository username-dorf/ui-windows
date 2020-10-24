using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UIWindowsManager;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;


namespace UIWindow.Tools
{
    public static class UIWindowTools
    {

        private static readonly string FileNamePlaceholder = "<SCRIPT_NAME>"; 
      
        private static GameObject WindowPrefab
        {
            get
            {
                #if UNITY_EDITOR
                return (GameObject)AssetDatabase.LoadAssetAtPath("Assets/UIWindows/Resources/WWindow.prefab",
                    typeof(GameObject));
                #else
                return null;
                #endif
            }
        }
        private static GameObject WindowsManagerPrefab
        {
            get
            {
                #if UNITY_EDITOR
                return (GameObject)AssetDatabase.LoadAssetAtPath("Assets/UIWindows/Resources/UIWindows.prefab",
                    typeof(GameObject));
                #else
                return null;
                #endif
            }
        }
   
        
        private static readonly string LocalPath = $"/UIWindows/Windows/";


        public static void CreateWindowObject(string windowName)
        {
            var windowsManager = Object.FindObjectOfType<UIWindowsManager.UIWindows>();
            var windowCanvasTransform = windowsManager.GetComponentsInChildren<Canvas>()[0].transform;
            var window = UnityEngine.Object.Instantiate(WindowPrefab, windowCanvasTransform);
            window.name = GetScriptName(windowName);

            window.AddComponent(Type.GetType(GetScriptName(windowName)));
            var windowEntity = window.GetComponent((GetScriptName(windowName))) as UIWindowsManager.Window;
            windowEntity.type = (UIWindowsManager.WindowType) Enum.Parse(typeof(UIWindowsManager.WindowType), windowName);
            var windowType= windowEntity.type;
            
            windowEntity.AutoAssign();
            windowsManager.windows.Add(new UIWindows.WindowEntity(windowType,windowEntity));
        }
        public static void CreateWindowScript(string windowName)
        {
            CreateWindowType(windowName);
            var filePath = $"{Application.dataPath}{LocalPath}";
            File.WriteAllText(filePath+GetFileName(windowName),GetFileTemplate(GetScriptName(windowName)));
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
        }

        public static void CreateWindowsManager()
        {
            var windowManager = UnityEngine.Object.Instantiate(WindowsManagerPrefab);
            windowManager.name = "UIWindows";
            var canvas=windowManager.GetComponentInChildren<Canvas>();
            canvas.worldCamera=Camera.main;
            
        }

        private static void CreateWindowType(string windowName)
        {
            string enumName = windowName;
            string[] enumEntries = GetExistingEnum(enumName);
            string filePathAndName = "Assets/UIWindows/Types.cs"; //The folder Scripts/Enums/ is expected to exist
 
            using ( StreamWriter streamWriter = new StreamWriter( filePathAndName ) )
            {
                streamWriter.WriteLine( "namespace UIWindowsManager{");
                streamWriter.WriteLine( "public enum WindowType");
                streamWriter.WriteLine( "{" );
                for( int i = 0; i < enumEntries.Length; i++ )
                {
                    streamWriter.WriteLine( "\t" + enumEntries[i] + "," );
                }
                streamWriter.WriteLine( "}" );
                streamWriter.WriteLine( "}" );
            }
            //AssetDatabase.Refresh();
        }

        private static string[] GetExistingEnum(string additional)
        {
            
            var values = Enum.GetValues(typeof(WindowType));
            List<string> toReturn=new List<string>();
            
            foreach (var value in values)
            {
                toReturn.Add(((WindowType) value).ToString());
            }
            toReturn.Add(additional);
            return toReturn.Distinct().ToArray();
        }
        private static string GetScriptName(string windowName)
        {
            return $"W{windowName}";
        }
        private static string GetFileName(string windowName)
        {
            return $"W{windowName}.cs";
        }
        
        private static string GetFileTemplate(string scriptName)
        {
            var template=File.ReadAllText($"{Application.dataPath}/UIWIndows/Resources/template.txt");
            return template.Replace(FileNamePlaceholder, scriptName);
        }

    }
}
