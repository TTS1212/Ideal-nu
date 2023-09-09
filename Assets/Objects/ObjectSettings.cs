using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(menuName = "NPCs and Objects/Object Settings")]

public class ObjectSettings : ScriptableObject
{
    public enum Type
    {
        NPC,
        Item
    }

    public Type type;
    
    [Header("Prompt Settings")]
    public bool CanCooldown;
    public float MaxTimeToPressDown, WaitAfterCooldown;

    [Header("Item Settings (Don't use if target is not an item.)")]
    public bool DialogueNeeded;

    public AudioClip[] clips;
}
