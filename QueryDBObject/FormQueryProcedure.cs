﻿using ScintillaNET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QueryDBObject
{
    public partial class FormQueryProcedure : Form
    {

        #region fields
        public Form LoginForm;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connectionString;
        /// <summary>
        /// 数据库服务器名/IP
        /// </summary>
        private string server;
        /// <summary>
        /// 登录数据的用户名
        /// </summary>
        private string uid;
        /// <summary>
        /// 连接的数据库名
        /// </summary>
        private string dbName;
        /// <summary>
        /// 执行时间
        /// </summary>
        private string executeTime = "00:00:00";
        /// <summary>
        /// 受影响行数
        /// </summary>
        private string rowCount = "0";

        private SearchFlags searchFlags = SearchFlags.None;

        private List<Models.Igrone> igrones = null;
        private List<Models.DBProcedure> objects = null;

        #endregion

        #region constructor

        public FormQueryProcedure(SqlConnectionStringBuilder conStringBuilder)
        {
            InitializeComponent();
            //this.StartPosition = FormStartPosition.CenterScreen;
            //this.WindowState = FormWindowState.Maximized;

            this.connectionString = conStringBuilder.ToString();
            this.server = conStringBuilder.DataSource;
            this.uid = conStringBuilder.UserID;
            this.dbName = conStringBuilder.InitialCatalog;

            
            this.grid.AutoGenerateColumns = false;

            //this.txtNotContains.Text = "test,backup,bak";
            var cScript = this.txtScript as Scintilla;
            if (cScript != null)
            {
                cScript.Text = "";
                InitializeScintillaControls(cScript);
            }

            InitCobobox();

            igrones = new List<Models.Igrone>
            {
                new Models.Igrone("test"),
                new Models.Igrone("backup"),
                new Models.Igrone("bak")
            };
            BindIgronesGrid();


        }
        #endregion

        #region event handles
        private void btnQuery_Click(object sender, EventArgs e)
        {
            var keyWord = this.txtKeyWord.Text.Trim();
            //if (string.IsNullOrEmpty(keyWord))
            //{
            //    MessageHelper.ShowMessage("请输入查询关键字");
            //    return;
            //}
            var nameKeyWord = this.txtName.Text.Trim();
            var notContainsKeyWord = string.Empty;// this.txtNotContains.Text.Trim();
            this.objects = GetObjects(keyWord, nameKeyWord, notContainsKeyWord);
            BindObjectGrid();

        }
        private void Main_Load(object sender, EventArgs e)
        {
            //GetConnectInfo();
            ShowStipLabel();
        }
        
        /// <summary>
        /// 在行标头中添加编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // https://stackoverflow.com/questions/9581626/show-row-number-in-row-header-of-a-datagridview
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.ColumnIndex == 2 || e.ColumnIndex == 3) && e.Value != null)
            {
                var value = (DateTime)e.Value;
                e.Value = value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                Models.DBProcedure obj = (sender as DataGridView).Rows[e.RowIndex].DataBoundItem as Models.DBProcedure;
                if (obj == null) return;
                if (string.IsNullOrEmpty(obj.script))
                {
                    if (string.IsNullOrEmpty(obj.name)) return;
                    var procedureName = obj.name;
                    DataTable dtScripts = null;
                    try
                    {
                        dtScripts = GetProcedureScripts(procedureName);
                        obj.script = FormatScript(dtScripts);
                    }
                    catch (Exception ex)
                    {
                        MessageHelper.ShowMessage(ex.Message, "Error");
                        Logger.Error(string.Format("查询脚存储过程{0}本失败", obj.script), ex);
                        return;
                    }
                }
                this.txtScript.ReadOnly = false;
                this.txtScript.Text = obj.script;
                this.txtScript.ReadOnly = true;
            }
        }

        /// <summary>
        /// 脚本显示控件，按下CTRL+F组合键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtScript_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                var target = this.txtScript.SelectedText;

                var fw = new FindWindow(target, this.txtScript.Text);
                fw.StartPosition = FormStartPosition.CenterParent;
                fw.FindeNext += Fw_FindeNext;
                fw.Show();
            }
        }
        /// <summary>
        /// 搜索窗体，搜索下一个按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Fw_FindeNext(object sender, FindNextEventArgs e)
        {
            var index = e.NextIndex;

            this.txtScript.SearchFlags = searchFlags;
            this.txtScript.TargetStart = Math.Max(this.txtScript.CurrentPosition, this.txtScript.AnchorPosition);
            this.txtScript.TargetEnd = this.txtScript.TextLength;

            var pos = this.txtScript.SearchInTarget(e.SearchTarget);
            if (pos >= 0)
                this.txtScript.SetSel(this.txtScript.TargetStart, this.txtScript.TargetEnd);
        }
        /// <summary>
        /// 查询内容输入框按钮按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btnQuery.PerformClick();
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// 为列表查询绑定数据方法
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="nameKeyWord"></param>
        /// <param name="notContainsKeyWord"></param>
        private List<Models.DBProcedure> GetObjects(string keyWord, string nameKeyWord, string notContainsKeyWord)
        {
            var sqlComment = new StringBuilder("SELECT DISTINCT sys.objects.object_id AS id" +
                             "      ,sys.objects.name " +
                             "	    ,sys.objects.modify_date " +
                             "	    ,sys.objects.create_date " +
                             "FROM sys.syscomments " +
                             "LEFT JOIN sys.objects ON sys.syscomments.id = sys.objects.object_id " +
                             "WHERE type = 'p' ");
            //text LIKE '%' + @KeyWord + '%'
            var paras = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(keyWord))
            {
                sqlComment.Append(" AND sys.syscomments.text LIKE @KeyWord");
                paras.Add(new SqlParameter
                {
                    ParameterName = "@KeyWord",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = "%" + keyWord + "%"
                });
            }
            if (!string.IsNullOrEmpty(nameKeyWord))
            {
                sqlComment.Append(" AND sys.objects.name LIKE @NameKeyWord");
                paras.Add(new SqlParameter
                {
                    ParameterName = "@NameKeyWord",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = "%" + nameKeyWord + "%"
                });
            }
            if (this.igrones != null && this.igrones.Any())
            {
                var igronKeyWords = this.igrones.Where(o => string.Equals(o.IgroneType, "KeyWord"))
                                                .Select(o => o.Content)
                                                .AsEnumerable();
                if (igronKeyWords.Any())
                {
                    foreach (string kw in igronKeyWords)
                    {
                        var para = new SqlParameter
                        {
                            ParameterName = "@NotLike_" + kw,
                            SqlDbType = SqlDbType.VarChar,
                            Value = "%" + kw + "%"
                        };
                        sqlComment.Append(" AND sys.objects.name NOT LIKE " + para.ParameterName);
                        paras.Add(para);
                    }
                }
                var names = this.igrones.Where(o => string.Equals(o.IgroneType, "Name"))
                                        .Select(o => o.Content)
                                        .AsEnumerable();
                if (names.Any())
                {
                    var nameParas = new List<SqlParameter>();
                    foreach (string name in names)
                    {
                        var para = new SqlParameter
                        {
                            ParameterName = "@NotEquals_" + name,
                            SqlDbType = SqlDbType.VarChar,
                            Value = name
                        };
                        nameParas.Add(para);
                    }
                    paras.AddRange(nameParas);
                    sqlComment.AppendFormat(" sys.objects.name NOT IN ({0})", string.Join(",", nameParas.Select(p => p.ParameterName).ToArray()));
                }

            }

            var sort = string.Empty;

            var item = this.cboSort.SelectedItem as dynamic;
            if (item != null)
            {
                sort = item.Value;
            }
            if (!string.IsNullOrEmpty(sort))
            {
                item = this.cboSortType.SelectedItem as dynamic;
                if (item != null)
                {
                    sort += " " + item.Value;
                }
                sqlComment.Append(" order by ");
                sqlComment.Append(sort);
            }

            //Action<object> actionQuery = (s) => 
            //{
            try
            {
                IDictionary dic = new Dictionary<object, object>();
                var sqlHelper = new Common.SqlHelper();
                var list = sqlHelper.ExecuteQuery<Models.DBProcedure>(this.connectionString, sqlComment.ToString(), out dic, paras.ToArray());
                this.rowCount = list.Count.ToString();

                var et = (long)dic["ExecutionTime"];
                ComputeTime(et);
                this.rowCount = dic["SelectRows"].ToString();
                ShowStipLabel();
                return list;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("查询失败：" + ex.Message, "Error");
                Logger.Error("根据关键字查询存储过程事变", ex);
            }
            return new List<Models.DBProcedure>();
            //};
            //var state = (object)new string[] { };
            //var task = new Task(actionQuery, state);
        }
        /// <summary>
        /// 状态栏显示
        /// </summary>
        private void ShowStipLabel()
        {
            if (string.IsNullOrEmpty(this.server))
            {
                this.tssLblServer.Text =
                    this.tssLblSplit1.Text = string.Empty;
            }
            else
            {
                this.tssLblServer.Text = server;
                this.tssLblSplit1.Text = " | ";
            }
            if (string.IsNullOrEmpty(this.dbName))
            {
                this.tssLblDataBase.Text =
                    this.tssLblSplit2.Text = string.Empty;
            }
            else
            {
                this.tssLblDataBase.Text = dbName;
                this.tssLblSplit2.Text = " | ";
            }
            if (string.IsNullOrEmpty(this.uid))
            {
                this.tssLblUserID.Text =
                    this.tssLblSplit3.Text = string.Empty;
            }
            else
            {
                this.tssLblUserID.Text = uid;
                this.tssLblSplit3.Text = " | ";
            }
            if (string.IsNullOrEmpty(this.executeTime))
            {
                this.tssLblExecuteTime.Text =
                    this.tssLblSplit4.Text = string.Empty;
            }
            else
            {
                this.tssLblExecuteTime.Text = executeTime;
                this.tssLblSplit4.Text = " | ";
            }
            if (string.IsNullOrEmpty(this.rowCount))
            {
                this.tssLblRowCount.Text = string.Empty;
            }
            else
            {
                this.tssLblRowCount.Text = rowCount + "行";
            }
        }
        /// <summary>
        /// 将数据库执行时间由秒数转换为0：00：00的格式
        /// </summary>
        /// <param name="seconds"></param>
        private void ComputeTime(long seconds)
        {
            seconds = seconds / 1000;
            int hour = 0;
            int min = 0;
            if (seconds >= 3600)
            {
                hour = (int)seconds / 3600;
                seconds = seconds % 3600;
            }
            if (seconds >= 60)
            {
                min = (int)seconds / 60;
                seconds = seconds % 60;
            }
            this.executeTime = hour.ToString().PadLeft(2, '0') + ":" + min.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
        }
        /// <summary>
        /// 查询存储过程脚本
        /// </summary>
        /// <param name="prodecureName"></param>
        /// <returns></returns>
        private DataTable GetProcedureScripts(string prodecureName)
        {
            var sqlCommand = "declare @object_id	int,                                                                 " +
                             "        @sourcecode   nvarchar(max),                                                       " +
                             "        @line         nvarchar(max),                                                       " +
                             "        @end          int,                                                                 " +
                             "        @rn           nvarchar(2),                                                         " +
                             "        @tab          nvarchar(1)                                                          " +
                             "declare @source table(source nvarchar(max)  COLLATE Chinese_PRC_CS_AI_WS)                  " +
                             "                                                                                           " +
                             "set @rn = char(13)+char(10)                                                                           " +
                             "set @tab = char(9)                                                                            " +
                             "                                                                                           " +
                             "select @sourcecode = definition  COLLATE Chinese_PRC_CS_AI_WS from sys.sql_modules where object_id = object_id(@objname) " +
                             "                                                                                           " +
                             "while (charindex(@rn, @sourcecode) != 0)                                                   " +
                             "begin                                                                                      " +
                             "   set @end = charindex(@rn, @sourcecode)                                                  " +
                             "   set @line = replace(substring(@sourcecode, 1, @end - 1), @tab, 'char(9)')               " +
                             "   insert into @source(source) values(@line)                                               " +
                             "   set @end = @end + 2                                                                     " +
                             "   set @sourcecode = substring(@sourcecode, @end, len(@sourcecode))                        " +
                             "end                                                                                        " +
                             "insert into @source(source) values(@sourcecode)                                            " +
                             "                                                                                           " +
                             "select* from @source                                                                       ";
            try
            {
                IDictionary dic = new Dictionary<object, object>();
                var paraObjName = new SqlParameter
                {
                    ParameterName = "@objname",
                    SqlDbType = SqlDbType.VarChar,
                    Value = prodecureName
                };
                var sqlHelper = new Common.SqlHelper();
                return sqlHelper.ExecuteQuery(this.connectionString, sqlCommand, CommandType.Text, paraObjName);
            }
            catch (Exception ex)
            {
                throw new Exception("查询存储过程脚本错误", ex);
            }
        }
        /// <summary>
        /// 格式化存储过程脚本
        /// </summary>
        /// <param name="dtScripts"></param>
        /// <returns></returns>
        private string FormatScript(DataTable dtScripts)
        {
            var sb = new StringBuilder();

            foreach (DataRow dr in dtScripts.Rows)
            {
                var val = dr[0];
                if (val == null || DBNull.Value.Equals(val))
                {
                    sb.AppendLine();
                    continue;
                }
                var str = val.ToString().Replace("char(9)", "\t");
                sb.AppendLine(str);
            }
            var script = sb.ToString();
            return script;
        }
        /// <summary>
        /// 初始化脚本显示控件
        /// </summary>
        /// <param name="scintilla"></param>
        private void InitializeScintillaControls(Scintilla scintilla)
        {

            //scintilla.LexerLanguage = "mssql";
            // Reset the styles
            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].BackColor = SystemColors.Window;
            scintilla.Styles[Style.Default].Font = PreferredFont();
            scintilla.Styles[Style.Default].SizeF = 10F;

            scintilla.ExtraAscent = 3;
            scintilla.ExtraDescent = 3;

            scintilla.StyleClearAll();

            // Set the SQL Lexer
            scintilla.Lexer = Lexer.Sql;

            // Show line numbers
            scintilla.Margins[0].Width = 30;

            // Set the Styles
            scintilla.Styles[Style.Sql.Identifier].ForeColor = Color.SeaGreen;
            scintilla.Styles[Style.LineNumber].ForeColor = Color.FromArgb(255, 128, 128, 128);  //Dark Gray
            scintilla.Styles[Style.LineNumber].BackColor = Color.FromArgb(255, 228, 228, 228);  //Light Gray
            scintilla.Styles[Style.Sql.Comment].ForeColor = Color.Green;
            scintilla.Styles[Style.Sql.CommentLine].ForeColor = Color.Green;
            scintilla.Styles[Style.Sql.CommentLineDoc].ForeColor = Color.Green;
            scintilla.Styles[Style.Sql.Number].ForeColor = Color.Black;
            scintilla.Styles[Style.Sql.Word].ForeColor = Color.Blue;
            scintilla.Styles[Style.Sql.Word2].ForeColor = Color.Teal;
            scintilla.Styles[Style.Sql.User1].ForeColor = Color.Green;
            scintilla.Styles[Style.Sql.User2].ForeColor = Color.Fuchsia;
            scintilla.Styles[Style.Sql.User3].ForeColor = Color.DarkGray;
            scintilla.Styles[Style.Sql.User4].ForeColor = Color.DarkGoldenrod;
            scintilla.Styles[Style.Sql.String].ForeColor = Color.Crimson;
            scintilla.Styles[Style.Sql.Character].ForeColor = Color.Red;
            scintilla.Styles[Style.Sql.Operator].ForeColor = Color.DarkGoldenrod;

            // Set keyword lists
            #region Word = 0
            scintilla.SetKeywords(0, @"action add all alter any as asc authorization backup begin break browse bulk by " +
                                      "cascade case check checkpoint close clustered coalesce collate column commit " +
                                      "committed compute confirm constraint contains containstable continue controlrow " +
                                      "convert create cross current current_date current_time current_timestamp current_user " +
                                      "cursor database dbcc deallocate declare default delete deny desc disable disk distinct " +
                                      "distributed double drop dummy dump else enable end errlvl errorexit escape except exec " +
                                      "execute exists exit fetch file fillfactor floppy for foreign forward_only freetext " +
                                      "freetexttable from full function go goto grant group having holdlock identity identity_insert " +
                                      "identitycol if in index inner insert intersect into is isolation join key kill left level " +
                                      "lineno load mirrorexit move national no nocheck nonclustered not nounload null nullif of off " +
                                      "offsets on once only open opendatasource openquery openrowset option order outer output over " +
                                      "percent perm permanent pipe plan precision prepare primary print privileges proc procedure " +
                                      "processexit public raiserror read readtext read_only reconfigure recovery references repeatable " +
                                      "replication restore restrict return returns revoke right rollback rowcount rowguidcol rule save " +
                                      "schema select serializable session_user set setuser shutdown some statistics stats system_user table " +
                                      "tape temp temporary textsize then to top tran transaction trigger truncate tsequal uncommitted unique " +
                                      "update updatetext use user values varying view waitfor when where while with work writetext ");
            #endregion
            #region word2 = 1
            scintilla.SetKeywords(1, @"bigint binary bit char character datetime dec decimal float image int integer money nchar ntext " +
                                      "numeric nvarchar real smalldatetime smallint smallmoney sql_variant sysname text timestamp tinyint " +
                                      "uniqueidentifier varbinary varchar ");
            #endregion
            #region user1 = 4
            scintilla.SetKeywords(4, @"deleted inserted sysallocations sysalternates sysaltfiles syscacheobjects syscharsets syscolumns " +
                                      "syscomments sysconfigures sysconstraints syscurconfigs syscursorcolumns syscursorrefs syscursors " +
                                      "syscursortables sysdatabases sysdepends sysdevices sysfilegroups sysfiles sysfiles1 sysforeignkeys " +
                                      "sysfulltextcatalogs sysindexes sysindexkeys syslanguages syslockinfo syslocks syslogins sysmembers " +
                                      "sysmessages sysobjects sysoledbusers sysperfinfo syspermissions sysprocesses sysprotects sysreferences " +
                                      "sysremote_catalogs sysremote_column_privileges sysremote_columns sysremote_foreign_keys sysremote_indexes " +
                                      "sysremote_primary_keys sysremote_provider_types sysremote_schemata sysremote_statistics sysremote_table_privileges " +
                                      "sysremote_tables sysremote_views sysremotelogins syssegments sysservers systypes sysusers sysxlogins ");
            #endregion
            #region user2 = 5
            scintilla.SetKeywords(5, @"abs acos app_name ascii asin atan atn2 avg binary_checksum case cast ceiling char charindex checksum " +
                                      "checksum_agg coalesce collationproperty col_length col_name columns_updated columnproperty convert cos " +
                                      "cot count count_big current_timestamp current_user cursor_status databaseproperty databasepropertyex " +
                                      "datalength dateadd datediff datename datepart day db_id db_name degrees difference exp file_id file_name " +
                                      "filegroup_id filegroup_name filegroupproperty fileproperty floor fn_helpcollations fn_listextendedproperty " +
                                      "fn_servershareddrives fn_trace_geteventinfo fn_trace_getfilterinfo fn_trace_getinfo fn_trace_gettable " +
                                      "fn_virtualfilestats formatmessage fulltextcatalogproperty fulltextserviceproperty getansinull getdate " +
                                      "getutcdate grouping has_dbaccess host_id host_name ident_current ident_incr ident_seed index_col indexkey_property " +
                                      "indexproperty is_member is_srvrolemember isdate isnull isnumeric left len log log10 lower ltrim max min month nchar " +
                                      "newid nullif object_id object_name objectproperty parsename patindex permissions pi power quotename radians rand " +
                                      "replace replicate reverse right round rowcount_big rtrim scope_identity serverproperty sessionproperty session_user " +
                                      "sign sin soundex space sqare sql_variant_property sqrt stats_date stdev stdevp str stuff substring sum suser_sid " +
                                      "suser_sname system_user tan typeproperty unicode upper user_id user_name var varp year ( ) * ");
            #endregion
            #region user3 = 6
            scintilla.SetKeywords(6, @"sp_activedirectory_obj sp_activedirectory_scp sp_activedirectory_start sp_add_agent_parameter sp_add_agent_profile " +
                                      "sp_add_data_file_recover_suspect_db sp_add_log_file_recover_suspect_db sp_add_log_shipping_alert_job " +
                                      "sp_add_log_shipping_primary_database sp_add_log_shipping_primary_secondary sp_add_log_shipping_secondary_database " +
                                      "sp_add_log_shipping_secondary_primary sp_addalias sp_addapprole sp_addarticle sp_adddatatype sp_adddatatypemapping " +
                                      "sp_adddistpublisher sp_adddistributiondb sp_adddistributor sp_adddynamicsnapshot_job sp_addextendedproc sp_addextendedproperty " +
                                      "sp_addgroup sp_addlinkedserver sp_addlinkedsrvlogin sp_addlogin sp_addlogreader_agent sp_addmergealternatepublisher " +
                                      "sp_addmergearticle sp_addmergefilter sp_addmergelogsettings sp_addmergepartition sp_addmergepublication sp_addmergepullsubscription " +
                                      "sp_addmergepullsubscription_agent sp_addmergepushsubscription_agent sp_addmergesubscription sp_addmessage sp_addpublication " +
                                      "sp_addpublication_snapshot sp_addpullsubscription sp_addpullsubscription_agent sp_addpushsubscription_agent sp_addqreader_agent " +
                                      "sp_addqueued_artinfo sp_addremotelogin sp_addrole sp_addrolemember sp_addscriptexec sp_addserver sp_addsrvrolemember " +
                                      "sp_addsubscriber sp_addsubscriber_schedule sp_addsubscription sp_addsynctriggers sp_addsynctriggerscore sp_addtabletocontents " +
                                      "sp_addtype sp_addumpdevice sp_adduser sp_adjustpublisheridentityrange sp_altermessage sp_approlepassword sp_article_validation " +
                                      "sp_articlecolumn sp_articlefilter sp_articleview sp_assemblies_rowset sp_assemblies_rowset_rmt sp_assemblies_rowset2 " +
                                      "sp_assembly_dependencies_rowset sp_assembly_dependencies_rowset_rmt sp_assembly_dependencies_rowset2 sp_attach_db " +
                                      "sp_attach_single_file_db sp_attachsubscription sp_autostats sp_bcp_dbcmptlevel sp_bindefault sp_bindrule sp_browsemergesnapshotfolder " +
                                      "sp_browsereplcmds sp_browsesnapshotfolder sp_can_tlog_be_applied sp_catalogs sp_catalogs_rowset sp_catalogs_rowset_rmt sp_catalogs_rowset2 " +
                                      "sp_certify_removable sp_change_agent_parameter sp_change_agent_profile sp_change_log_shipping_primary_database " +
                                      "sp_change_log_shipping_secondary_database sp_change_log_shipping_secondary_primary sp_change_subscription_properties " +
                                      "sp_change_users_login sp_changearticle sp_changearticlecolumndatatype sp_changedbowner sp_changedistpublisher sp_changedistributiondb " +
                                      "sp_changedistributor_password sp_changedistributor_property sp_changedynamicsnapshot_job sp_changegroup sp_changelogreader_agent " +
                                      "sp_changemergearticle sp_changemergefilter sp_changemergelogsettings sp_changemergepublication sp_changemergepullsubscription " +
                                      "sp_changemergesubscription sp_changeobjectowner sp_changepublication sp_changepublication_snapshot sp_changeqreader_agent " +
                                      "sp_changereplicationserverpasswords sp_changesubscriber sp_changesubscriber_schedule sp_changesubscription sp_changesubscriptiondtsinfo " +
                                      "sp_changesubstatus sp_check_constbytable_rowset sp_check_constbytable_rowset2 sp_check_constraints_rowset sp_check_constraints_rowset2 " +
                                      "sp_check_dynamic_filters sp_check_for_sync_trigger sp_check_join_filter sp_check_log_shipping_monitor_alert sp_check_publication_access " +
                                      "sp_check_removable sp_check_subset_filter sp_check_sync_trigger sp_checkinvalidivarticle sp_checkoraclepackageversion sp_cleanmergelogfiles " +
                                      "sp_cleanup_log_shipping_history sp_cleanupdbreplication sp_cleanupwebtask sp_column_privileges sp_column_privileges_ex " +
                                      "sp_column_privileges_rowset sp_column_privileges_rowset_rmt sp_column_privileges_rowset2 sp_columns sp_columns_90 sp_columns_90_rowset " +
                                      "sp_columns_90_rowset_rmt sp_columns_90_rowset2 sp_columns_ex sp_columns_ex_90 sp_columns_rowset sp_columns_rowset_rmt sp_columns_rowset2 " +
                                      "sp_configure sp_constr_col_usage_rowset sp_constr_col_usage_rowset2 sp_convertwebtasks sp_copymergesnapshot sp_copysnapshot " +
                                      "sp_copysubscription sp_create_removable sp_createmergepalrole sp_createstats sp_createtranpalrole sp_cursor_list sp_cycle_errorlog " +
                                      "sp_databases sp_datatype_info sp_datatype_info_90 sp_db_vardecimal_storage_format sp_dbcmptlevel sp_dbfixedrolepermission " +
                                      "sp_dbmmonitoraddmonitoring sp_dbmmonitorchangealert sp_dbmmonitorchangemonitoring sp_dbmmonitordropalert sp_dbmmonitordropmonitoring " +
                                      "sp_dbmmonitorhelpalert sp_dbmmonitorhelpmonitoring sp_dbmmonitorresults sp_dbmmonitorupdate sp_dboption sp_dbremove sp_ddopen sp_defaultdb " +
                                      "sp_defaultlanguage sp_delete_log_shipping_alert_job sp_delete_log_shipping_primary_database sp_delete_log_shipping_primary_secondary " +
                                      "sp_delete_log_shipping_secondary_database sp_delete_log_shipping_secondary_primary sp_deletemergeconflictrow sp_deletepeerrequesthistory " +
                                      "sp_deletetracertokenhistory sp_denylogin sp_depends sp_describe_cursor sp_describe_cursor_columns sp_describe_cursor_tables sp_detach_db " +
                                      "sp_disableagentoffload sp_distcounters sp_drop_agent_parameter sp_drop_agent_profile sp_dropalias sp_dropanonymousagent " +
                                      "sp_dropanonymoussubscription sp_dropapprole sp_droparticle sp_dropdatatypemapping sp_dropdevice sp_dropdistpublisher " +
                                      "sp_dropdistributiondb sp_dropdistributor sp_dropdynamicsnapshot_job sp_dropextendedproc sp_dropextendedproperty sp_dropgroup " +
                                      "sp_droplinkedsrvlogin sp_droplogin sp_dropmergealternatepublisher sp_dropmergearticle sp_dropmergefilter sp_dropmergelogsettings " +
                                      "sp_dropmergepartition sp_dropmergepublication sp_dropmergepullsubscription sp_dropmergesubscription sp_dropmessage sp_droppublication " +
                                      "sp_droppublisher sp_droppullsubscription sp_dropremotelogin sp_dropreplsymmetrickey sp_droprole sp_droprolemember sp_dropserver " +
                                      "sp_dropsrvrolemember sp_dropsubscriber sp_dropsubscription sp_droptype sp_dropuser sp_dropwebtask sp_dsninfo sp_enable_heterogeneous_subscription " +
                                      "sp_enableagentoffload sp_enum_oledb_providers sp_enumcodepages sp_enumcustomresolvers sp_enumdsn sp_enumeratependingschemachanges " +
                                      "sp_enumerrorlogs sp_enumfullsubscribers sp_enumoledbdatasources sp_estimated_rowsize_reduction_for_vardecimal sp_expired_subscription_cleanup " +
                                      "sp_firstonly_bitmap sp_fkeys sp_foreign_keys_rowset sp_foreign_keys_rowset_rmt sp_foreign_keys_rowset2 sp_foreign_keys_rowset3 sp_foreignkeys " +
                                      "sp_fulltext_catalog sp_fulltext_column sp_fulltext_database sp_fulltext_querytimeout sp_fulltext_recycle_crawl_log sp_fulltext_service " +
                                      "sp_fulltext_table sp_generate_agent_parameter sp_generatefilters sp_get_distributor sp_get_job_status_mergesubscription_agent " +
                                      "sp_get_mergepublishedarticleproperties sp_get_oracle_publisher_metadata sp_getagentparameterlist sp_getapplock sp_getdefaultdatatypemapping " +
                                      "sp_getmergedeletetype sp_getpublisherlink sp_getqueuedarticlesynctraninfo sp_getqueuedrows sp_getsqlqueueversion " +
                                      "sp_getsubscription_status_hsnapshot sp_getsubscriptiondtspackagename sp_grant_publication_access sp_grantdbaccess sp_grantlogin " +
                                      "sp_help sp_help_agent_default sp_help_agent_parameter sp_help_agent_profile sp_help_datatype_mapping sp_help_fulltext_catalog_components " +
                                      "sp_help_fulltext_catalogs sp_help_fulltext_catalogs_cursor sp_help_fulltext_columns sp_help_fulltext_columns_cursor " +
                                      "sp_help_fulltext_system_components sp_help_fulltext_tables sp_help_fulltext_tables_cursor sp_help_log_shipping_alert_job " +
                                      "sp_help_log_shipping_monitor sp_help_log_shipping_monitor_primary sp_help_log_shipping_monitor_secondary sp_help_log_shipping_primary_database " +
                                      "sp_help_log_shipping_primary_secondary sp_help_log_shipping_secondary_database sp_help_log_shipping_secondary_primary sp_help_publication_access " +
                                      "sp_helpallowmerge_publication sp_helparticle sp_helparticlecolumns sp_helparticledts sp_helpconstraint sp_helpdatatypemap sp_helpdb sp_helpdbfixedrole " +
                                      "sp_helpdevice sp_helpdistpublisher sp_helpdistributiondb sp_helpdistributor sp_helpdistributor_properties sp_helpdynamicsnapshot_job " +
                                      "sp_helpextendedproc sp_helpfile sp_helpfilegroup sp_helpgroup sp_helpindex sp_helplanguage sp_helplinkedsrvlogin sp_helplogins " +
                                      "sp_helplogreader_agent sp_helpmergealternatepublisher sp_helpmergearticle sp_helpmergearticlecolumn sp_helpmergearticleconflicts " +
                                      "sp_helpmergeconflictrows sp_helpmergedeleteconflictrows sp_helpmergefilter sp_helpmergelogfiles sp_helpmergelogfileswithdata " +
                                      "sp_helpmergelogsettings sp_helpmergepartition sp_helpmergepublication sp_helpmergepullsubscription sp_helpmergesubscription " +
                                      "sp_helpntgroup sp_helppeerrequests sp_helppeerresponses sp_helppublication sp_helppublication_snapshot sp_helppublicationsync " +
                                      "sp_helppullsubscription sp_helpqreader_agent sp_helpremotelogin sp_helpreplfailovermode sp_helpreplicationdb sp_helpreplicationdboption " +
                                      "sp_helpreplicationoption sp_helprole sp_helprolemember sp_helprotect sp_helpserver sp_helpsort sp_helpsrvrole sp_helpsrvrolemember " +
                                      "sp_helpstats sp_helpsubscriberinfo sp_helpsubscription sp_helpsubscription_properties sp_helpsubscriptionerrors sp_helptext " +
                                      "sp_helptracertokenhistory sp_helptracertokens sp_helptrigger sp_helpuser sp_helpxactsetjob sp_http_generate_wsdl_defaultcomplexorsimple " +
                                      "sp_http_generate_wsdl_defaultsimpleorcomplex sp_identitycolumnforreplication sp_ih_lr_getcachedata sp_ihadd_sync_command sp_iharticlecolumn " +
                                      "sp_ihget_loopback_detection sp_ihscriptidxfile sp_ihscriptschfile sp_ihvalidaterowfilter sp_ihxactsetjob sp_indexes sp_indexes_90_rowset " +
                                      "sp_indexes_90_rowset_rmt sp_indexes_90_rowset2 sp_indexes_rowset sp_indexes_rowset_rmt sp_indexes_rowset2 sp_indexoption sp_invalidate_textptr " +
                                      "sp_ivindexhasnullcols sp_lightweightmergemetadataretentioncleanup sp_link_publication sp_linkedservers sp_linkedservers_rowset sp_linkedservers_rowset2 " +
                                      "sp_lock sp_logshippinginstallmetadata sp_lookupcustomresolver sp_makewebtask sp_mapdown_bitmap sp_markpendingschemachange sp_marksubscriptionvalidation " +
                                      "sp_mergearticlecolumn sp_mergecleanupmetadata sp_mergedummyupdate sp_mergemetadataretentioncleanup sp_mergesubscription_cleanup sp_mergesubscriptionsummary " +
                                      "sp_monitor sp_ms_replication_installed sp_msacquireheadofqueuelock sp_msacquireserverresourcefordynamicsnapshot sp_msacquireslotlock " +
                                      "sp_msacquiresnapshotdeliverysessionlock sp_msactivate_auto_sub sp_msactivatelogbasedarticleobject sp_msactivateprocedureexecutionarticleobject " +
                                      "sp_msadd_anonymous_agent sp_msadd_article sp_msadd_compensating_cmd sp_msadd_distribution_agent sp_msadd_distribution_history " +
                                      "sp_msadd_dynamic_snapshot_location sp_msadd_filteringcolumn sp_msadd_log_shipping_error_detail sp_msadd_log_shipping_history_detail " +
                                      "sp_msadd_logreader_agent sp_msadd_logreader_history sp_msadd_merge_agent sp_msadd_merge_anonymous_agent sp_msadd_merge_history " +
                                      "sp_msadd_merge_history90 sp_msadd_merge_subscription sp_msadd_mergereplcommand sp_msadd_mergesubentry_indistdb sp_msadd_publication " +
                                      "sp_msadd_qreader_agent sp_msadd_qreader_history sp_msadd_repl_alert sp_msadd_repl_command sp_msadd_repl_commands27hp sp_msadd_repl_error " +
                                      "sp_msadd_replcmds_mcit sp_msadd_replmergealert sp_msadd_snapshot_agent sp_msadd_snapshot_history sp_msadd_subscriber_info sp_msadd_subscriber_schedule " +
                                      "sp_msadd_subscription sp_msadd_subscription_3rd sp_msadd_tracer_history sp_msadd_tracer_token sp_msaddanonymousreplica sp_msadddynamicsnapshotjobatdistributor " +
                                      "sp_msaddguidcolumn sp_msaddguidindex sp_msaddinitialarticle sp_msaddinitialpublication sp_msaddinitialschemaarticle sp_msaddinitialsubscription " +
                                      "sp_msaddlightweightmergearticle sp_msaddmergedynamicsnapshotjob sp_msaddmergetriggers sp_msaddmergetriggers_from_template sp_msaddmergetriggers_internal " +
                                      "sp_msaddpeerlsn sp_msaddsubscriptionarticles sp_msadjust_pub_identity sp_msagent_retry_stethoscope sp_msagent_stethoscope sp_msallocate_new_identity_range " +
                                      "sp_msalreadyhavegeneration sp_msanonymous_status sp_msarticlecleanup sp_msbrowsesnapshotfolder sp_mscache_agent_parameter sp_mschange_article " +
                                      "sp_mschange_distribution_agent_properties sp_mschange_logreader_agent_properties sp_mschange_merge_agent_properties sp_mschange_mergearticle " +
                                      "sp_mschange_mergepublication sp_mschange_priority sp_mschange_publication sp_mschange_retention sp_mschange_retention_period_unit " +
                                      "sp_mschange_snapshot_agent_properties sp_mschange_subscription_dts_info sp_mschangearticleresolver sp_mschangedynamicsnapshotjobatdistributor " +
                                      "sp_mschangedynsnaplocationatdistributor sp_mschangeobjectowner sp_mscheck_agent_instance sp_mscheck_jet_subscriber sp_mscheck_logicalrecord_metadatamatch " +
                                      "sp_mscheck_merge_subscription_count sp_mscheck_pub_identity sp_mscheck_pull_access sp_mscheck_snapshot_agent sp_mscheck_subscription " +
                                      "sp_mscheck_subscription_expiry sp_mscheck_subscription_partition sp_mscheck_tran_retention sp_mscheckexistsgeneration sp_mscheckexistsrecguid " +
                                      "sp_mscheckidentityrange sp_mschecksharedagentforpublication sp_mschecksnapshotstatus sp_mscleanup_agent_entry sp_mscleanup_conflict " +
                                      "sp_mscleanup_publication_adinfo sp_mscleanup_subscription_distside_entry sp_mscleanupdynamicsnapshotfolder sp_mscleanupdynsnapshotvws " +
                                      "sp_mscleanupforpullreinit sp_mscleanupmergepublisher sp_mscleanupmergepublisher_internal sp_msclear_dynamic_snapshot_location " +
                                      "sp_msclearresetpartialsnapshotprogressbit sp_mscomputelastsentgen sp_mscomputemergearticlescreationorder sp_mscomputemergeunresolvedrefs " +
                                      "sp_msconflicttableexists sp_mscreate_all_article_repl_views sp_mscreate_article_repl_views sp_mscreate_dist_tables sp_mscreate_logical_record_views " +
                                      "sp_mscreate_sub_tables sp_mscreatedisabledmltrigger sp_mscreatedummygeneration sp_mscreateglobalreplica sp_mscreatelightweightinsertproc " +
                                      "sp_mscreatelightweightmultipurposeproc sp_mscreatelightweightprocstriggersconstraints sp_mscreatelightweightupdateproc sp_mscreatemergedynamicsnapshot " +
                                      "sp_mscreateretry sp_msdbuseraccess sp_msdbuserpriv sp_msdefer_check sp_msdelete_tracer_history sp_msdeletefoldercontents sp_msdeletemetadataactionrequest " +
                                      "sp_msdeleteretry sp_msdeletetranconflictrow sp_msdelgenzero sp_msdelrow sp_msdelrowsbatch sp_msdelrowsbatch_downloadonly sp_msdelsubrows " +
                                      "sp_msdelsubrowsbatch sp_msdependencies sp_msdetect_nonlogged_shutdown sp_msdetectinvalidpeerconfiguration sp_msdetectinvalidpeersubscription " +
                                      "sp_msdist_activate_auto_sub sp_msdist_adjust_identity sp_msdistpublisher_cleanup sp_msdistribution_counters sp_msdistributoravailable " +
                                      "sp_msdodatabasesnapshotinitiation sp_msdopartialdatabasesnapshotinitiation sp_msdrop_6x_publication sp_msdrop_6x_replication_agent " +
                                      "sp_msdrop_anonymous_entry sp_msdrop_article sp_msdrop_distribution_agent sp_msdrop_distribution_agentid_dbowner_proxy sp_msdrop_dynamic_snapshot_agent " +
                                      "sp_msdrop_logreader_agent sp_msdrop_merge_agent sp_msdrop_merge_subscription sp_msdrop_publication sp_msdrop_qreader_history sp_msdrop_snapshot_agent " +
                                      "sp_msdrop_snapshot_dirs sp_msdrop_subscriber_info sp_msdrop_subscription sp_msdrop_subscription_3rd sp_msdroparticleconstraints sp_msdroparticletombstones" +
                                      " sp_msdropconstraints sp_msdropdynsnapshotvws sp_msdropfkreferencingarticle sp_msdropmergearticle sp_msdropmergedynamicsnapshotjob sp_msdropretry " +
                                      "sp_msdroptemptable sp_msdummyupdate sp_msdummyupdate_logicalrecord sp_msdummyupdate90 sp_msdummyupdatelightweight sp_msdynamicsnapshotjobexistsatdistributor " +
                                      "sp_msenable_publication_for_het_sub sp_msensure_single_instance sp_msenum_distribution sp_msenum_distribution_s sp_msenum_distribution_sd " +
                                      "sp_msenum_logicalrecord_changes sp_msenum_logreader sp_msenum_logreader_s sp_msenum_logreader_sd sp_msenum_merge sp_msenum_merge_agent_properties " +
                                      "sp_msenum_merge_s sp_msenum_merge_sd sp_msenum_merge_subscriptions sp_msenum_merge_subscriptions_90_publication sp_msenum_merge_subscriptions_90_publisher " +
                                      "sp_msenum_metadataaction_requests sp_msenum_qreader sp_msenum_qreader_s sp_msenum_qreader_sd sp_msenum_replication_agents sp_msenum_replication_job " +
                                      "sp_msenum_replqueues sp_msenum_replsqlqueues sp_msenum_snapshot sp_msenum_snapshot_s sp_msenum_snapshot_sd sp_msenum_subscriptions sp_msenumallpublications " +
                                      "sp_msenumallsubscriptions sp_msenumarticleslightweight sp_msenumchanges sp_msenumchanges_belongtopartition sp_msenumchanges_notbelongtopartition " +
                                      "sp_msenumchangesdirect sp_msenumchangeslightweight sp_msenumcolumns sp_msenumcolumnslightweight sp_msenumdeletes_forpartition sp_msenumdeleteslightweight " +
                                      "sp_msenumdeletesmetadata sp_msenumdistributionagentproperties sp_msenumerate_pal sp_msenumgenerations sp_msenumgenerations90 sp_msenumpartialchanges " +
                                      "sp_msenumpartialchangesdirect sp_msenumpartialdeletes sp_msenumpubreferences sp_msenumreplicas sp_msenumreplicas90 sp_msenumretries sp_msenumschemachange " +
                                      "sp_msenumsubscriptions sp_msenumthirdpartypublicationvendornames sp_msestimatemergesnapshotworkload sp_msestimatesnapshotworkload sp_msevalsubscriberinfo " +
                                      "sp_msevaluate_change_membership_for_all_articles_in_pubid sp_msevaluate_change_membership_for_pubid sp_msevaluate_change_membership_for_row " +
                                      "sp_msexecwithlsnoutput sp_msfast_delete_trans sp_msfetchadjustidentityrange sp_msfetchidentityrange sp_msfillupmissingcols sp_msfilterclause " +
                                      "sp_msfix_6x_tasks sp_msfixlineageversions sp_msfixsubcolumnbitmaps sp_msfixupbeforeimagetables sp_msflush_access_cache sp_msforce_drop_distribution_jobs " +
                                      "sp_msforcereenumeration sp_msforeach_worker sp_msforeachdb sp_msforeachtable sp_msgenerateexpandproc sp_msget_agent_names sp_msget_attach_state " +
                                      "sp_msget_ddl_after_regular_snapshot sp_msget_dynamic_snapshot_location sp_msget_identity_range_info sp_msget_jobstate sp_msget_last_transaction " +
                                      "sp_msget_load_hint sp_msget_log_shipping_new_sessionid sp_msget_logicalrecord_lineage sp_msget_max_used_identity sp_msget_msmerge_rowtrack_colinfo " +
                                      "sp_msget_new_xact_seqno sp_msget_oledbinfo sp_msget_partitionid_eval_proc sp_msget_publication_from_taskname sp_msget_publisher_rpc " +
                                      "sp_msget_repl_cmds_anonymous sp_msget_repl_commands sp_msget_repl_error sp_msget_session_statistics sp_msget_shared_agent sp_msget_snapshot_history " +
                                      "sp_msget_subscriber_partition_id sp_msget_subscription_dts_info sp_msget_subscription_guid sp_msget_synctran_commands sp_msget_type_wrapper " +
                                      "sp_msgetagentoffloadinfo sp_msgetalertinfo sp_msgetalternaterecgens sp_msgetarticlereinitvalue sp_msgetchangecount sp_msgetconflictinsertproc " +
                                      "sp_msgetconflicttablename sp_msgetcurrentprincipal sp_msgetdatametadatabatch sp_msgetdbversion sp_msgetdynamicsnapshotapplock sp_msgetdynsnapvalidationtoken " +
                                      "sp_msgetisvalidwindowsloginfromdistributor sp_msgetlastrecgen sp_msgetlastsentgen sp_msgetlastsentrecgens sp_msgetlastupdatedtime sp_msgetlightweightmetadatabatch " +
                                      "sp_msgetmakegenerationapplock sp_msgetmakegenerationapplock_90 sp_msgetmaxbcpgen sp_msgetmaxsnapshottimestamp sp_msgetmergeadminapplock " +
                                      "sp_msgetmetadata_changedlogicalrecordmembers sp_msgetmetadatabatch sp_msgetmetadatabatch90 sp_msgetmetadatabatch90new sp_msgetonerow " +
                                      "sp_msgetonerowlightweight sp_msgetpeerlsns sp_msgetpeertopeercommands sp_msgetpubinfo sp_msgetreplicainfo sp_msgetreplicastate " +
                                      "sp_msgetrowmetadata sp_msgetrowmetadatalightweight sp_msgetserverproperties sp_msgetsetupbelong_cost sp_msgetsubscriberinfo " +
                                      "sp_msgetsupportabilitysettings sp_msgettrancftsrcrow sp_msgettranconflictrow sp_msgrantconnectreplication sp_mshaschangeslightweight " +
                                      "sp_mshasdbaccess sp_mshelp_article sp_mshelp_distdb sp_mshelp_distribution_agentid sp_mshelp_identity_property sp_mshelp_logreader_agentid " +
                                      "sp_mshelp_merge_agentid sp_mshelp_profile sp_mshelp_profilecache sp_mshelp_publication sp_mshelp_repl_agent sp_mshelp_replication_status " +
                                      "sp_mshelp_replication_table sp_mshelp_snapshot_agent sp_mshelp_snapshot_agentid sp_mshelp_subscriber_info sp_mshelp_subscription sp_mshelp_subscription_status " +
                                      "sp_mshelpcolumns sp_mshelpconflictpublications sp_mshelpcreatebeforetable sp_mshelpdestowner sp_mshelpdynamicsnapshotjobatdistributor sp_mshelpfulltextindex " +
                                      "sp_mshelpfulltextscript sp_mshelpindex sp_mshelplogreader_agent sp_mshelpmergearticles sp_mshelpmergeconflictcounts sp_mshelpmergedynamicsnapshotjob " +
                                      "sp_mshelpmergeidentity sp_mshelpmergeschemaarticles sp_mshelpobjectpublications sp_mshelpreplicationtriggers sp_mshelpsnapshot_agent sp_mshelpsummarypublication " +
                                      "sp_mshelptracertokenhistory sp_mshelptracertokens sp_mshelptranconflictcounts sp_mshelptype sp_mshelpvalidationdate sp_msifexistssubscription sp_msindexspace " +
                                      "sp_msinit_publication_access sp_msinit_subscription_agent sp_msinitdynamicsubscriber sp_msinsert_identity sp_msinsertdeleteconflict sp_msinserterrorlineage " +
                                      "sp_msinsertgenerationschemachanges sp_msinsertgenhistory sp_msinsertlightweightschemachange sp_msinsertschemachange sp_msinvalidate_snapshot " +
                                      "sp_msisnonpkukupdateinconflict sp_msispeertopeeragent sp_msispkupdateinconflict sp_msispublicationqueued sp_msisreplmergeagent sp_msissnapshotitemapplied " +
                                      "sp_mskilldb sp_mslock_auto_sub sp_mslock_distribution_agent sp_mslocktable sp_msloginmappings sp_msmakearticleprocs sp_msmakebatchinsertproc " +
                                      "sp_msmakebatchupdateproc sp_msmakeconflictinsertproc sp_msmakectsview sp_msmakedeleteproc sp_msmakedynsnapshotvws sp_msmakeexpandproc " +
                                      "sp_msmakegeneration sp_msmakeinsertproc sp_msmakemetadataselectproc sp_msmakeselectproc sp_msmakesystableviews sp_msmakeupdateproc " +
                                      "sp_msmap_partitionid_to_generations sp_msmarkreinit sp_msmatchkey sp_msmerge_alterschemaonly sp_msmerge_altertrigger " +
                                      "sp_msmerge_alterview sp_msmerge_ddldispatcher sp_msmerge_getgencur_public sp_msmerge_is_snapshot_required sp_msmerge_log_identity_range_allocations " +
                                      "sp_msmerge_upgrade_subscriber sp_msmergesubscribedb sp_msmergeupdatelastsyncinfo sp_msneedmergemetadataretentioncleanup sp_msnonsqlddl sp_msnonsqlddlforschemaddl " +
                                      "sp_msobjectprivs sp_mspeerapplyresponse sp_mspeerdbinfo sp_mspeersendresponse sp_mspeertopeerfwdingexec sp_mspost_auto_proc sp_mspostapplyscript_forsubscriberprocs " +
                                      "sp_msprep_exclusive sp_msprepare_mergearticle sp_msprofile_in_use sp_msproxiedmetadata sp_msproxiedmetadatabatch sp_msproxiedmetadatalightweight " +
                                      "sp_mspub_adjust_identity sp_mspublication_access sp_mspublicationcleanup sp_mspublicationview sp_msquery_syncstates sp_msquerysubtype " +
                                      "sp_msrecordsnapshotdeliveryprogress sp_msreenable_check sp_msrefresh_anonymous sp_msrefresh_publisher_idrange sp_msregenerate_mergetriggersprocs " +
                                      "sp_msregisterdynsnapseqno sp_msregistermergesnappubid sp_msregistersubscription sp_msreinit_failed_subscriptions sp_msreinit_hub sp_msreinit_subscription " +
                                      "sp_msreinitoverlappingmergepublications sp_msreleasedynamicsnapshotapplock sp_msreleasemakegenerationapplock sp_msreleasemergeadminapplock sp_msreleaseslotlock " +
                                      "sp_msreleasesnapshotdeliverysessionlock sp_msremove_mergereplcommand sp_msremoveoffloadparameter sp_msrepl_agentstatussummary sp_msrepl_backup_complete " +
                                      "sp_msrepl_backup_start sp_msrepl_check_publisher sp_msrepl_createdatatypemappings sp_msrepl_distributionagentstatussummary sp_msrepl_dropdatatypemappings " +
                                      "sp_msrepl_enumarticlecolumninfo sp_msrepl_enumpublications sp_msrepl_enumpublishertables sp_msrepl_enumsubscriptions sp_msrepl_enumtablecolumninfo " +
                                      "sp_msrepl_fixpalrole sp_msrepl_getdistributorinfo sp_msrepl_getpkfkrelation sp_msrepl_gettype_mappings sp_msrepl_helparticlermo sp_msrepl_init_backup_lsns " +
                                      "sp_msrepl_isdbowner sp_msrepl_islastpubinsharedsubscription sp_msrepl_isuserinanypal sp_msrepl_linkedservers_rowset sp_msrepl_mergeagentstatussummary " +
                                      "sp_msrepl_pal_rolecheck sp_msrepl_raiserror sp_msrepl_schema sp_msrepl_setnfr sp_msrepl_snapshot_helparticlecolumns sp_msrepl_snapshot_helppublication " +
                                      "sp_msrepl_startup sp_msrepl_startup_internal sp_msrepl_subscription_rowset sp_msrepl_testadminconnection sp_msrepl_testconnection sp_msreplagentjobexists " +
                                      "sp_msreplcheck_permission sp_msreplcheck_pull sp_msreplcheck_subscribe sp_msreplcheck_subscribe_withddladmin sp_msreplcheckoffloadserver sp_msreplcopyscriptfile " +
                                      "sp_msreplraiserror sp_msreplremoveuncdir sp_msreplupdateschema sp_msrequestreenumeration sp_msrequestreenumeration_lightweight sp_msreset_attach_state " +
                                      "sp_msreset_queued_reinit sp_msreset_subscription sp_msreset_subscription_seqno sp_msreset_synctran_bit sp_msreset_transaction sp_msresetsnapshotdeliveryprogress " +
                                      "sp_msrestoresavedforeignkeys sp_msretrieve_publication_attributes sp_msscript_article_view sp_msscript_dri sp_msscript_pub_upd_trig sp_msscript_sync_del_proc " +
                                      "sp_msscript_sync_del_trig sp_msscript_sync_ins_proc sp_msscript_sync_ins_trig sp_msscript_sync_upd_proc sp_msscript_sync_upd_trig sp_msscriptcustomdelproc " +
                                      "sp_msscriptcustominsproc sp_msscriptcustomupdproc sp_msscriptdatabase sp_msscriptdb_worker sp_msscriptforeignkeyrestore sp_msscriptsubscriberprocs " +
                                      "sp_msscriptviewproc sp_mssendtosqlqueue sp_msset_dynamic_filter_options sp_msset_logicalrecord_metadata sp_msset_new_identity_range sp_msset_oledb_prop " +
                                      "sp_msset_snapshot_xact_seqno sp_msset_sub_guid sp_msset_subscription_properties sp_mssetaccesslist sp_mssetalertinfo sp_mssetartprocs sp_mssetbit " +
                                      "sp_mssetconflictscript sp_mssetconflicttable sp_mssetcontext_bypasswholeddleventbit sp_mssetcontext_replagent sp_mssetgentozero sp_mssetlastrecgen " +
                                      "sp_mssetlastsentgen sp_mssetreplicainfo sp_mssetreplicaschemaversion sp_mssetreplicastatus sp_mssetrowmetadata sp_mssetserverproperties sp_mssetsubscriberinfo " +
                                      "sp_mssettopology sp_mssetup_identity_range sp_mssetup_partition_groups sp_mssetup_use_partition_groups sp_mssetupbelongs sp_mssetupnosyncsubwithlsnatdist " +
                                      "sp_mssharedfixeddisk sp_mssqldmo70_version sp_mssqldmo80_version sp_mssqldmo90_version sp_mssqlole_version sp_mssqlole65_version sp_msstartdistribution_agent " +
                                      "sp_msstartmerge_agent sp_msstartsnapshot_agent sp_msstopdistribution_agent sp_msstopmerge_agent sp_msstopsnapshot_agent sp_mssub_check_identity " +
                                      "sp_mssub_set_identity sp_mssubscription_status sp_mssubscriptionvalidated sp_mstablechecks sp_mstablekeys sp_mstablerefs sp_mstablespace sp_mstestbit " +
                                      "sp_mstran_ddlrepl sp_mstran_is_snapshot_required sp_mstrypurgingoldsnapshotdeliveryprogress sp_msuniquename sp_msunmarkifneeded sp_msunmarkreplinfo " +
                                      "sp_msunmarkschemaobject sp_msunregistersubscription sp_msupdate_agenttype_default sp_msupdate_singlelogicalrecordmetadata sp_msupdate_subscriber_info " +
                                      "sp_msupdate_subscriber_schedule sp_msupdate_subscriber_tracer_history sp_msupdate_subscription sp_msupdate_tracer_history sp_msupdatecachedpeerlsn " +
                                      "sp_msupdategenhistory sp_msupdateinitiallightweightsubscription sp_msupdatelastsyncinfo sp_msupdatepeerlsn sp_msupdaterecgen sp_msupdatereplicastate " +
                                      "sp_msupdatesysmergearticles sp_msuplineageversion sp_msuploadsupportabilitydata sp_msuselightweightreplication sp_msvalidate_dest_recgen " +
                                      "sp_msvalidate_subscription sp_msvalidate_wellpartitioned_articles sp_msvalidatearticle sp_mswritemergeperfcounter sp_objectfilegroup " +
                                      "sp_oledb_database sp_oledb_defdb sp_oledb_deflang sp_oledb_language sp_oledb_ro_usrname sp_oledbinfo sp_orbitmap sp_password sp_pkeys " +
                                      "sp_posttracertoken sp_primary_keys_rowset sp_primary_keys_rowset_rmt sp_primary_keys_rowset2 sp_primarykeys sp_procedure_params_90_rowset " +
                                      "sp_procedure_params_90_rowset2 sp_procedure_params_managed sp_procedure_params_rowset sp_procedure_params_rowset2 sp_procedures_rowset " +
                                      "sp_procedures_rowset2 sp_processlogshippingmonitorhistory sp_processlogshippingmonitorprimary sp_processlogshippingmonitorsecondary " +
                                      "sp_processlogshippingretentioncleanup sp_processmail sp_procoption sp_prop_oledb_provider sp_provider_types_90_rowset sp_provider_types_rowset " +
                                      "sp_publication_validation sp_publicationsummary sp_publishdb sp_publisherproperty sp_readerrorlog sp_readwebtask sp_recompile " +
                                      "sp_refresh_heterogeneous_publisher sp_refresh_log_shipping_monitor sp_refreshsqlmodule sp_refreshsubscriptions sp_register_custom_scripting " +
                                      "sp_registercustomresolver sp_reinitmergepullsubscription sp_reinitmergesubscription sp_reinitpullsubscription sp_reinitsubscription " +
                                      "sp_releaseapplock sp_remoteoption sp_removedbreplication sp_removedistpublisherdbreplication sp_removesrvreplication sp_rename sp_renamedb " +
                                      "sp_repladdcolumn sp_replcleanupccsprocs sp_repldeletequeuedtran sp_repldropcolumn sp_replgetparsedddlcmd sp_replica sp_replication_agent_checkup " +
                                      "sp_replicationdboption sp_replincrementlsn sp_replmonitorchangepublicationthreshold sp_replmonitorhelpmergesession sp_replmonitorhelpmergesessiondetail " +
                                      "sp_replmonitorhelpmergesubscriptionmoreinfo sp_replmonitorhelppublication sp_replmonitorhelppublicationthresholds sp_replmonitorhelppublisher " +
                                      "sp_replmonitorhelpsubscription sp_replmonitorrefreshjob sp_replmonitorsubscriptionpendingcmds sp_replpostsyncstatus sp_replqueuemonitor " +
                                      "sp_replrestart sp_replsetoriginator sp_replshowcmds sp_replsqlqgetrows sp_replsync sp_requestpeerresponse sp_resetsnapshotdeliveryprogress " +
                                      "sp_resetstatus sp_resign_database sp_resolve_logins sp_restoredbreplication sp_restoremergeidentityrange sp_resyncmergesubscription " +
                                      "sp_revoke_publication_access sp_revokedbaccess sp_revokelogin sp_runwebtask sp_schemafilter sp_schemata_rowset sp_script_reconciliation_delproc " +
                                      "sp_script_reconciliation_insproc sp_script_reconciliation_sinsproc sp_script_reconciliation_vdelproc sp_script_reconciliation_xdelproc " +
                                      "sp_script_synctran_commands sp_scriptdelproc sp_scriptdynamicupdproc sp_scriptinsproc sp_scriptmappedupdproc sp_scriptpublicationcustomprocs " +
                                      "sp_scriptsinsproc sp_scriptsubconflicttable sp_scriptsupdproc sp_scriptupdproc sp_scriptvdelproc sp_scriptvupdproc sp_scriptxdelproc " +
                                      "sp_scriptxupdproc sp_server_info sp_serveroption sp_setapprole sp_setautosapasswordanddisable sp_setdefaultdatatypemapping sp_setnetname " +
                                      "sp_setoraclepackageversion sp_setreplfailovermode sp_setsubscriptionxactseqno sp_settriggerorder sp_showcolv sp_showlineage sp_showpendingchanges " +
                                      "sp_showrowreplicainfo sp_spaceused sp_special_columns sp_special_columns_90 sp_sproc_columns sp_sproc_columns_90 sp_sqlexec sp_srvrolepermission " +
                                      "sp_startmergepullsubscription_agent sp_startmergepushsubscription_agent sp_startpublication_snapshot sp_startpullsubscription_agent " +
                                      "sp_startpushsubscription_agent sp_statistics sp_statistics_rowset sp_statistics_rowset2 sp_stopmergepullsubscription_agent sp_stopmergepushsubscription_agent " +
                                      "sp_stoppublication_snapshot sp_stoppullsubscription_agent sp_stoppushsubscription_agent sp_stored_procedures sp_subscribe sp_subscription_cleanup " +
                                      "sp_subscriptionsummary sp_table_constraints_rowset sp_table_constraints_rowset2 sp_table_privileges sp_table_privileges_ex sp_table_privileges_rowset " +
                                      "sp_table_privileges_rowset_rmt sp_table_privileges_rowset2 sp_table_statistics_rowset sp_table_statistics2_rowset sp_table_validation sp_tablecollations " +
                                      "sp_tablecollations_90 sp_tableoption sp_tables sp_tables_ex sp_tables_info_90_rowset sp_tables_info_90_rowset_64 sp_tables_info_90_rowset2 " +
                                      "sp_tables_info_90_rowset2_64 sp_tables_info_rowset sp_tables_info_rowset_64 sp_tables_info_rowset2 sp_tables_info_rowset2_64 sp_tables_rowset " +
                                      "sp_tables_rowset_rmt sp_tables_rowset2 sp_tableswc sp_trace_getdata sp_unbindefault sp_unbindrule sp_unregister_custom_scripting sp_unregistercustomresolver " +
                                      "sp_unsetapprole sp_unsubscribe sp_update_agent_profile sp_updateextendedproperty sp_updatestats sp_upgrade_log_shipping sp_user_counter1 sp_user_counter10 " +
                                      "sp_user_counter2 sp_user_counter3 sp_user_counter4 sp_user_counter5 sp_user_counter6 sp_user_counter7 sp_user_counter8 sp_user_counter9 sp_usertypes_rowset " +
                                      "sp_usertypes_rowset_rmt sp_usertypes_rowset2 sp_validatecache sp_validatelogins sp_validatemergepublication sp_validatemergepullsubscription " +
                                      "sp_validatemergesubscription sp_validlang sp_validname sp_verifypublisher sp_views_rowset sp_views_rowset2 sp_vupgrade_mergeobjects sp_vupgrade_mergetables " +
                                      "sp_vupgrade_replication sp_vupgrade_replsecurity_metadata sp_who sp_who2 sp_xml_schema_rowset sp_xml_schema_rowset2 xp_grantlogin xp_logininfo " +
                                      "xp_repl_convert_encrypt_sysadmin_wrapper xp_revokelogin ");
            #endregion
            // user4 = 7
            scintilla.SetKeywords(7, @"between and or union like ");

            scintilla.Text = "";
        }
        /// <summary>
        /// 获取脚本字体
        /// </summary>
        /// <returns></returns>
        private string PreferredFont()
        {
            return "宋体";
            //"Consolas", "Courier New", "Courier",
            //var fontNames = new string[] { "宋体" };
            //var fontsCollection = new System.Drawing.Text.InstalledFontCollection();
            //var installedNames = fontsCollection.Families.Select(ff => ff.Name);

            //var preferredFont = fontNames.FirstOrDefault(fn => installedNames.Contains(fn));
            //if (string.IsNullOrEmpty(preferredFont))
            //    preferredFont = "宋体";
            //return preferredFont;
        }

        private void InitCobobox()
        {
            var sortItems = new object[]
            {
                new { Text = "名称", Value = "name"},
                new { Text = "创建时间",Value = "create_date"},
                new { Text = "最后修改时间", Value = "modify_date"}
            };
            this.cboSort.Items.Clear();
            this.cboSort.Items.AddRange(sortItems);
            this.cboSort.DisplayMember = "Text";
            this.cboSort.ValueMember = "Value";
            this.cboSort.SelectedIndex = 2;

            var sortTypeItems = new object[]
            {
                new { Text = "升序", Value = "asc"},
                new { Text = "降序",Value = "desc"}
            };
            this.cboSortType.Items.Clear();
            this.cboSortType.Items.AddRange(sortTypeItems);
            this.cboSortType.DisplayMember = "Text";
            this.cboSortType.ValueMember = "Value";
            this.cboSortType.SelectedIndex = 1;
        }

        private void BindIgronesGrid()
        {
            BindingSource source = new BindingSource();
            source.DataSource = this.igrones;
            this.dgvIgrone.DataSource = source;
        }

        private void BindObjectGrid()
        {
            IQueryable<Models.DBProcedure> objs = null;
            if (this.objects != null)
            {
                var nc = this.igrones.Where(o => string.Equals(o.IgroneType, "KeyWord")).Select(o => o.Content).AsQueryable();
                objs = this.objects.AsQueryable();
                if (nc.Any())
                    objs = objs.Where(o => !nc.Any(c => o.name.Contains(c)));
                var names = this.igrones.Where(o => string.Equals(o.IgroneType, "Name")).Select(o => o.Content).AsQueryable();
                if (names.Any())
                    objs = objs.Where(o => !names.Contains(o.name));
                //var objs = (from o in this.objects
                //            where 
                //            && !names.Contains(o.name)
                //            select new Models.DBObject
                //            {
                //                id = o.id,
                //                name = o.name,
                //                create_date = o.create_date,
                //                modify_date = o.modify_date
                //            }).ToList();
            }
            BindingSource bs = new BindingSource();
            if (objs != null)
            {
                bs.DataSource = objs.ToList();
            }
            this.grid.DataSource = bs;
        }

        #endregion

        private void grid_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            this.grid.ContextMenuStrip = this.dgvIgrone.ContextMenuStrip = null;
            if (e.RowIndex >= 0)
            {
                if (sender == this.grid
                    && this.grid.SelectedCells[0].RowIndex == e.RowIndex
                    && this.grid.SelectedCells[0].ColumnIndex == e.ColumnIndex)
                {
                    this.grid.ContextMenuStrip = this.cmsGrid;
                }
                else if (sender == this.dgvIgrone
                    && this.dgvIgrone.SelectedCells[0].RowIndex == e.RowIndex
                    && this.dgvIgrone.SelectedCells[0].ColumnIndex == e.ColumnIndex)
                {
                    this.dgvIgrone.ContextMenuStrip = this.cmsGridIgrone;
                }
            }
        }

        private void tsmiAddIgrone_Click(object sender, EventArgs e)
        {
            var igrone = this.grid.SelectedCells.Count == 0 ? string.Empty : this.grid.SelectedCells[0].Value as string;
            if (string.IsNullOrEmpty(igrone)) return;
            if (this.igrones.Any(o => string.Equals(o.Content, igrone))) return;
            this.igrones.Add(new Models.Igrone(igrone, "Name"));
            BindIgronesGrid();
            BindObjectGrid();
        }

        private void tsmiRemoveIgrone_Click(object sender, EventArgs e)
        {
            var igrone = this.dgvIgrone.SelectedCells.Count == 0 ? string.Empty : this.dgvIgrone.SelectedCells[0].Value as string;
            if (string.IsNullOrEmpty(igrone)) return;
            var item = this.igrones.FirstOrDefault(o => string.Equals(o.Content, igrone));
            this.igrones.Remove(item);

            BindIgronesGrid();
            BindObjectGrid();
        }

        private void dgvIgrone_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var igrone = this.dgvIgrone.Rows[e.RowIndex].DataBoundItem as Models.Igrone;
            if (igrone == null) igrone = new Models.Igrone();
            igrone.Content = this.dgvIgrone.Rows[e.RowIndex].Cells[1].Value as string;
            igrone.IgroneType = this.dgvIgrone.Rows[e.RowIndex].Cells[2].Value as string;
            if (!this.igrones.Any(o => string.Equals(o.ID, igrone.ID)))
            {
                this.igrones.Add(igrone);
                BindIgronesGrid();
            }
            if (string.IsNullOrEmpty(igrone.Content) || string.IsNullOrEmpty(igrone.IgroneType)) return;

            BindObjectGrid();
        }
    }
}
