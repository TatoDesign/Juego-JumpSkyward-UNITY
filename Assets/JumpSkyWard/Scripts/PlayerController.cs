using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    [Space]
    [Header("Movimiento jugador: ")]
    public float velocidad;
    public float saltar;

    [Space]
    [Header("Armas: ")]
    public GameObject mano;
    public GameObject espada;

    [Space]
    [Header("Control Ataque")]
    public int da�o;
    [SerializeField] private float tiempoEntreAtaque;
    [SerializeField] private float tiempoSiguienteAtaque;
    [SerializeField] private Transform controladorGolpe1;
    [SerializeField] private float radioGolpe;
    [SerializeField] private Vector2 size;
    [SerializeField] private float angulo;

    [Space]
    [Header("Variable globales: ")]
    bool moving;
    bool moving2;
    bool canJump;
    bool arma = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Movimiento();
        Ataque();
    }

    private void Movimiento()
    {

        if (moving = Input.GetAxisRaw("Horizontal") == 1)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if(moving2 = Input.GetAxisRaw("Horizontal") == -1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        animator.SetBool("Walk", moving);
        animator.SetBool("Walk2", moving2);

        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            rb.AddForce(Vector2.up * saltar, ForceMode2D.Impulse);
            canJump = false;
            animator.SetTrigger("Jump");
        }
    }

    private void Ataque()
    {
        if (tiempoSiguienteAtaque > 0)
        {
            tiempoSiguienteAtaque -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0) && tiempoSiguienteAtaque <= 0 && arma)
        {
            tiempoSiguienteAtaque = tiempoEntreAtaque;
            Da�o();
        }
    }

    private void FixedUpdate()
    {
        Vector2 newVelocity;
        newVelocity.x = Input.GetAxisRaw("Horizontal") * velocidad;
        newVelocity.y = rb.velocity.y;

        rb.velocity = newVelocity;
    }

    private void Da�o()
    {
        animator.SetTrigger("Golpe");
        Collider2D[] objetos = Physics2D.OverlapBoxAll(controladorGolpe1.position, size, angulo);

        /*foreach (Collider2D collisionador in objetos)
        {
            if (collisionador.CompareTag("Enemigo"))
            {

            }
        }*/
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawWireSphere(controladorGolpe1.position, radioGolpe);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Suelo")
        {
            canJump = true;
            animator.SetBool("Suelo", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Suelo")
        {
            animator.SetBool("Suelo", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Arma"))
        {
            Destroy(collision.gameObject);
            arma = true;
            mano.SetActive(true);
            espada.SetActive(true);
        }
    }
}
