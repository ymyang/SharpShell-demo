using System;
using System.IO;
using System.Runtime.InteropServices;

using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;

using Microsoft.Data.Sqlite;

namespace MyNormalIconOverlayHandler
{
    [ComVisible(true)]
    public class MyNormalIconOverlayHandler : SharpIconOverlayHandler
    {
        protected override bool CanShowOverlay(string path, FILE_ATTRIBUTE attributes)
        {
            //  Return true if the file is read only, meaning we'll show the overlay.
            try
            {  
                string app = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string dbPath = Path.Combine(app, @"yliyun\db\file_status.db");
                using (SqliteConnection conn = new SqliteConnection("Data Source =" + dbPath))
                {
                    string rootDir = @"D:\一粒云盘";

                    conn.Open();

                    SqliteCommand cmd1 = new SqliteCommand("SELECT syncRoot FROM yliyun_conf", conn);

                    SqliteDataReader reader1 = cmd1.ExecuteReader();
                    if (reader1.Read())
                    {
                        rootDir = reader1.GetString(0);
                    }

                    if (path.StartsWith(Path.Combine(rootDir, @"个人空间\"))
                        || path.StartsWith(Path.Combine(rootDir, @"群组空间\"))
                        || path.StartsWith(Path.Combine(rootDir, @"部门空间\"))
                        || path.StartsWith(Path.Combine(rootDir, @"共享空间\")))
                    {
                        string sql = "SELECT lastModified FROM file_status WHERE filePath = @fp";
                        SqliteCommand cmd = new SqliteCommand(sql, conn);
                        cmd.Parameters.AddRange(new[]
                            {
                                new SqliteParameter("@fp", path)
                            });

                        SqliteDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
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
            return Properties.Resources.NormalIcon;
        }

        protected override int GetPriority()
        {
            return 2;
        }
    }
}
