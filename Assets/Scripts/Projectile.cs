using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float velocidade = 15f;
    public float tempoDeVida = 3f;
    public int dano = 1;

    private Rigidbody rb;
    private GameObject dono;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void Disparar(Vector3 direcao, GameObject atirador)
    {
        dono = atirador;
        rb.linearVelocity = direcao * velocidade;
        Destroy(gameObject, tempoDeVida);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == dono || other.GetComponent<Projectile>() != null) return;

        // other.GetComponent<Vida>()?.ReceberDano(dano);

        Destroy(gameObject);
    }
}

