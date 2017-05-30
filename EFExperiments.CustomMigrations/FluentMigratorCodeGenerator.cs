using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations.Design;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Utilities;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.IO;
using System.Linq;

namespace EFExperiments.CustomMigrations
{
    public class FluentMigratorCodeGenerator : CSharpMigrationCodeGenerator
    {
        private string _migrationId;

        public override ScaffoldedMigration Generate(string migrationId, IEnumerable<MigrationOperation> operations, string sourceModel, string targetModel, string @namespace, string className)
        {
            _migrationId = migrationId; // TODO this is ugly global state...
            return base.Generate(migrationId, operations, sourceModel, targetModel, @namespace, className);
        }

        protected override string Generate(IEnumerable<MigrationOperation> operations, string @namespace, string className)
        {
            using (var stringWriter = new StringWriter(CultureInfo.InvariantCulture)) {
                using (var writer = new IndentedTextWriter(stringWriter)) {
                    WriteClassStart(@namespace, className, _migrationId, writer, "Migration", false, GetNamespaces(operations));
                    writer.WriteLine("public override void Up()");
                    writer.WriteLine("{");
                    writer.Indent++;
                    foreach (var operation in operations) {
                        Generate((dynamic) operation, writer);
                    }
                    writer.Indent--;
                    writer.WriteLine("}");
                    writer.WriteLine();
                    writer.WriteLine("public override void Down()");
                    writer.WriteLine("{");
                    writer.Indent++;
                    operations = operations
                        .Select(o => o.Inverse)
                        .Where(o => o != null)
                        .Reverse();
                    var hasUnsupportedOperations = operations.Any(o => o is NotSupportedOperation);
                    foreach (var operation in operations.Where(o => !(o is NotSupportedOperation))) {
                        Generate((dynamic) operation, writer);
                    }
                    if (hasUnsupportedOperations) {
                        writer.Write("throw new NotSupportedException(");
                        // TODO: Apparently, only Stored Procedures are not supported?
                        // writer.Write(Generate(Strings.ScaffoldSprocInDownNotSupported));
                        writer.WriteLine(");");
                    }
                    writer.Indent--;
                    writer.WriteLine("}");
                    WriteClassEnd(@namespace, writer);
                }
                return stringWriter.ToString();
            }
        }

        protected override string Generate(string migrationId, string sourceModel, string targetModel, string @namespace, string className)
        {
            return base.Generate(migrationId, sourceModel, targetModel, @namespace, className);
        }

        protected override void WriteProperty(string name, string value, IndentedTextWriter writer)
        {
            base.WriteProperty(name, value, writer);
        }

        protected virtual void WriteClassAttributes(IndentedTextWriter writer, string migrationId)
        {
            writer.WriteLine($"[Migration({DateTime.UtcNow:yyyyMMddHHmmss}, \"{migrationId}\")]");
        }

        protected virtual void WriteClassStart(string @namespace, string className, string migrationId, IndentedTextWriter writer, string @base, bool designer = false,
            IEnumerable<string> namespaces = null)
        {
            foreach (var n in namespaces ?? GetDefaultNamespaces(designer)) {
                writer.WriteLine("using " + n + ";");
            }
            if (!string.IsNullOrWhiteSpace(@namespace)) {
                writer.Write("namespace ");
                writer.WriteLine(@namespace);
                writer.WriteLine("{");
                writer.Indent++;
            }
            writer.WriteLine();
            WriteClassAttributes(writer, migrationId);
            writer.Write("public ");
            if (designer) {
                writer.Write("sealed ");
            }
            writer.Write("partial class ");
            writer.Write(className);
            writer.Write(" : ");
            writer.Write(@base);
            writer.WriteLine();
            writer.WriteLine("{");
            writer.Indent++;
        }

        protected override void Generate(AddColumnOperation addColumnOperation, IndentedTextWriter writer)
        {
            base.Generate(addColumnOperation, writer);
        }

