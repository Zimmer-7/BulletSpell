using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy inimigoPrefab;
    public Transform player;
 
    [Header("Spawn")]
    public float inimigosPorSegundo = 2f;
    public int maximoDeInimigos = 200;
    [Tooltip("Quanto além da borda da tela eles nascem (0.1 = 10% da tela)")]
    public float margemForaDaTela = 0.1f;
 
    [Header("Reciclagem")]
    [Tooltip("Inimigos mais longe do player que isso voltam para o pool")]
    public float distanciaMaxima = 60f;
 
    private Camera cam;
    private readonly List<Enemy> ativos = new List<Enemy>();
    private readonly Stack<Enemy> pool = new Stack<Enemy>();
    private float acumuladorDeSpawn;
 
    void Start()
    {
        cam = Camera.main;
 
        for (int i = 0; i < maximoDeInimigos; i++)
        {
            Enemy e = Instantiate(inimigoPrefab, transform);
            e.spawner = this;
            e.gameObject.SetActive(false);
            pool.Push(e);
        }
    }
 
    void Update()
    {
        float dt = Time.deltaTime;
 
        acumuladorDeSpawn += inimigosPorSegundo * dt;
        while (acumuladorDeSpawn >= 1f)
        {
            acumuladorDeSpawn -= 1f;
            Gerar();
        }
 
        Vector3 posPlayer = player.position;
        float distMaxSqr = distanciaMaxima * distanciaMaxima;
        for (int i = ativos.Count - 1; i >= 0; i--)
        {
            Enemy e = ativos[i];
            e.Mover(posPlayer, dt);
            if ((e.transform.position - posPlayer).sqrMagnitude > distMaxSqr) Devolver(e);
        }
    }
 
    private void Gerar()
    {
        if (pool.Count == 0) return;
        if (!PontoForaDaTela(out Vector3 posicao)) return;
 
        Enemy e = pool.Pop();
        e.indiceNaLista = ativos.Count;
        ativos.Add(e);
        e.Ativar(posicao);
    }
 
    public void Devolver(Enemy e)
    {
        if (!e.gameObject.activeSelf) return;
 
        // Remove trocando com o último da lista (O(1), sem realocar)
        int i = e.indiceNaLista;
        Enemy ultimo = ativos[ativos.Count - 1];
        ativos[i] = ultimo;
        ultimo.indiceNaLista = i;
        ativos.RemoveAt(ativos.Count - 1);
 
        e.gameObject.SetActive(false);
        pool.Push(e);
    }
 
    // Escolhe um ponto numa borda aleatória um pouco fora da tela
    // e projeta no chão (plano na altura do player). Funciona com câmera inclinada.
    private bool PontoForaDaTela(out Vector3 posicao)
    {
        float m = margemForaDaTela;
        float t = Random.value;
        Vector2 vp;
        switch (Random.Range(0, 4))
        {
            case 0: vp = new Vector2(t, 1f + m); break;  // cima
            case 1: vp = new Vector2(t, -m); break;      // baixo
            case 2: vp = new Vector2(-m, t); break;      // esquerda
            default: vp = new Vector2(1f + m, t); break; // direita
        }
 
        Ray raio = cam.ViewportPointToRay(vp);
        Plane chao = new Plane(Vector3.up, player.position);
        if (chao.Raycast(raio, out float distancia))
        {
            posicao = raio.GetPoint(distancia);
            return true;
        }
        posicao = Vector3.zero;
        return false; // raio não bate no chão (câmera muito deitada); tenta de novo no próximo spawn
    }
}
