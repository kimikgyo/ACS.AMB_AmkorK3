using System;

namespace INA_ACS_Server
{
    /*
     * 
     * 
    CREATE TABLE [dbo].[POPSERVER_1Table](
	    [ID] [int] IDENTITY(1,1) NOT NULL,
	    [LINE_CD] [varchar](50) NULL,
	    [POST_CD] [int] NULL,
	    [COMM_PO] [varchar](50) NULL,
	    [COMM_ANG] [int] NULL,
	    [RETU_TYPE] [varchar](50) NULL,
	    [POST_STA] [varchar](50) NULL,
	    [ACS_IF_FLAG] [varchar](50) NULL,
	    [CREATE_DT] [datetime] NULL,
    CONSTRAINT [PK_POPSERVER_1Table] PRIMARY KEY CLUSTERED 
    (
	    [ID] ASC
    )
    WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
     * 
     * 
     */


    public class PopModel
    {
        //일단 변수명은 DB에서 주어진 이름을 사용함
        //POP 서버 DB에 주어진 DATA를 읽기만함(오퍼레이터 Call)
        public int Id { get; set; }
        public string PopServerIndex { get; set; }        //POP Server 번호(혹시 필요할수 있을것으로 생각)
        public string PopServerIpAddress { get; set; }    //POP Server 접속을 위한 ip주소
        /*===== ACS 에서 읽기 =====*/
        public string LINE_CD { get; set; }               //[생산라인]
        public int POST_CD { get; set; }                  //[Post 코드] *Post Code고정렉 번호
        public string COMM_PO { get; set; }               //[통합사용렉] *통합 사용렉 사용 유무(통합 사용 "N" 설정이면 각도값이 '0' 이고 "Y" 설정이면 각도값이 '0'보다 크다)
        public int COMM_ANG { get; set; }                 //[각도] *통합 사용렉이 "Y"일 경우 사용(POP서버에서 읽어온 각도값이 있을경우 읽어온 각도값에 의하여 품번 품명이 유동적이다)
        public string RETU_TYPE { get; set; }             //[공용기 여부]* 공용기 사용여부 (완제품 BOX와 공BOX 구분할수있다 데이터가 "Y" 일경우 '공BOX' 이며 "N" 일경우 '완제품' 박스이다)
        public string ACS_IF_Flag { get; set; }           //[ACS 에서 자료 변경] *전부 생성된 데이터를 ACS가져가면 ACS에서 자료변경 "Y"로 변경해준다.
        public DateTime CREATE_DT { get; set; } = DateTime.Now; //[생성일시] *데이터 생성한 날짜와 일 시
        public string POST_STA { get; set; }              //[박스감지 센서 신호] *고정렉에 있는 센서 감지 신호 데이터 를 보내준다

        //공용기 여부가 "N" 경우 박스 감지 "N" 일때 ACS가 읽어 간다(공용기 여부가 "N"일경우 창고에서 자재를 가지고 와서 내려두어야 하기때문에 감지센서 데이터가 "N"이여야함
        //공용기 여부가 "Y" 경우 박스 감지 "Y" 일때 ACS가 읽어 간다(공용기 여부가 "Y"일경우 고정 렉에서 공BOX를 가지고  공BOX창고에 가져다 주어야 하기때문에 감지센서가 "Y"이여야 한다)

        /*ACS에서 선택하여 읽어가야하는 순서
         * 1.public string ACS_IF_FLAG [ACS 에서 자료변경] "N"인것만 추린다.
         * 2.public string RETU_TYPE [공용기 여부] =="N" && public string POST_STA [박스감지 센서 신호] 일치한것을 추린다.
         * 3.public string ACS_IF_FLAG [ACS 에서 자료변경] "Y"로 변경한다.
         * 4. public string LINE_CD / public int POST_CD /  public string COMM_PO / public int COMM_ANG 4가지의 Data를 가지고 제품정보 검색용으로 사용한다
         */

        public override string ToString()
        {
            string postName = ""; // post name
            if (RETU_TYPE == "N") postName = "공급";
            else if (RETU_TYPE == "Y") postName = "회수";

            //return $"Id={Id} LINE_CD={LINE_CD} POST_CD={POST_CD} {postName} RETU_TYPE={RETU_TYPE} FLAG={ACS_IF_Flag}";
            //return $"{Id,-5} {LINE_CD} {POST_CD,-2} {postName} {COMM_PO} {COMM_ANG,-3} {RETU_TYPE} {ACS_IF_Flag} {CREATE_DT:yyyy-MM-dd HH:mm:ss}";
            return $"{LINE_CD} {POST_CD,-2} {postName} {COMM_PO} {COMM_ANG,-3} {RETU_TYPE} {ACS_IF_Flag} {CREATE_DT:yyyy-MM-dd HH:mm:ss}";
        }
    }
}