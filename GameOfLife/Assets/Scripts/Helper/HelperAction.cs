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
    private float fireTimer = 0f;

    // variable for drag and drop
    private bool isDragging = false;
    private Vector3 offset;
    public bool isSet = false;
    public List<GameObject> gameTable;
   
    void Start()
    {
        script = canvas.GetComponent<Money>();

        // dont show the helper
        this.gameObject.GetComponent<Renderer>().enabled = false;
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
            }
        }

    }

    private void OnMouseUp()
    {
        Debug.Log(gameTable.Count);
        for (int i = 0; i< gameTable.Count; i++)
        {
            
            if (transform.position.x > (gameTable[i].transform.position.x - 1.5)
                && transform.position.x < (gameTable[i].transform.position.x + 1.5)
                && transform.position.y > (gameTable[i].transform.position.y - 1.5)
                && transform.position.y < (gameTable[i].transform.position.y + 1.5)
                )
            {
                transform.position = gameTable[i].transform.position;
            }
            isDragging = false;
            isSet = true;
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

            projectileRB.velocity = new Vector2(1f, 0f);
            projectileRB.gravityScale = 0f;
            projectileRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

}