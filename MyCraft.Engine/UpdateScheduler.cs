using MyCraft.Engine.Abstract;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCraft.Engine
{
    /// <summary>
    /// Update scheudler that fires every 500ms
    /// </summary>
    public class UpdateScheduler : IUpdateScheduler
    {
        private ConcurrentDictionary<Guid, ScheduledAction> _scheduledActions;
        private TimeSpan _updateInterval = TimeSpan.FromMilliseconds(500);

        public UpdateScheduler()
        {
            _scheduledActions = new ConcurrentDictionary<Guid, ScheduledAction>();
        }

        public void Schedule(TimeSpan countdown, Action action)
        {
            var id = Guid.NewGuid();
            _scheduledActions[id] = new ScheduledAction(countdown, action, id);
        }

        public Task Start()
        {
            return Task.Factory.StartNew(() => Update());
        }

        private void Update()
        {
            while (true)
            {
                Thread.Sleep(500);
                foreach(var i in _scheduledActions)
                {
                    i.Value.Countdown = i.Value.Countdown.Subtract(_updateInterval);
                    if (i.Value.Countdown <= TimeSpan.Zero)
                    {
                        try
                        {
                            i.Value.Action.Invoke();
                            ScheduledAction a;
                            _scheduledActions.TryRemove(i.Key, out a);
                        }
                        catch
                        {
                            // TODO do something
                        }
                    }
                }
            }
        }
    }

    internal class ScheduledAction
    {
        public ScheduledAction(TimeSpan countdown, Action action, Guid id)
        {
            Countdown = countdown;
            Action = action;
            Id = id;
        }

        public TimeSpan Countdown { get; set; }

        public Action Action { get; }

        public Guid Id { get; }
    }
}
