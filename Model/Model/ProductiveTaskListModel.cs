using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    ///  此字段完全对应数据库表字段，顺序不能改变，模板数据导入功能后续批量插入 
    /// </summary>
    public class ProductiveTaskListModel:NotificationObject
    {   
        public int ID { get; set; }

        public int FInterID { get; set; }

        public string FWorkNo { get; set; }

        public int FSEOrderEntryID { get; set; }

        public DateTime FProductionDate { get; set; }

        public string FType { get; set; }

        public int FitemID { get; set; }

        public string FitemName { get; set; }

        public string FBatchNo { get; set; }

        public decimal FQuantity { get; set; }

        public string FHasSmallMaterial { get; set; }

        public string FPackage { get; set; }

        public string FPackageSpecification { get; set; }

        public string FBucketName { get; set; }

        public string FOrgID { get; set; }

        public string FLabel { get; set; }

        public string FBillNo { get; set; }

        public DateTime CreateDate { get; set; }

        public string FNote { get; set; }

        public string FReservedSample { get; set; }

        public int? FStockInCount { get; set; }

        public int? FRecievedCount { get; set; }

        public decimal FResidue { get; set; }

        private string fAuditTip;

        public string FAuditTip
        {
            get { return fAuditTip; }
            set
            {
                fAuditTip = value;
                this.RaisePropertyChanged(nameof(FAuditTip));
            }
        }

        public Byte[] RowHashValue { get; set; }

        public string SafeCode { get; set; }

        public decimal RowQuantity { get; set; }

        public string ProductionType { get; set; }

        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                this.RaisePropertyChanged(nameof(IsChecked));
            }
        }

        public int PaintSampleTotal { get; set; }

        public string BrandName { get; set; }


    }
}
