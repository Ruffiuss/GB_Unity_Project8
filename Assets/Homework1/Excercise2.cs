using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Homework1
{
    public class Excercise2 : MonoBehaviour
    {
        #region Fields

        public CancellationTokenSource CancellationTokenSource1;
        public CancellationTokenSource CancellationTokenSource2;
        private CancellationToken _cancellationToken1;
        private CancellationToken _cancellationToken2;

        private int _task1Delay = 1000;
        private int _task1DelayDivisionFactor = 10;
        private int _task2Delay = 60;

        #endregion

        #region UnityMethods

        private void Start()
        {
            CancellationTokenSource1 = new CancellationTokenSource();
            CancellationTokenSource2 = new CancellationTokenSource();
            _cancellationToken1 = CancellationTokenSource1.Token;
            _cancellationToken2 = CancellationTokenSource2.Token;
        }

        private void OnDestroy()
        {
            CancellationTokenSource1.Dispose();
            CancellationTokenSource2.Dispose();
        }

        #endregion

        #region Methods

        public void ButtonTask1OnClick() => StartTask1();
        public void ButtonTask2OnClick() => StartTask2();

        public Task StartTask1() => Task.Run(() => Delay1(_cancellationToken1));
        public Task StartTask2() => Task.Run(() => Delay2(_cancellationToken2));

        async void Delay1(CancellationToken cancellationToken)
        {
            var ms = 0;
            while (ms != _task1Delay)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Debug.Log("Task1 cancelled");
                    return;
                }
                await Task.Delay(_task1Delay / _task1DelayDivisionFactor);
                ms += _task1Delay / _task1DelayDivisionFactor;
            }
            Debug.Log("Task1 complete");
        }

        async void Delay2(CancellationToken cancellationToken)
        {
            var frame = 0;
            while (frame < _task2Delay)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Debug.Log("Task2 cancelled");
                    return;
                }
                await Task.Yield();
                frame++;
            }
            Debug.Log("Task2 complete");
        }

        #endregion
    }
}