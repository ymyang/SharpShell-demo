using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using SharpShell.Interop;
using SharpShell.SharpIconOverlayHandler;

using System.Data.SQLite;

namespace MyIgnoredIconOverlayHandler
{
    [ComVisible(true)]
    public class MyIgnoredIconOverlayHandler : SharpIconOverlayHandler
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

                    if (path.StartsWith(Path.Combine(rootDir, @"个人空间\"))
                        || path.StartsWith(Path.Combine(rootDir, @"群组空间\"))
                        || path.StartsWith(Path.Combine(rootDir, @"部门空间\"))
                        || path.StartsWith(Path.Combine(rootDir, @"共享空间\")))
                    {
                        string sql = "SELECT lastModified FROM file_status WHERE filePath = @fp";
                        SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                        cmd.Parameters.AddRange(new[]
                            {
                                new SQLiteParameter("@fp", path)
                            });

                        SQLiteDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            return false;
                        }
                        else
                        {
                            return isTmpFile(path);
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
            return Properties.Resources.IgnoredIcon;
        }

        protected override int GetPriority()
        {
            return 3;
        }

        private bool isTmpFile(string path)
        {
            string REG_TMP_FILE = @"^(.*)\.(yli)\.(part)$";
            string REG_OFFICE_TMP_FILE = @"(^(~\$).*\.(doc|docx|ppt|pptx|xls|xlsx)$)|(^~.*\.(tmp)$)";
            string name = Path.GetFileName(path);
            if (Regex.IsMatch(name, REG_TMP_FILE) || Regex.IsMatch(name, REG_OFFICE_TMP_FILE))
            {
                return true;
            }
            return false;
        }
    }
}
