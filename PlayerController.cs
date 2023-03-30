using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int playerLifes;
    [SerializeField] private int playerBullets;
    [SerializeField] private int playerShots;
    [SerializeField] public int playerPoints;
    [SerializeField] private int playerPosition;
    [SerializeField] private int playerGun;
    [SerializeField] private float playerFading;
    [SerializeField] private int colliderTime;
    [SerializeField] private int X2Time;
    [SerializeField] private bool playerX2;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private Material playerMaterial;
    [SerializeField] private GameObject [] playerPositions;
    [SerializeField] private GameObject [] playerTypeShoots;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        playerPoints = 0;
        playerPosition = 0;
        playerGun = 0;
        playerShots = 3;
        playerBullets = 3;
        this.gameObject.transform.position = playerPositions[0].transform.position;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Inputs del player. 1º y 2º Movimiento, 3º Disparo, 4º Recarga.
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) 
        {
            playerPosition++;
            if(playerPosition >= 1)
            {
                playerPosition = 1;
                PlayerMove();
            }
            else
            {
                PlayerMove();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            playerPosition--;
            if(playerPosition <= -1)
            {
                playerPosition = -1;
                PlayerMove();
            }
            else
            {
                PlayerMove();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerShoot();
        }
    }

    //Funcion del movimineto del player.
    private void PlayerMove()
    {
        if(playerPosition == 0)
        {
            this.gameObject.transform.position = playerPositions[0].transform.position;
        }
        else if(playerPosition == 1)
        {
            this.gameObject.transform.position = playerPositions[1].transform.position;
        }
        else
        {
            this.gameObject.transform.position = playerPositions[2].transform.position;
        }
    }
    
    //Funcion del disparo del player.
    private void PlayerShoot()
    {
        GameObject Bullet = null;
        if(playerBullets <= 0)
        {
            playerBullets = 0;
            Debug.Log("No puedes disparar");
        }
        else
        {
            playerBullets--;
            if(playerGun == 0)
            {
                Bullet = Instantiate(playerTypeShoots[0]);
                Bullet.transform.position = this.gameObject.transform.position;
            }
            else if(playerGun == 1)
            {
                Bullet = Instantiate(playerTypeShoots[1]);
                Bullet.transform.position = this.gameObject.transform.position;
            }
            else if(playerGun == 2)
            {
                Bullet = Instantiate(playerTypeShoots[2]);
                Bullet.transform.position = this.gameObject.transform.position;
            }
            
        }
    }

    //Funcion de recarga de disparos.
    public void ReloadBullet()
    {
        if(playerBullets < playerShots)
        {
            playerBullets++;
            if (playerBullets > playerShots)
                playerBullets = playerShots;
        }
    }

    //Funcion de la puntuación del player.
    public void PlayerPoints(int points)
    {
        if(playerX2 == true)
        {
            playerPoints += points * 2;
        }
        else
        {
            playerPoints += points;
        }
    }

    //Triggers del player. 1º Obstaculos, 2º Puntos dobles, 3º,4º y 5º Armas.
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            playerLifes--;
            playerMaterial.color = new Color(playerMaterial.color.r, playerMaterial.color.g, playerMaterial.color.b, playerFading);
            playerCollider.enabled = false; 
            if (playerLifes <= 0)
            {
                playerLifes = 0;
                gameManager.EndGame();
            }
            StartCoroutine(WaitForCollider(colliderTime));
        }
        else if(other.tag == "X2")
        {
            StartCoroutine(DoublePointsTime(X2Time));
        }
        else if(other.tag == "Gun0")
        {
            playerGun = 0;
            playerShots = 3;
        }
        else if (other.tag == "Gun1")
        {
            playerGun = 1;
            playerShots = 1;
        }
        else if (other.tag == "Gun2")
        {
            playerGun = 2;
            playerShots = 2;
        }
    }

    // Subrutinas. 1º Invulnerabilidad, 2º Puntos dobles.
    IEnumerator WaitForCollider(int colliderTime)
    {
        yield return new WaitForSeconds(colliderTime);
        playerMaterial.color = new Color(playerMaterial.color.r, playerMaterial.color.g, playerMaterial.color.b, 1f);
        playerCollider.enabled = true;
    }

    IEnumerator DoublePointsTime( int X2Time)
    {
        playerX2 = true;
        yield return new WaitForSeconds(X2Time);
        playerX2 = false;
    }
}
