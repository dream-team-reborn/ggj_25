using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Camera mainCamera;
    private Animator animator;
    
    void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (!mainCamera)
            return;
        
        transform.eulerAngles = mainCamera.transform.eulerAngles;
        
        if (PlayerManager.Instance.transform.position.y > transform.position.y)
            animator.SetBool("isBack", true);
        else
            animator.SetBool("isBack", false);
    }
}
