using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vuforia;

public class SmoothCamera : MonoBehaviour
{
    public int smoothingFrames = 10;

    private ObserverBehaviour mObserverBehaviour;
    private Queue<Quaternion> rotations;
    private Queue<Vector3> positions;

    private Quaternion smoothedRotation;
    private Vector3 smoothedPosition;

    void Start()
    {
        rotations = new Queue<Quaternion>(smoothingFrames);
        positions = new Queue<Vector3>(smoothingFrames);

        mObserverBehaviour = GetComponent<ObserverBehaviour>();
        if (mObserverBehaviour)
        {
            mObserverBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    void OnTargetStatusChanged(ObserverBehaviour observer, TargetStatus targetStatus)
    {
        if (targetStatus.Status == Status.TRACKED)
        {
            // Target is tracked
            OnTargetTracked();
        }
        else
        {
            // Target is not tracked
            OnTargetLost();
        }
    }

    void OnTargetTracked()
    {
        // Handle target tracked event
    }

    void OnTargetLost()
    {
        // Handle target lost event
    }

    void UpdateSmoothedValues()
    {
        if (rotations.Count >= smoothingFrames)
        {
            rotations.Dequeue();
            positions.Dequeue();
        }

        rotations.Enqueue(transform.rotation);
        positions.Enqueue(transform.position);

        Vector4 avgr = Vector4.zero;
        foreach (Quaternion singleRotation in rotations)
        {
            Math3d.AverageQuaternion(ref avgr, singleRotation, rotations.Peek(), rotations.Count);
        }

        Vector3 avgp = Vector3.zero;
        foreach (Vector3 singlePosition in positions)
        {
            avgp += singlePosition;
        }
        avgp /= positions.Count;

        smoothedRotation = new Quaternion(avgr.x, avgr.y, avgr.z, avgr.w);
        smoothedPosition = avgp;
    }

    void LateUpdate()
    {
        UpdateSmoothedValues();
        transform.rotation = smoothedRotation;
        transform.position = smoothedPosition;
    }
}
