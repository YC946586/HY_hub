using HY.Client.Entity.UserEntitys;
using HY.Client.Execute.Commons;
using HY.RequestConver.Bridge;
using HY.RequestConver.InterFace;
using HY_Main.Common.CoreLib;
using HY_Main.ViewModel.HomePage.UserControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HY_Main.ViewModel.Mine.UserControls
{
     public  class GameDwonloadViewModel : BaseOperation<UserGamesEntity>
    {

        public event Action ShowList;
        private UserGamesEntity _pageCollection = new UserGamesEntity();


        public UserGamesEntity PageCollection
        {
            get { return _pageCollection; }
            set
            {
                _pageCollection = value;
                RaisePropertyChanged();
            }
        }
        public   void InitAsyncViewModel()
        {
            base.InitViewModel();

            var drive = DriveInfo.GetDrives();
            if (drive.Length != 0)
            {
                var driveDate = drive.Where(s => s.IsReady).ToList();
                if (driveDate.Any())
                {
                    if (driveDate.Count > 1)
                    {
                        PageCollection.StrupPath = driveDate[1].Name + "HYhubInstallation";
                    }
                    else
                    {
                        PageCollection.StrupPath = driveDate[0].Name + "HYhubInstallation";
                    }
                }
            }
            PageCollection.IsSelected = false;
            PageCollection.GameSize = CommonsCall.ConvertByG(PageCollection.fileSize);


        }
        /// <summary>
        /// 选择安装路径
        /// </summary>
        public override void Reset()
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "请选择安装路径";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    var drive = fbd.SelectedPath.Substring(0, 3);
                    DriveInfo d = new DriveInfo(drive);
                    if (d.TotalFreeSpace<= PageCollection.fileSize)
                    {
                        HY.Client.Execute.Commons.Message.Info("磁盘大小不足,请您选择其他路径");
                        return;
                    }
                    PageCollection.StrupPath = fbd.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                HY.Client.Execute.Commons.Message.ErrorException(ex);
            }
        }

      
       

        public override async void Query()
        {
            try
            {
            
                IStore store = BridgeFactory.BridgeManager.GetStoreManager();
                var genrator = await store.GetGameFiles(PageCollection.id);
                if (!genrator.code.Equals("000"))
                {
                    HY.Client.Execute.Commons.Message.Info(genrator.Message);
                }
                else
                {
                    var Results = JsonConvert.DeserializeObject<List<string>>(genrator.result.ToString());
                    var gramPath = AppDomain.CurrentDomain.BaseDirectory + @"DownloadGeam\" + PageCollection.fileDir+"\\";
                    foreach (var item in Results)
                    {
                        HttpWebClient httpWebClient = new HttpWebClient();
                        httpWebClient.DownloadFile(item, gramPath,1);
                    }
                }
            }
            catch (Exception ex)
            {
                HY.Client.Execute.Commons.Message.ErrorException(ex);
            }
            finally
            {
                ShowList?.Invoke();
            }
        }

    }
}
