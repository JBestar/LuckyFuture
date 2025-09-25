using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Threading;

namespace LuckyFuture.UI
{
    public partial class FrmUpdate : Form
    {
        

        private FrmUpdate()
        {
            InitializeComponent();
            CenterToParent();
            InitializeComponentEx();

        }

        public static readonly FrmUpdate Default = new FrmUpdate();
        
        
        private WebClient mWebClient = new WebClient();
        private Dictionary<string, string> mUpdateData = new Dictionary<string, string>();

        private int m_iResCode = 0;     //0:Didn't Update 1:Updated 2:비정상

        private string mDownloadUrl;
        private string mUpdateVersion;
        private string m_strWorkDir;
        private int m_nTotalFiles;
        private string m_strCurFile;


        private void InitializeComponentEx()
        {
            mWebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Client_DownloadProgress);
            mWebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Client_DownloadCompleted);

            progTotal.Value = 0;
            progFile.Value = 0;
            lbLogTotal.Text = "";
            lbLogFile.Text = "";
            

            mDownloadUrl = AppAuthor.URL_DOWNLOAD;
            m_strWorkDir = Environment.CurrentDirectory;

            ClearBackupFiles();
        }

        public bool CheckUpdate()
        {
            mUpdateVersion = AppAuthor.Default.GetUpdateVersion(mUpdateData);

            if (mUpdateVersion.Length > 0)
                return true;
            else return false;

        }

        private void StartUpdate()
        {
            

            if (mUpdateVersion.Length < 1)
            {
                CloseForm();
                return;
            }
                

            if (mUpdateData == null)
            {
                CloseForm();
                return;
            }
                

            m_nTotalFiles = mUpdateData.Count;

            if(m_nTotalFiles < 1)
            {
                CloseForm();
                return;
            }
                

            DownloadCurFile();
        }

        private void ClearBackupFiles()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(m_strWorkDir);

            FileInfo[] fileInfos = dirInfo.GetFiles();
            try
            {            
                for (int i = 0; i < fileInfos.Length; i++)
                {
                    string extension = Path.GetExtension(fileInfos[i].Name);
                    if (extension == ".bak")
                    {
                        File.Delete(fileInfos[i].FullName);
                    }
                    else if (fileInfos[i].Name.Contains("TradeKing.exe"))
                    {
                        File.Delete(fileInfos[i].FullName);
                    }
                }
            }
            catch (Exception) { }
        }

        private void DownloadCurFile()
        {
            int nCurFiles = mUpdateData.Count;
            
            string strTotal = string.Format("{0}/{1} file(s) downloaded.", m_nTotalFiles - nCurFiles, m_nTotalFiles);
            lbLogTotal.Text = strTotal;
            progTotal.Value = (m_nTotalFiles - nCurFiles) * progTotal.Maximum / m_nTotalFiles;


            if (nCurFiles < 1)
            {
                CloseForm();
                return;
            }   

            KeyValuePair<string, string> filePair = mUpdateData.FirstOrDefault();
            if (filePair.Equals(default(KeyValuePair<string, string>)))
            {
                CloseForm();
                return;
            }
                

            mUpdateData.Remove(filePair.Key);
            m_strCurFile = filePair.Key;

            string strFilePath = m_strWorkDir + Path.DirectorySeparatorChar.ToString() + m_strCurFile;

            bool bNeedDown = false;
            using (MD5 md5Hash = MD5.Create())
            {
                bNeedDown = CompareMd5Hash(md5Hash, strFilePath, filePair.Value);
            }

            if (bNeedDown)                  //Download File From Server
            {
                try
                {
                    if (m_strCurFile.IndexOf(Path.AltDirectorySeparatorChar) > 0)
                    {
                        string strDirPath = m_strWorkDir + Path.DirectorySeparatorChar.ToString() + m_strCurFile.Substring(0, m_strCurFile.LastIndexOf(Path.AltDirectorySeparatorChar));
                        DirectoryInfo subDir = new DirectoryInfo(strDirPath);
                        if (!subDir.Exists)
                            subDir.Create();
                    }
                    else if (m_strCurFile.IndexOf(Path.DirectorySeparatorChar) > 0)
                    {
                        string strDirPath = m_strWorkDir + Path.DirectorySeparatorChar.ToString() + m_strCurFile.Substring(0, m_strCurFile.LastIndexOf(Path.DirectorySeparatorChar));
                        DirectoryInfo subDir = new DirectoryInfo(strDirPath);
                        if (!subDir.Exists)
                            subDir.Create();
                    }


                    string sServerUri = mDownloadUrl + mUpdateVersion + "/" + m_strCurFile;
                    string sClientPath = m_strWorkDir + Path.DirectorySeparatorChar.ToString() + m_strCurFile;

                    mWebClient.DownloadFileAsync(new Uri(sServerUri), sClientPath);
                }
                catch (Exception)
                {
                    m_iResCode = 2;
                    CloseForm();
                    return;
                }
                
            }
                
            else DownloadCurFile();
        }

        private bool CompareMd5Hash(MD5 md5Hash, string filePath, string sVersionHash)
        {
            
            FileStream fi = null;
            try
            {
                
                FileInfo fileInfo = new FileInfo(filePath);
                if (!fileInfo.Exists)
                    return true;

                long lFileLength = fileInfo.Length;
                if (lFileLength < 1)
                    return true;

                byte[] readBytes = new byte[lFileLength];

                fi = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                int i, index = 0;
                do
                {
                    i = fi.ReadByte();      //한바이트를 읽고 그 값을 i에 저장
                    if (i != -1)
                    {
                        readBytes[index] = (byte)i;
                        index++;
                        if (index > lFileLength - 1)
                            break;
                    }

                } while (i != -1);

                fi.Close();

                string sReadHash = GetMd5Hash(md5Hash, readBytes);

                if (sReadHash != sVersionHash)
                {
                    string sNewPath = m_strWorkDir + Path.DirectorySeparatorChar.ToString() + m_strCurFile + ".bak";
                    File.Move(filePath, sNewPath);
                    return true;
                }
                return false;

            }
            catch (Exception)
            {
                m_iResCode = 2;
                if(fi != null)
                    fi.Close();
                
            }
            
            return true;
        }

        private string GetMd5Hash(MD5 md5Hash, byte[] inputBytes)
        {
            //입력한 바이트 배렬의 MD5문자렬 반환
            byte[] data = md5Hash.ComputeHash(inputBytes);

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }


        private void CloseForm()
        {
            if (m_iResCode == 1)
            {
                MessageBox.Show("업데이트가 완료되었습니다. 다시 실행시켜 주세요.", "알림");
                this.DialogResult = DialogResult.Cancel;
            } else if(m_iResCode == 2)
            {
                MessageBox.Show("업데이트중 오류가 발생되었습니다. 다시 실행시켜 주세요.", "경고");
                this.DialogResult = DialogResult.Cancel;
            } else
            {
                this.DialogResult = DialogResult.OK;
            }
            

        }

        private void Client_DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            
            string strFile = string.Format("Downloading {0}...({1} /{2} bytes)", m_strCurFile, e.BytesReceived, e.TotalBytesToReceive);
            lbLogFile.Text = strFile;
            progFile.Value = e.ProgressPercentage * progFile.Maximum / 100;

        }

        private void Client_DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {

            if (e.Error != null)
            {
                m_iResCode = 2;
                CloseForm();
                return;
            } else if(m_iResCode != 2)
                m_iResCode = 1;

            DownloadCurFile();
        }
        
        private void FrmUpdate_Load(object sender, EventArgs e)
        {
            StartUpdate();
        }

        private void FrmUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
        
        }
    }
}
