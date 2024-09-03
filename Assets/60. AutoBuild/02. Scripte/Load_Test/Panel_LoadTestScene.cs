using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using IdleGame.Core;
using IdleGame.Core.Panel.LogCollector;
using IdleGame.Data;
using IdleGame.Data.Common.Log;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace IdleGame.Main.Scene.Load
{


    public class Panel_LoadTestScene : Base_ScenePanel
    {
        [System.Serializable]
        private class VersionInfo
        {
            public string version;

            public VersionInfo(string version)
            {
                this.version = version;
            }
        }

        /// <summary>
        /// [종류] 자동 빌드의 발생할수있는 가능성들을 지정합니다. 
        /// </summary>
        private enum eAutoBuildType
        {
            /// <summary> 유저 이름이 없음 </summary>
            EmptyUserName,
            /// <summary> 공유 폴더안에 빌드가 존재하지 않습니다. </summary>
            NoneCloudBuild,
            /// <summary> 공유 폴더안에 빌드가 존재하지않으며 다운받아둔 빌드가 존재하지않음 </summary>
            NoneLocalAndCloudBuild,
            /// <summary> 공유 빌드의 버전명이 양식에 맞지 않습니다.</summary>
            CloudeVersionMissingFormat,



            /// <summary> 게임 시작이 가능한 상태입니다.</summary>
            PossibleGameStart
        }

        /// <summary>
        /// [기능] 로그 수신기
        /// </summary>
        [SerializeField] private Panel_LogCollector _Log;

        [SerializeField] private TMP_Text _t_loadInfo;
        [SerializeField] private Image _i_currentVersion;
        [SerializeField] private TMP_Text _t_currentVersion;
        [SerializeField] private Image _i_newVersion;
        [SerializeField] private TMP_Text _t_newVersion;
        [SerializeField] private TMP_Text _t_Context;
        [SerializeField] private Button _b_retry;

        [SerializeField] private TMP_InputField _if_userName;
        [SerializeField] private TMP_Dropdown _dw_tableType;
        [SerializeField] private Toggle _tg_initSave;

        [SerializeField] private Button _b_gameStart;

        private (string, string) _cloudeFild;

        /// <summary>
        /// [상태] 빌드가 없음.
        /// </summary>
        private bool _check_NonBuild;

        protected override void Logic_Init_Custom()
        {
            Logic_LoadPrefs();
        }

        /// <summary>
        /// [기능] 기본 설정값을 불러옵니다.
        /// </summary>
        private void Logic_LoadPrefs()
        {
            _dw_tableType.value = PlayerPrefs.GetInt("load_tabletype");
            _if_userName.text = PlayerPrefs.GetString("load_username");
            _tg_initSave.isOn = PlayerPrefs.GetInt("load_initsave") == 1;
        }


        /// <summary>
        /// [기능] 특정 진행 상황이 변경된 경우 호출합니다. 
        /// </summary>
        private void Logic_SetProgeesType(eAutoBuildType m_type)
        {
            _b_retry.interactable = true;

            switch (m_type)
            {
                case eAutoBuildType.NoneLocalAndCloudBuild:
                    _b_gameStart.interactable = false;
                    break;
                case eAutoBuildType.PossibleGameStart:
                    _b_gameStart.interactable = true;
                    _t_loadInfo.text = "게임 시작 가능";

                    break;
                default:
                    break;
            }


            Logic_SetContext(m_type);
        }


        /// <summary>
        /// [기능] 텍스트 타입을 지정합니다. 
        /// </summary>
        private void Logic_SetContext(eAutoBuildType m_type)
        {
            switch (m_type)
            {
                case eAutoBuildType.EmptyUserName:
                    _t_Context.text = "플레이 과정에서 발생하는 로그를 추적하고 관리하기위해서 반드시 유저이름을 입력해주세요.\n\n " +
                        "아무거나 입력해주셔도 상관없습니다.";
                    break;
                case eAutoBuildType.NoneCloudBuild:
                    break;
                case eAutoBuildType.NoneLocalAndCloudBuild:
                    _t_Context.text = "클라우드 및 로컬 모두 실행할 빌드가 존재하지 않습니다. \n 별도의 조치가 필요합니다." +
                        "\n 로그가 Alld에게 전송되었으나, 혹시 모르니 별도로 Alld에게 요청하세요.";
                    break;
                case eAutoBuildType.CloudeVersionMissingFormat:
                    _t_Context.text = "클라우드에 지정된 빌드의 버전이 포멧양식과 맞지않습니다. \n" +
                        "올드에게 문의하거나, 해당 구글 드라이브에가서 현재 알맞는 버전과 함께 [00000_v] 형태로 양식에 맞게 변경해주세요.";
                    break;
            }

            _Log.Logic_PutLog(new Data_Log($"User:{_if_userName.text} + {_t_Context.text}"));
        }

        /// <summary>
        /// [기능] 게임 시작이 가능한지를 확인합니다. 
        /// </summary>
        public bool TryPossibleStart()
        {
            if (string.IsNullOrEmpty(_if_userName.text))
            {
                Logic_SetProgeesType(eAutoBuildType.EmptyUserName);
                return false;
            }

            return true;
        }

        #region 통신처리

        private DriveService _driveService;
        public TextAsset serviceAccountJson;
        private string targetFolder;


        void Start()
        {
            targetFolder = Path.Combine(Application.persistentDataPath, "IdleGamesBuild");
            Authenticate();
        }


        private void Authenticate()
        {
            string[] scopes = { DriveService.Scope.DriveReadonly };

            // 서비스 계정 인증
            GoogleCredential credential;
            using (var stream = new MemoryStream(serviceAccountJson.bytes))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(scopes);
            }

            // Google Drive API 클라이언트 생성
            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Unity Google Drive Example"
            });

            // 파일 목록 가져오기
            ListFiles();
        }

        private void ListFiles()
        {
            _b_retry.interactable = false;
            _b_gameStart.interactable = false;


            var request = _driveService.Files.List();
            request.Q = "'1-szFVe1vdsUuArCcFlhMOvXeplrGXIeD' in parents"; // 폴더 ID를 입력
            request.Fields = "nextPageToken, files(id, name)";
            var result = request.Execute();

            var files = request.Execute();
            if (files.Files.Count == 0)
            {
                _cloudeFild = new(null, null);
                if (IsExistsBuild())
                    Logic_SetProgeesType(eAutoBuildType.NoneCloudBuild);
                else
                    Logic_SetProgeesType(eAutoBuildType.NoneLocalAndCloudBuild);
            }
            else
                _cloudeFild = new(files.Files[0].Name, files.Files[0].Id);

            Logic_UpdateVersionCheck();
        }

        /// <summary>
        /// [기능] 현재 버전 상태를 검증합니다.
        /// </summary>
        public void Logic_UpdateVersionCheck()
        {
            _t_loadInfo.text = "버전 체크 중....";

            bool checkA = IsExistsBuild(), checkB = _cloudeFild != (null, null);

            if (checkA)
                _t_currentVersion.text = Logic_ReadVersionFile().ToString();
            else
                _t_currentVersion.text = "-";

            if (checkB)
                _t_newVersion.text = int.Parse(_cloudeFild.Item1.Split("_")[0]).ToString();
            else
                _t_newVersion.text = "-";

            if (checkB)
            {
                int cloudVersion;
                if (int.TryParse(_cloudeFild.Item1.Split("_")[0], out cloudVersion))
                {
                    if (cloudVersion <= Logic_ReadVersionFile())
                        Logic_SetProgeesType(eAutoBuildType.PossibleGameStart);
                    else
                        Logic_DownloadFile(_cloudeFild.Item2);
                }
                else
                    Logic_SetProgeesType(eAutoBuildType.CloudeVersionMissingFormat);

            }
            else if (checkA)
                Logic_SetProgeesType(eAutoBuildType.PossibleGameStart);
            else
                Logic_SetProgeesType(eAutoBuildType.NoneLocalAndCloudBuild);


        }

        /// <summary>
        /// [기능] 다운로드 절차를 진행합니다.
        /// </summary>
        public async void Logic_DownloadFile(string fileId)
        {
            _t_loadInfo.text = "다운로드 중....";
            _b_gameStart.interactable = false;
            _b_retry.interactable = false;

            try
            {
                // 파일을 다운로드하기 위한 요청
                var request = _driveService.Files.Get(fileId);
                var stream = new MemoryStream();

                // 비동기로 파일 다운로드
                await Task.Run(() => request.Download(stream));

                stream.Position = 0;

                // 폴더가 존재하는지 확인하고, 존재하면 삭제
                if (IsExistsBuild())
                {
                    Directory.Delete(targetFolder, true);
                }

                // 새 폴더 생성
                Directory.CreateDirectory(targetFolder);

                // ZIP 파일 저장
                string zipPath = Path.Combine(targetFolder, "downloaded.zip");
                using (var fileStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write))
                {
                    await stream.CopyToAsync(fileStream);
                }

                _t_loadInfo.text = "다운로드 완료!!";

                // 압축 해제
                await Logic_ExtractZipAsync(zipPath);
            }
            catch (Exception ex)
            {
                Debug.LogError("다운로드 중 오류 발생: " + ex.Message);
            }
            finally
            {
                _b_retry.interactable = true;
            }
        }

        /// <summary>
        /// [기능] 압축해제 절차
        /// </summary>
        private async Task Logic_ExtractZipAsync(string zipPath)
        {
            _b_retry.interactable = false;
            _b_gameStart.interactable = false;

            // 압축 해제
            await Task.Run(() => ZipFile.ExtractToDirectory(zipPath, targetFolder));
            _t_loadInfo.text = "압축 해제 완료!!";

            // 압축 파일 삭제
            File.Delete(zipPath);

            Logic_CreateVersionData();

            Logic_SetProgeesType(eAutoBuildType.PossibleGameStart);
        }

        /// <summary>
        /// [기능] 버전 정보 기록
        /// </summary>
        private void Logic_CreateVersionData()
        {
            VersionInfo versionInfo = new VersionInfo(_cloudeFild.Item1.Split("_")[0]);
            string json = JsonUtility.ToJson(versionInfo, true); // JSON 형식으로 변환

            string path = Path.Combine(targetFolder, "data.ver");
            File.WriteAllText(path, json); // 파일 작성

            Debug.Log($"버전 정보가 저장되었습니다: {path}");
        }

        /// <summary>
        /// [기능] 버전 정보 읽기
        /// </summary>
        /// <returns></returns>
        public int Logic_ReadVersionFile()
        {
            string path = Path.Combine(targetFolder, "data.ver");

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path); // 파일 내용 읽기
                VersionInfo versionInfo = JsonUtility.FromJson<VersionInfo>(json); // JSON을 객체로 변환

                return int.Parse(versionInfo.version);
            }
            else
            {
                Debug.LogWarning("버전 파일이 존재하지 않습니다.");
                return 0;
            }
        }

        private bool IsExistsBuild() => Directory.Exists(targetFolder);

        #endregion


        #region 콜백 함수

        /// <summary>
        /// [버튼콜백] 현재 설정된 상태값을 기반으로 게임을 시작합니다.
        /// </summary>
        public void OnClickGameStart()
        {
            if (!TryPossibleStart()) return;

            PlayerPrefs.SetInt("load_tabletype", _dw_tableType.value);
            PlayerPrefs.SetString("load_username", _if_userName.text);
            PlayerPrefs.SetInt("load_initsave", _tg_initSave.isOn ? 1 : 0);

            GameManager.Log.Logic_PutLog(new Data_Log($"테스트용 빌드 시작체크, 시작 타입 : {Global_Data.Editor.LocalData_Grid}"));
        }

        /// <summary>
        /// [버튼콜백] 재로드를 시도합니다.
        /// </summary>
        public void OnClickReTry()
        {
            ListFiles();
        }
        #endregion
    }
}