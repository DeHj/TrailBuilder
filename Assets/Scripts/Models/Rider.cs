using Interfaces;
using UnityEngine;

namespace Models
{
    public class Rider : MonoBehaviour, ICameraTraceable
    {
        public SpringJoint2D footsJoint;
        public SpringJoint2D handsJoint;

        public HingeJoint2D footsConnectionJoint;
        public HingeJoint2D handsConnectionJoint;

        public Rigidbody2D cameraFollowedObject;

        public Vector2 GetPosition()
        {
            return cameraFollowedObject.position;
        }
    }
}