using System;
using System.Collections;
using System.Collections.Generic;
using UIWindowsManager;
using UnityEngine;
using UnityEngine.UI;


public class Usage : MonoBehaviour
{
   public WindowType type;
   [SerializeField] private Button openButton;
   [SerializeField] private Button closeButton;

   private void OnEnable()
   {
      openButton.onClick.AddListener(Open);
      closeButton.onClick.AddListener(Close);
   }

   private void OnDisable()
   {
      openButton.onClick.RemoveListener(Open);
      closeButton.onClick.RemoveListener(Close);
   }

   private void Open()
   {
      UIWindows.Open(WindowType.Testing).OnComplete(()=>Debug.Log("Opened"));
   }

   private void Close()
   {
      UIWindows.Close(WindowType.Testing).OnComplete(()=>Debug.Log("Closed"));
   }
}
