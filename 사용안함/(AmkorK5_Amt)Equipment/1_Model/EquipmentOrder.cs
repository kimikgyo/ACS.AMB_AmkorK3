using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    /*
     * 
    CREATE TABLE [dbo].[EquipmentOrders]
    (
	    --[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	    [Id] [int] NOT NULL PRIMARY KEY,
	    [EQP_NAME] [varchar](50) NOT NULL,
	    [COMMAND] [varchar](50) NOT NULL,
	    [INCH_TYPE] [varchar](50) NOT NULL,
	    [IF_FLAG] [varchar](50) NOT NULL,
	    [CREATE_DT] [datetime] NOT NULL DEFAULT getdate(),
	    [MODIFY_DT] [datetime] NOT NULL DEFAULT getdate()
    )
     * 
     */

    public class EquipmentOrder
    {
        public int Id { get; set; }
        public string EQP_NAME { get; set; }        // RHS1, RHS2, ...
        public string COMMAND { get; set; }         // CALL, CANCEL
        public string INCH_TYPE { get; set; }       // 7, 13
        public string IF_FLAG { get; set; }         // N => P(처리중) => Y(완료) or C(취소)
        public DateTime CREATE_DT { get; set; }
        public DateTime MODIFY_DT { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this); ;
        }

    }
}
