using UnityEngine; // Unity needs this extra namespace, don't worry about it


// Define our script as PilotController and inherit from MonoBehavior for Game Object functionality
public class PilotController : MonoBehaviour 
{
    // These SerializeField annotations let us assign these variables in the Inspector
    [SerializeField] private GameObject projectilePrefab; // We can't spawn a bullet when we shoot unless we store a reference to the prefab
    [SerializeField] private float speed; // This controls the side-to-side speed of Pilot when moved with input

    // The last annotated properties let us tweak the respawning logic from the Inspector
    [SerializeField] private float respawnSpeed; // Change how fast the Pilot respawns
    [SerializeField] private Vector3 spawnStart, spawnEnd; // When respawning, Pilot starts at spawnStart and moves to spawnEnd

    // These variables store references to our RigidBody2D for velocity and PolygonCollider2D for the isTrigger property
    private Rigidbody2D body;
    private PolygonCollider2D polygonCollider;

    // Lastly, we need a screenBoundary constant to mark the edges of the camera where we want the pilot to collide
    private float screenBoundary = 8f;

    // Our body and polygonCollider variables are still undefined, so we grab the corresponding components when the Pilot "wakes up"
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    // Just like in the MeteorScript, we check for the collisions we need. Pilot only cares about collisions with Mothership. 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mothership")) // Don't forget to make this tag and assign it in the Inspector
        {
            // Since the Meteors push Pilot, reset its velocity and rotation
            body.linearVelocity = Vector2.zero;
            transform.rotation = new Quaternion(0f, 0f, 180f, 1f);
            
            polygonCollider.isTrigger = true; // The collider is still enabled, but the Meteors won't push it while respawning
            gameObject.tag = "Respawn"; // Set the tag to Respawn so our Update() logic knows Pilot is respawning
            
            transform.position = spawnStart; // Teleport the Pilot to spawnStart
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        // Since the Meteors push the Pilot in this version, we fix the rotation in Update() to keep Pilot facing up
        transform.rotation = new Quaternion(0f, 0f, 180f, 1f);

        // Before checking for input, we check to see if the Pilot is respawning
        if (gameObject.tag == "Respawn")
        {
            // If it is, and it is still below spawnEnd, we move it at spawnSpeed towards spawnEnd
            if (transform.position.y < spawnEnd.y) transform.position = Vector3.MoveTowards(
                transform.position,
                spawnEnd,
                respawnSpeed * Time.deltaTime
            );

            // If Pilot already caught up to spawnEnd, we re-enable physics interactions with Meteors and change the tag back
            else
            {
                gameObject.tag = "Player";
                polygonCollider.isTrigger = false;
            }
        }

        else // (If Pilot isn't respawning)
        {
            // We capture the player's horizontal input (left and right) and then move them in that direction at a fixed rate.
            float xInput = Input.GetAxis("Horizontal");
            transform.Translate(xInput * speed * Time.deltaTime, 0f, 0f);

            // We then "clamp" its position with screenBoundary to enforce the left and right boundaries
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -screenBoundary, screenBoundary), transform.position.y, transform.position.z);

            // Finally, if the player pressed the "Jump" key (space bar) we fire a projectile
            if (Input.GetButtonDown("Jump"))
            {
                // To fire the projectile, we find the position right in front of the pilot...
                Vector3 position = new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z);
                // ...and then instantiate ProjectilePrefab from our refererence. Its logic is handled in ProjectileScript.
                Instantiate(projectilePrefab, position, Quaternion.identity);
            }
        }
    }
    
}
