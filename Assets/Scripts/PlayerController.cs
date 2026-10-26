using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionReference moveAction;
    public InputActionReference fireAction;
    public float velocidade = 5f;

    [Header("Tiro")]
    public Projectile projetilPrefab;
    public Transform pontoDeTiro;
    public float intervaloEntreTiros = 0.2f;

    private Camera cam;
    private float proximoTiro;

    void OnEnable()
    {
        moveAction.action.Enable();
        fireAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        fireAction.action.Disable();
    }

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (fireAction.action.IsPressed() && Time.time >= proximoTiro)
        {
            atirar();
            proximoTiro = Time.time + intervaloEntreTiros;
        }
    }

    void FixedUpdate()
    {
        movePlayer();
    }

    private void movePlayer()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        transform.Translate(move * Time.deltaTime * velocidade);
    }

    private void atirar()
    {
        Vector3 origem = pontoDeTiro != null ? pontoDeTiro.position : transform.position;
        Vector3 direcao = direcaoDoMouse(origem);
        if (direcao == Vector3.zero) return;

        Projectile projetil = Instantiate(projetilPrefab, origem, Quaternion.LookRotation(direcao));
        projetil.Disparar(direcao, gameObject);
    }

    private Vector3 direcaoDoMouse(Vector3 origem)
    {
        Ray raio = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane chao = new Plane(Vector3.up, origem);
        if (!chao.Raycast(raio, out float distancia)) return Vector3.zero;

        Vector3 direcao = raio.GetPoint(distancia) - origem;
        direcao.y = 0;
        return direcao.normalized;
    }
}

