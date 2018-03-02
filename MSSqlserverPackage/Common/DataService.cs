using MSSqlserverPackage.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MSSqlserverPackage.Common
{
    public class DataService
    {
        public static IEnumerable<Schema> GetSchemas()
        {
            IEnumerable<Schema> schemas = CacheManager.GetValue("Schemas") as IEnumerable<Schema>;
            if (schemas == null || !schemas.Any())
            {
                schemas = Common.SqlHelper.GetSchemas();
                CacheManager.SetValue("Schemas", schemas);
            }
            return schemas;
        }
        public static IEnumerable<Identity> GetIdentities()
        {
            var identities = CacheManager.GetValue("Identities") as IEnumerable<Identity>;
            if (identities == null || !identities.Any())
            {
                identities = SqlHelper.GetIdentities();
                if (identities != null)
                {
                    CacheManager.SetValue("Identities", identities);
                }
            }
            return identities;
        }
        public static IEnumerable<ExtendedProperty> GetExtendedProperties()
        {
            var extendedProperties = CacheManager.GetValue("ExtendedProperties") as IEnumerable<ExtendedProperty>;
            if (extendedProperties == null || !extendedProperties.Any())
            {
                extendedProperties = SqlHelper.GetExtendedProperties();
                if (extendedProperties != null)
                {
                    CacheManager.SetValue("ExtendedProperties", extendedProperties);
                }
            }
            return extendedProperties;
        }
        public static IEnumerable<Column> GetColumns()
        {
            var identities = GetIdentities();
            var extendedProperties = GetExtendedProperties();
            var columns = CacheManager.GetValue("Columns") as IEnumerable<Column>;
            if (columns == null || !columns.Any())
            {
                columns = SqlHelper.GetColumns();
                columns = (from c in columns
                           join i in identities
                           on new { c.ObjectID, ColumnID = c.ID } equals new { i.ObjectID, ColumnID = i.ColumnID }
                           into tmpI
                           join ext in extendedProperties
                           on new { c.ObjectID, ColumnID = c.ID } equals new { ObjectID = ext.MajorID, ColumnID = ext.MinorID }
                           into tmpE
                           select new Column
                           {
                               ID = c.ID,
                               Name = c.Name,
                               ObjectID = c.ObjectID,
                               Nullable = c.Nullable,
                               MaxLength = c.MaxLength,
                               DataType = c.DataType,
                               Identity = tmpI.FirstOrDefault(),
                               ExtendedProperties = tmpE.AsEnumerable()
                           }).AsEnumerable();
                if (columns != null)
                {
                    CacheManager.SetValue("Columns", columns);
                }
            }
            return columns;
        }

        public static IEnumerable<Parameter> GetParameters()
        {
            var parameters = CacheManager.GetValue("Parameters") as List<Parameter>;
            if (parameters == null)
            {
                parameters = Common.SqlHelper.GetParameters();
                if (parameters != null)
                {
                    CacheManager.SetValue("Parameters", parameters);
                }
            }
            return parameters;
        }

        public static IEnumerable<DBObject> GetObjects()
        {
            var objects = CacheManager.GetValue("Objects") as List<DBObject>;
            if (objects == null || !objects.Any())
            {
                var schemas = GetSchemas();
                objects = SqlHelper.GetObjects();
                if (objects != null)
                {
                    objects.ForEach(o =>
                    {
                        o.Schema = schemas.FirstOrDefault(s => s.ID == o.SchemaID);
                        var objType = string.IsNullOrEmpty(o.TypeName) ? string.Empty : o.TypeName.Trim();
                        switch (o.TypeName)
                        {
                            case ("U"):
                                o.ObjectType = ConstValue.ObjectType.Table;
                                break;
                            case ("V"):
                                o.ObjectType = ConstValue.ObjectType.View;
                                break;
                            case ("P"):
                                o.ObjectType = ConstValue.ObjectType.Procedure;
                                break;
                            default:
                                o.ObjectType = ConstValue.ObjectType.Unknown;
                                break;

                        }
                    });
                    CacheManager.SetValue("Objects", objects);
                }
            }
            return objects;
        }
        public static IEnumerable<Table> GetTables()
        {
            var tables = CacheManager.GetValue("Tables") as List<Table>;
            if (tables == null || !tables.Any())
            {
                var objects = GetObjects();
                if (objects != null)
                {
                    var indexes = GetIndexes();
                    var pks = GetPrimaryKeys() ?? new List<PrimaryKey>();
                    var columns = GetColumns() ?? new List<Column>();
                    tables = (from o in objects
                              where o.ObjectType == ConstValue.ObjectType.Table
                              select new Table
                              {
                                  ObjectID = o.ObjectID,
                                  Name = o.Name,
                                  Schema = o.Schema,
                                  ObjectType = o.ObjectType,
                                  Columns = columns.Where(c => c.ObjectID == o.ObjectID).AsEnumerable(),
                                  PrimaryKey = pks.FirstOrDefault(p => p.ObjectID == o.ObjectID),
                                  TypeName = o.TypeName,
                                  Indexes = indexes.Where(i => i.ObjectID == o.ObjectID).AsEnumerable()
                              }).ToList();

                    var fks = GetReferenceKeys(tables);
                    tables.ForEach(t =>
                    {
                        t.ReferenceKeys = fks.Where(o => o.ObjectID == t.ObjectID).AsEnumerable();
                    });
                    CacheManager.SetValue("Tables", tables);

                }
            }
            return tables;
        }
        public static IEnumerable<View> GetViews()
        {
            var views = CacheManager.GetValue("Views") as List<View>;
            if (views == null || !views.Any())
            {
                var columns = GetColumns() ?? new List<Column>();
                var objects = GetObjects();
                var indexes = GetIndexes();
                views = (from o in objects
                         where !string.IsNullOrEmpty(o.TypeName) && o.TypeName.Trim().Equals("V", System.StringComparison.InvariantCultureIgnoreCase)
                         select new View
                         {
                             ObjectID = o.ObjectID,
                             Name = o.Name,
                             Schema = o.Schema,
                             ObjectType = o.ObjectType,
                             Columns = columns.Where(c => c.ObjectID == o.ObjectID).AsEnumerable(),
                             Indexes = indexes == null ? null : indexes.Where(i => i.ObjectID == o.ObjectID).AsEnumerable()
                         }).ToList();
                CacheManager.SetValue("Views", views);
            }
            return views;
        }

        public static IEnumerable<Procedure> GetProcedures()
        {
            var procedures = CacheManager.GetValue("Procedures") as List<Procedure>;
            if (procedures == null || !procedures.Any())
            {
                var objects = GetObjects();
                var parameters = GetParameters();
                if (parameters == null) parameters = new List<Parameter>();
                if (objects != null)
                {
                    procedures = (from o in objects
                                  where o.ObjectType == ConstValue.ObjectType.Procedure
                                  select new Procedure
                                  {
                                      ObjectID = o.ObjectID,
                                      TypeName = o.TypeName,
                                      Name = o.Name,
                                      ObjectType = ConstValue.ObjectType.Procedure,
                                      Parameters = parameters.Where(p => p.ObjectID == o.ObjectID)
                                  }).ToList();
                    CacheManager.SetValue("Procedures", procedures);
                }
            }
            return procedures;
        }

        public static IEnumerable<PrimaryKey> GetPrimaryKeys()
        {
            var keys = CacheManager.GetValue("PrimaryKeys") as IEnumerable<PrimaryKey>;
            if (keys == null || !keys.Any())
            {
                keys = SqlHelper.GetPrimaryKeys();
                if (keys != null)
                {
                    CacheManager.SetValue("PrimaryKeys", keys);
                }
            }
            return keys;
        }
        public static IEnumerable<ReferenceKey> GetReferenceKeys(IEnumerable<Table> tables)
        {
            if (tables == null) throw new ArgumentNullException("tables不能为空");
            var keys = CacheManager.GetValue("ReferenceKeys") as List<ReferenceKey>;
            if (keys == null || !keys.Any())
            {
                keys = SqlHelper.GetReferenceKeys();
                if (keys != null)
                {
                    keys.ForEach(k =>
                    {
                        var refObject = tables.FirstOrDefault(o => o.ObjectID == k.ReferenceObjectID);
                        if (refObject != null)
                        {
                            k.ReferenceObject = refObject;
                            k.ReferenceColumn = refObject.Columns == null ? null : refObject.Columns.FirstOrDefault(c => c.ID == k.ReferenceColumnID);
                        }
                    });
                    CacheManager.SetValue("ReferenceKeys", keys);
                }
            }
            return keys;
        }
        public static IEnumerable<Index> GetIndexes()
        {
            var index = CacheManager.GetValue("Indexes") as IEnumerable<Index>;
            if (index == null || !index.Any())
            {
                index = SqlHelper.GetIndexes();
                if (index != null)
                {
                    CacheManager.SetValue("Indexes", index);
                }
            }
            return index;
        }
    }
}
