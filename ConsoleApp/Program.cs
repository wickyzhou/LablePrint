using Common;
using K3ApiModel;
using K3ApiModel.Organization;
using K3ApiModel.Request;
using K3ApiModel.Response;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Ui.Helper;
using Ui.Service;

namespace ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            // 获取最近结束并且允许发货的流程数据
            DataTable data = SqlHelper.ExecuteDataTableOa(@" select * from SJKeHuFaHuoShenQingView ");
            if (data.Rows.Count > 0)
            {
                var lists = SqlHelper.DataTableToModelList<KeHuFaHuoShenQingModel>(data);

                foreach (var item in lists)
                {
                    // 构造Json数据
                    var org = new OrganizationService().GetOrganizationByNumber(item.OrgNumber);
                    if (org != null)
                    {
                        var requestModel = new K3ApiUpdateRequestModel<CustomerUpdateFStatusModel>
                        {
                            Data = new K3ApiUpdateDataRequestModel<CustomerUpdateFStatusModel>
                            {
                                FNumber = org.FNumber,
                                Data = new CustomerUpdateFStatusModel { FNumber = org.FNumber, FName = org.FName, FStatus = new BaseIdNameX { FID = "ZT01", FName = "使用" } }
                            }
                        };

                        string postJson = SerializationHelper.ObjectToJson(requestModel);

                        // API插入到后台
                        K3ApiUpdateDataResponseModel response = new K3ApiService().Update("Customer", postJson);

                        // 写后台更改记录
                        SqlHelper.ExecuteNonQueryOa(@" insert into SJOAKeHuFaHuoShenQingLog(FlowId,FlowName,FlowDate,OrgNumber,Permission,OrgSourceStatus,OrgTargetStatus,Data,DataDesc,FItemID,FNumber,FStatus) 
                            values      (@FlowId,@FlowName,@FlowDate,@OrgNumber,@Permission,@OrgSourceStatus,@OrgTargetStatus,@Data,@DataDesc,@FItemID,@FNumber,@FStatus)"
                                , new SqlParameter[]
                                {
                                        new SqlParameter("@FlowId", item.FlowId),
                                        new SqlParameter("@FlowName", item.FlowName),
                                        new SqlParameter("@FlowDate", item.FlowDate) ,
                                        new SqlParameter("@OrgNumber", item.OrgNumber) ,
                                        new SqlParameter("@Permission", item.Permission) ,
                                        new SqlParameter("@OrgSourceStatus", org.FStatus),
                                        new SqlParameter("@OrgTargetStatus", "ZT01 使用"),
                                        new SqlParameter("@Data", response.Data),
                                        new SqlParameter("@DataDesc", response.DataDesc),
                                        new SqlParameter("@FItemID", response.FItemID),
                                        new SqlParameter("@FNumber", response.FNumber),
                                        new SqlParameter("@FStatus", response.FStatus)
                                });
                    }
                }
            }

        }
    }
}
