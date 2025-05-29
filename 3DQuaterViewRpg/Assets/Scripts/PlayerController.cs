using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private PlayerInputAciton inputAction;

    // 진행 방향 확인
    private Vector3 moveDir;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        inputAction = new PlayerInputAciton();
    }
    private void OnEnable()
    {
        inputAction.Player.Move.performed += OnClick;
        inputAction.Enable();
    }
    private void OnDisable()
    {
        inputAction.Player.Move.performed -= OnClick;
        inputAction.Disable();
    }
    private void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            agent.SetDestination(hit.point);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, moveDir * 2f);
    }
}
