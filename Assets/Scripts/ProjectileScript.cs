using UnityEngine; // Unity needs this extra namespace, don't worry about it

// Define our script as ProjectileScript and inherit from MonoBehavior for Game Object functionality
public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private float speed; // The [SerializeField] annotation lets us change speed in the Inspector
    private Rigidbody2D rigidBody; // We also need a reference to Projectile's RigidBody2D for velocity.

    private void Awake() => rigidBody = GetComponent<Rigidbody2D>(); // Assign the Projectile's RigidBody2D as soon as it "wakes up"
    private void Start() => rigidBody.linearVelocityY = speed; // Start moving the projectile right before the first Update()

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.y >= 6f) Destroy(gameObject); // Clean up any Projectiles that leave the camera by Destroying them
    }
}
