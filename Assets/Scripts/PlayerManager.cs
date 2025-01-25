using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Image player;
    [SerializeField] private float speed = 0.5f;

    private Transform playerTrns;
    
    public void Start()
    {
        InputSystem.InputSystem.Instance.OnInput += OnInput;
        playerTrns = player.transform;
    }
    
    private void OnInput(float obj)
    {
        playerTrns.Translate(Vector3.up * (speed * Time.deltaTime));
    }
}
