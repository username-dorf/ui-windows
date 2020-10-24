# ui-windows
 simple windows manager

 ## How to
 - Step 1
 Import package to your project
 - Step 2
 Add manager to your scene:
    - Open UIWindows  
    ![manger-window](.Images/components-uiwindows.jpg?raw=true "manager-window")      
    - Add manager object to scene  
    ![manger-on-scene](.Images/create-mager.jpg?raw=true "manager-on-scene")  
 - Step 3  
 Creating windows  
    - Name your future window  
    ![window-naming](.Images/enter-window-name.jpg?raw=true "window-naming")  
    - Create scripts  
    ![scripts-generation](.Images/generate-scripts.jpg?raw=true "scripts-generation")  
    - Wait till unity finish compile  
    ![wait](.Images/wait.jpg?raw=true "wait")  
    - Create window object  
    ![generate-window](.Images/generate-window.jpg?raw=true "generate-window")  
 ## Code  
  - Override and use
   ```cs
   public class <SCRIPT_NAME>: Window
   {

     protected override void OnEnable()
     {
         base.OnEnable();
     }

     protected override void OnDisable()
     {
         base.OnDisable();        
     }

     public override void Open<T>(T value)
     {
         base.Open();       
     }

     public override void Open()
     {
        base.Open();       
     }

   }
  ```
