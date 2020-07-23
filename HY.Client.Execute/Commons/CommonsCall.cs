using HandyControl.Controls;
using HY.Client.Entity.HomeEntitys;
using HY.Client.Entity.ToolEntitys;
using HY.Client.Entity.UserEntitys;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HY.Client.Execute.Commons
{
    public class CommonsCall
    {
        ///<summary>
        /// 通过WMI读取系统信息里的网卡MAC
        ///</summary>
        ///<returns></returns>
        public static List<string> GetMacByWMI()
        {
            List<string> macs = new List<string>();
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        mac = mo["MacAddress"].ToString();
                        macs.Add(mac);
                    }
                }
                moc = null;
                mc = null;
            }
            catch
            {
            }

            return macs;
        }

        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <returns></returns>
        public static int GetRandomSeed(int next)
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng =
                new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            var seed = BitConverter.ToInt32(bytes, 0);
            var random = new Random(seed).Next(next);

            return random;

        }
        


        public static string ConvertByG(float fileSize)
        {
            try
            {
                var dd = fileSize / (1024 * 1024 * 1024);
                return dd.ToString("F2");
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 判断句子中是否含有中文
        /// </summary>
        /// <param >字符串</param>
        public static bool WordsIScn(string words)
        {

            string TmmP;
            for (int i = 0; i < words.Length; i++)
            {
                TmmP = words.Substring(i, 1);
                byte[] sarr = Encoding.GetEncoding("gb2312").GetBytes(TmmP);
                if (sarr.Length == 2)
                {
                    return true;
                }

            }
            return false;

        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file"></param>
        public static void DeleteDir(string file)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                System.IO.DirectoryInfo fileInfo = new DirectoryInfo(file);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                //去除文件的只读属性
                System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);

                //判断文件夹是否还存在
                if (Directory.Exists(file))
                {
                    foreach (string f in Directory.GetFileSystemEntries(file))
                    {
                        if (File.Exists(f))
                        {
                            //如果有子文件删除文件
                            File.Delete(f);
                            Console.WriteLine(f);
                        }
                        else
                        {
                            //循环递归删除子文件夹
                            DeleteDir(f);
                        }
                    }

                    //删除空文件夹
                    Directory.Delete(file);
                    Console.WriteLine(file);
                }

            }
            catch (Exception ex) // 异常处理
            {
                Console.WriteLine(ex.Message.ToString());// 异常信息
            }
        }
        #region  注册表操作

        /// <summary>
        /// 记住游戏ID和安装路径
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="route"></param>
        public static void HyGameInstall(string gameId, string route,string gameName)
        {
            try
            {
                RegistryKey CUKey = null;
                RegistryKey CNCTKJPTKey = null;
                CUKey = Registry.CurrentUser;
                CNCTKJPTKey = CUKey.OpenSubKey(@"Software\HyGameInstall", true);
                if (CNCTKJPTKey == null)
                {
                    //说明这个路径不存在，需要创建
                    CUKey.CreateSubKey(@"Software\HyGameInstall");
                    CNCTKJPTKey = CUKey.OpenSubKey(@"Software\HyGameInstall", true);
                }
                RegistryKey userInfo = CNCTKJPTKey.OpenSubKey(gameId, true);
                if (userInfo == null)
                {
                    //说明这个路径不存在，需要创建
                    CNCTKJPTKey.CreateSubKey(gameId);
                    userInfo = CNCTKJPTKey.OpenSubKey(gameId, true);
                }
                //记录当前checkbox的状态为选中状态
                userInfo.SetValue("route", route);
                userInfo.SetValue("gameId", gameId);
                userInfo.SetValue("gameName", gameName);
                userInfo.SetValue("establishDate", DateTime.Now);
                userInfo.Close();
                CNCTKJPTKey.Close();
                CUKey.Close();

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 读取游戏注册表
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static ToolEntity ReadUserGameInfo(string gameId)
        {
            try
            {
                ToolEntity toolEntity = new ToolEntity();
                RegistryKey cuKey = Registry.CurrentUser;
                RegistryKey cnctkjptKey = cuKey.OpenSubKey(@"Software\HyGameInstall\" + gameId, true);
                if (cnctkjptKey != null)
                {
                    object objDate = cnctkjptKey.GetValue("route");
                    object gameName = cnctkjptKey.GetValue("gameName");
                    
                    if (objDate!=null)
                    {
                        toolEntity.Key = objDate.ToString();
                        toolEntity.remarks = gameName.ToString();
                    }
                }
                return toolEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 删除根节点
        /// </summary>
        public static void DeleteSubKeyTree(string gameId)
        {
            try
            {

                RegistryKey CUKey = Registry.CurrentUser;
                RegistryKey CNCTKJPTKey = null;
                CNCTKJPTKey = CUKey.OpenSubKey(@"Software\HyGameInstall", true);
                if (CNCTKJPTKey == null)
                {
                    //说明这个路径不存在，不需要删除
                    return;
                }
                RegistryKey userInfo = CNCTKJPTKey.OpenSubKey(gameId, true);
                if (userInfo == null)
                {
                    return;
                }
                else
                {
                    //删除指定项
                    CNCTKJPTKey.DeleteSubKey(gameId, true);
                }
                userInfo.Close();
                CNCTKJPTKey.Close();
                CUKey.Close();

            }
            catch (Exception ex)
            {

            }
        }

        const string _uriDeviecId = "SOFTWARE\\Microsoft\\YouYangCong";
        public static string GetDeviceId()
        {
            string ret = string.Empty;
            using (var obj = Registry.CurrentUser.OpenSubKey(_uriDeviecId, false))
            {
                if (obj != null)
                {
                    var value = obj.GetValue("DeviceId");
                    if (value != null)
                        ret = Convert.ToString(value);
                }
            }
            return ret;
        }

        public static string SetDeviceId()
        {
            string ret = string.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString()));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                string id = sBuilder.ToString();
                using (var tempk = Registry.CurrentUser.CreateSubKey(_uriDeviecId))
                {
                    tempk.SetValue("DeviceId", id);
                }
                ret = id;
            }
            return ret;
        }
        #endregion

        #region  属性
        //静态属性通知事件 (4.5支持)
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        private static string _DownProgress;
        /// <summary>
        ///  
        /// </summary>
        public static string DownProgress
        {
            get { return _DownProgress; }
            set
            {
                _DownProgress = value;
                if (StaticPropertyChanged != null)
                {
                    StaticPropertyChanged.Invoke(null, new PropertyChangedEventArgs("DownProgress"));
                }
            }
        }

        private static float _UserBalance;
        /// <summary>
        ///  
        /// </summary>
        public static float UserBalance
        {
            get { return _UserBalance; }
            set
            {
                _UserBalance = value;
                if (StaticPropertyChanged != null)
                {
                    StaticPropertyChanged.Invoke(null, new PropertyChangedEventArgs("UserBalance"));
                }
            }
        }

        private static string _showUser;
        /// <summary>
        ///  
        /// </summary>
        public static string ShowUser
        {
            get { return _showUser; }
            set { _showUser = value; 
                if (StaticPropertyChanged != null)
                {
                    StaticPropertyChanged.Invoke(null, new PropertyChangedEventArgs("ShowUser"));
                }
            }
        }

        private static ObservableCollection<UserGamesEntity> userGamesEntities = new ObservableCollection<UserGamesEntity>();

        public static ObservableCollection<UserGamesEntity> UserGames
        {
            get { return userGamesEntities; }
            set
            {
                userGamesEntities = value;
                if (StaticPropertyChanged != null)
                {
                    StaticPropertyChanged.Invoke(null, new PropertyChangedEventArgs("UserGames"));
                }
            }
        }
        #endregion


    }
}
