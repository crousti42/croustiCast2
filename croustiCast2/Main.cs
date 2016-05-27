using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace croustiCast
{

    public class Main
    {
        // Ressource
        protected Assembly assembly = Assembly.GetExecutingAssembly();

        // Log
        protected static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Tray Icon
        //protected NotifyIcon TrayIcon;
        //protected ContextMenuStrip TrayIconContextMenu;
        //protected ToolStripMenuItem CloseMenuItem;

        // Path
        protected string sAppPath = "";

        public Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            // Gestion des repertoires
            sAppPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "croustiCast");
            
            if (!Directory.Exists(sAppPath))
            {
                Directory.CreateDirectory(sAppPath);
                Directory.CreateDirectory(Path.Combine(sAppPath, "img"));
            }
            
            // Gestion de la tray icons
            //TrayIcon = new NotifyIcon();
            //TrayIcon.Visible = true;

            //TrayIcon.BalloonTipIcon = ToolTipIcon.Info;
            //TrayIcon.Icon = Properties.Resources.rss_circle;

            //TrayIcon.BalloonTipText = "I noticed that you double-clicked me! What can I do for you?";
            //TrayIcon.BalloonTipTitle = "Info.";

            //TrayIcon.DoubleClick += TrayIcon_DoubleClick;

            //TrayIconContextMenu = new ContextMenuStrip();
            //CloseMenuItem = new ToolStripMenuItem();
            //TrayIconContextMenu.SuspendLayout();

            //this.TrayIconContextMenu.Items.AddRange(new ToolStripItem[] {
            //this.CloseMenuItem});
            //this.TrayIconContextMenu.Name = "TrayIconContextMenu";
            //this.TrayIconContextMenu.Size = new Size(153, 70);

            //this.CloseMenuItem.Name = "CloseMenuItem";
            //this.CloseMenuItem.Size = new Size(152, 22);
            //this.CloseMenuItem.Text = "Close the tray icon program";
            //this.CloseMenuItem.Click += new EventHandler(this.CloseMenuItem_Click);
        }

        //private void TrayIcon_DoubleClick(object sender, EventArgs e)
        //{
        //    TrayIcon.ShowBalloonTip(10000);
        //}

        //private void CloseMenuItem_Click(object sender, EventArgs e)
        //{
        //    if (MessageBox.Show("Do you really want to close me?",
        //            "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
        //            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        //    {
        //        Application.Exit();
        //    }
        //}

        //private void OnApplicationExit(object sender, EventArgs e)
        //{
        //    TrayIcon.Visible = false;
        //}
    }
}