using System;
using System.IO;
using System.Runtime.InteropServices;

using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;

using System.Data.SQLite;

namespace MyRootIconOverlayhandler
{
    [ComVisible(true)]
    public class MyRootIconOverHandler : SharpIconOverlayHandler
    {
        protected override bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes)
        {
            //  Return true if the file is read only, meaning we'll show the overlay.
            try
            {
                string app = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string dbPath = Path.Combine(app, @"yliyun\db\file_status.db");
                using (SQLiteConnection conn = new SQLiteConnection("Data Source =" + dbPath))
                {
                    string rootDir = @"D:\一粒云盘";

                    conn.Open();

                    SQLiteCommand cmd1 = new SQLiteCommand("SELECT syncRoot FROM yliyun_conf", conn);

                    SQLiteDataReader reader1 = cmd1.ExecuteReader();
                    if (reader1.Read())
                    {
                        rootDir = reader1.GetString(0);
                    }

                    if (path.Equals(rootDir)
                        || path.Equals(Path.Combine(rootDir, @"个人空间"))
                        || path.Equals(Path.Combine(rootDir, @"群组空间"))
                        || path.Equals(Path.Combine(rootDir, @"部门空间"))
                        || path.Equals(Path.Combine(rootDir, @"共享空间")))
                    {
                        return true;
                    } else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override System.Drawing.Icon GetOverlayIcon()
        {
            return Properties.Resources.yliyun_48;
        }

        protected override int GetPriority()
        {
            return 1;
        }
    }
}
