using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceShipScript : MonoBehaviour

{
    private Camera _mainCam;

    Rigidbody2D rb;
    Animator ani;
    public float speed;
    public int health = 100;
    public int enemyRedHit=5;

    public float rotationSpeed = 5f;  // D�nd�rme h�z�
    public GameObject projectilePrefab;  // Mermi prefab'�
    public Transform shootPoint;  // Merminin ate� edilece�i nokta
    public Image ultiImage; // Sprite'� de�i�tirece�imiz Image objesi
    public Sprite ultiReadySprite;
    public Sprite ultinotReadySprite;


    public  Image progressbarui;
    public GameObject gameover;
    public Text ultiText;

    public float maxChargeTime = 10f; // Ultimate i�in maksimum �arj s�resi (saniye)
    private float currentChargeTime = 0f; // �u anki �arj zaman�
    public bool isUltimateReady = false; // Ulti haz�r m�?

    public GameObject laserPrefab; // Lazer prefab'�
    public Transform laserSpawnPoint;
    public Transform laserSpawnPoint2;// Lazerin ��kaca�� nokta


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani=GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _mainCam=Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * speed,0));
        rb.AddForce(new Vector2(0,Input.GetAxis("Vertical") * speed));

        // Fare imlecinin pozisyonunu al
        Vector2 mousePosition = _mainCam.ScreenToWorldPoint(Input.mousePosition);

        // Uzay gemisinin pozisyonunu al
        Vector2 direction = mousePosition - (Vector2)transform.position;

        // Y�n� normalize et ve d�n
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Ate� etme i�lemi
        if (Input.GetMouseButtonDown(0))  // Sol fare tu�una bas�ld���nda
        {
            Shoot();
        }

        // Zamanla ulti bar�n� doldur
        if (currentChargeTime < maxChargeTime)
        {
            currentChargeTime += Time.deltaTime; // Her frame'de bir miktar artt�r

            float percentage = Mathf.Lerp(0f, 100f, currentChargeTime /maxChargeTime); // 0 ile 100 aras�nda bir de�er hesapla
            ultiText.text = Mathf.FloorToInt(percentage).ToString() + "%";
        }
        else
        {
            isUltimateReady = true; // Bar %100 olunca ulti haz�r
            ultiImage.sprite = ultiReadySprite; //ulti ready resmi de haz�r
        }

        // Ultiyi kullanma
        if (isUltimateReady && Input.GetKeyDown(KeyCode.X))
        {
            FireLaser(); // Lazer ate�le
            currentChargeTime = 0f; // Ulti kullan�ld���nda bar� s�f�rla
            isUltimateReady = false; // Ultiyi kullan�nca haz�r durumu s�f�rla
            ultiImage.sprite = ultinotReadySprite;//ulti haz�r degilresmi

        }
        void FireLaser()
        {
            // Lazer ate�le
            GameObject laser1=Instantiate(laserPrefab, laserSpawnPoint.position, laserSpawnPoint.rotation);
            GameObject laser2 = Instantiate(laserPrefab, laserSpawnPoint2.position, laserSpawnPoint2.rotation);

            // Merminin hareket etmesi i�in Rigidbody2D'yi al
            Rigidbody2D rb1 = laser1.GetComponent<Rigidbody2D>(); 
            Rigidbody2D rb2 = laser2.GetComponent<Rigidbody2D>();


            // Mermiyi ileriye do�ru hareket ettir
            if (rb1&&rb2 != null)
            {
                // Mermiyi geminin bakt��� y�nde hareket ettiriyoruz
                rb1.velocity = transform.up * 10f;
                rb2.velocity = transform.up * 10f; // Geminin "yukar�" y�n� (forward y�n�) ile hareket eder
            }
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
                DecreaseHealth(enemyRedHit);
            }
        }
    }
    public void Death()
    {
        health = 0;
        gameover.SetActive(true);
    }
    public void IncreaseHealth(int amount)
    {
        health += amount;
        // Sa�l�k s�n�rlar�n� kontrol et (�rne�in, 100'�n �zerine ��kmamas� i�in)
        health = Mathf.Min(health, 100);
        Debug.Log("Player Health: " + health);
        progressbarui.fillAmount = progressbarui.fillAmount + 0.01f*amount;

    }
    public void DecreaseHealth(int amount)
    {
        health -= amount;
        
        health = Mathf.Min(health, 100);
        Debug.Log("Player Health: " + health);
        progressbarui.fillAmount = progressbarui.fillAmount - 0.01f * amount;

        ani.SetTrigger("getHit");

    }
}

