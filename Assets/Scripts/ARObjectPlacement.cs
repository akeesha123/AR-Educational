using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARObjectPlacement : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject objectToPlace;


public GameObject PlacedObject { get; private set; }

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began)
            return;

        if (raycastManager == null || objectToPlace == null)
            return;

        if (raycastManager.Raycast(
            touch.position,
            hits,
            TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if (PlacedObject == null)
            {
                PlacedObject = Instantiate(
                    objectToPlace,
                    hitPose.position,
                    hitPose.rotation
                );
            }
            else
            {
                PlacedObject.transform.SetPositionAndRotation(
                    hitPose.position,
                    hitPose.rotation
                );
            }
        }
    }


}
