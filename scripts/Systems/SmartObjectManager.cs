using System.Diagnostics;

namespace Incorgnito.scripts.Systems;
using System.Collections.Generic;
using Godot;

public partial class SmartObjectManager: Node
{
    //allows for list of objects to change dynamically
    public List<SmartObject> RegisteredObjects { get; private set; } = new List<SmartObject>();
    
    //currently singleton - may need to change later for different states? scnenes?
    // private static SmartObjectManager _instance;
    public static SmartObjectManager Instance { get; private set; }
    public override void _Ready()
    {
        if (Instance != null)
        {
            GD.PrintErr($"Error: Trying to create second SmartObject on {Name}");
            QueueFree();
            return;
        }
        
        Instance = this;
        // //persist across scenes
        SetProcess(true);
    }

    public void RegisterSmartObject(SmartObject toRegister)
    {
        RegisteredObjects.Add(toRegister); 
        GD.Print(toRegister.DisplayName);
        
    }

    public void DeregisterSmartObject(SmartObject toDeregister)
    {
        RegisteredObjects.Remove(toDeregister);
    }
}