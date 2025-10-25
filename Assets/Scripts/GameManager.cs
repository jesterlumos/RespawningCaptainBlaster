using UnityEngine; // Unity needs this extra namespace, don't worry about it
using TMPro; // Don't forget to manually use TextMeshPro so that we can reference it in script!

// Define our script as GameManager and inherit from MonoBehavior for Game Object functionality
public class GameManager : MonoBehaviour
{
    // These SerializeField annotations let us assign these variables in the Inspector
    // We store a reference to our Meteor Prefab so we can spawn it and our Score TextMeshPro so we can update its text
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private TextMeshProUGUI scoreTMP;

    // We also need to note the boundaries of the screen to keep the player inside them, the range of the meteor delay, and the score
    private float screenBoundary = 8f;
    private float minMeteorDelay = 1f, maxMeteorDelay = 3f;
    private int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start() => SpawnMeteor(); // When the game starts, we initiate Meteor spawning by manually spawning the first one

    // We call this function to spawn each Meteor
    private void SpawnMeteor()
    {
        // We randomly determine where to spawn the meteor, and how long to wait before spawning the next one
        float xPosition = Random.Range(-screenBoundary, screenBoundary);
        float meteorDelay = Random.Range(minMeteorDelay, maxMeteorDelay);

        // Then we spawn a meteor at GameManager's Y position, the random xPosition, and no rotation
        Vector3 meteorPosition = new Vector3(xPosition, transform.position.y, transform.position.z);
        Instantiate(meteorPrefab, meteorPosition, Quaternion.identity);

        // Finally, we invoke SpawnMeteor from within itself to spawn the next meteor with our random delay
        Invoke(nameof(SpawnMeteor), meteorDelay);
    }

    // We call this function to increment the score and update our ScoreTMP to show the new score
    public void AddPoint()
    {
        score++; // This is the same as the commented lines below:
        // score += 1;
        // score = score + 1;
        scoreTMP.text = $"Score: {score.ToString("D3")}"; // This special line keeps the leading 000s in our score
    }
}
