using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;

using SharpShell.Attributes;
using SharpShell.SharpContextMenu;

namespace MyContextMenu
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    public class MyContextMenu : SharpContextMenu
    {
        protected override bool CanShowMenu()
        {
            return true;
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menuItem = new ToolStripMenuItem
                {
                    Text = "保存至一粒云",
                    Image = Properties.Resources.yliyun_16
                };

            menuItem.Click += (sender, args) => OnClick();

            var menu = new ContextMenuStrip();
            menu.Items.Add(menuItem);

            return menu;
        }

        private void OnClick()
        {
            try
            {

                RegistryKey key = Registry.LocalMachine;

                RegistryKey key1 = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\yliyun_start.exe");

                if (key1 != null)
                {
                    string app = key1.GetValue("").ToString();

                    key1.Close();

                    if (app != null && !app.Trim().Equals(""))
                    {
                        //  Builder for the output.
                        var builder = new StringBuilder("--action=up");

                        builder.Append(" --file=");

                        //  Go through each file.
                        foreach (var filePath in SelectedItemPaths)
                        {
                            //  Count the lines.
                            builder.Append(filePath).Append(":");
                        }

                        builder.Remove(builder.Length - 1, 1);

                        Process proc = new Process();
                        proc.StartInfo.CreateNoWindow = true;
                        proc.StartInfo.UseShellExecute = true;
                        proc.StartInfo.FileName = app;
                        proc.StartInfo.Arguments = builder.ToString();
                        // proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                        proc.Start();
                    }
                    else
                    {
                        MessageBox.Show("未找到一粒云启动程序。请重新安装一粒云");
                    }
                } else
                {
                    MessageBox.Show("未找到一粒云启动程序。请重新安装一粒云");
                }
            } catch
            {
                MessageBox.Show("未找到一粒云启动程序。请重新安装一粒云");
            }

        }
    }
}
