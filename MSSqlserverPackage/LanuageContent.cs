using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSqlserverPackage
{
    public class LanuageContent
    {
        public string Name { get; set; }
        public string Display { get; set; }

        [LanuageItemKey("MainFrom_Text")]
        public string MainFormText { get; set; }

        [LanuageItemKey("MainForm_MenuItem_File")]
        public string MainFormMenuItemFile { get; set; }
        
        [LanuageItemKey("MainForm_MenuItem_File_Open")]
        public string MainFormMenuItemFileOpen { get; set; }

        [LanuageItemKey("MainForm_MenuItem_File_Connect")]
        public string MainFormMenuItemFileConnect { get; set; }

        [LanuageItemKey("MainForm_TreeNode_Table")]
        public string MainFormTreeNodeTable { get; set; }

        [LanuageItemKey("MainFrom_TreeNode_View")]
        public string MainFormTreeNodeView { get; set; }

        [LanuageItemKey("MianForm_ToolStripMenuItemData")]
        public string MianFormToolStripMenuItemData { get; set; }

        [LanuageItemKey("MianForm_ToolStripMenuItemDesign")]
        public string MianFormToolStripMenuItemDesign { get; set; }

        [LanuageItemKey("MianForm_ToolStripMenuItemScript")]
        public string MianFormToolStripMenuItemScript { get; set; }

        [LanuageItemKey("MainForm_ToolStripMenuItem_Seeting")]
        public string MainFormToolStripMenuItemSeeting { get; set; }

        [LanuageItemKey("MainForm_ErrorMessage_QueryDataBaseObjectInfo")]
        public string MainFormErrorMessageQueryDataBaseObjectInfo { get; set; }
    }
}
