using UnityEngine;

public class Test_Vent : MonoBehaviour 
{
    [SerializeField] private bool unlockVentTrigger;
    [SerializeField] private bool isVentUnlocked;

    #if UNITY_EDITOR
    private void OnValidate() {
        if (unlockVentTrigger) {
            Manager_Game.Instance.UnlockVent();
            unlockVentTrigger = false;
        }
    }
    #endif

    private void Start() {
        if (isVentUnlocked) {
            Manager_Game.Instance.UnlockVent();
        }
    }
}