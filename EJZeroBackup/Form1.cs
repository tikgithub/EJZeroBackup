using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace EJZeroBackup
{
    public partial class Form1 : Form
    {
        String fileBackup = @"C:\Program Files\NCR APTRA\Advance NDC\Data\EJDATA.LOG";
        String LOBFile = @"C:\Program Files\NCR APTRA\Advance NDC\Data\EJDATA.LOB";
        String BackupFlag = "";
        String ConfigFile =  @"D:\config.ini";
        IniFile ini;
        public Form1()
        {
            InitializeComponent();
            ini = new IniFile(this.ConfigFile);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           

            this.WindowState = FormWindowState.Minimized;
            BackupFlag = ini.ReadINI("BACKUP_DATA", "BACKUP_FLAG");
            if (BackupFlag == "DONE")
            {
                ini.WriteINI("BACKUP_DATA", "BACKUP_FLAG","NONE");
            }
            timer1.Start();
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                FileInfo f = new FileInfo(this.fileBackup);
                FileInfo lob = new FileInfo(this.LOBFile);

                if (f.Length == 0)
                {
                    Thread.Sleep(3000);
                    BackupFlag = ini.ReadINI("BACKUP_DATA", "BACKUP_FLAG");
                    String LastSize = ini.ReadINI("BACKUP_DATA", "LAST_SIZE");

                    if (BackupFlag == "NONE")
                    {
                        if (LastSize == lob.Length.ToString())
                        {
                            return;
                        }
                        String todayDate = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("d2") + DateTime.Now.Day.ToString("d2");

                        //Get Last Backup time from config file
                        String LastBackupTime = ini.ReadINI("BACKUP_DATA", "LAST_BACKUP");


                        if (LastBackupTime != todayDate)
                        {

                            //Set filename = today date
                            String filename = todayDate + "_BLANKEJ"; //DateTime.Now.Hour.ToString("d2") + DateTime.Now.Minute.ToString("d2") + DateTime.Now.Second.ToString("d2");

                            File.Copy(this.LOBFile, @"C:\EJbackup\EJDATA_" + filename + ".LOG", true);

                            ini.WriteINI("BACKUP_DATA", "LAST_BACKUP", todayDate);
                            ini.WriteINI("BACKUP_DATA", "BACKUP_FLAG", "DONE");
                            Debug.WriteLine("FIlesize: " + lob.Length);
                            ini.WriteINI("BACKUP_DATA", "LAST_SIZE", lob.Length.ToString());
                        }
                    }

                }
            }
            catch(Exception ex)
            {

            }
       
        }
    }
}
