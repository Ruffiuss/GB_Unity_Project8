using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Homework1
{
    public class Excercise3 : MonoBehaviour
    {
        #region Fields

        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;

        [SerializeField] private Excercise2 _excercise2;

        #endregion

        #region UnityMethods

        private void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Dispose();
        }

        #endregion

        #region Methods

        public void StartTask()
        {
            //Task task3 = Task.Run(() => WhatTaskFasterAsync(
            //    _cancellationToken,
            //    _excercise2.StartTask1(),
            //    _excercise2.StartTask2()
            //    ));
            //while (task3.IsCompleted || task3.IsCanceled)
            //{
            //    Debug.Log($"Task3 status: {task3.Status}");
            //}
            //Debug.Log($"Task3 {task3.Status}");
        }

        //private Task<bool> WhatTaskFasterAsync(CancellationToken ct, Task task1, Task task2)
        //{

        //    bool isComplete = false;
        //    while (!isComplete)
        //    {
        //        if (ct.IsCancellationRequested)
        //        {
        //            Debug.Log("Task3 canceled by token");
        //            return false;
        //        }
        //        return Task.WaitAny(task1, task2);
        //    }
        //    return false;
        //}

        #endregion
    }
}