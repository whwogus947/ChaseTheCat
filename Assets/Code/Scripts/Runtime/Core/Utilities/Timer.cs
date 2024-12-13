using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com2usGameDev
{
    public class Timer
    {
        private readonly Dictionary<Type, bool> timerTasks = new();

        public void StartTimer<T>(float seconds)
        {
            if (!timerTasks.ContainsKey(typeof(T)) || timerTasks[typeof(T)])
                SetTimer<T>(seconds).Forget();
        }

        private async UniTaskVoid SetTimer<T>(float seconds)
        {
            timerTasks[typeof(T)] = false;
            await UniTask.WaitForSeconds(seconds);
            timerTasks[typeof(T)] = true;
        }

        public void ResetTimer<T>()
        {
            timerTasks[typeof(T)] = false;
        }

        public void EndTimer<T>()
        {
            timerTasks[typeof(T)] = true;
        }

        public bool HasTimerExpired<T>()
        {
            return !timerTasks.ContainsKey(typeof(T)) || timerTasks[typeof(T)];
        }

        public void RemoveTimer<T>()
        {
            timerTasks.Remove(typeof(T));
        }
    }

    public struct CountdownTimer
    {
        private float duration;
        private float remainingTime;

        public CountdownTimer(float durationInSeconds)
        {
            duration = durationInSeconds;
            remainingTime = durationInSeconds;
        }

        public void Start(float durationInSeconds)
        {
            duration = durationInSeconds;
            remainingTime = durationInSeconds;
        }

        public void Tick()
        {
            remainingTime = Mathf.Max(0, remainingTime - Time.deltaTime);
        }

        public readonly float TimeRemaining => remainingTime;

        public readonly bool IsFinished => remainingTime <= 0;

        public void Reset()
        {
            remainingTime = duration;
        }
    }
}
