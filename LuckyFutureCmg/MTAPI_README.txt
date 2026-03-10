MtApi 연동 안내 (CMG - MetaTrader.cs)
========================================

선배님이 만든 설치파일로 MtApi가 설치된 후, 이 프로젝트는 MT4(AgiliumTrade) 대신
로컬 MtApi Expert Advisor와 통신하도록 변경되었습니다.

[사용 절차]
1. cmgau.net에서 받은 프로그램(MT4)을 실행하고, 차트에 MtApi Expert Advisor를 드래그하여 붙입니다.
2. MtApi 속성에서 Common 탭 → "Allow DLL imports" 체크 후 OK
3. 선배님이 만든 프로그램(이 솔루션 빌드 결과)을 실행한 뒤 접속 버튼 클릭

[빌드 시]
- MtApi.dll이 필요합니다.
  - 설치 경로가 "C:\Program Files (x86)\MtApi" 라면:
    LuckyFutureCmg.csproj에서 MtApi 참조의 HintPath를 해당 경로로 바꾸거나,
  - 이 폴더(lib\MtApi)에 MtApi.dll을 복사해 두세요.
- 선배님 설치파일로 MtApi를 설치하면 보통 MtApi.dll이 포함된 폴더가 생성됩니다.

[연결 설정]
- MetaTrader.cs 상수: MTAPI_DEFAULT_HOST = "localhost", MTAPI_DEFAULT_PORT = 8222
- MT4에서 MtApi EA의 포트가 8222가 아니면 위 포트를 EA 설정과 맞추세요.

[키움]
- 키움 관련 코드(KFOpen, 키움 API)는 수정하지 않았습니다. CMG(MetaTrader) 부분만 MtApi로 전환되었습니다.
