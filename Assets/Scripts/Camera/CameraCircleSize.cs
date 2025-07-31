using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;

public class CameraCircleSize : MonoBehaviour
{
    [Title("DOES NOT WORK IN RUNTIME")]
    [Title("References")]
    [SerializeField] private SplineContainer cameraSplineContainer;
    [Title("Settings")]
    [SerializeField, OnValueChanged(nameof(UpdateRadius))] private float radius = 1f;
    [SerializeField, OnValueChanged(nameof(UpdateRadius))] private float height;

    private void UpdateRadius()
    {
        List<BezierKnot> knots = cameraSplineContainer.Spline.Knots.ToList();
        cameraSplineContainer.Spline.SetKnot(0, UpdateKnot(knots[0], new Vector3(radius,0,0), 0));
        cameraSplineContainer.Spline.SetKnot(1, UpdateKnot(knots[1], new Vector3(0,0, radius), 270));
        cameraSplineContainer.Spline.SetKnot(2, UpdateKnot(knots[2], new Vector3(-radius, 0, 0), 180));
        cameraSplineContainer.Spline.SetKnot(3, UpdateKnot(knots[3], new Vector3(0, 0, -radius), 90));
        cameraSplineContainer.transform.position = new Vector3(0, height, 0);
    }

    private BezierKnot UpdateKnot(BezierKnot knot, Vector3 position, float rotation)
    {
        knot.Position = position;
        knot.TangentIn = new Vector3(0, 0, -0.552284749831f * radius);
        knot.TangentOut = new Vector3(0, 0, 0.552284749831f * radius);
        knot.Rotation = Quaternion.Euler(0, rotation, 0);
        return knot;
    }
}
