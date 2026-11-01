using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform alvo;
    [Tooltip("Valores maiores deixam o movimento mais suave")]
    public float suavidade = 0.01f;
 
    private Vector3 offset;
 
    void Start()
    {
        offset = transform.position - alvo.position;
    }
 
    void LateUpdate()
    {
        Vector3 destino = alvo.position + offset;
        transform.position = Vector3.Lerp(transform.position, destino, Time.deltaTime / suavidade);
    }
}
