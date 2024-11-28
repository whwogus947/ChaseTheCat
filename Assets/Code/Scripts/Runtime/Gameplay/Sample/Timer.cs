using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

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
            var task = UniTask.Delay(TimeSpan.FromSeconds(seconds))
                .ContinueWith(() => true);

            timerTasks[typeof(T)] = false;
            await task;
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
}