        protected override void Generate(DropColumnOperation dropColumnOperation, IndentedTextWriter writer)
        {
            base.Generate(dropColumnOperation, writer);
        }

        protected override void Generate(AlterColumnOperation alterColumnOperation, IndentedTextWriter writer)
        {
            base.Generate(alterColumnOperation, writer);
        }

        protected override void GenerateAnnotations(IDictionary<string, object> annotations, IndentedTextWriter writer)
        {
            base.GenerateAnnotations(annotations, writer);
        }

        protected override void GenerateAnnotations(IDictionary<string, AnnotationValues> annotations, IndentedTextWriter writer) { }

        protected override void GenerateAnnotation(string name, object annotation, IndentedTextWriter writer)
        {
            base.GenerateAnnotation(name, annotation, writer);
        }

        protected override void Generate(CreateProcedureOperation createProcedureOperation, IndentedTextWriter writer)
        {
            base.Generate(createProcedureOperation, writer);
        }

        protected override void Generate(AlterProcedureOperation alterProcedureOperation, IndentedTextWriter writer)
        {
            base.Generate(alterProcedureOperation, writer);
        }

        protected override void Generate(ParameterModel parameterModel, IndentedTextWriter writer, bool emitName = false)
        {
            base.Generate(parameterModel, writer, emitName);
        }

        protected override void Generate(DropProcedureOperation dropProcedureOperation, IndentedTextWriter writer)
        {
            base.Generate(dropProcedureOperation, writer);
        }

        protected override void Generate(CreateTableOperation createTableOperation, IndentedTextWriter writer)
        {
            writer.Write("Create.Table(");
            writer.Write(Quote(createTableOperation.Name));
            writer.Write(")");
            // Name contains schemaname + tableName
//            writer.Write(".InSchema(");
//            writer.Write(Quote(createTableOperation.Name));
//            writer.Write(")");
            writer.Indent++;
            foreach (var column in createTableOperation.Columns) {
                writer.WriteLine();
                var scrubbedName = ScrubName(column.Name);
                writer.Write(".WithColumn(");
                writer.Write(Quote(scrubbedName));
                writer.Write(")");
                Generate(column, writer);
            }
            // TODO
            if (createTableOperation.Annotations.Any()) {
                writer.WriteLine(",");
                writer.Write("annotations: ");
                GenerateAnnotations(createTableOperation.Annotations, writer);
            }
            writer.WriteLine(";");
            writer.Indent--;
            GenerateInline(createTableOperation.PrimaryKey, writer);
            
            //
            //            _newTableForeignKeys
            //                .Where(t => t.Item1 == createTableOperation)
            //                .Each(t => GenerateInline(t.Item2, writer));
            //
            //            _newTableIndexes
            //                .Where(t => t.Item1 == createTableOperation)
            //                .Each(t => GenerateInline(t.Item2, writer));
        }

        protected override void Generate(AlterTableOperation alterTableOperation, IndentedTextWriter writer)
        {
            base.Generate(alterTableOperation, writer);
        }

        protected override void GenerateInline(AddPrimaryKeyOperation addPrimaryKeyOperation, IndentedTextWriter writer)
        {
            Generate(addPrimaryKeyOperation, writer);
        }

        protected override void GenerateInline(AddForeignKeyOperation addForeignKeyOperation, IndentedTextWriter writer)
        {
            base.GenerateInline(addForeignKeyOperation, writer);
        }

        protected override void GenerateInline(CreateIndexOperation createIndexOperation, IndentedTextWriter writer)
        {
            base.GenerateInline(createIndexOperation, writer);
        }

        protected override void Generate(IEnumerable<string> columns, IndentedTextWriter writer)
        {
            base.Generate(columns, writer);
        }

