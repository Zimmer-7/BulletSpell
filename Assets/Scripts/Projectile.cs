using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float velocidade = 15f;
    public float tempoDeVida = 3f;
    private int danoTiro;

    private Rigidbody rb;
    private GameObject dono;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void Disparar(Vector3 direcao, GameObject atirador, int dano)
    {
        dono = atirador;
        danoTiro = dano;
        rb.linearVelocity = direcao * velocidade;
        Destroy(gameObject, tempoDeVida);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == dono || other.GetComponent<Projectile>() != null) return;

        if (other.TryGetComponent(out Enemy inimigo)) inimigo.ReceberDano(danoTiro);

        Destroy(gameObject);
    }
}

