using MSSqlserverPackage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MSSqlserverPackage
{
    public partial class ScriptForm : DataBaseObjectForm
    {

        private string[] needShowMaxLengthTypes = new string[]
        {
            "varchar",
            "char",
            "nvarchar",
            "nchar"
        };

        public ScriptForm()
        {
            InitializeComponent();
        }
        public string BuildScript()
        {
            if (this.dbObject == null) return null;
            if (this.dbObject.ObjectType == Common.ConstValue.ObjectType.Table)
            {
                return BuildTableScript(this.dbObject as Table);
            }
            var types = new Common.ConstValue.ObjectType[] 
            {
                Common.ConstValue.ObjectType.View,
                Common.ConstValue.ObjectType.Function,
                Common.ConstValue.ObjectType.Procedure
            };
            if (types.Contains(this.dbObject.ObjectType))
            {
                return GetScript();
            }
            return string.Empty;
        }

        public string BuildTableScript(Table table)
        {
            var stringBuildScript = new StringBuilder();
            var sbProperties = new StringBuilder();
            if (this.dbObject.Schema != null)
            {
                stringBuildScript.Append(string.Format("\r\nCREATE {0} [{1}].[{2}]", this.ObjectType.ToUpper(), this.dbObject.Schema.Name, this.dbObject.Name));
            }
            else
            {
                stringBuildScript.Append(string.Format("\r\nCREATE {0} [{1}]", this.ObjectType.ToUpper(), this.dbObject.Name));
            }
            stringBuildScript.Append("\r\n(");
            var columns = table.Columns.ToArray();
            for (var i = 0; i < columns.Length; i++)
            {
                if ((columns.Length - i) > 1)
                {
                    stringBuildScript.Append("\r\n\t" + BuildFiledScript(columns[i]) + ",");
                }
                else
                {
                    stringBuildScript.Append("\r\n\t" + BuildFiledScript(columns[i]));
                }
                BuildExtendPropertiesScript(columns[i], ref sbProperties);
            }
            if (table.PrimaryKey != null)
            {
                stringBuildScript.Append(BuildPrimaryKeyScript(table));
            }
            var uqIndex = table.Indexes == null ? null : table.Indexes.FirstOrDefault(o => o.IsUniqueConstraint);
            if (uqIndex != null)
            {
                stringBuildScript.Append(BuildUniqueConstraintScript(uqIndex, table));
            }

            stringBuildScript.Append("\r\n)");
            if (table.PrimaryKey != null)
            {
                stringBuildScript.Append(" ON [PRIMARY] \r\n GO");
            }
            stringBuildScript.AppendLine(sbProperties.ToString());
            stringBuildScript.AppendLine(BuildForeignkeyScript(table));
            return stringBuildScript.ToString();
        }

        private string BuildFiledScript(Column column)
        {
            var sb = new StringBuilder();
            sb.Append("[" + column.Name + "]");
            sb.Append(" [" + column.DataType + "]");
            if (needShowMaxLengthTypes.Contains(column.DataType.ToLower()))
            {
                sb.Append("(" + column.MaxLength + ")");
            }
            if (column.Identity != null && column.Identity.IsIdentity)
            {
                sb.Append(" IDENTITY(" + column.Identity.SeedValue + "," + column.Identity.IncrementValue + ")");
            }
            if (column.Nullable)
            {
                sb.Append(" NULL");
            }
            else
            {
                sb.Append(" NOT NULL");
            }
            return sb.ToString();
        }

        private void BuildExtendPropertiesScript(Column column, ref StringBuilder scriptBuilder)
        {
            var properties = column.ExtendedProperties == null ? new List<ExtendedProperty>() : column.ExtendedProperties.Where(o => o.Name.Equals("MS_Description")).ToList();
            foreach (var p in properties)
            {
                scriptBuilder.AppendFormat("\r\n\r\nEXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'{0}'," +
                                            "\r\n\t@level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'{1}'," +
                                            "\r\n\t@level1name = N'{2}', @level2type = N'COLUMN'," +
                                            "\r\n\t@level2name = N'{3}'" +
                                            "\r\nGO", p.Value, this.ObjectType, this.dbObject.Name, column.Name);
            }
        }
        private void ScriptForm_Load(object sender, EventArgs e)
        {
            this.txtScript.Text = BuildScript();
        }

        private string BuildPrimaryKeyScript(Table table)
        {
            if (table == null || table.PrimaryKey == null || table.Columns == null)
                return string.Empty;
            var primaryColumn = table.Columns.FirstOrDefault(c => c.ID == table.PrimaryKey.ColumnID);
            if (primaryColumn == null)
                return string.Empty;
            var stringBuild = new StringBuilder(",");
            stringBuild.Append(string.Format("\r\n\tCONSTRAINT [{0}] PRIMARY KEY CLUSTERED", table.PrimaryKey.Name));
            stringBuild.Append("\r\n\t(");
            stringBuild.Append(string.Format("\r\n\t\t[{0}] ASC", primaryColumn.Name));
            stringBuild.Append("\r\n\t) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            return stringBuild.ToString();
        }

        private string BuildUniqueConstraintScript(Index uniqueConstraint, Table table)
        {
            var stringBuild = new StringBuilder(",");
            var keyColumn = table.Columns == null ? null : table.Columns.FirstOrDefault(o => o.ID == uniqueConstraint.ColumnID);
            if (keyColumn == null)
            {
                return string.Empty;
            }
            stringBuild.Append(string.Format("\r\n\tCONSTRAINT [{0}] UNIQUE NONCLUSTERED ", uniqueConstraint.Name));
            stringBuild.Append("\r\n\t(");
            stringBuild.Append(string.Format("\r\n\t\t[{0}] ASC", keyColumn.Name));
            stringBuild.Append("\r\n\t) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]");
            return stringBuild.ToString();
        }

        private string BuildForeignkeyScript(Table table)
        {
            if (table == null || table.ReferenceKeys == null || !table.ReferenceKeys.Any())
            {
                return string.Empty;
            }
            var stringBuild = new StringBuilder();
            foreach (var key in table.ReferenceKeys)
            {
                var keyColumn = table.Columns.FirstOrDefault(o => o.ID == key.ColumnID);
                if (keyColumn == null)
                {
                    continue;
                }
                stringBuild.AppendFormat("\r\nALTER TABLE [{0}].[{1}]  WITH CHECK ADD  CONSTRAINT [{2}] FOREIGN KEY ([{3}])" +
                                        " REFERENCES [{4}].[{5}]([{6}])" +
                                        "\r\nGO", table.Schema.Name,
                                        table.Name,
                                        key.Name,
                                        keyColumn.Name,
                                        key.ReferenceObject.Schema.Name,
                                        key.ReferenceObject.Name,
                                        key.ReferenceColumn.Name);

                stringBuild.AppendFormat("\r\nALTER TABLE [{0}].[{1}] CHECK CONSTRAINT [{2}]\r\nGO", table.Schema.Name, table.Name, key.Name);
            }
            return stringBuild.ToString();
        }

        private string GetScript()
        {
            return Common.SqlHelper.ExecuteSqlHelpText(this.dbObject.Name);
        }
    }
}
