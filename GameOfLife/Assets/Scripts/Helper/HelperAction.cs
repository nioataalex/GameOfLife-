using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update

    // variables for new instance
    public GameObject canvas;
    private Money script;
    public int price;
    public Transform spawnPoint;

    //variable for projectile
    public GameObject projectile;
    public Transform firePoint;
    private float fireTimer = 1.2f;

    // variable for drag and drop
    private bool isDragging = false;
    private Vector3 offset;
    public bool isSet = false;
    public List<GameObject> gameTable;
    private bool nothingHappens = true;

    // variable to memorize where is a helper on the table
    private int squareIndex = -1;

    // variable for collider
    private Collider2D colliderShield;

    void Start()
    {
        script = canvas.GetComponent<Money>();

        // dont show the helper
        this.gameObject.GetComponent<Renderer>().enabled = false;

        colliderShield = gameObject.GetComponent<Collider2D>();
    }

            
    // Update is called once per frame
    void Update()
    {
        // fire projectile just if the helper is set on the game table
        if (isSet)
        {
            if (fireTimer >= 2f)
            {
                FireProjectile();
                fireTimer = 0f;
            }

            fireTimer += Time.deltaTime;
        }
    }

    private void OnMouseDown()
    {
        if (!isSet)
        {
            if (script.variableToDisplay >= price)
            {
                spawnPoint.transform.position = transform.position;
                spawnPoint.transform.rotation = Quaternion.identity;

                Instantiate(this.gameObject, spawnPoint.transform.position, spawnPoint.transform.rotation);

                isDragging = true;
                offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                this.gameObject.GetComponent<Renderer>().enabled = true;

                script.variableToDisplay -= price;
                nothingHappens = false;

                colliderShield.enabled = false;
            } else
            {
                Instantiate(this.gameObject, spawnPoint.transform.position, spawnPoint.transform.rotation);
                nothingHappens = true;
            }
        } else if (squareIndex != -1)
        {
            SqaureDrop square = gameTable[squareIndex].GetComponent<SqaureDrop>();
            square.inUse = false;
            Destroy(gameObject);
        }

    }

    private void OnMouseUp()
    {
        Debug.Log(gameTable.Count);
        for (int i = 0; i< gameTable.Count && isDragging; i++)
        {
            
            if (transform.position.x > (gameTable[i].transform.position.x - 1.5)
                && transform.position.x < (gameTable[i].transform.position.x + 1.5)
                && transform.position.y > (gameTable[i].transform.position.y - 1.5)
                && transform.position.y < (gameTable[i].transform.position.y + 1.5)
                )
            {
                SqaureDrop square = gameTable[i].GetComponent<SqaureDrop>();
                if (square.inUse)
                {
                    break;
                }
                transform.position = gameTable[i].transform.position;
                squareIndex = i;
               
                square.inUse = true;

                isDragging = false;
                isSet = true;
                colliderShield.enabled = true;
            }
           
        }

        if (squareIndex == -1 && !nothingHappens)
        {
            script.variableToDisplay += price;
            Destroy(gameObject);
            
        } else if (squareIndex == -1 && nothingHappens)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
    }

    void FireProjectile()
    {
        if (projectile != null)
        {
            GameObject newProjectile = Instantiate(projectile, firePoint.position + new Vector3(1.0f, 0.0f, 0.0f), firePoint.rotation);
            Rigidbody2D projectileRB = newProjectile.GetComponent<Rigidbody2D>();

            projectileRB.velocity = new Vector2(1.5f, 0f);
            projectileRB.gravityScale = 0f;
            projectileRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

}
