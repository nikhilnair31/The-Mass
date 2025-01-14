using UnityEngine;

public class Test_Vent : MonoBehaviour 
{
    [SerializeField] private bool unlockVent;

    #if UNITY_EDITOR
    private void OnValidate() {
        if (unlockVent) {
            Manager_Game.Instance.UnlockVent();
            unlockVent = false;
        }
    }
    #endif
}