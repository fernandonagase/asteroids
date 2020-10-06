using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    private Vector2 lowerLeftLim;
    private Vector2 upperRightLim;

    // Start is called before the first frame update
    void Start()
    {
        Camera mainCamera = Camera.main;

        lowerLeftLim = mainCamera.ScreenToWorldPoint(Vector3.zero);
        upperRightLim = mainCamera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0)
        );
    }

    private void FixedUpdate()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit2D bottomHit = Physics2D.Raycast(
            lowerLeftLim,
            Vector3.right * upperRightLim.x * 2,
            Mathf.Infinity,
            layerMask
        );
        if (
            bottomHit.collider != null
        ) {
            Vector3 position = bottomHit.collider.transform.position;
            position.y = upperRightLim.y - 1;
            bottomHit.collider.transform.position = position;
        }

        RaycastHit2D leftHit = Physics2D.Raycast(
            lowerLeftLim,
            Vector3.up * upperRightLim.y * 2,
            Mathf.Infinity,
            layerMask
        );
        if (
            leftHit.collider != null
        ) {
            Vector3 position = leftHit.collider.transform.position;
            position.x = upperRightLim.x - 1;
            leftHit.collider.transform.position = position;
        }

        RaycastHit2D topHit = Physics2D.Raycast(
            upperRightLim,
            -Vector3.right * upperRightLim.x * 2,
            Mathf.Infinity,
            layerMask
        );
        if (
            topHit.collider != null
        ) {
            Vector3 position = topHit.collider.transform.position;
            position.y = lowerLeftLim.y + 1;
            topHit.collider.transform.position = position;
        }

        RaycastHit2D rightHit = Physics2D.Raycast(
            upperRightLim,
            -Vector3.up * upperRightLim.y * 2,
            Mathf.Infinity,
            layerMask
        );
        if (
            rightHit.collider != null
        ) {
            Vector3 position = rightHit.collider.transform.position;
            position.x = lowerLeftLim.x + 1;
            rightHit.collider.transform.position = position;
        }
    }
}
