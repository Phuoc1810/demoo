using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    private float horizontal;
    [SerializeField] private float jumpingPower = 10f;
    private bool isGround = false;

    [SerializeField] private Rigidbody2D rb;

    private AudioSource audioSource;
    [SerializeField] private AudioClip audioSlime; // Đay la AudioClip duoc gan qua Inspector

    //nhay nhanh va roi xuong nhanh hon
    [SerializeField] private float fallingMultipler = 2.5f;
    [SerializeField] private float lowJumpMultipler = 2f;

    [SerializeField] private GameObject OB;
    // Start is called before the first frame update
    void Start()
    {
        // Lay component AudioSource tu doi tuong
        audioSource = GetComponent<AudioSource>();

        // Kiem tra neu am thanh da duoc gan
        if (audioSlime == null)
        {
            Debug.LogError("Audio clip cho âm thanh 'Slime' chưa được gán.");
        }
    }

    // Update is called once per frame
    void Update()
    {      
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
        }
        //kiem tra nhan vat dang roi
        if(rb.velocity.y < 0)
        {
            //tang trong luc de roi nhanh hon
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallingMultipler - 1) * Time.deltaTime;
        }
        //kiem tra nhan vat dang nhay va da tha phim nhay
        else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            //Giam trong luc de nhay nhanh hon
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultipler - 1) * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = true;

            if (audioSlime != null)
            {
                audioSource.PlayOneShot(audioSlime);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OB"))
        {
            Destroy(gameObject);
            SceneManager.LoadScene(0);
        }
    }
}
