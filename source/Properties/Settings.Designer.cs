﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MySQL.ForExcel.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ExportDetectDataType {
            get {
                return ((bool)(this["ExportDetectDataType"]));
            }
            set {
                this["ExportDetectDataType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ExportAddBufferToVarchar {
            get {
                return ((bool)(this["ExportAddBufferToVarchar"]));
            }
            set {
                this["ExportAddBufferToVarchar"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ExportAutoIndexIntColumns {
            get {
                return ((bool)(this["ExportAutoIndexIntColumns"]));
            }
            set {
                this["ExportAutoIndexIntColumns"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ExportAutoAllowEmptyNonIndexColumns {
            get {
                return ((bool)(this["ExportAutoAllowEmptyNonIndexColumns"]));
            }
            set {
                this["ExportAutoAllowEmptyNonIndexColumns"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ExportUseFormattedValues {
            get {
                return ((bool)(this["ExportUseFormattedValues"]));
            }
            set {
                this["ExportUseFormattedValues"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool AppendPerformAutoMap {
            get {
                return ((bool)(this["AppendPerformAutoMap"]));
            }
            set {
                this["AppendPerformAutoMap"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool AppendAutoStoreColumnMapping {
            get {
                return ((bool)(this["AppendAutoStoreColumnMapping"]));
            }
            set {
                this["AppendAutoStoreColumnMapping"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool AppendReloadColumnMapping {
            get {
                return ((bool)(this["AppendReloadColumnMapping"]));
            }
            set {
                this["AppendReloadColumnMapping"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool AppendUseFormattedValues {
            get {
                return ((bool)(this["AppendUseFormattedValues"]));
            }
            set {
                this["AppendUseFormattedValues"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Generic.List<MySQL.ForExcel.Classes.MySqlColumnMapping> StoredDataMappings {
            get {
                return ((global::System.Collections.Generic.List<MySQL.ForExcel.Classes.MySqlColumnMapping>)(this["StoredDataMappings"]));
            }
            set {
                this["StoredDataMappings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int ExportLimitPreviewRowsQuantity {
            get {
                return ((int)(this["ExportLimitPreviewRowsQuantity"]));
            }
            set {
                this["ExportLimitPreviewRowsQuantity"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int AppendLimitPreviewRowsQuantity {
            get {
                return ((int)(this["AppendLimitPreviewRowsQuantity"]));
            }
            set {
                this["AppendLimitPreviewRowsQuantity"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("15")]
        public uint GlobalConnectionConnectionTimeout {
            get {
                return ((uint)(this["GlobalConnectionConnectionTimeout"]));
            }
            set {
                this["GlobalConnectionConnectionTimeout"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("60")]
        public uint GlobalConnectionCommandTimeout {
            get {
                return ((uint)(this["GlobalConnectionCommandTimeout"]));
            }
            set {
                this["GlobalConnectionCommandTimeout"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int ImportPreviewRowsQuantity {
            get {
                return ((int)(this["ImportPreviewRowsQuantity"]));
            }
            set {
                this["ImportPreviewRowsQuantity"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ImportEscapeFormulaTextValues {
            get {
                return ((bool)(this["ImportEscapeFormulaTextValues"]));
            }
            set {
                this["ImportEscapeFormulaTextValues"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ConvertedSettingsStoredMappingsCasing {
            get {
                return ((bool)(this["ConvertedSettingsStoredMappingsCasing"]));
            }
            set {
                this["ConvertedSettingsStoredMappingsCasing"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool EditUseOptimisticUpdate {
            get {
                return ((bool)(this["EditUseOptimisticUpdate"]));
            }
            set {
                this["EditUseOptimisticUpdate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool GlobalSqlQueriesPreviewQueries {
            get {
                return ((bool)(this["GlobalSqlQueriesPreviewQueries"]));
            }
            set {
                this["GlobalSqlQueriesPreviewQueries"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool GlobalSqlQueriesShowQueriesWithResults {
            get {
                return ((bool)(this["GlobalSqlQueriesShowQueriesWithResults"]));
            }
            set {
                this["GlobalSqlQueriesShowQueriesWithResults"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ImportCreateExcelTable {
            get {
                return ((bool)(this["ImportCreateExcelTable"]));
            }
            set {
                this["ImportCreateExcelTable"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("MySqlDefault")]
        public string ImportExcelTableStyleName {
            get {
                return ((string)(this["ImportExcelTableStyleName"]));
            }
            set {
                this["ImportExcelTableStyleName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool EditSessionsRestoreWhenOpeningWorkbook {
            get {
                return ((bool)(this["EditSessionsRestoreWhenOpeningWorkbook"]));
            }
            set {
                this["EditSessionsRestoreWhenOpeningWorkbook"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Generic.List<MySQL.ForExcel.Classes.EditConnectionInfo> EditConnectionInfosList {
            get {
                return ((global::System.Collections.Generic.List<MySQL.ForExcel.Classes.EditConnectionInfo>)(this["EditConnectionInfosList"]));
            }
            set {
                this["EditConnectionInfosList"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool EditSessionsReuseWorksheets {
            get {
                return ((bool)(this["EditSessionsReuseWorksheets"]));
            }
            set {
                this["EditSessionsReuseWorksheets"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ImportPrefixExcelTable {
            get {
                return ((bool)(this["ImportPrefixExcelTable"]));
            }
            set {
                this["ImportPrefixExcelTable"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("MySQL")]
        public string ImportPrefixExcelTableText {
            get {
                return ((string)(this["ImportPrefixExcelTableText"]));
            }
            set {
                this["ImportPrefixExcelTableText"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AppendSqlQueriesDisableIndexes {
            get {
                return ((bool)(this["AppendSqlQueriesDisableIndexes"]));
            }
            set {
                this["AppendSqlQueriesDisableIndexes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ExportSqlQueriesCreateIndexesLast {
            get {
                return ((bool)(this["ExportSqlQueriesCreateIndexesLast"]));
            }
            set {
                this["ExportSqlQueriesCreateIndexesLast"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool HideLocalizedDateFormatNames {
            get {
                return ((bool)(this["HideLocalizedDateFormatNames"]));
            }
            set {
                this["HideLocalizedDateFormatNames"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Generic.List<MySQL.ForExcel.Classes.ImportConnectionInfo> ImportConnectionInfosList {
            get {
                return ((global::System.Collections.Generic.List<MySQL.ForExcel.Classes.ImportConnectionInfo>)(this["ImportConnectionInfosList"]));
            }
            set {
                this["ImportConnectionInfosList"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ExportShowAllMySqlDataTypes {
            get {
                return ((bool)(this["ExportShowAllMySqlDataTypes"]));
            }
            set {
                this["ExportShowAllMySqlDataTypes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SchemasDisplayCollations {
            get {
                return ((bool)(this["SchemasDisplayCollations"]));
            }
            set {
                this["SchemasDisplayCollations"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool EditPreviewMySqlData {
            get {
                return ((bool)(this["EditPreviewMySqlData"]));
            }
            set {
                this["EditPreviewMySqlData"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30")]
        public int ConnectionInfosLastAccessDays {
            get {
                return ((int)(this["ConnectionInfosLastAccessDays"]));
            }
            set {
                this["ConnectionInfosLastAccessDays"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool DeleteAutomaticallyOrphanedConnectionInfos {
            get {
                return ((bool)(this["DeleteAutomaticallyOrphanedConnectionInfos"]));
            }
            set {
                this["DeleteAutomaticallyOrphanedConnectionInfos"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AppendShowDataTypes {
            get {
                return ((bool)(this["AppendShowDataTypes"]));
            }
            set {
                this["AppendShowDataTypes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AppendConfirmColumnMappingOverwriting {
            get {
                return ((bool)(this["AppendConfirmColumnMappingOverwriting"]));
            }
            set {
                this["AppendConfirmColumnMappingOverwriting"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ExportGenerateMultipleInserts {
            get {
                return ((bool)(this["ExportGenerateMultipleInserts"]));
            }
            set {
                this["ExportGenerateMultipleInserts"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AppendGenerateMultipleInserts {
            get {
                return ((bool)(this["AppendGenerateMultipleInserts"]));
            }
            set {
                this["AppendGenerateMultipleInserts"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AdjustedMultipleInsertFlags {
            get {
                return ((bool)(this["AdjustedMultipleInsertFlags"]));
            }
            set {
                this["AdjustedMultipleInsertFlags"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("m/d/yyyy h:mm")]
        public string ImportExcelFormatLongDates {
            get {
                return ((string)(this["ImportExcelFormatLongDates"]));
            }
            set {
                this["ImportExcelFormatLongDates"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("m/d/yyyy")]
        public string ImportExcelFormatShortDates {
            get {
                return ((string)(this["ImportExcelFormatShortDates"]));
            }
            set {
                this["ImportExcelFormatShortDates"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("hh:mm:ss")]
        public string ImportExcelFormatTime {
            get {
                return ((string)(this["ImportExcelFormatTime"]));
            }
            set {
                this["ImportExcelFormatTime"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int WorkbenchMigrationRetryDelay {
            get {
                return ((int)(this["WorkbenchMigrationRetryDelay"]));
            }
            set {
                this["WorkbenchMigrationRetryDelay"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool WorkbenchMigrationSucceeded {
            get {
                return ((bool)(this["WorkbenchMigrationSucceeded"]));
            }
            set {
                this["WorkbenchMigrationSucceeded"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime WorkbenchMigrationLastAttempt {
            get {
                return ((global::System.DateTime)(this["WorkbenchMigrationLastAttempt"]));
            }
            set {
                this["WorkbenchMigrationLastAttempt"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool GlobalImportDataRestoreWhenOpeningWorkbook {
            get {
                return ((bool)(this["GlobalImportDataRestoreWhenOpeningWorkbook"]));
            }
            set {
                this["GlobalImportDataRestoreWhenOpeningWorkbook"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WKT")]
        public string GlobalSpatialDataAsTextFormat {
            get {
                return ((string)(this["GlobalSpatialDataAsTextFormat"]));
            }
            set {
                this["GlobalSpatialDataAsTextFormat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsProviderAttribute(typeof(MySQL.ForExcel.Classes.MySqlForExcelSettings))]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ErrorOutAndAbort")]
        public string AppendDuplicateUniqueValuesAction {
            get {
                return ((string)(this["AppendDuplicateUniqueValuesAction"]));
            }
            set {
                this["AppendDuplicateUniqueValuesAction"] = value;
            }
        }
    }
}
