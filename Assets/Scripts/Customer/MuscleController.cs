﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class MuscleController : MonoBehaviour {

	private UnityEngine.AI.NavMeshAgent navAgent;

	public ConfigurableJoint rightArmJoint, leftArmJoint, bodyJoint, rightLegJoint, leftLegJoint;
	[Range(0, 1)] public float consciousness = 1, armsStrength = 0, legsStrength = 0;
	public Vector3 leftArmTarget, rightArmTarget,
							   leftLegTarget, rightLegTarget;

	void Start(){
		navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}

	// Use this for initialization
	void Update () {
		UpdateMuscles();
	}

	public void UpdateMuscles(){
		JointDrive leftArmDrive = new JointDrive(), rightArmDrive = new JointDrive(),
							 leftLegDrive = new JointDrive(), rightLegDrive = new JointDrive();

		leftArmDrive.positionDamper = 1;
		leftArmDrive.positionSpring = 1000 * armsStrength * consciousness;
		leftArmDrive.maximumForce = 1000;

		rightArmDrive.positionDamper = 1;
		rightArmDrive.positionSpring = 1000 * armsStrength * consciousness;
		rightArmDrive.maximumForce = 1000;

		leftLegDrive.positionDamper = 1;
		leftLegDrive.positionSpring = 1000 * legsStrength * consciousness;
		leftLegDrive.maximumForce = 1000;

		rightLegDrive.positionDamper = 1;
		rightLegDrive.positionSpring = 1000 * legsStrength * consciousness;
		rightLegDrive.maximumForce = 1000;

		rightArmJoint.slerpDrive = rightArmDrive;
		leftArmJoint.slerpDrive = leftArmDrive;
		rightLegJoint.slerpDrive = rightLegDrive;
		leftLegJoint.slerpDrive = leftLegDrive;

		SoftJointLimit bodyJointLimit = new SoftJointLimit();

		bodyJointLimit.limit = Mathf.Lerp(90, 3, consciousness);
		bodyJointLimit.limit = Mathf.Lerp(90, 3, consciousness);

		bodyJoint.angularZLimit = bodyJointLimit;
		bodyJoint.angularYLimit = bodyJointLimit;

		rightArmJoint.targetRotation = Quaternion.Euler(rightArmTarget);
		leftArmJoint.targetRotation = Quaternion.Euler(leftArmTarget);
		rightLegJoint.targetRotation = Quaternion.Euler(rightLegTarget);
		leftLegJoint.targetRotation = Quaternion.Euler(leftLegTarget);

		navAgent.speed = Mathf.Lerp(0, 3, consciousness);

		if(consciousness < 0.1f){
			//navAgent.enabled = false;
			bodyJoint.angularXMotion = ConfigurableJointMotion.Free;
			bodyJoint.angularYMotion = ConfigurableJointMotion.Free;
			bodyJoint.angularZMotion = ConfigurableJointMotion.Free;
			bodyJoint.xMotion = ConfigurableJointMotion.Limited;
			bodyJoint.yMotion = ConfigurableJointMotion.Limited;
			bodyJoint.zMotion = ConfigurableJointMotion.Free;
		}
	}
}