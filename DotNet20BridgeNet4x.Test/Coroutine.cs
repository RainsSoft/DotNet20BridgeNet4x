using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet20BridgeNet4x.Test
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Diagnostics;
    using System.Linq;

    namespace EasyDeferred.Coroutine.Ellpeck
    {
        /// <summary>
        /// An event is any kind of action that a <see cref="Wait"/> can listen for.
        /// Note that, by default, events don't have a custom <see cref="object.Equals(object)"/> implementation. 
        /// </summary>
        public class Event
        {

        }

        /// <summary>
        /// Represents either an amount of time, or an <see cref="Coroutine.Event"/> that is being waited for by an <see cref="ActiveCoroutine"/>.
        /// </summary>
        public struct Wait
        {

            internal readonly Event Event;
            private double seconds;

            /// <summary>
            /// Creates a new wait that waits for the given <see cref="Coroutine.Event"/>.
            /// </summary>
            /// <param name="evt">The event to wait for</param>
            public Wait(Event evt) {
                this.Event = evt;
                this.seconds = 0;
            }

            /// <summary>
            /// Creates a new wait that waits for the given amount of seconds.
            /// </summary>
            /// <param name="seconds">The amount of seconds to wait for</param>
            public Wait(double seconds) {
                this.seconds = seconds;
                this.Event = null;
            }

            /// <summary>
            /// Creates a new wait that waits for the given <see cref="TimeSpan"/>.
            /// Note that the exact value may be slightly different, since waits operate in <see cref="TimeSpan.TotalSeconds"/> rather than ticks.
            /// </summary>
            /// <param name="time">The time span to wait for</param>
            public Wait(TimeSpan time) : this(time.TotalSeconds) {
            }

            internal bool Tick(double deltaSeconds) {
                this.seconds -= deltaSeconds;
                return this.seconds <= 0;
            }

        }
        /// <summary>
        /// A reference to a currently running coroutine.
        /// This is returned by <see cref="CoroutineHandler.Start(System.Collections.Generic.IEnumerator{Coroutine.Wait},string,int)"/>.
        /// </summary>
        public class ActiveCoroutine : IComparable<ActiveCoroutine>
        {

            private readonly IEnumerator<Wait> enumerator;
            private readonly Stopwatch stopwatch;
            private Wait current;

            internal Event Event => this.current.Event;
            internal bool IsWaitingForEvent => this.Event != null;

            /// <summary>
            /// This property stores whether or not this active coroutine is finished.
            /// A coroutine is finished if all of its waits have passed, or if it <see cref="WasCanceled"/>.
            /// </summary>
            public bool IsFinished { get; private set; }
            /// <summary>
            /// This property stores whether or not this active coroutine was cancelled using <see cref="Cancel"/>.
            /// </summary>
            public bool WasCanceled { get; private set; }
            /// <summary>
            /// The total amount of time that <see cref="MoveNext"/> took.
            /// This is the amount of time that this active coroutine took for the entirety of its "steps", or yield statements.
            /// </summary>
            public TimeSpan TotalMoveNextTime { get; private set; }
            /// <summary>
            /// The total amount of times that <see cref="MoveNext"/> was invoked.
            /// This is the amount of "steps" in your coroutine, or the amount of yield statements.
            /// </summary>
            public int MoveNextCount { get; private set; }
            /// <summary>
            /// The amount of time that the last <see cref="MoveNext"/> took.
            /// This is the amount of time that this active coroutine took for the last "step", or yield statement.
            /// </summary>
            public TimeSpan LastMoveNextTime { get; private set; }

            /// <summary>
            /// An event that gets fired when this active coroutine finishes or gets cancelled.
            /// When this event is called, <see cref="IsFinished"/> is always true.
            /// </summary>
            public event FinishCallback OnFinished;
            /// <summary>
            /// The name of this coroutine.
            /// When not specified on startup of this coroutine, the name defaults to an empty string.
            /// </summary>
            public readonly string Name;
            /// <summary>
            /// The priority of this coroutine. The higher the priority, the earlier it is advanced compared to other coroutines that advance around the same time.
            /// When not specified at startup of this coroutine, the priority defaults to 0.
            /// </summary>
            public readonly int Priority;

            internal ActiveCoroutine(IEnumerator<Wait> enumerator, string name, int priority, Stopwatch stopwatch) {
                this.enumerator = enumerator;
                this.Name = name;
                this.Priority = priority;
                this.stopwatch = stopwatch;
            }

            /// <summary>
            /// Cancels this coroutine, causing all subsequent <see cref="Wait"/>s and any code in between to be skipped.
            /// </summary>
            /// <returns>Whether the cancellation was successful, or this coroutine was already cancelled or finished</returns>
            public bool Cancel() {
                if (this.IsFinished || this.WasCanceled)
                    return false;
                this.WasCanceled = true;
                this.IsFinished = true;
                this.OnFinished?.Invoke(this);
                return true;
            }

            internal bool Tick(double deltaSeconds) {
                if (!this.WasCanceled && this.current.Tick(deltaSeconds))
                    this.MoveNext();
                return this.IsFinished;
            }

            internal bool OnEvent(Event evt) {
                if (!this.WasCanceled && Equals(this.current.Event, evt))
                    this.MoveNext();
                return this.IsFinished;
            }

            internal bool MoveNext() {
                //this.stopwatch.Restart();
                this.stopwatch.Reset();
                this.stopwatch.Start();
                var result = this.enumerator.MoveNext();
                this.stopwatch.Stop();
                this.LastMoveNextTime = this.stopwatch.Elapsed;
                this.TotalMoveNextTime += this.stopwatch.Elapsed;
                this.MoveNextCount++;

                if (!result) {
                    this.IsFinished = true;
                    this.OnFinished?.Invoke(this);
                    return false;
                }
                this.current = this.enumerator.Current;
                return true;
            }

            /// <summary>
            /// A delegate method used by <see cref="ActiveCoroutine.OnFinished"/>.
            /// </summary>
            /// <param name="coroutine">The coroutine that finished</param>
            public delegate void FinishCallback(ActiveCoroutine coroutine);

            /// <inheritdoc />
            public int CompareTo(ActiveCoroutine other) {
                return other.Priority.CompareTo(this.Priority);
            }

        }

        /// <summary>
        /// This class can be used for static coroutine handling of any kind.
        /// Note that it uses an underlying <see cref="CoroutineHandlerInstance"/> object for management.
        /// </summary>
        public static class CoroutineHandler
        {

            private static readonly CoroutineHandlerInstance Instance = new CoroutineHandlerInstance();

            /// <inheritdoc cref="CoroutineHandlerInstance.TickingCount"/>
            public static int TickingCount => Instance.TickingCount;
            /// <inheritdoc cref="CoroutineHandlerInstance.EventCount"/>
            public static int EventCount => Instance.EventCount;

            /// <inheritdoc cref="CoroutineHandlerInstance.Start(IEnumerable{Wait},string,int)"/>
            public static ActiveCoroutine Start(IEnumerable<Wait> coroutine, string name = "", int priority = 0) {
                return Instance.Start(coroutine, name, priority);
            }

            /// <inheritdoc cref="CoroutineHandlerInstance.Start(IEnumerator{Wait},string,int)"/>
            public static ActiveCoroutine Start(IEnumerator<Wait> coroutine, string name = "", int priority = 0) {
                return Instance.Start(coroutine, name, priority);
            }

            /// <inheritdoc cref="CoroutineHandlerInstance.InvokeLater"/>
            public static ActiveCoroutine InvokeLater(Wait wait, Action action, string name = "", int priority = 0) {
                return Instance.InvokeLater(wait, action, name, priority);
            }

            /// <inheritdoc cref="CoroutineHandlerInstance.Tick(double)"/>
            public static void Tick(double deltaSeconds) {
                Instance.Tick(deltaSeconds);
            }
            /// <inheritdoc cref="CoroutineHandlerInstance.Tick(TimeSpan)"/>
            public static void Tick(TimeSpan delta) {
                Instance.Tick(delta);
            }

            /// <inheritdoc cref="CoroutineHandlerInstance.RaiseEvent"/>
            public static void RaiseEvent(Event evt) {
                Instance.RaiseEvent(evt);
            }

            /// <inheritdoc cref="CoroutineHandlerInstance.GetActiveCoroutines"/>
            public static IEnumerable<ActiveCoroutine> GetActiveCoroutines() {
                return Instance.GetActiveCoroutines();
            }

        }
        /// <summary>
        /// An object of this class can be used to start, tick and otherwise manage active <see cref="ActiveCoroutine"/>s as well as their <see cref="Event"/>s.
        /// Note that a static implementation of this can be found in <see cref="CoroutineHandler"/>.
        /// </summary>
        public class CoroutineHandlerInstance
        {
            struct KeyValuePair2<TKey, TValue>
            {
                public KeyValuePair2(TKey key, TValue value) {
                    Item1 = key;
                    Item2 = value;
                }
                public TKey Item1 { get; }
                public TValue Item2 { get; }

            }
            private readonly List<ActiveCoroutine> tickingCoroutines = new List<ActiveCoroutine>();
            private readonly Dictionary<Event, List<ActiveCoroutine>> eventCoroutines = new Dictionary<Event, List<ActiveCoroutine>>();
            private readonly HashSet<KeyValuePair2<Event, ActiveCoroutine>> eventCoroutinesToRemove = new HashSet<KeyValuePair2<Event, ActiveCoroutine>>(); //new HashSet<(Event, ActiveCoroutine)>();
            private readonly HashSet<ActiveCoroutine> outstandingEventCoroutines = new HashSet<ActiveCoroutine>();
            private readonly HashSet<ActiveCoroutine> outstandingTickingCoroutines = new HashSet<ActiveCoroutine>();
            private readonly Stopwatch stopwatch = new Stopwatch();
            private readonly object lockObject = new object();

            /// <summary>
            /// The amount of <see cref="ActiveCoroutine"/> instances that are currently waiting for a tick (waiting for time to pass)
            /// </summary>
            public int TickingCount {
                get {
                    lock (this.lockObject)
                        return this.tickingCoroutines.Count;
                }
            }
            /// <summary>
            /// The amount of <see cref="ActiveCoroutine"/> instances that are currently waiting for an <see cref="Event"/>
            /// </summary>
            public int EventCount {
                get {
                    lock (this.lockObject) {
                        //return this.eventCoroutines.Sum(c => c.Value.Count);
                        return this.eventCoroutines.Select(c => c.Value.Count).Sum();
                    }
                }
            }

            /// <summary>
            /// Starts the given coroutine, returning a <see cref="ActiveCoroutine"/> object for management.
            /// Note that this calls <see cref="IEnumerable{T}.GetEnumerator"/> to get the enumerator.
            /// </summary>
            /// <param name="coroutine">The coroutine to start</param>
            /// <param name="name">The <see cref="ActiveCoroutine.Name"/> that this coroutine should have. Defaults to an empty string.</param>
            /// <param name="priority">The <see cref="ActiveCoroutine.Priority"/> that this coroutine should have. The higher the priority, the earlier it is advanced. Defaults to 0.</param>
            /// <returns>An active coroutine object representing this coroutine</returns>
            public ActiveCoroutine Start(IEnumerable<Wait> coroutine, string name = "", int priority = 0) {
                return this.Start(coroutine.GetEnumerator(), name, priority);
            }

            /// <summary>
            /// Starts the given coroutine, returning a <see cref="ActiveCoroutine"/> object for management.
            /// </summary>
            /// <param name="coroutine">The coroutine to start</param>
            /// <param name="name">The <see cref="ActiveCoroutine.Name"/> that this coroutine should have. Defaults to an empty string.</param>
            /// <param name="priority">The <see cref="ActiveCoroutine.Priority"/> that this coroutine should have. The higher the priority, the earlier it is advanced compared to other coroutines that advance around the same time. Defaults to 0.</param>
            /// <returns>An active coroutine object representing this coroutine</returns>
            public ActiveCoroutine Start(IEnumerator<Wait> coroutine, string name = "", int priority = 0) {
                var inst = new ActiveCoroutine(coroutine, name, priority, this.stopwatch);
                if (inst.MoveNext()) {
                    lock (this.lockObject)
                        this.GetOutstandingCoroutines(inst.IsWaitingForEvent).Add(inst);
                }
                return inst;
            }

            /// <summary>
            /// Causes the given action to be invoked after the given <see cref="Wait"/>.
            /// This is equivalent to a coroutine that waits for the given wait and then executes the given <see cref="Action"/>.
            /// </summary>
            /// <param name="wait">The wait to wait for</param>
            /// <param name="action">The action to execute after waiting</param>
            /// <param name="name">The <see cref="ActiveCoroutine.Name"/> that the underlying coroutine should have. Defaults to an empty string.</param>
            /// <param name="priority">The <see cref="ActiveCoroutine.Priority"/> that the underlying coroutine should have. The higher the priority, the earlier it is advanced compared to other coroutines that advance around the same time. Defaults to 0.</param>
            /// <returns>An active coroutine object representing this coroutine</returns>
            public ActiveCoroutine InvokeLater(Wait wait, Action action, string name = "", int priority = 0) {
                return this.Start(InvokeLaterImpl(wait, action), name, priority);
            }

            /// <summary>
            /// Ticks this coroutine handler, causing all time-based <see cref="Wait"/>s to be ticked.
            /// </summary>
            /// <param name="deltaSeconds">The amount of seconds that have passed since the last time this method was invoked</param>
            public void Tick(double deltaSeconds) {
                lock (this.lockObject) {
                    this.MoveOutstandingCoroutines(false);
                    this.tickingCoroutines.RemoveAll(c => {
                        if (c.Tick(deltaSeconds)) {
                            return true;
                        }
                        else if (c.IsWaitingForEvent) {
                            this.outstandingEventCoroutines.Add(c);
                            return true;
                        }
                        return false;
                    });
                }
            }

            /// <summary>
            /// Ticks this coroutine handler, causing all time-based <see cref="Wait"/>s to be ticked.
            /// This is a convenience method that calls <see cref="Tick(double)"/>, but accepts a <see cref="TimeSpan"/> instead of an amount of seconds.
            /// </summary>
            /// <param name="delta">The time that has passed since the last time this method was invoked</param>
            public void Tick(TimeSpan delta) {
                this.Tick(delta.TotalSeconds);
            }

            /// <summary>
            /// Raises the given event, causing all event-based <see cref="Wait"/>s to be updated.
            /// </summary>
            /// <param name="evt">The event to raise</param>
            public void RaiseEvent(Event evt) {
                lock (this.lockObject) {
                    this.MoveOutstandingCoroutines(true);
                    var coroutines = this.GetEventCoroutines(evt, false);
                    if (coroutines != null) {
                        for (var i = 0; i < coroutines.Count; i++) {
                            var c = coroutines[i];
                            KeyValuePair2<Event, ActiveCoroutine> tup = new KeyValuePair2<Event, ActiveCoroutine>(c.Event, c);
                            if (this.eventCoroutinesToRemove.Contains(tup))
                                continue;
                            if (c.OnEvent(evt)) {
                                this.eventCoroutinesToRemove.Add(tup);
                            }
                            else if (!c.IsWaitingForEvent) {
                                this.eventCoroutinesToRemove.Add(tup);
                                this.outstandingTickingCoroutines.Add(c);
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Returns a list of all currently active <see cref="ActiveCoroutine"/> objects under this handler.
            /// </summary>
            /// <returns>All active coroutines</returns>
            public IEnumerable<ActiveCoroutine> GetActiveCoroutines() {
                lock (this.lockObject)
                    return this.tickingCoroutines.Concat(this.eventCoroutines.Values.SelectMany(c => c));
            }

            private void MoveOutstandingCoroutines(bool evt) {
                // RemoveWhere is twice as fast as iterating and then clearing
                if (this.eventCoroutinesToRemove.Count > 0) {
                    this.eventCoroutinesToRemove.RemoveWhere(c => {
                        this.GetEventCoroutines(c.Item1, false).Remove(c.Item2);
                        return true;
                    });
                }
                var coroutines = this.GetOutstandingCoroutines(evt);
                if (coroutines.Count > 0) {
                    coroutines.RemoveWhere(c => {
                        var list = evt ? this.GetEventCoroutines(c.Event, true) : this.tickingCoroutines;
                        var position = list.BinarySearch(c);
                        list.Insert(position < 0 ? ~position : position, c);
                        return true;
                    });
                }
            }

            private HashSet<ActiveCoroutine> GetOutstandingCoroutines(bool evt) {
                return evt ? this.outstandingEventCoroutines : this.outstandingTickingCoroutines;
            }

            private List<ActiveCoroutine> GetEventCoroutines(Event evt, bool create) {
                if (!this.eventCoroutines.TryGetValue(evt, out var ret) && create) {
                    ret = new List<ActiveCoroutine>();
                    this.eventCoroutines.Add(evt, ret);
                }
                return ret;
            }

            private static IEnumerator<Wait> InvokeLaterImpl(Wait wait, Action action) {
                yield return wait;
                action();
            }

        }

    }

}
