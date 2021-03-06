﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ShippingBillEntryModel:NotificationObject
    {
        public bool IsSystem { get; set; }

        public int Id { get; set; }

        public int MainId { get; set; }

        public int EntryId { get; set; }

        public int CustId { get; set; }

        public int DeptId { get; set; }

        public int CaseId { get; set; }

        public int BrandId { get; set; }

        public string CustName { get; set; }

        public string DeptName { get; set; }

        public string CaseName { get; set; }

        public string BrandName { get; set; }


        private float quantity;

        public float Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                this.RaisePropertyChanged(nameof(Quantity));
                //if(GoodsType==3)
                //{
                //    ApportionedAmount = (float)(Quantity * 0.81);
                //}
            }
        }


        //private float totalQuantity;

        //public float TotalQuantity
        //{
        //    get { return totalQuantity; }
        //    set
        //    {
        //        totalQuantity = value;
        //        this.RaisePropertyChanged(nameof(TotalQuantity));
        //    }
        //}



        //private float totalAmount;

        //public float TotalAmount
        //{
        //    get { return totalAmount; }
        //    set
        //    {
        //        totalAmount = value;
        //        this.RaisePropertyChanged(nameof(TotalAmount));
        //    }
        //}

        private int goodsType;

        public int GoodsType
        {
            get { return goodsType; }
            set
            {
                goodsType = value;
                this.RaisePropertyChanged(nameof(GoodsType));
            }
        }



        private float amount;

        public float Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                this.RaisePropertyChanged(nameof(Amount));
            }
        }


        private string note;

        public string Note
        {
            get { return note; }
            set
            {
                note = value;
                this.RaisePropertyChanged(nameof(Note));
            }
        }



    }
}
