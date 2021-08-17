using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessingDemo
{
    public class FormX : Form
    {
        #region Static Members ####################################################  
        private static readonly ILog log = LogManager.GetLogger(typeof(FormX));
        #endregion

        #region Members Variables #################################################  
        private bool logWatching = true;
        private log4net.Appender.MemoryAppender logger;
        private Thread logWatcher;
        /// <summary>  
        /// The TextBox for our logging messages  
        /// </summary>  
        private System.Windows.Forms.TextBox mLog;
        /// <summary>  
        /// Required designer variable.  
        /// </summary>  
        private System.ComponentModel.Container components = null;
        #endregion

        #region Constructors #################################################  
        public FormX()
        {
            //  
            // Required for Windows Form Designer support  
            //  
            InitializeComponent();

            this.Closing += new CancelEventHandler(FormX_Closing);
            logger = new log4net.Appender.MemoryAppender();

            // Could use a fancier Configurator if you don't want to catch every message  
            log4net.Config.BasicConfigurator.Configure(logger);

            // Since there are no events to catch on logging, we dedicate  
            // a thread to watching for logging events  
            logWatcher = new Thread(new ThreadStart(LogWatcher));
            logWatcher.Start();
        }
        #endregion

        // [...]  

        private void FormX_Closing(object sender, CancelEventArgs e)
        {
            // Gotta stop our logging thread  
            logWatching = false;
            logWatcher.Join();
        }

        private void LogWatcher()
        {
            // we loop until the Form is closed  
            while (logWatching)
            {
                LoggingEvent[] events = logger.Events;
                if (events != null && events.Length > 0)
                {
                    // if there are events, we clear them from the logger,  
                    // since we're done with them  
                    logger.Clear();
                    foreach (LoggingEvent ev in events)
                    {
                        StringBuilder builder;
                        // the line we want to log  
                        string line = ev.LoggerName + ": " + ev.RenderedMessage + "\r\n";
                        // don't want to grow this log indefinetly, so limit to 100 lines  
                        if (mLog.Lines.Length > 99)
                        {
                            builder = new StringBuilder(mLog.Text);
                            // strip out a nice chunk from the beginning  
                            builder.Remove(0, mLog.Text.IndexOf('\r', 3000) + 2);
                            builder.Append(line);
                            mLog.Clear();
                            // using AppendText since that makes sure the TextBox stays 
                            // scrolled at the bottom 
                            mLog.AppendText(builder.ToString());
                        }
                        else
                        {
                            mLog.AppendText(line);
                        }
                    }
                }
                // nap for a while, don't need the events on the millisecond.  
                Thread.Sleep(500);
            }
        }
    }
}
