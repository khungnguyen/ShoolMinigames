using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class CharFollower : MonoBehaviour
{
    
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
                Debug.LogError("Calling" + pathCreator.path.localPoints);
                Debug.LogError("Calling" + pathCreator.path.localTangents);
                Debug.LogError("Calling" + pathCreator.path.length);
                Debug.LogError("Calling" + pathCreator.path.cumulativeLengthAtEachVertex);
            }
        }
        bool stop = false;
        void Update()
        {
            if (pathCreator != null && !stop)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                Quaternion r = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation =  Quaternion.Euler(r.x,r.y,r.z);
                // if(Vector2.Distance(transform.position,pathCreator.path.GetPoint(1))<=speed * Time.deltaTime) {
                //     stop = true;
                // }
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
            
        }
}
