using System;
using UnityEngine;

public class WeaponIK : MonoBehaviour
{
    [Serializable]
    public class HumanBone
    { 
        public HumanBodyBones bone;
        public float weight = 1.0f;
    }

    public Transform targetTransform;
    public Transform aimTransform;

    public HumanBone[] humanBones;
    Transform[] boneTransforms;

    public int interations = 10;
    [Range(0,1)]
    public float weight = 1.0f;
    public float angleLimit = 90.0f;
    public float distanceLimit = 1.5f;


    private void Start()
    {
        Animator animator = GetComponentInChildren<Animator>();
        boneTransforms = new Transform[humanBones.Length];
        for (int i = 0; i < boneTransforms.Length; i++)
        {
            boneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone);
        }
    }

    Vector3 GetTargetPosition()
    {
        Vector3 _targetDirection = targetTransform.position - aimTransform.position;
        Vector3 _aimDirection = aimTransform.forward;
        float _blendOut = 0.0f;

        float _targetAngle = Vector3.Angle(_targetDirection, _aimDirection);
        if(_targetAngle > angleLimit)
        {
            _blendOut += (_targetAngle - angleLimit) / 50.0f;
        }

        float _targetDistance = _targetDirection.magnitude;
        if(_targetDistance < distanceLimit)
        {
            _blendOut += distanceLimit - _targetDistance;
        }

        Vector3 _direction = Vector3.Slerp(_targetDirection, _aimDirection, _blendOut);
        return aimTransform.position + _direction;
    }

    private void LateUpdate()
    {
        if(aimTransform == null)
        {
            return;
        }

        if (targetTransform == null)
        {
            return;
        }
        Vector3 _targetPosition = GetTargetPosition();

        for(int i = 0; i < interations; i++)
        {
            for(int j = 0; j < boneTransforms.Length; j++)
            {
                Transform _bone = boneTransforms[j];
                float _boneWeight = humanBones[j].weight * weight;
                AimAtTarget(_bone, _targetPosition, _boneWeight);
            }
            
        }
        
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        Vector3 _aimDirection = aimTransform.forward;
        Vector3 _targetDirection = targetPosition - aimTransform.position;
        Quaternion _aimTowards = Quaternion.FromToRotation(_aimDirection, _targetDirection);
        Quaternion _blendRotation = Quaternion.Slerp(Quaternion.identity, _aimTowards, weight);
        bone.rotation = _blendRotation * bone.rotation;

    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    public void SetAimTransform(Transform aim)
    {
        aimTransform = aim;
    }
}
