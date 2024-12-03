using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceShipScript : MonoBehaviour

{
    Rigidbody2D rb;
    public float speed;
    public int health = 100;

    public float rotationSpeed = 5f;  // D�nd�rme h�z�
    public GameObject projectilePrefab;  // Mermi prefab'�
    public Transform shootPoint;  // Merminin ate� edilece�i nokta

    
    public  Image progressbarui;
    public GameObject gameover;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * speed,0));
        rb.AddForce(new Vector2(0,Input.GetAxis("Vertical") * speed));

        // Fare imlecinin pozisyonunu al
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Uzay gemisinin pozisyonunu al
        Vector2 direction = mousePosition - (Vector2)transform.position;

        // Y�n� normalize et ve d�n
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Ate� etme i�lemi
        if (Input.GetMouseButtonDown(0))  // Sol fare tu�una bas�ld���nda
        {
            Shoot();
        }

    }
    void Shoot()
    {
        // Mermiyi olu�tur
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, transform.rotation);

        // Merminin hareket etmesi i�in Rigidbody2D'yi al
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Mermiyi ileriye do�ru hareket ettir
        if (rb != null)
        {
            // Mermiyi geminin bakt��� y�nde hareket ettiriyoruz
            rb.velocity = transform.up * 10f;  // Geminin "yukar�" y�n� (forward y�n�) ile hareket eder
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyRedBullet")
        {
            if (health < 1)
            {
                //�l
                Death();
            }
            else
            {
                health = health - 5;
                progressbarui.fillAmount =progressbarui.fillAmount-0.05f;
            }
        }
    }
    public void Death()
    {
        gameover.SetActive(true);
    }
}

