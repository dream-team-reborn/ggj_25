using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
    }
    
    void Update()
    {
        if (!mainCamera)
            return;
        
        transform.eulerAngles = mainCamera.transform.eulerAngles;
    }
}
