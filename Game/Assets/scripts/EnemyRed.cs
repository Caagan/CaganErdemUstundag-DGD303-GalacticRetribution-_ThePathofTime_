using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyShipScript : MonoBehaviour
{
    public int enemyhealth=100;
    public float speed = 0.6f;  // D��man gemisinin h�z�n� belirler
    public float rotationSpeed = 3f;  // D�nme h�z�
    public GameObject projectilePrefab;  // Mermi prefab'�
    public Transform shootPoint;  // Merminin ate� edilece�i nokta

    private Transform player;  // Oyuncu (spaceship) referans�
    private Rigidbody2D rb;
    private Animator ani;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("SpaceShip").transform;  // Oyuncu gemisini bul
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Oyuncuya do�ru hareket etme
        MoveTowardsPlayer();

        // Oyuncuya do�ru ate� etme
        FireAtPlayer();
    }

    void MoveTowardsPlayer()
    {
        // Oyuncu ile d��man aras�ndaki mesafeyi hesapla
        Vector2 direction = (player.position - transform.position).normalized;

        // D��man gemisini oyuncuya do�ru hareket ettir
        rb.velocity = direction * speed;

        // D��man gemisinin oyuncuya do�ru d�nmesi i�in
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void FireAtPlayer()
    {
        // D��man gemisinin ate� etme s�kl���n� belirlemek i�in
        if (Random.Range(0f, 1f) < 0.005f)  // %1 ihtimalle ate� etme
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Mermiyi olu�tur
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        // Merminin hareket etmesi i�in Rigidbody2D'yi al
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Oyuncu ile d��man aras�ndaki y�n� hesapla
        Vector2 direction = (player.position - shootPoint.position).normalized;

        // Mermiyi oyuncuya do�ru hareket ettir
        if (rb != null)
        {
            // Mermiyi oyuncuya do�ru y�nlendiriyoruz
            rb.velocity = direction * 10f;  // H�z 10f olarak ayarlanabilir
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Bullet")
        {
            if (enemyhealth < 1)
            {
                //�l
                Die();
            }
            else
            {
                enemyhealth = enemyhealth - 10;
                ani.SetTrigger("redHit");
               
            }
        }
    }
    public void Die()
    {
        Destroy(gameObject, 1f);
    }
}
