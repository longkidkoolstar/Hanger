using UnityEngine;

public class RopeSwing : MonoBehaviour
{
    public DistanceJoint2D ropeJoint;
    public LineRenderer ropeRenderer;
    public LayerMask grappleLayer;
    public float maxDistance = 10f; // Max rope length
    public float minDistance = 2f;  // Min rope length
    public float retractSpeed = 5f; // How fast the rope retracts/extends
    public float grappleRadius = 1.5f; // Radius around the clicked point to find grapple objects
    private Vector2 grapplePoint;
    private bool isSwinging;

    void Start()
    {
        ropeJoint.enabled = false;
        ropeRenderer.enabled = false;
        ropeJoint.autoConfigureDistance = false; // Disable auto distance adjustment
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D closestGrapple = Physics2D.OverlapCircle(mousePos, grappleRadius, grappleLayer);

            if (closestGrapple != null)
            {
                Vector2 closestPoint = closestGrapple.ClosestPoint(mousePos);
                float distance = Vector2.Distance(transform.position, closestPoint);

                if (distance >= minDistance && distance <= maxDistance)
                {
                    grapplePoint = closestPoint;
                    ropeJoint.connectedAnchor = grapplePoint;
                    ropeJoint.distance = distance; // Set initial rope length
                    ropeJoint.enabled = true;
                    isSwinging = true;
                    ropeRenderer.enabled = true;

                    // Ensure correct rendering
                    ropeRenderer.positionCount = 2;
                    ropeRenderer.startWidth = 0.05f;
                    ropeRenderer.endWidth = 0.05f;
                    ropeRenderer.useWorldSpace = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            ropeJoint.enabled = false;
            isSwinging = false;
            ropeRenderer.enabled = false;
        }

        if (isSwinging)
        {
            ropeRenderer.SetPosition(0, transform.position);
            ropeRenderer.SetPosition(1, grapplePoint);

            // Adjust rope length with scroll wheel
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                float newDistance = ropeJoint.distance - scroll * retractSpeed;
                ropeJoint.distance = Mathf.Clamp(newDistance, minDistance, maxDistance);
            }
        }
    }
}
