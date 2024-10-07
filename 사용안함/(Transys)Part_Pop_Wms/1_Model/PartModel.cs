using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    /*
     * 
     * 
    CREATE TABLE [dbo].[PartModels](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [LINE_CD] [varchar](50) NULL,
	    [POST_CD] [int] NULL,
	    [COMM_PO] [varchar](50) NULL,
	    [OUT_Q] [int] NULL,
	    [COMM_ANG] [int] NULL,
	    [PART_CD] [varchar](50) NULL,
	    [PART_NM] [varchar](50) NULL,
	    [NP_MODE] [varchar](50) NULL,
	    [NP_OUT_Q] [int] NULL,
	    [NP_PART_CD] [varchar](50) NULL,
	    [NP_PART_NM] [varchar](50) NULL,
     CONSTRAINT [PK_PartModelsTable] PRIMARY KEY CLUSTERED 
    (
	    [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
     * 
     * 
     */


    public class PartModel
    {
        //일단 변수명은 DB에서 주어진 이름을 사용함
        //GUI 관련된 PartList(POP(Call)에서 올라온 정보에대한 품번 및 각도.수량.품번.품명을 조회하기위함)
        public int Id { get; set; }
        public string LINE_CD { get; set; }  //[생산라인] *오퍼레이터 가 직접 추가 및 삭제 수정을합니다.  
        public int POST_CD { get; set; }     //[POST 코드] *오퍼레이터 가 직접 추가 및 삭제 수정을합니다.
        public string COMM_PO { get; set; }    //[통합 사용렉] *오퍼레이터 가 직접 추가 및 삭제 수정을합니다.
        public int COMM_ANG { get; set; }      //[각도] *오퍼레이터 가 직접 추가 및 삭제 수정을합니다.

        public int OUT_Q { get; set; }     //[출고수량] *오퍼레이터 가 직접 추가 및 삭제 수정을합니다.
        public string PART_CD { get; set; }  //[품번] *오퍼레이터 가 직접 추가 및 삭제 수정을합니다.
        public string PART_NM { get; set; }  //[품명] *오퍼레이터 가 직접 추가 및 삭제 수정을합니다.

        public string NP_MODE { get; set; }    //[추가 파트 선택 "Y" / "N"] *새로운 라인에는 수량이 변경되어 2개의 제품이 갈수있게 셋팅해달라고 요청

        public int NP_OUT_Q { get; set; }       //[추가 파트 출고 수량] *추가 파트 대한 수량
        public string NP_PART_CD { get; set; }    //[추가 파트 출고 품번] *추가 파트 대한 품번
        public string NP_PART_NM { get; set; }    //[추가 파트 출고 품명] *추가 파트 대한 품명


        public override string ToString()
        {
            //return $"Id={Id}    LINE_CD={LINE_CD}    POST_CD={POST_CD}    PART_CD={PART_CD}    PART_NM={PART_NM}";
            return $"Id={Id,-5} {LINE_CD} {POST_CD,-2} OUT_Q={OUT_Q} PART_CD={PART_CD} PART_NM={PART_NM}";
        }
    }
}
