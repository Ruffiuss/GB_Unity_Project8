using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

namespace Assets.Homework2
{
    public class Excercise3 : MonoBehaviour
    {
        #region Fields

        public Transform[] Targets;
        public Vector3 TargetVector;

        public int RotateSpeed;
        public bool IsRotating;

        private JobHandle _handler;
        private TransformAccessArray _transformAccessArray;

        private float _angle = 0;

        #endregion

        #region UnityMethods

        private void Start()
        {
            _transformAccessArray = new TransformAccessArray(Targets);
        }

        void Update()
        {
            if (IsRotating)
            {
                RotateJob rotateJob = new RotateJob();
                rotateJob.RotationAngle = _angle;
                rotateJob.RotationVector = TargetVector;

                _handler = rotateJob.Schedule(_transformAccessArray);

                if (_angle == 180)
                    _angle = 0;
                else
                    _angle += RotateSpeed;
            }
            else
            {
                for (int i = 0; i < Targets.Length; i++)
                {
                    Targets[i].localRotation = Quaternion.AngleAxis(0, TargetVector);
                }
                _handler.Complete();
            }
        }

        private void OnDestroy()
        {
            _transformAccessArray.Dispose();
        }

        #endregion

        #region Methods

        public void Calculate()
        {
            IsRotating = !IsRotating;
        }

        #endregion
    }

    public struct RotateJob : IJobParallelForTransform
    {
        public float RotationAngle;
        public Vector3 RotationVector;

        public void Execute(int index, TransformAccess transform)
        {
            transform.localRotation = Quaternion.AngleAxis(RotationAngle, RotationVector);
        }
    }
}