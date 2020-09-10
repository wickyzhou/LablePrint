﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class InventoryBatchNoModel:NotificationObject
    {

        public int MaterialId { get; set; }

        public string MaterialName { get; set; }

        public string MaterialNumber { get; set; }

        public int StockId { get; set; }

        public string StockName { get; set; }

        public string BatchNo { get; set; }

        public float TotalWeight { get; set; }

        private int? transferWeight;

        public int? TransferWeight
        {
            get { return transferWeight; }
            set
            {
                transferWeight = value;
                this.RaisePropertyChanged(nameof(TransferWeight));
            }
        }

    }
}
