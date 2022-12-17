using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Assets.Homework2
{
    public class Excercise2 : MonoBehaviour
    {
        #region Fields

        [Range(1, 1000)] public int ArrayLength;
        [Range(-1000, 1000)] public float MinValue;
        [Range(-1000, 1000)] public float MaxValue;

        private JobHandle _handler;
        private NativeArray<Vector3> _positions;
        private NativeArray<Vector3> _velocities;
        private NativeArray<Vector3> _finalPositions;

        private Vector3[] arr1;
        private Vector3[] arr2;
        private Vector3[] arr3;

        #endregion

        #region UnityMethods

        private void Start()
        {
            arr1 = new Vector3[ArrayLength];
            arr2 = new Vector3[ArrayLength];
            arr3 = new Vector3[ArrayLength];

            for (int i = 0; i < ArrayLength; i++)
            {
                arr1[i] = new Vector3(
                    Random.Range(MinValue, MaxValue),
                    Random.Range(MinValue, MaxValue),
                    Random.Range(MinValue, MaxValue));
                //Debug.Log($"arr1[{i}]:x{arr1[i].x};y{arr1[i].y}");
                arr2[i] = new Vector3(
                    Random.Range(MinValue, MaxValue),
                    Random.Range(MinValue, MaxValue),
                    Random.Range(MinValue, MaxValue));
                //Debug.Log($"arr2[{i}]:x{arr2[i].x};y{arr2[i].y}");
                arr3[i] = Vector3.zero;
            }

            _positions = new NativeArray<Vector3>(arr1, Allocator.Persistent);
            _velocities = new NativeArray<Vector3>(arr2, Allocator.Persistent);
            _finalPositions = new NativeArray<Vector3>(arr3, Allocator.Persistent);
        }

        private void OnDestroy()
        {
            _positions.Dispose();
            _velocities.Dispose();
            _finalPositions.Dispose();
        }

        #endregion

        #region Methods

        public void Calculate()
        {
            SumJob sumJob = new SumJob();
            sumJob.Positions = _positions;
            sumJob.Velocities = _velocities;
            sumJob.FinalPositions = _finalPositions;

            _handler = sumJob.Schedule(ArrayLength, 0);
            _handler.Complete();

            for (int i = 0; i < sumJob.FinalPositions.Length; i++)
            {
                Debug.Log($"SumVector {i}: {sumJob.FinalPositions[i].x}; {sumJob.FinalPositions[i].y}; {sumJob.FinalPositions[i].z}");
            }
        }

        #endregion
    }

    public struct SumJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Vector3> Positions;
        [ReadOnly]
        public NativeArray<Vector3> Velocities;
        public NativeArray<Vector3> FinalPositions;

        public void Execute(int index)
        {
            FinalPositions[index] = Positions[index] + Velocities[index];
        }
    }
}