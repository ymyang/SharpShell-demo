using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

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
                    Text = "上传至一粒云盘",
                    Image = Properties.Resources.yliyun_16
                };

            menuItem.Click += (sender, args) => OnClick();

            var menu = new ContextMenuStrip();
            menu.Items.Add(menuItem);

            return menu;
        }

        private void OnClick()
        {
            //  Builder for the output.
            var builder = new StringBuilder();

            //  Go through each file.
            foreach (var filePath in SelectedItemPaths)
            {
                //  Count the lines.
                builder.AppendLine(Path.GetFileName(filePath));
            }

            //  Show the ouput.
            MessageBox.Show(builder.ToString());
        }
    }
}
