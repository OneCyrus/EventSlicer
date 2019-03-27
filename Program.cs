using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EventSlicer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var eventStates = new LinkedList<StateEvent>(new List<StateEvent>
            {
                new StateEvent
                {
                    EventTime = new DateTime(2019, 1, 18, 8, 0, 0),
                    State = StateEnum.Down
                },
                new StateEvent
                {
                    EventTime = new DateTime(2019, 2, 5, 8, 0, 0),
                    State = StateEnum.Ok
                },
                new StateEvent
                {
                    EventTime = new DateTime(2019, 2, 10, 8, 0, 0),
                    State = StateEnum.Down
                },
                new StateEvent
                {
                    EventTime = new DateTime(2019, 2, 15, 8, 0, 0),
                    State = StateEnum.Ok
                },
                new StateEvent
                {
                    EventTime = new DateTime(2019, 2, 20, 8, 0, 0),
                    State = StateEnum.Down
                },
                new StateEvent
                {
                    EventTime = new DateTime(2019, 2, 20, 12, 0, 0),
                    State = StateEnum.Ok
                },
                new StateEvent
                {
                    EventTime = new DateTime(2019, 2, 21, 8, 0, 0),
                    State = StateEnum.Down
                },
                new StateEvent
                {
                    EventTime = new DateTime(2019, 2, 21, 9, 0, 0),
                    State = StateEnum.Ok
                },
                new StateEvent
                {
                    EventTime = new DateTime(2019, 2, 21, 10, 0, 0),
                    State = StateEnum.Down
                },
                new StateEvent
                {
                    EventTime = new DateTime(2019, 2, 21, 11, 0, 0),
                    State = StateEnum.Ok
                },
                new StateEvent
                {
                    EventTime = new DateTime(2019, 3, 18, 8, 0, 0),
                    State = StateEnum.Ok
                },
                new StateEvent
                {
                    EventTime = new DateTime(2019, 3, 20, 8, 0, 0),
                    State = StateEnum.Ok
                }
            });

            var firstDayOfPeriod = new DateTime(2019, 2, 1).StartOfDay();
            var lastDayOfPeriod = new DateTime(2019, 2, 28).EndOfDay();

            var currentEventPointer = eventStates.Last;

            while (currentEventPointer.Value.EventTime > lastDayOfPeriod)
            {
                currentEventPointer = currentEventPointer.Previous;
            }

            Console.WriteLine("first event before the one after the period: " + currentEventPointer.Value.EventTime);

            for (var day = lastDayOfPeriod; day >= firstDayOfPeriod; day = day.AddDays(-1))
            {
                do
                {
                    var endOfEvent = GetEndOfEvent(day, currentEventPointer.Next.Value);
                    var startOfEvent = GetStartOfEvent(day, currentEventPointer.Value);
                    Console.WriteLine($"{day.Date.ToString("dd.MM.yy")} " +
                        $"\t {startOfEvent.EventTime} -> {endOfEvent.EventTime}" +
                        $"\t {(endOfEvent.EventTime - startOfEvent.EventTime).TotalSeconds}" +
                        $"\t {startOfEvent.State.ToString()}");
                }
                while (currentEventPointer.Value.EventTime > day.StartOfDay()
                    && (currentEventPointer = currentEventPointer.Previous) != null);
            }
        }

        private static StateEvent GetStartOfEvent(DateTime day, StateEvent value) =>
            value.EventTime < day.StartOfDay() ?
                new StateEvent { EventTime = day.StartOfDay(), State = value.State }
                : value;

        private static StateEvent GetEndOfEvent(DateTime day, StateEvent value) =>
            value.EventTime > day.EndOfDay()
                ? new StateEvent { EventTime = day.EndOfDay(), State = value.State }
                : value;
    }

    public static class DateHelpers
    {
        public static DateTime StartOfDay(this DateTime theDate) => theDate.Date;

        public static DateTime EndOfDay(this DateTime theDate) => theDate.Date.AddDays(1).AddTicks(-1);
    }

    public class StateEvent
    {
        public DateTime EventTime { get; set; }
        public StateEnum State { get; set; }
    }

    public enum StateEnum {
        Ok,
        Warning,
        Down
    }
}
