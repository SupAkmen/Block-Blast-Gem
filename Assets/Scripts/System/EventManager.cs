using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventManager
{
     private static readonly Dictionary<EGameEvent, object> events = new();

     public static Event<T> GetEvent<T>(EGameEvent eventName)
     {
          if (events.TryGetValue(eventName, out var e) && e is Event<T> typeEvent)
          {
               return typeEvent;
          }
          
          var newEvent = new Event<T>();
          events[eventName] = newEvent;
          return newEvent;
     }

     public static Event GetEvent(EGameEvent eventName)
     {
          if (events.TryGetValue(eventName, out var e) && e is Event typeEvent)
          {
               return typeEvent;
          }
          
          var newEvent = new Event();
          events[eventName] = newEvent;
          return newEvent;
     }

     public static Dictionary<EGameEvent, object> GetSubcribeEvents()
     {
          return events;
     }

     public static Action<EGameState> OnGameStateChanged;
     private static EGameState gameStatus;

     public static EGameState GameStatus
     {
          get => gameStatus;

          set
          {
               gameStatus = value;
               OnGameStateChanged?.Invoke(gameStatus);
          }
     }
}

public class Event
{
     private event Action _event;

     public void Subscribe(Action subscriber)
     {
          _event += subscriber;
     }

     public void Unsubscribe(Action subscriber)
     {
          _event -= subscriber;
     }

     public void Invoke()
     {
          _event?.Invoke();
     }
}