        protected override void Generate(AddPrimaryKeyOperation addPrimaryKeyOperation, IndentedTextWriter writer)
        {
            writer.Write("Create.PrimaryKey(");
            writer.Write(Quote(addPrimaryKeyOperation.Name));
            writer.Write(")");
            writer.Write(".OnTable(");
            writer.Write(Quote(addPrimaryKeyOperation.Table));
            writer.Write(")");
            if (addPrimaryKeyOperation.Columns.Count == 1) {
                writer.Write(".Column(");
                writer.Write(Quote(addPrimaryKeyOperation.Columns.First()));
                writer.Write(")");
            } else {
                writer.Write(".Column(");
                writer.Write(String.Join(", ", addPrimaryKeyOperation.Columns.Select(Quote)));
                writer.Write(")");
            }
            writer.WriteLine(";");
        }

        protected override void Generate(DropPrimaryKeyOperation dropPrimaryKeyOperation, IndentedTextWriter writer)
        {
            base.Generate(dropPrimaryKeyOperation, writer);
        }

        protected override void Generate(AddForeignKeyOperation addForeignKeyOperation, IndentedTextWriter writer)
        {
            base.Generate(addForeignKeyOperation, writer);
        }

        protected override void Generate(DropForeignKeyOperation dropForeignKeyOperation, IndentedTextWriter writer)
        {
            base.Generate(dropForeignKeyOperation, writer);
        }

        protected override void Generate(CreateIndexOperation createIndexOperation, IndentedTextWriter writer)
        {
            base.Generate(createIndexOperation, writer);
        }

        protected override void Generate(DropIndexOperation dropIndexOperation, IndentedTextWriter writer)
        {
            base.Generate(dropIndexOperation, writer);
        }

