﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class SalesRebateAmountRangeModel:NotificationObject
    {
        public int Id { get; set; }

        private double amountLower;

        public double AmountLower
        {
            get { return amountLower; }
            set
            {
                amountLower = value;
                this.RaisePropertyChanged(nameof(AmountLower));
            }
        }


        private double amountUpper;

        public double AmountUpper
        {
            get { return amountUpper; }
            set
            {
                amountUpper = value;
                this.RaisePropertyChanged(nameof(AmountUpper));
            }
        }


        private double salesRebatePctValue;

        public double SalesRebatePctValue
        {
            get { return salesRebatePctValue; }
            set
            {
                salesRebatePctValue = value;
                this.RaisePropertyChanged(nameof(SalesRebatePctValue));
            }
        }

        private DateTime effectiveDate;

        public DateTime EffectiveDate
        {
            get { return effectiveDate; }
            set
            {
                effectiveDate = value;
                this.RaisePropertyChanged(nameof(EffectiveDate));
            }
        }

        private DateTime expirationDate;

        public DateTime ExpirationDate
        {
            get { return expirationDate; }
            set
            {
                expirationDate = value;
                this.RaisePropertyChanged(nameof(ExpirationDate));
            }
        }


        public DateTime CreateTime { get; set; }

        public Guid Guid { get; set; }

    }
}
