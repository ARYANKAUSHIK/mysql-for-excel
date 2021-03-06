﻿// Copyright (c) 2014, 2019, Oracle and/or its affiliates. All rights reserved.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;
using MySql.Utility.Classes;
using MySql.Utility.Forms;
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Previews the results of a procedure and lets users select rows to import to an Excel spreadsheet.
  /// </summary>
  public partial class ImportMultipleDialog : AutoStyleableBaseDialog
  {
    #region Constants

    /// <summary>
    /// The proposed name for a <see cref="ExcelInterop.PivotTable"/> created for all <see cref="ExcelInterop.ModelTable"/> objects in the active <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    public const string DATA_MODEL_PIVOT_TABLE_PROPOSED_NAME = "MultiTablePivot";

    #endregion Constants

    #region Fields

    /// <summary>
    /// A dictionary containing the names of imported <see cref="DbTable"/>s or <see cref="DbView"/>s and their corresponding created <see cref="ExcelInterop.ListObject"/> objects.
    /// </summary>
    private Dictionary<string, ExcelInterop.ListObject> _excelTablesDictionary;

    /// <summary>
    /// A list of the table names that will be imported by the <see cref="ImportData"/> method.
    /// </summary>
    private List<string> _importingTableNames;

    /// <summary>
    /// The Tables or Views selected by users for import.
    /// </summary>
    private readonly List<DbView> _importTablesOrViews;

    /// <summary>
    /// Flag indicating whether Excel relationships can be created.
    /// </summary>
    private bool _importRelationshipsEnabled;

    /// <summary>
    /// The Tables related to objects selected by the users.
    /// </summary>
    private readonly List<DbView> _relatedTables;

    /// <summary>
    /// A list of <see cref="MySqlDataRelationship"/> objects representing relationships to be created from the imported <see cref="DbTable"/> objects.
    /// </summary>
    private List<MySqlDataRelationship> _relationshipsToCreateList;

    /// <summary>
    /// The full Table or View DB objects list contained in the current selected schema.
    /// </summary>
    private readonly List<DbView> _tableOrViews;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportProcedureForm"/> class.
    /// </summary>
    /// <param name="tableOrViews">The full Table or View DB objects list contained in the current selected schema.</param>
    /// <param name="selectAllRelatedTables">Flag indicating whether all found related tables are selected by default.</param>
    public ImportMultipleDialog(List<DbView> tableOrViews, bool selectAllRelatedTables)
    {
      _importingTableNames = null;
      _tableOrViews = tableOrViews ?? throw new ArgumentNullException(nameof(tableOrViews));
      _tableOrViews.ForEach(dbo => dbo.Excluded = false);
      _importTablesOrViews = _tableOrViews.Where(dbo => dbo.Selected).ToList();
      _relatedTables = new List<DbView>();

      InitializeComponent();

      TablesViewsListView.ListViewItemSorter = new ListViewColumnSorter(0, SortOrder.Ascending);
      RelatedTablesListView.ListViewItemSorter = new ListViewColumnSorter(0, SortOrder.Ascending);
      SelectedTablesViewsLabel.Text += _importTablesOrViews.Count;
      RelatedTablesViewsLabel.Text += 0;
      SetWorkbookCompatibilityWarning();
      SetControlsEnabledState();
      ProcessSelectedTablesOrViews(selectAllRelatedTables);
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the Excel version is 2010 or lower.
    /// </summary>
    public static bool Excel2010OrLower => Globals.ThisAddIn.ExcelVersionNumber < ThisAddIn.EXCEL_2013_VERSION_NUMBER;

    /// <summary>
    /// Gets or sets the text associated with this control.
    /// </summary>
    public sealed override string Text
    {
      get => base.Text;
      set => base.Text = value;
    }

    #endregion Properties

    /// <summary>
    /// Adds the tables related to the given <see cref="DbTable"/> object to the <see cref="RelatedTablesListView"/>.
    /// </summary>
    /// <param name="dbTable">A <see cref="DbTable"/> already in the list.</param>
    /// <param name="checkAllRelatedTables">Flag indicating whether all found related tables not in the original selection are checked by default.</param>
    /// <param name="checkIfNotAlreadyInRelatedListView">Flag indicating whether any found related table should be checked if it exists already in the <see cref="RelatedTablesListView"/>.</param>
    private void AddRelatedTablesToRelatedTablesListView(DbTable dbTable, bool checkAllRelatedTables, bool checkIfNotAlreadyInRelatedListView)
    {
      var tablesInOriginalSelection = _importTablesOrViews.Where(dbo => dbo is DbTable).Select(dbo => dbo.Name).ToList();
      if (checkIfNotAlreadyInRelatedListView)
      {
        tablesInOriginalSelection.AddRange(RelatedTablesListView.Items.Cast<ListViewItem>().Select(item => item.Text));
      }

      var relationships = dbTable.Relationships.Where(rel => !tablesInOriginalSelection.Contains(rel.RelatedTableName)).ToList();
      if (relationships.Count == 0)
      {
        return;
      }

      foreach (var relationship in relationships)
      {
        if (!(_tableOrViews.FirstOrDefault(dbo => dbo.Name == relationship.RelatedTableName) is DbTable relatedTable) || _relatedTables.Contains(relatedTable))
        {
          continue;
        }

        // Fill the related tables and views list.
        _relatedTables.Add(relatedTable);
        var lvi = RelatedTablesListView.Items.Add(relationship.RelatedTableName, relationship.RelatedTableName, 0);
        lvi.SubItems.Add(relatedTable.RelatedTableNamesDelimitedList);
        lvi.Tag = relatedTable;
        lvi.Checked = checkAllRelatedTables;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AddRelatedTablesToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AddRelatedTablesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!(MiscUtilities.GetSelectedDbTableOrView(sender) is DbTable selectedTableOrView))
      {
        return;
      }

      AddRelatedTablesToRelatedTablesListView(selectedTableOrView, false, true);
      RelatedTablesListView.Sort();
    }

    /// <summary>
    /// Adds relationships in the given <see cref="DbTable"/> to the list of relationships to be created, if they do not create a circular reference in the data model.
    /// </summary>
    /// <param name="forDbTable">A <see cref="DbTable"/> instance.</param>
    private void AddRelationshipsAvoidingCircularReferences(DbTable forDbTable)
    {
      if (forDbTable == null
          || _importingTableNames == null
          || _importingTableNames.Count == 0)
      {
        return;
      }

      var forDbTableRelationships = forDbTable.Relationships.Where(rel => rel.ExistsAmongTablesInList(_importingTableNames)).ToList();
      if (forDbTableRelationships.Count == 0)
      {
        return;
      }

      if (_relationshipsToCreateList == null)
      {
        _relationshipsToCreateList = new List<MySqlDataRelationship>();
      }

      foreach (var relationship in forDbTableRelationships)
      {
        if (!relationship.CheckIfCreatesCircularReference(_relationshipsToCreateList))
        {
          _relationshipsToCreateList.Add(relationship);
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AdvancedOptionsButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AdvancedOptionsButton_Click(object sender, EventArgs e)
    {
      using (var optionsDialog = new ImportAdvancedOptionsDialog())
      {
        optionsDialog.ShowDialog();
        SetControlsEnabledState();
      }
    }

    /// <summary>
    /// Changes the checked status of all list view items on the related tables and views list.
    /// </summary>
    /// <param name="select">Flag indicating if the items are selected or not.</param>
    private void ChangeAllRelatedItemsSelection(bool select)
    {
      foreach (ListViewItem item in RelatedTablesListView.Items)
      {
        item.Checked = select;
      }
    }

    /// <summary>
    /// Creates a <see cref="ExcelInterop.ModelRelationship"/> objects representing relationships among imported <see cref="DbTable"/> objects.
    /// </summary>
    private void CreateExcelRelationships()
    {
      if (!CreateExcelRelationshipsCheckBox.Checked
          || _relationshipsToCreateList == null
          || _relationshipsToCreateList.Count == 0
          || _excelTablesDictionary == null)
      {
        return;
      }

      var relationshipsCreationErrorBuilder = new StringBuilder(_relationshipsToCreateList.Count * 200);
      foreach (var relationship in _relationshipsToCreateList)
      {
        var excelTableExists = _excelTablesDictionary.TryGetValue(relationship.TableName, out var excelTable);
        var relatedExcelTableExists = _excelTablesDictionary.TryGetValue(relationship.RelatedTableName, out var relatedExcelTable);
        if (!excelTableExists || !relatedExcelTableExists)
        {
          if (relationshipsCreationErrorBuilder.Length > 0)
          {
            relationshipsCreationErrorBuilder.Append(Environment.NewLine);
          }

          relationshipsCreationErrorBuilder.Append(relationship.GetCreationErrorMessage(MySqlDataRelationship.CreationStatus.ModelTablesNotFound));
          continue;
        }

        var creationStatus = relationship.CreateExcelRelationship(excelTable.Name, relatedExcelTable.Name);
        if (creationStatus == MySqlDataRelationship.CreationStatus.Success)
        {
          continue;
        }

        if (relationshipsCreationErrorBuilder.Length > 0)
        {
          relationshipsCreationErrorBuilder.Append(Environment.NewLine);
        }

        relationshipsCreationErrorBuilder.Append(relationship.GetCreationErrorMessage(creationStatus));
      }

      if (relationshipsCreationErrorBuilder.Length > 0)
      {
        InfoDialog.ShowDialog(InfoDialogProperties.GetErrorDialogProperties(Resources.ExcelRelationshipsCreationErrorTitle, Resources.ExcelRelationshipsCreationErrorDetail, null, relationshipsCreationErrorBuilder.ToString()));
        relationshipsCreationErrorBuilder.Clear();
      }

      _excelTablesDictionary.Clear();
    }

    /// <summary>
    /// Event delegate method fired when either the <see cref="CreatePivotTableCheckBox"/> or the <see cref="CreateExcelRelationshipsCheckBox"/> checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void CreatePivotOrRelationshipsCheckedChanged(object sender, EventArgs e)
    {
      var createPivotTable = CreatePivotTableCheckBox.Checked;
      PivotTablesComboBox.Enabled = !Excel2010OrLower && createPivotTable && CreateExcelRelationshipsCheckBox.Checked;
      PivotTablesComboBox.SelectedIndex = Excel2010OrLower || !CreateExcelRelationshipsCheckBox.Checked ? 1 : 0;
      if (!createPivotTable || !PivotTablesComboBox.CanFocus)
      {
        return;
      }

      // Give focus to the field related to the checkbox whose status changed.
      PivotTablesComboBox.Focus();
    }

    /// <summary>
    /// Creates a <see cref="ExcelInterop.PivotTable"/> in a new <see cref="ExcelInterop.Worksheet"/> that contains all <see cref="ExcelInterop.ModelTable"/>s in the active <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    private void CreatePivotTableForAllRelatedModelTables()
    {
      if (!CreatePivotTableCheckBox.Checked || PivotTablesComboBox.SelectedIndex != 0)
      {
        return;
      }

      var activeWorkbook = Globals.ThisAddIn.ActiveWorkbook;
      var workbookDataModelConnection = activeWorkbook.GetDataModelConnection();
      if (workbookDataModelConnection == null)
      {
        return;
      }

      // Create a new Excel Worksheet for the PivotTable since it will contain all related tables in the Workbook's data model
      var currentWorksheet = activeWorkbook.CreateWorksheet(DATA_MODEL_PIVOT_TABLE_PROPOSED_NAME, true);
      if (currentWorksheet == null)
      {
        return;
      }

      ExcelUtilities.CreatePivotTable(workbookDataModelConnection, Globals.ThisAddIn.Application.ActiveCell, DATA_MODEL_PIVOT_TABLE_PROPOSED_NAME);
    }

    /// <summary>
    /// Imports the selected MySQL tables data into new Excel worksheets.
    /// </summary>
    /// <returns><c>true</c> if the import is successful, <c>false</c> if errors were found during the import.</returns>
    private void ImportData()
    {
      // Import tables data in Excel worksheets
      _excelTablesDictionary = new Dictionary<string, ExcelInterop.ListObject>();
      var fullImportList = _importTablesOrViews.Concat(_relatedTables.Where(dbo => !dbo.Excluded)).ToList();
      _importingTableNames = fullImportList.Where(dbo => dbo is DbTable).Select(dbo => dbo.Name).ToList();
      foreach (var importTableOrView in fullImportList)
      {
        // Refresh import parameter values
        SetImportParameterValues(importTableOrView);

        // Import the table/view data into an Excel worksheet
        var importTuple = importTableOrView.ImportData();
        if (!(importTuple.Item2 is ExcelInterop.ListObject excelTable)
            || !(importTableOrView is DbTable dbTable)
            || !CreateExcelRelationshipsCheckBox.Checked)
        {
          continue;
        }

        // Add relationships of the current importing
        _excelTablesDictionary.Add(importTuple.Item1.TableName, excelTable);
        AddRelationshipsAvoidingCircularReferences(dbTable);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportMultipleDialog"/> is closing.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportMultipleDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult != DialogResult.OK)
      {
        return;
      }

      Cursor = Cursors.WaitCursor;
      ImportData();
      CreateExcelRelationships();
      CreatePivotTableForAllRelatedModelTables();
      Cursor = Cursors.Default;
    }

    /// <summary>
    /// Event delegate method fired when a <see cref="ListView"/> column header is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e"><see cref="ColumnClickEventArgs"/> arguments.</param>
    private void ListViewColumnClick(object sender, ColumnClickEventArgs e)
    {
      var listView = sender as ListView;
      if (!(listView?.ListViewItemSorter is ListViewColumnSorter sorter))
      {
        return;
      }

      // Determine if clicked column is already the column that is being sorted.
      if (e.Column == sorter.SortColumnIndex)
      {
        // Reverse the current sort direction for this column.
        sorter.Order = sorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
      }
      else
      {
        // Set the column number that is to be sorted; default to ascending.
        sorter.SortColumnIndex = e.Column;
        sorter.Order = SortOrder.Ascending;
      }

      // Perform the sort with these new sort options.
      listView.Sort();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewDataToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var previewTableOrView = MiscUtilities.GetSelectedDbTableOrView(sender);
      if (previewTableOrView == null)
      {
        return;
      }

      using (var previewDialog = new PreviewTableViewDialog(previewTableOrView, false))
      {
        previewDialog.ShowDialog();
      }
    }

    /// <summary>
    /// Refreshes the list with tables and views.
    /// </summary>
    /// <param name="checkAllRelatedTables">Flag indicating whether all found related tables not in the original selection are checked by default.</param>
    private void ProcessSelectedTablesOrViews(bool checkAllRelatedTables)
    {
      foreach (var tableOrView in _importTablesOrViews)
      {
        // Fill the selected tables and views list.
        var dbTable = tableOrView as DbTable;
        var lvi = TablesViewsListView.Items.Add(tableOrView.Name, tableOrView.Name, dbTable == null ? 1 : 0);
        lvi.SubItems.Add(string.Empty);
        lvi.Tag = tableOrView;
        if (dbTable == null)
        {
          continue;
        }

        // Get the related tables that are not in the original selection so we can fill the Related Tables list view.
        lvi.SubItems[1].Text = dbTable.RelatedTableNamesDelimitedList;
        AddRelatedTablesToRelatedTablesListView(dbTable, checkAllRelatedTables, false);
      }

      RelatedTablesListView.Sort();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="RelatedTablesListView"/> gets an item checked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RelatedTablesViewsListView_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
      RelatedTablesViewsLabel.Text = Resources.SelectedRelatedTablesAndViewsText + RelatedTablesListView.CheckedIndices.Count;
      if (!(e.Item.Tag is DbObject relatedDbObject))
      {
        return;
      }

      relatedDbObject.Excluded = !e.Item.Checked;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SelectAllToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ChangeAllRelatedItemsSelection(true);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SelectNoneToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SelectNoneToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ChangeAllRelatedItemsSelection(false);
    }

    /// <summary>
    /// Sets the enabled or disabled status of controls in the form.
    /// </summary>
    private void SetControlsEnabledState()
    {
      _importRelationshipsEnabled = !Excel2010OrLower && Settings.Default.ImportCreateExcelTable;
      AddSummaryFieldsCheckBox.Checked = Settings.Default.ImportCreateExcelTable && AddSummaryFieldsCheckBox.Checked;
      AddSummaryFieldsCheckBox.Enabled = Settings.Default.ImportCreateExcelTable;
      CreateExcelRelationshipsCheckBox.Checked = _importRelationshipsEnabled;
      CreateExcelRelationshipsCheckBox.Enabled = _importRelationshipsEnabled;
      WhyDisabledLinkLabel.Visible = !_importRelationshipsEnabled;
      CreatePivotOrRelationshipsCheckedChanged(this, EventArgs.Empty);
    }

    /// <summary>
    /// Sets the import parameter values into the given database object.
    /// This is needed before getting any data from it.
    /// </summary>
    private void SetImportParameterValues(DbView dbTableOrView)
    {
      dbTableOrView.ImportParameters.AddSummaryRow = AddSummaryFieldsCheckBox.Checked;
      dbTableOrView.ImportParameters.ColumnsNamesList = null;
      dbTableOrView.ImportParameters.CreatePivotTable = CreatePivotTableCheckBox.Checked && PivotTablesComboBox.SelectedIndex == 1;
      dbTableOrView.ImportParameters.FirstRowIndex = -1;
      dbTableOrView.ImportParameters.ForEditDataOperation = false;
      dbTableOrView.ImportParameters.IncludeColumnNames = true;
      dbTableOrView.ImportParameters.IntoNewWorksheet = true;
      dbTableOrView.ImportParameters.RowsCount = -1;
    }

    /// <summary>
    /// Sets the warning about the active <see cref="ExcelInterop.Workbook"/> being in compatibility mode.
    /// </summary>
    private void SetWorkbookCompatibilityWarning()
    {
      var workbookInCompatibilityMode = Globals.ThisAddIn.ActiveWorkbook.Excel8CompatibilityMode;
      WorkbookInCompatibilityModeWarningLabel.Text = Resources.WorkbookInCompatibilityModeWarning;
      WorkbookInCompatibilityModeWarningLabel.Visible = workbookInCompatibilityMode;
      WorkbookInCompatibilityModeWarningPictureBox.Visible = workbookInCompatibilityMode;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="TablesViewsContextMenuStrip"/> context menu is being opened.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TablesViewsContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      var dbView = MiscUtilities.GetSelectedDbTableOrView(sender, out var listView);
      if (listView == null)
      {
        return;
      }

      var showRelatedTablesItems = listView != TablesViewsListView;
      var dbViewIsSelected = dbView != null;
      AddRelatedTablesToolStripMenuItem.Visible = dbViewIsSelected && showRelatedTablesItems;
      PreviewDataToolStripMenuItem.Visible = dbViewIsSelected;
      SelectAllToolStripMenuItem.Visible = showRelatedTablesItems;
      SelectNoneToolStripMenuItem.Visible = showRelatedTablesItems;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="WhyDisabledLinkLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void WhyDisabledLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      InfoDialog.ShowDialog(InfoDialogProperties.GetInformationDialogProperties(
        Resources.ImportMultipleRelationshipsNotSupportedTitleText,
        Excel2010OrLower
          ? Resources.ImportMultipleRelationshipsNotSupportedExcelVersionWarningText
          : Resources.ImportMultipleRelationshipsNotSupportedNoExcelTablesWarningText,
        null,
        Resources.ImportMultipleSinglePivotTableNotSupportedText));
    }
  }
}