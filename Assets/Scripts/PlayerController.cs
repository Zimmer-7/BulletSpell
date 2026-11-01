using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionReference moveAction;
    public InputActionReference fireAction;
    public float velocidade = 5f;
    public float velocidadeDeRotacao = 0f;

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
        olharParaMouse();

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
        Vector3 frente = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up);

        if (frente.sqrMagnitude < 0.01f) 
            frente = Vector3.ProjectOnPlane(cam.transform.up, Vector3.up);
        
        frente.Normalize();
        Vector3 direita = Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized;
        Vector3 move = direita * input.x + frente * input.y;
        transform.Translate(move * Time.deltaTime * velocidade, Space.World);
    }

    private void olharParaMouse()
    {
        Vector3 direcao = direcaoDoMouse(transform.position);
        if (direcao == Vector3.zero) return;

        Quaternion alvo = Quaternion.LookRotation(direcao);
        transform.rotation = alvo;
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

