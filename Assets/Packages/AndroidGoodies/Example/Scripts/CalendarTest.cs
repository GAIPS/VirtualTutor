﻿#if UNITY_ANDROID
using DeadMosquito.AndroidGoodies;
using System;
using UnityEngine;

namespace AndroidGoodiesExamples {
    public class CalendarTest : MonoBehaviour
    {
        public void OnCreateEventClick()
        {
            var beginTime = DateTime.Now;
            var endTime = beginTime.AddHours(2);
            var eventBuilder = new AGCalendar.EventBuilder("Lunch with someone special", beginTime);
            eventBuilder.SetEndTime(endTime);
            eventBuilder.SetIsAllDay(false);
            eventBuilder.SetLocation("Miami beach");
            eventBuilder.SetDescription("Amazing lunch with a beautiful lady");
            eventBuilder.SetEmails(new [] { "lol@gmail.com", "test@gmail.com" });
            eventBuilder.SetRRule("FREQ=DAILY");
            eventBuilder.SetAccessLevel(AGCalendar.EventAccessLevel.Public);
            eventBuilder.SetAvailability(AGCalendar.EventAvailability.Free);
            eventBuilder.BuildAndShow();
        }

        public void OnOpenCalendarDateClick()
        {
            // Open calendar on a date a week ahead
            AGCalendar.OpenCalendarForDate(DateTime.Now.AddDays(7));
        }
    }
}
#endif
