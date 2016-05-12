﻿using CommonCounter.Interface;

namespace CommonCounter
{
    public abstract class BaseCounter<T> : ICounter<T> where T : ICounterModel
    {
        private T counter { get; set; }

        private bool hasValue { get; set; }
        private int calculateNewValue { get; set; }
        private bool isEqualMaxValue { get; set; }
        private bool initialStatus { get; set; }

        protected BaseCounter(T counter)
        {
            this.counter = counter;
        }

        public virtual bool Initial()
        {
            initialStatus = false;
            if (!HasData(counter)) return false;

            initialStatus = true;
            hasValue = HasValue(counter.Value);
            if (!hasValue) return true;

            calculateNewValue = CalculateNewValue((int)counter.Value, counter.IncrementValue);
            isEqualMaxValue = IsEqualMaxValue(counter.MaxValue, calculateNewValue);
            return true;
        }

        public virtual T GetNextCounterData()
        {
            if (!initialStatus) return default(T);

            this.counter.Value = CalcNextCounterValue(hasValue, counter.InitialValue, calculateNewValue, isEqualMaxValue, counter.Recyle);

            return this.counter;
        }

        protected virtual int? CalcNextCounterValue(bool hasValue, int initialValue, int calculateNewValue, bool isEqualMaxValue, bool recyle)
        {
            if (!hasValue) return initialValue;

            if (!isEqualMaxValue) return calculateNewValue;

            if (recyle) return initialValue;

            return null;
        }

        protected virtual bool IsEqualMaxValue(int maxValue, int calcNewValue)
        {
            return maxValue < calcNewValue;
        }

        protected virtual int CalculateNewValue(int value, int incrementvalue)
        {
            return value + incrementvalue;
        }

        private static bool HasValue(int? value)
        {
            return value != null;
        }

        private static bool HasData(object data)
        {
            return data != null;
        }
    }
}