        protected override void Generate(ColumnModel column, IndentedTextWriter writer, bool emitName = false)
        {
            if (!string.IsNullOrWhiteSpace(column.StoreType)) {
                writer.Write(".AsCustom(");
                writer.Write(Quote(column.StoreType));
                writer.Write(")");
            } else {
                switch (column.Type) {
                    case PrimitiveTypeKind.String:
                        writer.Write(".As");
                        if (column.IsFixedLength == true) {
                            writer.Write("FixedLength");
                        }
                        if (column.IsUnicode == true) {
                            writer.WriteLine("Ansi");
                        }
                        writer.Write("String(");
                        if (column.MaxLength.HasValue) {
                            writer.Write(column.MaxLength.Value);
                        }
                        writer.Write(")");
                        break;
                    case PrimitiveTypeKind.Binary:
                        writer.Write(".AsBinary(");
                        if (column.MaxLength.HasValue) {
                            writer.Write(column.MaxLength.Value);
                        }
                        writer.Write(")");
                        break;
                    case PrimitiveTypeKind.Boolean:
                        writer.Write(".AsBoolean()");
                        break;
                    case PrimitiveTypeKind.DateTime:
                        writer.Write(".AsDateTime()");
                        break;
                    case PrimitiveTypeKind.DateTimeOffset:
                        writer.Write(".AsDateTimeOffset()");
                        break;
                    case PrimitiveTypeKind.Byte:
                        writer.Write(".AsByte()");
                        break;
                    case PrimitiveTypeKind.Int16:
                        writer.Write(".AsInt16()");
                        break;
                    case PrimitiveTypeKind.Int32:
                        writer.Write(".AsInt32()");
                        break;
                    case PrimitiveTypeKind.Int64:
                        writer.Write(".AsInt64()");
                        break;
                    case PrimitiveTypeKind.Double:
                        writer.Write(".AsDouble()");
                        break;
                    case PrimitiveTypeKind.Single:
                        writer.Write(".AsFloat()");
                        break;
                    case PrimitiveTypeKind.Decimal:
                        writer.Write(".AsDecimal(");
                        if (column.Precision.HasValue && column.Scale.HasValue) {
                            writer.Write(column.Scale.Value);
                            writer.Write(", ");
                            writer.Write(column.Precision.Value);
                        }
                        writer.Write(")");
                        break;
                    case PrimitiveTypeKind.Guid:
                        writer.Write(".AsGuid()");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            writer.Write(column.IsNullable == false ? ".NotNullable()" : ".Nullable()");
            if (column.IsIdentity) {
                writer.Write(".Identity()");
            }
            // TODO Default value
            //            if (column.DefaultValue != null) {
            //                args.Add("defaultValue: " + Generate((dynamic) column.DefaultValue));
            //            }
            //            if (!string.IsNullOrWhiteSpace(column.DefaultValueSql)) {
            //                args.Add("defaultValueSql: " + Quote(column.DefaultValueSql));
            //            }
            // TODO RowVersion
            //            if (column.IsTimestamp) {
            //                args.Add("timestamp: true");
            //            }
            if (column.Annotations.Any()) {
                GenerateAnnotations(column.Annotations, writer);
            }
        }

        protected override string Generate(byte[] defaultValue)
        {
            return base.Generate(defaultValue);
        }

        protected override string Generate(DateTime defaultValue)
        {
            return base.Generate(defaultValue);
        }

        protected override string Generate(DateTimeOffset defaultValue)
        {
            return base.Generate(defaultValue);
        }

        protected override string Generate(decimal defaultValue)
        {
            return base.Generate(defaultValue);
        }

        protected override string Generate(Guid defaultValue)
        {
            return base.Generate(defaultValue);
        }

        protected override string Generate(long defaultValue)
        {
            return base.Generate(defaultValue);
        }

        protected override string Generate(float defaultValue)
        {
            return base.Generate(defaultValue);
        }

        protected override string Generate(string defaultValue)
        {
            return base.Generate(defaultValue);
        }

        protected override string Generate(TimeSpan defaultValue)
        {
            return base.Generate(defaultValue);
        }

        protected override string Generate(DbGeography defaultValue)
        {
            return base.Generate(defaultValue);
        }

        protected override string Generate(DbGeometry defaultValue)
        {
            return base.Generate(defaultValue);
        }

        protected override string Generate(object defaultValue)
        {
            return base.Generate(defaultValue);
        }

        protected override void Generate(DropTableOperation dropTableOperation, IndentedTextWriter writer)
        {
            base.Generate(dropTableOperation, writer);
        }

        protected override void Generate(MoveTableOperation moveTableOperation, IndentedTextWriter writer)
        {
            base.Generate(moveTableOperation, writer);
        }

        protected override void Generate(MoveProcedureOperation moveProcedureOperation, IndentedTextWriter writer)
        {
            base.Generate(moveProcedureOperation, writer);
        }

        protected override void Generate(RenameTableOperation renameTableOperation, IndentedTextWriter writer)
        {
            base.Generate(renameTableOperation, writer);
        }

        protected override void Generate(RenameProcedureOperation renameProcedureOperation, IndentedTextWriter writer)
        {
            base.Generate(renameProcedureOperation, writer);
        }

        protected override void Generate(RenameColumnOperation renameColumnOperation, IndentedTextWriter writer)
        {
            base.Generate(renameColumnOperation, writer);
        }

        protected override void Generate(RenameIndexOperation renameIndexOperation, IndentedTextWriter writer)
        {
            base.Generate(renameIndexOperation, writer);
        }

        protected override void Generate(SqlOperation sqlOperation, IndentedTextWriter writer)
        {
            base.Generate(sqlOperation, writer);
        }

        protected override string ScrubName(string name)
        {
            return base.ScrubName(name);
        }

        protected override string TranslateColumnType(PrimitiveTypeKind primitiveTypeKind)
        {
            return base.TranslateColumnType(primitiveTypeKind);
        }

        protected override string Quote(string identifier)
        {
            return base.Quote(identifier);
        }

        protected override IEnumerable<string> GetNamespaces(IEnumerable<MigrationOperation> operations)
        {
            return base.GetNamespaces(operations);
        }

        protected override IEnumerable<string> GetDefaultNamespaces(bool designer = false)
        {
            return base.GetDefaultNamespaces(designer).Concat(new[] {
                "FluentMigrator"
            }).OrderBy(n => n);
        }
    }
}