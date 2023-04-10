using UnityEngine;

/// <summary>
/// Drag a Rigidbody2D by selecting one of its colliders by pressing the mouse down.
/// When the collider is selected, add a TargetJoint2D.
/// While the mouse is moving, continually set the target to the mouse position.
/// When the mouse is released, the TargetJoint2D is deleted.`
/// </summary>
public class DragTarget : MonoBehaviour
{
    public LayerMask dragLayers;

    [Range (0.0f, 100.0f)]
    public float damping = 1.0f;

    [Range (0.0f, 100.0f)]
    public float frequency = 5.0f;

    private TargetJoint2D _targetJoint;

    void Update ()
    {
        // Calculate the world position for the mouse.
        var worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

        if (Input.GetMouseButtonDown (0))
        {
            // Fetch the first collider.
            // NOTE: We could do this for multiple colliders.
            var collider = Physics2D.OverlapPoint (worldPos, dragLayers);
            if (!collider)
                return;

            // Fetch the collider body.
            var body = collider.attachedRigidbody;
            if (!body)
                return;

            // Add a target joint to the Rigidbody2D GameObject.
            _targetJoint = body.gameObject.AddComponent<TargetJoint2D> ();
            _targetJoint.dampingRatio = damping;
            _targetJoint.frequency = frequency;

            // Attach the anchor to the local-point where we clicked.
            _targetJoint.anchor = _targetJoint.transform.InverseTransformPoint (worldPos);		
        }
        else if (Input.GetMouseButtonUp (0))
        {
            Destroy (_targetJoint);
            _targetJoint = null;
            return;
        }

        // Update the joint target.
        if (_targetJoint)
        {
            _targetJoint.target = worldPos;
        }
    }
}