using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField] float speed=5f;

    [SerializeField] float jumpForce=7f;

    bool isGrounded = false;

    Rigidbody2D rb;

    [SerializeField] LayerMask groundLayer; 

    SpriteRenderer sprite;

    private float inputX;

    [Header("SaltoPared")]

    [SerializeField] private Transform controladorPared;

    [SerializeField] private Vector3 dimensionesCajaPared;

    private bool enPared;

    private bool deslizando;

    [SerializeField] private float velocidadDeslizar;

    [SerializeField] private float fuerzaSaltoParedX;

    [SerializeField] private float fuerzaSaltoParedY;

    [SerializeField] private float tiempoSaltoPared;

    private bool saltandoDePared;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        float horizontalInput = inputX;

        if (horizontalInput > 0)
        {
            sprite.flipX = false; 
        }
        if (horizontalInput < 0)
        {
            sprite.flipX = true;
        }

        // Controles de personaje
        if(!saltandoDePared)
        {
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }
        

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !deslizando)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.Space) && enPared && deslizando)
        {
            //SaltoPared
            SaltoPared();
        }
        

        if(!isGrounded && enPared && inputX != 0)
        {
            deslizando = true;
        }
        else
        {
            deslizando = false;
        }



        // Almacenando el colisionador en una variable separada para facilitar su uso
        Collider2D col = GetComponent<Collider2D>();
        // Creando un área circular debajo de los pies del personaje
        isGrounded = Physics2D.OverlapCircle(transform.position - transform.up * ((col.bounds.extents.y / transform.localScale.y - col.offset.y) * transform.localScale.y), 0.01f, groundLayer);
    }

    private void SaltoPared()
    {
        enPared = false;
        rb.velocity = new Vector2(fuerzaSaltoParedX * -inputX, fuerzaSaltoParedY);
        //Espere
        StartCoroutine(CambioSaltoPared());
    }

    IEnumerator CambioSaltoPared()
    {
        saltandoDePared = true;
        yield return new WaitForSeconds(tiempoSaltoPared);
        saltandoDePared = false;
    }


    private void FixedUpdate()
    {
        enPared = Physics2D.OverlapBox(controladorPared.position, dimensionesCajaPared, 0f, groundLayer);

        if(deslizando)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -velocidadDeslizar, float.MaxValue));
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorPared.position, dimensionesCajaPared);
    }
}
