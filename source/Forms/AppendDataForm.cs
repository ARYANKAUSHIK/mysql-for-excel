﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.Utility;
using MySql.Data.MySqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel
{
  public partial class AppendDataForm : AutoStyleableBaseDialog
  {
    private MySqlWorkbenchConnection wbConnection;
    private ExportDataHelper exportDataHelper;
    private MySQLTable exportTable { get { return exportDataHelper.ExportTable; } }
    private List<ColumnGuessData> headerRowColumnsGuessData { get { return exportDataHelper.HeaderRowColumnsGuessData; } }
    private List<ColumnGuessData> dataRowsColumnsGuessData { get { return exportDataHelper.DataRowsColumnsGuessData; } }
    private DataTable formattedExcelData { get { return exportDataHelper.FormattedExcelData; } }
    private DataTable unformattedExcelData { get { return exportDataHelper.UnformattedExcelData; } }
    private DataTable fromExcelDataTable { get { return (Settings.Default.AppendUseFormattedValues ? formattedExcelData : unformattedExcelData); } }
    private DataTable toMySQLDataTable = null;
    private Rectangle dragBoxFromMouseDown = Rectangle.Empty;
    private Point screenOffset;
    private int grdColumnIndexToDrag = -1;
    private int grdToTableColumnIndexToDrop = -1;
    private int maxMappingCols = 0;
    private Cursor linkCursor;
    private MySQLColumnMapping currentColumnMapping = null;
    private List<MySQLColumnMapping> storedColumnMappingsList;

    public AppendDataForm(MySqlWorkbenchConnection wbConnection, Excel.Range exportDataRange, DBObject importDBObject)
    {
      this.wbConnection = wbConnection;
      linkCursor = Utilities.CreateCursor(new Bitmap(Properties.Resources.chain_link_24x24), 3, 3);

      InitializeComponent();

      grdFromExcelData.EnableHeadersVisualStyles = false;
      exportDataHelper = new ExportDataHelper(wbConnection, exportDataRange, importDBObject.Name);
      initializeToTableGrid(importDBObject);
      string excelRangeAddress = exportDataRange.Address.Replace("$", String.Empty);
      Text = String.Format("Append Data [{0}])", excelRangeAddress);
      changeFormattedDataSource();
      chkFirstRowHeaders_CheckedChanged(chkFirstRowHeaders, EventArgs.Empty);
      maxMappingCols = Math.Min(grdToMySQLTable.Columns.Count, grdFromExcelData.Columns.Count);
      clearMappingsOnToTableGridAndMySQLTable();
      loadStoredColumnMappings();
      if (Settings.Default.AppendPerformAutoMap)
        cmbMappingMethod.SelectedIndex = 0;
      else if (!selectStoredMappingForTargetTable())
        cmbMappingMethod.SelectedIndex = 1;
    }

    private bool selectStoredMappingForTargetTable()
    {
      bool appliedStoredMapping = false;

      foreach (MySQLColumnMapping mapping in storedColumnMappingsList)
      {
        if (mapping.TableName == toMySQLDataTable.TableName && mapping.AllColumnsMatch(toMySQLDataTable, true))
        {
          cmbMappingMethod.SelectedIndex = cmbMappingMethod.Items.IndexOf(mapping.Name);
          appliedStoredMapping = true;
          break;
        }
      }

      return appliedStoredMapping;
    }

    private void initializeToTableGrid(DBObject importDBObject)
    {
      toMySQLDataTable = Utilities.GetDataFromTableOrView(wbConnection, importDBObject, null, 0, 10);
      long totalRowsCount = Utilities.GetRowsCountFromTableOrView(wbConnection, importDBObject);
      grdToMySQLTable.DataSource = toMySQLDataTable;
      foreach (DataGridViewColumn gridCol in grdToMySQLTable.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }
      grdToMySQLTable.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
    }

    private void refreshMappingMethodCombo()
    {
      cmbMappingMethod.Items.Clear();
      cmbMappingMethod.Items.Add("Automatic");
      cmbMappingMethod.Items.Add("Manual");

      if (storedColumnMappingsList != null)
        foreach (MySQLColumnMapping mapping in storedColumnMappingsList)
          cmbMappingMethod.Items.Add(mapping.Name);
      
      cmbMappingMethod.SelectedIndex = 0;
    }

    private void loadStoredColumnMappings()
    {
      storedColumnMappingsList = new List<MySQLColumnMapping>();

      // TODO: Insert logic here to read from \users\<username>\AppData\Local\MySQL For Excel\column_mappings.xml
      // and fill storedColumnMappingsList, Schema and TableName must match current Schema in wbConnection and
      // the name of the table being appended to.

      refreshMappingMethodCombo();
    }

    private string[] getColumnNamesArray(DataTable dt)
    {
      string[] retArray = null;

      if (dt != null && dt.Columns.Count > 0)
      {
        retArray = new string[dt.Columns.Count];
        for (int i = 0; i < dt.Columns.Count; i++)
          retArray[i] = dt.Columns[i].ColumnName;
      }

      return retArray;
    }

    private MySQLColumnMapping createColumnMappingForAutomatic()
    {
      MySQLColumnMapping autoMapping = new MySQLColumnMapping("Automatic", getColumnNamesArray(fromExcelDataTable), getColumnNamesArray(toMySQLDataTable));
      autoMapping.SchemaName = wbConnection.Schema;
      autoMapping.TableName = exportDataHelper.ExportTable.Name;
      int autoMappedColumns = 0;
      
      // Attempt to auto-map using column names if the Excel data contains the column names
      if (chkFirstRowHeaders.Checked)
      {
        for (int toColIdx = 0; toColIdx < toMySQLDataTable.Columns.Count; toColIdx++)
        {
          string targetColName = toMySQLDataTable.Columns[toColIdx].ColumnName;
          string matchSourceName = String.Empty;
          int fromColIdx = -1;
          foreach (DataGridViewColumn gridCol in grdFromExcelData.Columns)
          {
            if (gridCol.HeaderText.ToLowerInvariant() == targetColName)
            {
              matchSourceName = gridCol.HeaderText;
              fromColIdx = gridCol.Index;
              break;
            }
          }
          if (matchSourceName.Length > 0)
          {
            autoMapping.MappedSourceIndexes[toColIdx] = fromColIdx;
            autoMappedColumns++;
          }
        }
      }

      // Auto-map 1-1 if data types match
      if (autoMappedColumns != maxMappingCols)
      {
        autoMapping.ClearMappings();
        for (int colIdx = 0; colIdx < toMySQLDataTable.Columns.Count; colIdx++)
        {
          if (colIdx >= maxMappingCols)
            break;
          if (exportDataHelper.ExportTable.Columns[colIdx].DataType.ToLowerInvariant() == exportDataHelper.DataRowsColumnsGuessData[colIdx].MySQLType.ToLowerInvariant())
          {
            autoMapping.MappedSourceIndexes[colIdx] = colIdx;
            autoMappedColumns++;
          }
        }
      }

      // If auto-map was not successful return object without mappings
      if (autoMappedColumns != maxMappingCols)
        autoMapping.ClearMappings();

      return autoMapping;
    }

    private MySQLColumnMapping createColumnMappingForManual()
    {
      MySQLColumnMapping manualMapping;
      if (currentColumnMapping == null)
      {
        manualMapping = new MySQLColumnMapping(getColumnNamesArray(fromExcelDataTable), getColumnNamesArray(toMySQLDataTable));
        manualMapping.SchemaName = wbConnection.Schema;
        manualMapping.TableName = exportDataHelper.ExportTable.Name;
      }
      else
        manualMapping = currentColumnMapping;
      manualMapping.Name = "Manual";
      return manualMapping;
    }

    private void applySelectedStoredColumnMapping()
    {
      if (currentColumnMapping != null)
      {
        if (currentColumnMapping.Name != "Manual")
          clearMappingsOnToTableGridAndMySQLTable();
        for (int mappedIdx = 0; mappedIdx < currentColumnMapping.MappedSourceIndexes.Length; mappedIdx++)
        {
          int currentMappedSourceIndex = currentColumnMapping.MappedSourceIndexes[mappedIdx];
          string previouslyMappedColName = exportTable.Columns[mappedIdx].MappedDataColName;
          bool mapped = currentMappedSourceIndex >= 0;
          string mappedColName = (mapped ? currentColumnMapping.SourceColumns[currentMappedSourceIndex] : String.Empty);

          MultiHeaderColumn multiHeaderCol = grdToMySQLTable.MultiHeaderColumnList[mappedIdx];
          multiHeaderCol.HeaderText = mappedColName;
          multiHeaderCol.BackgroundColor = (mapped ? Color.LightGreen : Color.OrangeRed);
          exportTable.Columns[mappedIdx].MappedDataColName = (mapped ? mappedColName : null);

          if (!String.IsNullOrEmpty(previouslyMappedColName))
          {
            DataGridViewCellStyle newStyle = new DataGridViewCellStyle(grdFromExcelData.Columns[previouslyMappedColName].HeaderCell.Style);
            newStyle.BackColor = (mapped ? Color.LightGreen : SystemColors.Control);
            grdFromExcelData.Columns[previouslyMappedColName].HeaderCell.Style = newStyle;
          }
        }
        grdToMySQLTable.Refresh();
        grdFromExcelData.Refresh();
        grdToTable_SelectionChanged(grdToMySQLTable, EventArgs.Empty);
      }
      btnStoreMapping.Enabled = currentColumnMapping.MappedQuantity > 0;
    }

    private void clearMappingsOnToTableGridAndMySQLTable()
    {
      bool newMappings = grdToMySQLTable.MultiHeaderColumnList.Count == 0;
      for (int colIdx = 0; colIdx < grdToMySQLTable.Columns.Count; colIdx++)
      {
        if (newMappings)
          grdToMySQLTable.MultiHeaderColumnList.Add(new MultiHeaderColumn(String.Empty, colIdx, colIdx));
        else
          grdToMySQLTable.MultiHeaderColumnList[colIdx].HeaderText = String.Empty;

        grdToMySQLTable.MultiHeaderColumnList[colIdx].BackgroundColor = Color.OrangeRed;

        if (colIdx < grdFromExcelData.Columns.Count)
        {
          DataGridViewCellStyle newStyle = new DataGridViewCellStyle(grdFromExcelData.Columns[colIdx].HeaderCell.Style);
          newStyle.BackColor = SystemColors.Control;
          grdFromExcelData.Columns[colIdx].HeaderCell.Style = newStyle;
        }
        exportTable.Columns[colIdx].MappedDataColName = null;
      }
      grdToMySQLTable.Refresh();
      grdFromExcelData.Refresh();
      grdToTable_SelectionChanged(grdToMySQLTable, EventArgs.Empty);
      btnStoreMapping.Enabled = false;
    }

    private void performManualSingleColumnMapping(int fromColumnIndex, int toColumnIndex, string mappedColName)
    {
      string previouslyMappedColName = exportTable.Columns[toColumnIndex].MappedDataColName;
      bool mapping = !String.IsNullOrEmpty(mappedColName) && fromColumnIndex >= 0;

      MultiHeaderColumn multiHeaderCol = grdToMySQLTable.MultiHeaderColumnList[toColumnIndex];
      multiHeaderCol.HeaderText = (mapping ? mappedColName : String.Empty);
      multiHeaderCol.BackgroundColor = (mapping ? Color.LightGreen : Color.OrangeRed);
      exportTable.Columns[toColumnIndex].MappedDataColName = mappedColName;
      currentColumnMapping.MappedSourceIndexes[toColumnIndex] = fromColumnIndex;

      if (!String.IsNullOrEmpty(previouslyMappedColName))
      {
        DataGridViewCellStyle newStyle = new DataGridViewCellStyle(grdFromExcelData.Columns[previouslyMappedColName].HeaderCell.Style);
        newStyle.BackColor = (mapping ? Color.LightGreen : SystemColors.Control);
        grdFromExcelData.Columns[previouslyMappedColName].HeaderCell.Style = newStyle;
      }

      grdToMySQLTable.Refresh();
      grdFromExcelData.Refresh();
      grdToTable_SelectionChanged(grdToMySQLTable, EventArgs.Empty);
    }

    private void changeFormattedDataSource()
    {
      grdFromExcelData.DataSource = fromExcelDataTable;
      foreach (DataGridViewColumn gridCol in grdFromExcelData.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }
      grdFromExcelData.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      chkFirstRowHeaders_CheckedChanged(chkFirstRowHeaders, EventArgs.Empty);
    }

    private bool storeColumnMappingInFile(MySQLColumnMapping mapping)
    {
      bool success = true;

      // Insert logic to save the given mapping in XML file
      if (!storedColumnMappingsList.Contains(mapping))
      {
        storedColumnMappingsList.Add(mapping);
        refreshMappingMethodCombo();
      }

      return success;
    }

    private void chkFirstRowHeaders_CheckedChanged(object sender, EventArgs e)
    {
      if (grdFromExcelData.Rows.Count == 0)
        return;
      bool firstRowColNames = chkFirstRowHeaders.Checked;
      grdFromExcelData.CurrentCell = null;
      foreach (DataGridViewColumn gridCol in grdFromExcelData.Columns)
        gridCol.HeaderText = (firstRowColNames ? grdFromExcelData.Rows[0].Cells[gridCol.Index].Value.ToString() : formattedExcelData.Columns[gridCol.Index].ColumnName);
      grdFromExcelData.Rows[0].Visible = !firstRowColNames;
      if (!(chkFirstRowHeaders.Checked && grdFromExcelData.Rows.Count < 2))
        grdFromExcelData.FirstDisplayedScrollingRowIndex = (chkFirstRowHeaders.Checked ? 1 : 0);
      if (currentColumnMapping != null && currentColumnMapping.Name == "Automatic")
        cmbMappingMethod_SelectedIndexChanged(cmbMappingMethod, EventArgs.Empty);
    }

    private void grdPreviewData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      grdFromExcelData.ClearSelection();
    }

    private void grdToTable_SelectionChanged(object sender, EventArgs e)
    {
      bool anySelected = grdToMySQLTable.SelectedColumns.Count > 0;
      string mappedColName = (anySelected ? exportTable.Columns[grdToMySQLTable.SelectedColumns[0].DisplayIndex].MappedDataColName : null);
    }

    private void btnAppend_Click(object sender, EventArgs e)
    {
      if (exportTable.Columns.Count(col => !String.IsNullOrEmpty(col.MappedDataColName)) < maxMappingCols)
      {
        DialogResult dr = Utilities.ShowWarningBox(Properties.Resources.ColumnMappingIncomplete);
        if (dr == DialogResult.No)
          return;
      }
      bool success = exportDataHelper.InsertData(chkFirstRowHeaders.Checked, Settings.Default.AppendUseFormattedValues);
      if (success)
      {
        DialogResult = DialogResult.OK;
        Close();
      }
    }

    private void grdMouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
        return;
      DataGridView gridObject = (sender as DataGridView);
      DataGridView.HitTestInfo info = gridObject.HitTest(e.X, e.Y);
      grdColumnIndexToDrag = info.ColumnIndex;
      if (grdColumnIndexToDrag >= 0)
      {
        // Remember the point where the mouse down occurred. The DragSize indicates the size that the mouse can move before a drag event should be started.  
        Size dragSize = SystemInformation.DragSize;

        // Create a rectangle using the DragSize, with the mouse position being at the center of the rectangle.
        dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
      }
      else
        // Reset the rectangle if the mouse is not over an item in the ListBox.
        dragBoxFromMouseDown = Rectangle.Empty;
    }

    private void grdMouseUp(object sender, MouseEventArgs e)
    {
      // Reset the drag rectangle when the mouse button is raised.
      dragBoxFromMouseDown = Rectangle.Empty;
    }

    private void grdMouseMove(object sender, MouseEventArgs e)
    {
      if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
      {
        // If the mouse moves outside the rectangle, start the drag.
        if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
        {
          DataGridView gridObject = (sender as DataGridView);

          // The screenOffset is used to account for any desktop bands that may be at the top or left side of the screen when determining when to cancel the drag drop operation.
          screenOffset = SystemInformation.WorkingArea.Location;

          // Proceed with the drag-and-drop, passing in the list item.           
          DragDropEffects dropEffect = gridObject.DoDragDrop(grdColumnIndexToDrag, DragDropEffects.All | DragDropEffects.Link);
        }
      }
    }

    private void grdGiveFeedback(object sender, GiveFeedbackEventArgs e)
    {
      e.UseDefaultCursors = false;
      if ((e.Effect & DragDropEffects.Link) == DragDropEffects.Link)
        Cursor.Current = linkCursor;
      else
        Cursor.Current = Cursors.Default;
    }

    private void grdQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      // Cancel the drag if the mouse moves off the form. The screenOffset takes into account any desktop bands that may be at the top or left side of the screen.
      if (((Control.MousePosition.X - screenOffset.X) < this.DesktopBounds.Left) ||
          ((Control.MousePosition.X - screenOffset.X) > this.DesktopBounds.Right) ||
          ((Control.MousePosition.Y - screenOffset.Y) < this.DesktopBounds.Top) ||
          ((Control.MousePosition.Y - screenOffset.Y) > this.DesktopBounds.Bottom))
      {
        e.Action = DragAction.Cancel;
      }
    }

    private void grdToMySQLTable_GridDragLeave(object sender, EventArgs e)
    {
      if (grdColumnIndexToDrag >- 1)
        performManualSingleColumnMapping(-1, grdColumnIndexToDrag, null);
    }

    private void grdToTable_GridDragDrop(object sender, DragEventArgs e)
    {
      // Ensure that the dragged item is contained in the data.
      if (e.Data.GetDataPresent(typeof(System.Int32)))
      {
        int fromColumnIndex = Convert.ToInt32(e.Data.GetData(typeof(System.Int32)));
        string draggedColumnName = grdFromExcelData.Columns[fromColumnIndex].HeaderText;
        if (e.Effect == DragDropEffects.Link && grdToTableColumnIndexToDrop >= 0)
        {
          if (!String.IsNullOrEmpty(exportTable.Columns[grdToTableColumnIndexToDrop].MappedDataColName))
          {
            bool isIdenticalMapping = exportTable.Columns[grdToTableColumnIndexToDrop].MappedDataColName == draggedColumnName;
            DialogResult dr = (isIdenticalMapping ? DialogResult.No : Utilities.ShowWarningBox(Properties.Resources.ColumnMappedOverwrite));
            if (dr == DialogResult.Yes)
              performManualSingleColumnMapping(-1, grdToTableColumnIndexToDrop, null);
            else
            {
              e.Effect = DragDropEffects.None;
              return;
            }
          }
          performManualSingleColumnMapping(fromColumnIndex, grdToTableColumnIndexToDrop, draggedColumnName);
        }
      }
      grdToTableColumnIndexToDrop = -1;
    }

    private void grdToTable_GridDragOver(object sender, DragEventArgs e)
    {
      // Determine whether string data exists in the drop data. If not, then the drop effect reflects that the drop cannot occur.
      if (!e.Data.GetDataPresent(typeof(System.String)))
      {
        e.Effect = DragDropEffects.None;
        grdToTableColumnIndexToDrop = -1;
        return;
      }
      e.Effect = DragDropEffects.Link;
      Point clientPoint = grdToMySQLTable.PointToClient(new Point(e.X, e.Y));
      DataGridView.HitTestInfo info = grdToMySQLTable.HitTest(clientPoint.X, clientPoint.Y);
      grdToTableColumnIndexToDrop = info.ColumnIndex;
    }

    private void btnStoreMapping_Click(object sender, EventArgs e)
    {
      int numericSuffix = 1;
      string proposedMappingName = String.Empty;
      do
      {
        proposedMappingName = String.Format("{0}Mapping{1}", exportDataHelper.ExportTable.Name, (numericSuffix > 1 ? numericSuffix.ToString() : String.Empty));
        numericSuffix++;
      }
      while (storedColumnMappingsList.Any(mapping => mapping.Name == proposedMappingName));
      AppendNewColumnMappingDialog newColumnMappingDialog = new AppendNewColumnMappingDialog(proposedMappingName);
      DialogResult dr = newColumnMappingDialog.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;
      currentColumnMapping.Name = newColumnMappingDialog.ColumnMappingName;
      storeColumnMappingInFile(currentColumnMapping);
    }

    private void btnAdvanced_Click(object sender, EventArgs e)
    {
      bool previousUseFormattedValue = Settings.Default.AppendUseFormattedValues;
      AppendAdvancedOptionsDialog optionsDialog = new AppendAdvancedOptionsDialog();
      DialogResult dr = optionsDialog.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;
      if (previousUseFormattedValue != Settings.Default.AppendUseFormattedValues)
        changeFormattedDataSource();
    }

    private void cmbMappingMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (cmbMappingMethod.Text)
      {
        case "Automatic":
          currentColumnMapping = createColumnMappingForAutomatic();
          if (!currentColumnMapping.AllColumnsMapped)
            cmbMappingMethod.SelectedIndex = 0;
          else
            applySelectedStoredColumnMapping();
          break;
        case "Manual":
          currentColumnMapping = createColumnMappingForManual();
          applySelectedStoredColumnMapping();
          break;
        default:
          currentColumnMapping.MatchWithOtherColumnMapping(storedColumnMappingsList[cmbMappingMethod.SelectedIndex - 2], false);
          applySelectedStoredColumnMapping();
          break;
      }
    }

  }

  public class MySQLColumnMapping
  {
    public string Name { get; set; }
    public string SchemaName { get; set; }
    public string TableName { get; set; }
    public string[] SourceColumns { get; set; }
    public string[] TargetColumns { get; set; }
    public int[] MappedSourceIndexes { get; set; }
    public int MappedQuantity
    {
      get { return MappedSourceIndexes.Count(idx => idx >= 0); }
    }
    public bool AllColumnsMapped
    {
      get { return MappedQuantity == MappedSourceIndexes.Length; }
    }

    public MySQLColumnMapping(string mappingName, string[] sourceColNames, string[] targetColNames)
    {
      Name = mappingName;
      SchemaName = String.Empty;
      TableName = String.Empty;

      if (sourceColNames != null)
      {
        SourceColumns = new string[sourceColNames.Length];
        sourceColNames.CopyTo(SourceColumns, 0);
      }
      if (targetColNames != null)
      {
        TargetColumns = new string[targetColNames.Length];
        targetColNames.CopyTo(TargetColumns, 0);
        MappedSourceIndexes = new int[targetColNames.Length];
      }

      ClearMappings();
    }

    public MySQLColumnMapping(string[] sourceColNames, string[] targetColNames) : this(String.Empty, sourceColNames, targetColNames)
    {
    }

    public void ClearMappings()
    {
      if (MappedSourceIndexes != null && TargetColumns != null)
        for (int i = 0; i < TargetColumns.Length; i++)
          MappedSourceIndexes[i] = -1;
    }

    public int GetMatchingColumnsQuantity(DataTable dataTable, bool sameOrdinals)
    {
      int matchingColumnsQty = 0;
      if (dataTable != null && TargetColumns != null)
      {
        for (int colIdx = 0; colIdx < TargetColumns.Length; colIdx++)
        {
          string colName = TargetColumns[colIdx];
          if (sameOrdinals)
          {
            if (dataTable.Columns[colIdx].ColumnName.ToLowerInvariant() == colName.ToLowerInvariant())
              matchingColumnsQty++;
          }
          else
          {
            if (dataTable.Columns.Contains(colName))
              matchingColumnsQty++;
          }
        }
      }
      return matchingColumnsQty;
    }

    public bool AllColumnsMatch(DataTable dataTable, bool sameOrdinals)
    {
      return (TargetColumns != null ? GetMatchingColumnsQuantity(dataTable, sameOrdinals) == TargetColumns.Length : false);
    }

    public int MatchWithOtherColumnMapping(MySQLColumnMapping otherColMapping, bool enforceSchemaAndTableEquality)
    {
      int columnsMatched = 0;

      if (enforceSchemaAndTableEquality && otherColMapping.SchemaName.ToLowerInvariant() != SchemaName.ToLowerInvariant() && otherColMapping.TableName.ToLowerInvariant() != TableName.ToLowerInvariant())
        return columnsMatched;

      ClearMappings();
      for (int thisTargetIdx = 0; thisTargetIdx < TargetColumns.Length; thisTargetIdx++)
      {
        string thisTargetColName = TargetColumns[thisTargetIdx].ToLowerInvariant();
        int foundAtOtherTargetIndex = -1;
        for (int otherTargetIdx = 0; otherTargetIdx < otherColMapping.TargetColumns.Length; otherTargetIdx++)
        {
          if (otherColMapping.TargetColumns[otherTargetIdx].ToLowerInvariant() == thisTargetColName)
          {
            foundAtOtherTargetIndex = otherTargetIdx;
            break;
          }
        }
        if (foundAtOtherTargetIndex >= 0)
        {
          MappedSourceIndexes[thisTargetIdx] = otherColMapping.MappedSourceIndexes[foundAtOtherTargetIndex];
          columnsMatched++;
        }
      }

      return columnsMatched;
    }
  }

}