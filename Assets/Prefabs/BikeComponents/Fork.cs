using System;
using Configuration;
using Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Prefabs.BikeComponents
{
    public class Fork : MonoBehaviour
    {
        public ForkConfiguration configuration;

        public LineRenderer forkLineRenderer;
        public LineRenderer lowerLineRenderer;

        public SliderJoint2D sliderJoint;
        public SpringJoint2D springJoint;

        public Rigidbody2D lowerBody;
        public Vector2 WheelConnectedPosition => new Vector2(0, -configuration.LowerLength);

        private void Start()
        {
            lowerBody.transform.localPosition = new Vector2(0, -configuration.travel);
            
            forkLineRenderer.SetPositions(new []
            {
                new Vector3(0, 0),
                new Vector3(0, -configuration.travel)
            });

            lowerLineRenderer.SetPositions(new []
            {
                new Vector3(0, 0),
                new Vector3(0, -configuration.LowerLength)
            });

            springJoint.anchor = new Vector2(0, -configuration.LowerLength);
            springJoint.connectedAnchor = new Vector2(0, -configuration.length + configuration.springLength);
            springJoint.dampingRatio = configuration.dampingRatio;
            springJoint.frequency = configuration.frequency;

            sliderJoint.useLimits = true;
            sliderJoint.limits = new JointTranslationLimits2D
            {
                min = 0,
                max = configuration.travel
            };
        }
    }
}