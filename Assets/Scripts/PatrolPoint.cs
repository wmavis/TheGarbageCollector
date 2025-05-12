using System.Security.Cryptography;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    float gizmoRadius = 0.2f;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }
}
