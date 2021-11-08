namespace FTPHelper
{
    using System; //.net基础类型和通用类型
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net;
//重点是里面用来网络开发的WebRequest 和 WebResponse 类
    using System.IO;
//对文件和目录操作
    using System.Threading;
    using System.Windows;

    public class FtpHelper
    {
        // 默认常量定义
        private static readonly string rootPath = "/";

        private static readonly int defaultReadWriteTimeout = 300000;

        //读写时间
        private static readonly int defaultFtpPort = 21;
        //ftp默认端口号21

        #region 设置初始化参数

        //region用于将代码块折叠，方便查找
        private string host = string.Empty;

        public string Host
        {
            get { return this.host ?? string.Empty; }
            //访问器
        }

        private string username = string.Empty;

        public string Username
        {
            get { return this.username; }
        }

        private string password = string.Empty;

        public string Password
        {
            get { return this.password; }
        }

        IWebProxy proxy = null;

        public IWebProxy Proxy
        {
            get { return this.proxy; }
            set { this.proxy = value; }
        }

        private int port = defaultFtpPort;

        public int Port
        {
            get { return port; }
            set { this.port = value; }
        }

        private bool enableSsl = false;

        public bool EnableSsl
        {
            get { return enableSsl; }
        }

        private bool usePassive = true;

        public bool UsePassive
        {
            get { return usePassive; }
            set { this.usePassive = value; }
        }

        private bool useBinary = true;

        public bool UserBinary
        {
            get { return useBinary; }
            set { this.useBinary = value; }
        }

        private string remotePath = rootPath;

        public string RemotePath
        {
            get { return remotePath; }
            set
            {
                string result = rootPath;
                if (!string.IsNullOrEmpty(value) && value != rootPath)
                {
                    result = Path.Combine(Path.Combine(rootPath, value.TrimStart('/').TrimEnd('/')), "/"); // 进行路径的拼接
                }

                this.remotePath = result;
            }
        }

        private int readWriteTimeout = defaultReadWriteTimeout;

        public int ReadWriteTimeout
        {
            get { return readWriteTimeout; }
            set { this.readWriteTimeout = value; }
        }

        #endregion

        #region 构造函数

        public FtpHelper(string host, string username, string password)
            : this(host , username, password, defaultFtpPort, null, false, true, true, defaultReadWriteTimeout)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host">主机名</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="port">端口号 默认21</param>
        /// <param name="proxy">代理 默认没有</param>
        /// <param name="enableSsl">是否使用ssl 默认不用</param>
        /// <param name="useBinary">使用二进制</param>
        /// <param name="usePassive">获取或设置客户端应用程序的数据传输过程的行为</param>
        /// <param name="readWriteTimeout">读写超时时间 默认5min</param>
        public FtpHelper(string host, string username, string password, int port, IWebProxy proxy, bool enableSsl,
            bool useBinary, bool usePassive, int readWriteTimeout)
        {
            this.host = host.ToLower().StartsWith("ftp://") ? host : "ftp://" + host;
            this.username = username;
            this.password = password;
            this.port = port;
            this.proxy = proxy;
            this.enableSsl = enableSsl;
            this.useBinary = useBinary;
            this.usePassive = usePassive;
            this.readWriteTimeout = readWriteTimeout;
        }

        #endregion

        /// <summary>
        /// 拼接URL
        /// </summary>
        /// <param name="host">主机名</param>
        /// <param name="remotePath">地址</param>
        /// <param name="fileName">文件名</param>
        /// <returns>返回完整的URL</returns>
        private string UrlCombine(string host, string remotePath, string fileName)
        {
            string result = new Uri(new Uri(new Uri(host.TrimEnd('/')), remotePath), fileName).ToString();
            ;
            return result;
        }

        private string UrlCombine(string host, string remotePath)
        {
            string result = new Uri(new Uri(host.TrimEnd('/')), remotePath).ToString();
            ;
            return result;
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="method">方法</param>
        /// <returns>返回 request对象</returns>
        private FtpWebRequest CreateConnection(string url, string method)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
            request.Credentials = new NetworkCredential(this.username, this.password);
            request.Proxy = this.proxy;
            request.KeepAlive = false;
            request.UseBinary = useBinary;
            request.UsePassive = usePassive;
            request.EnableSsl = enableSsl;
            request.Method = method;
            Console.WriteLine(request);
            return request;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="localFile">本地文件</param>
        /// <param name="remoteFileName">上传文件名</param>
        /// <returns>上传成功返回 true</returns>
        public bool Upload(FileInfo localFile, string remoteFileName, ref float percent)
        {
            bool result = false;
            if (localFile.Exists)
            {
                try
                {
                    percent = 0;
                    long current = 0;
                    long total = 1;
                    total = localFile.Length;

                    string url = UrlCombine(Host, RemotePath, remoteFileName); //统一资源定位符
                    FtpWebRequest
                        request = CreateConnection(url, WebRequestMethods.Ftp.UploadFile); //用FtpWebRequest创建连接

                    using (Stream rs = request.GetRequestStream())
                    using (FileStream fs = localFile.OpenRead())
                        //using用于在代码块执行结束后释放资源
                    {
                        byte[] buffer = new byte[1024 * 4];
                        int count = fs.Read(buffer, 0, buffer.Length);
                        while (count > 0)
                        {
                            current += count;
                            percent = (float)(100 * (double)current / total);

                            rs.Write(buffer, 0, count);
                            count = fs.Read(buffer, 0, buffer.Length);
                        }

                        fs.Close();
                        result = true;
                        //filestream -> stream
                    }
                }
                catch (WebException ex)
                {
                    // MessageBox.Show(ex.Message);
                }

                return result;
            }

            // 处理本地文件不存在的情况
            return false;
        }


        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="serverName">服务器文件名称</param>
        /// <param name="localName">需要保存在本地的文件名称</param>
        /// <returns>下载成功返回 true</returns>
        public bool Download(string serverName, string localName, ref float percent)
        {
            bool result = false;
            using (FileStream fs = new FileStream(localName, FileMode.OpenOrCreate))
            {
                try
                {
                    percent = 0;
                    long current = 0;
                    long total = 1;

                    string url = UrlCombine(Host, RemotePath, serverName);
                    Console.WriteLine(url);

                    FtpWebRequest request = CreateConnection(url, WebRequestMethods.Ftp.DownloadFile);
                    request.ContentOffset = fs.Length;

                    total = GetFileSize(url, 0);

                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        fs.Position = fs.Length;
                        byte[] buffer = new byte[1024 * 4];
                        int count = response.GetResponseStream().Read(buffer, 0, buffer.Length);
                        while (count > 0)
                        {
                            fs.Write(buffer, 0, count);
                            count = response.GetResponseStream().Read(buffer, 0, buffer.Length);

                            current = fs.Length;
                            percent = (float)(100 * (double)current / total);
                        }

                        response.GetResponseStream().Close();
                    }

                    result = true;
                }
                catch (WebException ex)
                {
                    // 处理ftp连接中的异常
                }
            }

            return result;
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        public void Delete(string fileName)
        {
            try
            {
                string uri = UrlCombine(Host, RemotePath, fileName);
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(this.username, this.password);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFTP.UsePassive = false;

                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("远程服务器不存在目标文件");
                throw new Exception("FtpHelper Delete Error --> " + ex.Message + "  文件名:" + fileName);
            }
        }

        /// <summary>
        /// 从ftp服务器上获得文件夹列表
        /// </summary>
        /// <param name="RequedstPath">服务器下的相对路径</param>
        /// <returns></returns>
        public List<string> GetDirctory(string remotePath)
        {
            List<string> strs = new List<string>();
            try
            {
                string uri = UrlCombine(host, remotePath);   //目标路径 path为服务器地址
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(username, password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());//中文文件名

                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line.Contains("<DIR>"))
                    {
                        string msg = line.Substring(line.LastIndexOf("<DIR>") + 5).Trim();
                        strs.Add(msg);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return strs;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取目录出错：" + ex.Message);
            }
            return strs;
        }


        /// <summary>
        /// 从ftp服务器上获得文件列表
        /// </summary>
        /// <param name="RequedstPath">服务器下的相对路径</param>
        /// <returns></returns>
        public List<string> GetFile(string remotePath)
        {
            List<string> strs = new List<string>();
            try
            {
                string uri = UrlCombine(host, remotePath);   //目标路径 path为服务器地址
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(username, password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());//中文文件名

                string line = reader.ReadLine();
                while (line != null)
                {
                    if (!line.Contains("<DIR>"))
                    {
                        string msg = line.Substring(39).Trim();
                        strs.Add(msg);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return strs;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取文件出错：" + ex.Message);
            }
            return strs;
        }


        public void CreateDir(string dirName)
        {
            try
            {
                string uri = UrlCombine(host, dirName);
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // 指定数据传输类型
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(username, password);
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("创建目录出错：" + ex.Message);
            }
        }


        /// <summary>
        /// 删除目录 上一级必须先存在
        /// </summary>
        /// <param name="dirName">服务器下的相对路径</param>
        public void DeleteDir(string dirName)
        {
            try
            {
                string uri = UrlCombine(host, dirName);
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(username, password);
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("删除目录出错：" + ex.Message);
            }
        }


        ///// <summary>
        ///// 获取文件大小
        ///// </summary>
        ///// <param name="file">ip服务器下的相对路径</param>
        ///// <returns>文件大小</returns>
        //public long GetFileSize(string file)
        //{
        //    StringBuilder result = new StringBuilder();
        //    FtpWebRequest request;
        //    try
        //    {
        //        string uri = UrlCombine(host, file);
        //        request = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
        //        request.UseBinary = true;
        //        request.Credentials = new NetworkCredential(username, password);//设置用户名和密码
        //        request.Method = WebRequestMethods.Ftp.GetFileSize;

        //        //request.GetResponse();
        //        //long dataLength = request.GetResponse().ContentLength;

        //        return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("获取文件大小出错：" + ex.Message);
        //        return -1;
        //    }
        //}

        /// <summary>
        /// 获得文件大小
        /// </summary>
        /// <param name="url">FTP文件的完全路径</param>
        /// <returns></returns>
        public long GetFileSize(string fileName)//获取大小就要重新连接吗？是的，method只能有一个
        {

            long fileSize = 0;
            try
            {
                string uri = UrlCombine(Host, RemotePath, fileName);
                FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));//路径创造新的连接
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(username, password);//连接
                reqFtp.Method = WebRequestMethods.Ftp.GetFileSize;//方法大小
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                fileSize = response.ContentLength;//获得大小

                response.Close();
            }
            catch (Exception ex)
            {
                
            }
            return fileSize;
        }

        public long GetFileSize(string url, int type)//获取大小就要重新连接吗？是的，method只能有一个
        {

            long fileSize = 0;
            try
            {
                FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));//路径创造新的连接
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(username, password);//连接
                reqFtp.Method = WebRequestMethods.Ftp.GetFileSize;//方法大小
                FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
                fileSize = response.ContentLength;//获得大小

                response.Close();
            }
            catch (Exception ex)
            {

            }
            return fileSize;
        }


        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="ftpPath">ftp文件路径</param>
        /// <param name="currentFilename"></param>
        /// <param name="newFilename"></param>
        public bool Rename(string currentFileName, string newFileName)
        {
            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            try
            {
                string uri = UrlCombine(host, currentFileName);
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(username, password);
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.Method = WebRequestMethods.Ftp.Rename;
                ftpWebRequest.RenameTo = newFileName;

                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                ftpResponseStream = ftpWebResponse.GetResponseStream();

            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                if (ftpResponseStream != null)
                {
                    ftpResponseStream.Close();
                }
                if (ftpWebResponse != null)
                {
                    ftpWebResponse.Close();
                }
            }
            return success;
        }
    }
}