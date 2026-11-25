using UnityEngine;

public class DevOnly : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(Debug.isDebugBuild);
    }
}