using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Light))]
public class ViewCone : MonoBehaviour
{
    [SerializeField] private float fov;
    [SerializeField] private float range;
    [SerializeField] private Light spotLight;
    [SerializeField] private ConeMeshGenerator coneMeshGenerator;
    [SerializeField] private Transform detectorPoint;
    [SerializeField] private Reference<GameObject> target;
    [SerializeField] private UnityEvent<GameObject> targetDetected;
    [SerializeField] private UnityEvent<GameObject> targetLost;
    [SerializeField] private UnityEvent<GameObject> targetSeen;
    private float halfFov;
    private bool detected;

    private void Awake()
    {
        halfFov = fov / 2;
        detected = false;
        if (coneMeshGenerator != null)
        {
            coneMeshGenerator.length = range;
            coneMeshGenerator.angle = fov;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (spotLight != null)
        {
            if (spotLight.type != LightType.Spot)
            {
                spotLight = null;
            }
            else
            {
                spotLight.spotAngle = fov;
                spotLight.innerSpotAngle = fov;
                spotLight.range = range;
            }
        }
    }

    private void FixedUpdate()
    {
        if (target.IsValid())
        {
            Transform targetTransform = target.Get().transform;
            Vector3 toTarget = targetTransform.position - detectorPoint.transform.position;
            float distance = toTarget.magnitude;
            toTarget.Normalize();
            if (Vector3.Angle(detectorPoint.forward, toTarget) <= halfFov && toTarget.magnitude <= range)
            {
                LayerMask mask = LayerMask.GetMask("Player");
                if (Physics.Raycast(detectorPoint.position, toTarget, out RaycastHit hit, distance))
                {
                    if (mask == (mask | (1 << hit.collider.gameObject.layer)))
                    {
                        if (!detected)
                        {
                            detected = true;
                            targetDetected?.Invoke(target.Get());
                        }
                        targetSeen?.Invoke(target.Get());
                        return;
                    }
                }
            } 
        }
        if (detected)
        {
            detected = false;
            targetLost?.Invoke(target.Get());
        }
    }
}
