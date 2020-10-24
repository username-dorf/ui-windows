using System;
using System.Collections;
using System.Collections.Generic;
using UIWindow.Tools;
using UIWindowsManager;
using UnityEditor;
using UnityEngine;

public class EditorDisplay : EditorWindow
{
    private const string WindowNameEditorPrefKey = "WindowName";
    private string windowName="default";

    
    
    [MenuItem("Component/UIWindows")]
    public static void Show()
    {
        GetWindow<EditorDisplay>("UIWindows");
    }

    private void OnGUI()
    {
        
        
        var windowManager = GameObject.FindObjectOfType<UIWindows>();
        if (windowManager == null)
        {
            if (GUILayout.Button("Create window manager"))
            {
                UIWindowTools.CreateWindowsManager();
            }
            GUILayout.Space(40);
        }
      
        
        GUILayout.Label("Enter window name");
        windowName = EditorGUILayout.TextField("Name", windowName);

        if (GUILayout.Button("Generate Window Script"))
        {
            
            UIWindowTools.CreateWindowScript(windowName);
        }
        GUILayout.Label("Press after unity finish compiling");
        if (GUILayout.Button("Generate Window"))
        {
            
            UIWindowTools.CreateWindowObject(windowName);
            
        }

    }

    
}
