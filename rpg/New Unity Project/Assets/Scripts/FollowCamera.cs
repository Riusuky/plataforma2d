using UnityEngine;

public class FollowCamera : MonoBehaviour {

    // Distance in the x axis the player can move before the camera follows.
    public float xMargin = 1.5f;
    // Distance in the y axis the player can move before the camera follows.
    public float yMargin = 1.5f;
    // How smoothly the camera catches up with it's target movement in the x axis.
    public float xSmooth = 1.5f;
    // How smoothly the camera catches up with it's target movement in the y axis.
    public float ySmooth = 1.5f;
    // The maximum x and y coordinates the camera can have.
    private Vector2 maxXAndY;
    // The minimum x and y coordinates the camera can have.
    private Vector2 minXAndY;
    // Reference to the player's transform.
    public Transform player;

    
    void Awake()
    {
        // check the player reference.
        player = GameObject.Find("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player object not found");
        }

        // Get the bounds for the background texture - world size
        var backgroundBounds = GameObject.Find("background").GetComponent<Renderer>().bounds;

        // Get the viewable bounds of the camera in world coordinates
        var camTopLeft = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0, 0, 0));
        var camBottomRight = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1, 1, 0));
        
        // Automatically set the 
        minXAndY.x = backgroundBounds.min.x - camTopLeft.x;
        maxXAndY.x = backgroundBounds.max.x - camBottomRight.x;
    }

    bool CheckXMargin()
    {
        // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }

    bool CheckYMargin()
    {
        // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }

    void FixedUpdate()
    {
        // By default the target x and y coordinates of the camera are it's current x and y coordinates.
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        // Check if the player has moved beyond the x margin
        if (CheckXMargin())
            // The target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
            targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.fixedDeltaTime);

        // Check if the player has moved beyond the y margin
        if (CheckYMargin())
            // The target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
            targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.fixedDeltaTime);

        // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        // Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(targetX, targetY, transform.position.z);        
    }
}
