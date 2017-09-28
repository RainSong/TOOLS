using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSqlserverPackage
{
    public class LanuageConfig
    {
        public string Name { get; set; }
        public string Display { get; set; }

        [LanuageItemKey("MainFrom_Text")]
        public string MainFromText { get; set; }

        [LanuageItemKey("MainFrom_MenuItem_File")]
        public string MainFromMenuItemFile { get; set; }
        
        [LanuageItemKey("MainFrom_MenuItem_File_Open")]
        public string MainFromMenuItemFileOpen { get; set; }

        [LanuageItemKey("MainFrom_MenuItem_File_Connect")]
        public string MainFromMenuItemFileConnect { get; set; }

        [LanuageItemKey("MainFrom_TreeNode_Table")]
        public string MainFromTreeNodeTable { get; set; }

        [LanuageItemKey("MainFrom_TreeNode_View")]
        public string MainFromTreeNodeView { get; set; }

        [LanuageItemKey("MianForm_ToolStripMenuItemData")]
        public string MianFormToolStripMenuItemData { get; set; }

        [LanuageItemKey("MianForm_ToolStripMenuItemDesign")]
        public string MianFormToolStripMenuItemDesign { get; set; }

        [LanuageItemKey("MianForm_ToolStripMenuItemScript")]
        public string MianFormToolStripMenuItemScript { get; set; }

        [LanuageItemKey("MainForm_ToolStripMenuItem_Seeting")]
        public string MainFormToolStripMenuItemSeeting { get; set; }
    }
}
