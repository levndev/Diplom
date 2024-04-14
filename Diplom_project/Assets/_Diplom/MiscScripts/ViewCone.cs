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
    [SerializeField] public UnityEvent<GameObject> targetDetected;
    [SerializeField] public UnityEvent<GameObject> targetLost;
    [SerializeField] public UnityEvent<GameObject> targetSeen;

    [System.Serializable]
    public struct ColorMode
    {
        public Color LightColor;
        public Material coneMeshMaterial;
        public Material bodyMaterial;
    };

    [SerializeField] private MeshRenderer coneMeshRenderer;
    [SerializeField] private MeshRenderer bodyRenderer;

    [SerializeField] private List<ColorMode> colorModes;

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
        setColorMode(0);
    }

    public void setColorMode(int index)
    {
        if (index > 0 && index < colorModes.Count)
        {
            ColorMode mode = colorModes[index];
            if (spotLight != null && mode.LightColor != null)
                spotLight.color = mode.LightColor;
            if (coneMeshRenderer != null && mode.coneMeshMaterial != null)
                coneMeshRenderer.material = mode.coneMeshMaterial;
            if (bodyRenderer != null && mode.bodyMaterial != null)
                bodyRenderer.material = mode.bodyMaterial;
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
