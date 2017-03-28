﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MoneyFox.Foundation.Models
{
    public class StatisticItem : INotifyPropertyChanged
    {
        private string label;
        private double value;

        /// <summary>
        ///     Value of this item
        /// </summary>
        public double Value
        {
            get { return value; }
            set
            {
                if (Math.Abs(this.value - value) < 0.01) return;
                this.value = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Label to show in the chart
        /// </summary>
        public string Label
        {
            get { return label; }
            set
            {
                if (label == value) return;
                label = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}