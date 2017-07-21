#if UNITY_ANDROID
using UnityEngine;
using UnityEngine.UI;
using System;
using DeadMosquito.AndroidGoodies;

namespace AndroidGoodiesExamples
{
    public class DateTimePickerTest : MonoBehaviour
    {
        public Text timeText;
        public Text dateText;

        public void OnPickDateClick()
        {
            var now = DateTime.Now;
            AGDateTimePicker.ShowDatePicker(now.Year, now.Month, now.Day, OnDatePicked, OnDatePickCancel);
        }

        private void OnDatePicked(int year, int month, int day)
        {
            var picked = new DateTime(year, month, day);
            dateText.text = picked.ToString("yyyy MMMMM dd");
        }

        private void OnDatePickCancel()
        {
            dateText.text = "Cancelled picking date";
        }

        public void OnTimePickClick()
        {
            var now = DateTime.Now;
            var theme = AGDialogTheme.Default;
            var is24HourFormat = false;
            AGDateTimePicker.ShowTimePicker(now.Hour, now.Minute, OnTimePicked, OnTimePickCancel, theme, is24HourFormat);
        }

        private void OnTimePicked(int hourOfDay, int minute)
        {
            var picked = new DateTime(2016, 11, 11, hourOfDay, minute, 00);
            timeText.text = picked.ToString("T");
        }

        private void OnTimePickCancel()
        {
            timeText.text = "Cancelled picking time";
        }
    }
}
#endif
