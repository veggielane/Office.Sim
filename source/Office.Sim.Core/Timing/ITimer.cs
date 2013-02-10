using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Office.Sim.Core.Timing
{
    public interface ITimer
    {
        IObservable<ITick> Ticks { get; }
        void Start();
        void Stop();

        TimeSpan Delta { get;  }
        Tick LastTickTime { get; }
    }

    public interface ITick
    {

        TimeSpan GameTimeElapsed { get; }
        TimeSpan GameTimeDelta { get; }
        long TickCount { get; }

        void Update(TimeSpan delta);
    }

    public class Tick : ITick
    {
        public TimeSpan GameTimeElapsed { get; private set; }
        public TimeSpan GameTimeDelta { get; private set; }
        public long TickCount { get; private set; }

        public void Update(TimeSpan delta)
        {
            GameTimeDelta = delta;
            GameTimeElapsed += GameTimeDelta;
            TickCount++;
        }
    }

    public class Timer:ITimer
    {
        public IObservable<ITick> Ticks { get; private set; }

        public TimeSpan Delta { get; private set; }
        public Tick LastTickTime { get; private set; }

        private readonly ISubject<ITick> _subject;

        private readonly IObservable<Int64> _timer; 

        public Timer()
        {
            _subject = new Subject<ITick>();

            Delta = TimeSpan.FromMilliseconds(1);
            _timer = Observable.Interval(Delta);
            Ticks = _subject.AsObservable();

            LastTickTime = new Tick();
        }

        private IDisposable _sub;

        public void Start()
        {

            _sub = _timer.Subscribe(t =>
            {
                LastTickTime.Update(Delta);
                _subject.OnNext(LastTickTime);
            });
        }

        public void Stop()
        {
            if(_sub != null)_sub.Dispose();
        }
    }
}
