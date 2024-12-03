using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using System.Xml.Schema;
namespace Algorithm
{
    public class Logger
    {
        private ILog _log = null;
        private static Logger _logger = null;
        public Logger()
        {
            FileInfo _configFile = new FileInfo(new ConfigFileCreate().GetConfigFile());
            log4net.Config.XmlConfigurator.ConfigureAndWatch(_configFile);
            _log = LogManager.GetLogger("ZKERPLog");
        }
        public static Logger Instance
        {
            get
            {
                if (_logger == null)
                    _logger = new Logger();
                return _logger;
            }
        }
        public void Fatal(string message, String form, String Event = "")
        {
            message +=  "模块:" + form;
            if (Event != "") message +=  ";事件:" + Event;
            _log.Fatal(message);
        }
        public void Fatal(String message, System.Exception exception, String form, String Event = "")
        {
            message +=  ";模块:" + form;
            if (Event != "") message +=  ";事件:" + Event;
            _log.Fatal(message, exception);
        }
        public void Warn(String message, String form, String Event = "")
        {
            message +=  ";模块:" + form;
            if (Event != "") message +=  ";事件:" + Event;
            _log.Warn(message);
        }
        public void Warn(String message, System.Exception exception, String form, String Event = "")
        {
            message +=  ";模块:" + form;
            if (Event != "") message +=  ";事件:" + Event;
            _log.Warn(message, exception);
        }
        public void Error(String message, String form, String Event = "")
        {
            message +=  ";模块:" + form;
            if (Event != "") message +=  ";事件:" + Event;
            _log.Error(message);
        }
        public void Error(String message, System.Exception exception, String form, String Event = "")
        {
            message +=  ";模块:" + form;
            if (Event != "") message +=  ";事件:" + Event;
            _log.Error(message, exception);
        }
        public void Debug(String message, String form, String Event = "")
        {
            {
                message +=  ";模块:" + form;
                if (Event != "") message +=  ";事件:" + Event;
                _log.Debug(message);
            }
        }
        public void Info(String message, String form, String Event = "")
        {
            message +=  ";模块:" + form;
            if (Event != "") message +=  ";事件:" + Event;
            _log.Info(message);
        }
        public void ClearFile()
        {
            string cFile = System.IO.Directory.GetCurrentDirectory();
            cFile += "\\Log\\Logger.log";
            FileStream stream = File.Create(cFile);
            stream.Close();
        }
        public void OpenFile(string cFile = "")
        {
            if (cFile == "")
            {
                cFile = System.IO.Directory.GetCurrentDirectory();
                cFile += "\\Log\\Logger.log";
            }
            System.Diagnostics.Process.Start(cFile);
        }
    }
    internal class ConfigFileCreate
    {
        public const string Log4Config = "log4net.xml";
        internal string GetConfigFile()
        {
            string _AssemblyCatalogue = GetAssemblyCatalogue();
            FileInfo _fileinfo = new FileInfo(_AssemblyCatalogue + @"/" + Log4Config);
            if (!_fileinfo.Exists)
            {
                CreateXMLConfig(_fileinfo);
            }
            return _fileinfo.FullName;
        }
        private string GetAssemblyCatalogue()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
        private void CreateXMLConfig(FileInfo fileInfo)
        {
            using (StreamWriter sw = File.CreateText(fileInfo.FullName))
            {
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                sw.WriteLine("<configuration>");
                sw.WriteLine(" <log4net><!-- contain the Log2net component 's configuration information -->");
                sw.WriteLine("<root>");
                sw.WriteLine("<!-- base configuration-->");
                sw.WriteLine("<level value=\"Debug\"></level>");
                sw.WriteLine("<appender-ref ref=\"RootAppender\" />");
                sw.WriteLine("</root>");
                sw.WriteLine("<appender name=\"RootAppender\" type=\"log4net.Appender.RollingFileAppender\">");
                sw.WriteLine("<file value=\" " + fileInfo.DirectoryName + "\\Log\\Logger.log\" />");
                sw.WriteLine("<appendToFile value=\"true\" />");
                sw.WriteLine(" <rollingStyle value=\"Composite\" />");
                sw.WriteLine("<datePattern value=\"yyyyMMdd\" />");
                sw.WriteLine("<param name=\"MaxSizeRollBackups\" value=\"-1\" />");
                sw.WriteLine("<param name=\"MaximumFileSize\" value=\"10MB\" />");
                sw.WriteLine("<lockingModel type=\"log4net.Appender.FileAppender+MinimalLock\" />");
                sw.WriteLine("<layout type=\"log4net.Layout.PatternLayout\" >");
                sw.WriteLine("<conversionPattern value=\"%date [%c]-[%p] %m%n\" />");
                sw.WriteLine("</layout>");
                sw.WriteLine("</appender>");
                sw.WriteLine("<logger name=\"Test\">");
                sw.WriteLine("<level value=\"Debug\"/>   ");
                sw.WriteLine("<appender-ref ref=\"RootAppender\" />");
                sw.WriteLine("</logger>");
                sw.WriteLine("</log4net>");
                sw.WriteLine("</configuration>");
                sw.Close();
            }
        }
    }
}