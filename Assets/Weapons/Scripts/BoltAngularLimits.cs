using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltAngularLimits : MonoBehaviour
{
    private ConfigurableJoint boltJoint;
    private JointDrive originalDrive;

    private float boltRotationThreshold = 60f;
    private float boltPullDistance = 0.1126f;
    private float boltForwardPosition = 0.06215f;
    [SerializeField] private GameObject rearBoltPiece;
    private void Start()
    {
        boltJoint = GetComponent<ConfigurableJoint>();
        originalDrive = boltJoint.angularXDrive;
        SetRotationDrive(false);

        SoftJointLimit linearLimit = boltJoint.linearLimit;
        linearLimit.limit = boltPullDistance;
        boltJoint.linearLimit = linearLimit;
    }
    private void Update()
    {
        HandleBoltInput();
        //MoveRearBoltPiece();
    }

    private void MoveRearBoltPiece()
    {
        rearBoltPiece.transform.localPosition = new Vector3(0, 0.111f, boltJoint.transform.position.z - 0.00655f);
    }

    private void SetRotationDrive(bool state)
    {
        if (state)
        {
            boltJoint.angularXDrive = originalDrive;
        }
        else
        {
            JointDrive disabledDrive = new JointDrive();
            boltJoint.angularXDrive = disabledDrive;
        }
    }

    private void HandleBoltInput()
    {
        float currentRotation = boltJoint.transform.localRotation.eulerAngles.z;

        if (currentRotation < boltRotationThreshold && IsBoltCloseToForward())
        {
            SetRotationDrive(true);
        }
        else if (currentRotation >= boltRotationThreshold || !IsBoltCloseToForward())
        {
            // Disable rotation drive when pushing the bolt forward
            SetRotationDrive(false);
        }
    }

    private bool IsBoltCloseToForward()
    {
        float currentZPosition = boltJoint.transform.localPosition.z;
        return Mathf.Approximately(currentZPosition, boltForwardPosition);
    }
}
