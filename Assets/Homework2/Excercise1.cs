using System.Collections;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Assets.Homework2
{
    public class Excercise1 : MonoBehaviour
    {
        #region Fields

        public int[] Numbers;

        private NativeArray<int> _arrayNum;
        private JobHandle handle;

        #endregion

        #region UnityMethods

        private void Start()
        {
            _arrayNum = new NativeArray<int>(Numbers, Allocator.Persistent);
        }

        private void OnDestroy()
        {
            _arrayNum.Dispose();
        }

        #endregion

        #region Methods

        public void Calculate()
        {
            LessThanTen job = new LessThanTen();
            job.array = _arrayNum;

            handle = job.Schedule();
            handle.Complete();

            Debug.Log("Output array:");
            for (int i = 0; i < _arrayNum.Length; i++)
                Debug.Log($"[{i}]{_arrayNum[i]}");
        }

        #endregion
    }

    public struct LessThanTen : IJob
    {
        public NativeArray<int> array;

        void IJob.Execute()
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > 10)
                    array[i] = 0;
            }
        }
    }
}