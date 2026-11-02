using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float velocidade = 3f;
    public int vidaMaxima = 1;

    public float ajusteDeRotacao = 0f;
 
    [HideInInspector] public EnemySpawner spawner;
    [HideInInspector] public int indiceNaLista;
 
    private int vida;
 
    public void Ativar(Vector3 posicao)
    {
        transform.position = posicao;
        vida = vidaMaxima;
        gameObject.SetActive(true);
    }
 
    public void Mover(Vector3 posicaoDoPlayer, float deltaTime)
    {
        Vector3 pos = transform.position;
        Vector3 direcao = posicaoDoPlayer - pos;
        direcao.y = 0;
        float distSqr = direcao.sqrMagnitude;
        if (distSqr < 0.0001f) return;
 
        direcao /= Mathf.Sqrt(distSqr);
        transform.SetPositionAndRotation(
            pos + direcao * (velocidade * deltaTime),
            Quaternion.LookRotation(direcao) * Quaternion.Euler(0, ajusteDeRotacao, 0));
    }
 
    public void ReceberDano(int dano)
    {
        vida -= dano;
        if (vida <= 0) spawner.Devolver(this);
    }
}

