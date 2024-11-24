using UnityEngine;

public abstract class Bomb : MonoBehaviour {
    public float CoolDown;
    public abstract void InitializeParent(Transform parent);
    public abstract void Drop();
}