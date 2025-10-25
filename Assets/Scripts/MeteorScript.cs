using UnityEngine; // Unity needs this extra namespace, don't worry about it

// Define our script as MeteorScript and inherit from MonoBehavior for Game Object functionality
public class MeteorScript : MonoBehaviour
{
    // We only assign one value in the inspector for the Meteor's falling speed
    [SerializeField] private float speed;

    // We also declare an unassigned reference to our GameManager Script and RigidBody2D Component
    private GameManager gameManager;
    private Rigidbody2D rigidBody;

    // We assign GameManager and RigidBody2D as soon as the Meteor "wakes up"
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start() => rigidBody.linearVelocityY = speed; // Use linear velocity to move the Meteor straight down at our defined speed

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.y <= -6f) Destroy(gameObject); // Clean up any Meteors that make it off-camera
    }

    // This function runs when the Meteor hits a collider with isTrigger enabled (Mothership or Projectile)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mothership")) // Don't forget to make this tag and assign it in the Inspector
        {
            Debug.Log("Meteor collided with Mothership!"); // TODO: Code to be run when Meteor collides with Mothership
        }

        else if (other.CompareTag("Projectile")) // Don't forget to make this tag and assign it in the Inspector
        {
            Destroy(other.gameObject);  // Destroy any Projectile the Meteor hits
            gameManager.AddPoint(); // Tell GameManager to add a point to the score when the Meteor hits a Projectile
        }

        Destroy(gameObject); // Destroy the Meteor whether it hit a Projectile or the Mothership
    }
}
