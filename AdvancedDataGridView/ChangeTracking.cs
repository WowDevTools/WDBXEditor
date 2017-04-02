using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADGV
{
    partial class AdvancedDataGridView : DataGridView
    {
        public event EventHandler UndoRedoChanged;
        private const int BulkDeleteAmount = 25;

        public bool CanRedo => redoStack.Count > 0;
        public bool CanUndo => undoStack.Count > 0;

        private ConcurrentStack<ChangeSet> undoStack = new ConcurrentStack<ChangeSet>();
        private ConcurrentStack<ChangeSet> redoStack = new ConcurrentStack<ChangeSet>();
        private object current = new object();

        private Func<object, object, bool> Compare = (c, n) => (c?.ToString() ?? "").Equals(n?.ToString() ?? ""); //Object string compare
        private Func<DataGridViewRow, DataRow> ToDataRow = r => ((DataRowView)r.DataBoundItem).Row;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && SelectedRows.Count > BulkDeleteAmount)
            {
                switch (MessageBox.Show("You've selected to bulk delete which cannot be undone. Do you wish to continue?", "", MessageBoxButtons.YesNo))
                {
                    case DialogResult.Yes:
                        BulkDeleteSelected();
                        return;
                    case DialogResult.No:
                        return;
                }
            }

            base.OnKeyDown(e);
        }

        protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
        {
            base.OnCellBeginEdit(e);
            current = Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
        }

        protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
        {
            base.OnCellEndEdit(e);

            if (!Compare(current, this.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
            {
                if(this.Rows[e.RowIndex].IsNewRow) //Technically OnUserAddedRow but fires this method
                {
                    OnUserAddedRow(new DataGridViewRowEventArgs(this.Rows[e.RowIndex]));
                }
                else
                {
                    var row = (DataGridViewRow)Rows[e.RowIndex].Clone();
                    for (int i = 0; i < Columns.Count; i++)
                        row.Cells[i].Value = (i == e.ColumnIndex ? current : Rows[e.RowIndex].Cells[i].Value);

                    undoStack.Push(new ChangeSet(row, ChangeAction.Update)); //Store undo
                    redoStack.Clear(); //Clear redo
                    ChangeValue(ToDataRow(this.Rows[e.RowIndex]));
                }

                this.Invoke(UndoRedoChanged);
            }
        }

        protected override void OnUserAddedRow(DataGridViewRowEventArgs e)
        {
            base.OnUserAddedRow(e);

            var row = (DataGridViewRow)e.Row.Clone();
            var datarow = ((DataRowView)(((BindingSource)DataSource).Current)).Row;
            var data = datarow.ItemArray;
            for (int i = 0; i < data.Length; i++)
                row.Cells[i].Value = data[i];


            AddRow(datarow); //Cache change
            undoStack.Push(new ChangeSet(row, ChangeAction.Add, e.Row.Index));//Store change
            redoStack.Clear();//Clear redo

            //Commit
            ((BindingSource)DataSource).EndEdit();
            this.EndEdit();

            this.Invoke(UndoRedoChanged); //Tell the main form things have changed
        }

        public void OnUserAddedRow(DataGridViewRow e)
        {
            OnUserAddedRow(new DataGridViewRowEventArgs(e));
        }

        protected override void OnUserDeletingRow(DataGridViewRowCancelEventArgs e)
        {
            undoStack.Push(new ChangeSet(e.Row, ChangeAction.Delete)); //Store change
            redoStack.Clear(); //Clear redo

            RemoveRow(ToDataRow(e.Row)); //Cache change
            this.Invoke(UndoRedoChanged); //Tell the main form things have changed
            base.OnUserDeletingRow(e);
        }

        public void Undo()
        {
            if (undoStack.Count == 0)
            {
                this.Invoke(UndoRedoChanged);
                return;
            }

            ChangeSet redo;
            if (!undoStack.TryPop(out redo))
                return;

            int index = ((BindingSource)DataSource).Find(primarykey.ColumnName, redo.Row.Cells[primarykey.Ordinal].Value);

            switch (redo.Action)
            {
                case ChangeAction.Add:
                    if (index == -1) return;

                    RemoveRow(ToDataRow(Rows[index]));
                    Rows.Remove(Rows[index]);
                    break;

                case ChangeAction.Delete:
                    var source = ((DataTable)((BindingSource)DataSource).DataSource);
                    var newrow = source.NewRow();
                    for (int i = 0; i < Columns.Count; i++)
                        newrow[i] = redo.Row.Cells[i].Value;

                    ((DataTable)((BindingSource)DataSource).DataSource).Rows.InsertAt(newrow, redo.Index);
                    Task.Run(() => AddRow(newrow));
                    break;

                case ChangeAction.Update:
                    if (index == -1) return;

                    var values = redo.Row.Cells.Cast<DataGridViewCell>().Select(x => x.Value).ToArray();
                    for (int i = 0; i < values.Length; i++)
                    {
                        redo.Row.Cells[i].Value = Rows[index].Cells[i].Value;
                        Rows[index].Cells[i].Value = values[i];
                    }

                    ((BindingSource)DataSource).EndEdit();
                    this.EndEdit();

                    Task.Run(() => ChangeValue(ToDataRow(Rows[index])));
                    break;
            }

            //End edit
            ((BindingSource)DataSource).EndEdit();
            this.EndEdit();

            redoStack.Push(redo);
            this.Invoke(UndoRedoChanged);
        }

        public void Redo()
        {
            if (redoStack.Count == 0)
            {
                this.Invoke(UndoRedoChanged);
                return;
            }

            ChangeSet undo;
            if (!redoStack.TryPop(out undo))
                return;

            int index = ((BindingSource)DataSource).Find(primarykey.ColumnName, undo.Row.Cells[primarykey.Ordinal].Value);

            switch (undo.Action)
            {
                case ChangeAction.Add:
                    var source = ((DataTable)((BindingSource)DataSource).DataSource);
                    var newrow = source.NewRow();
                    for (int i = 0; i < Columns.Count; i++)
                        newrow[i] = undo.Row.Cells[i].Value;

                    ((DataTable)((BindingSource)DataSource).DataSource).Rows.InsertAt(newrow, undo.Index);
                    Task.Run(() => AddRow(newrow));
                    break;

                case ChangeAction.Delete:
                    if (index == -1) return;

                    RemoveRow(ToDataRow(Rows[index]));
                    Rows.Remove(Rows[index]);
                    break;

                case ChangeAction.Update:
                    if (index == -1) return;

                    var values = undo.Row.Cells.Cast<DataGridViewCell>().Select(x => x.Value).ToArray();
                    for (int i = 0; i < values.Length; i++)
                    {
                        undo.Row.Cells[i].Value = Rows[index].Cells[i].Value;
                        Rows[index].Cells[i].Value = values[i];
                    }

                    ((BindingSource)DataSource).EndEdit();
                    this.EndEdit();

                    Task.Run(() => ChangeValue(ToDataRow(Rows[index])));
                    break;
            }

            //End Edit
            ((BindingSource)DataSource).EndEdit();
            this.EndEdit();

            undoStack.Push(undo);
            this.Invoke(UndoRedoChanged);
        }

        public void ClearChanges()
        {
            redoStack.Clear();
            undoStack.Clear();
        }

        public void CacheData()
        {
            Task.Run(() => Init(true));
        }

        #region Internal Methods
        protected virtual void OnUndoRedoChanged(EventArgs e)
        {
            if (UndoRedoChanged != null)
                this.UndoRedoChanged(this, e);
        }

        private void BulkDeleteSelected()
        {
            var resetindex = SelectedRows[SelectedRows.Count - 1].Index - 1;
            var data = ((DataTable)((BindingSource)DataSource).DataSource);
            var rows = SelectedRows.Cast<DataGridViewRow>().Select(x => ToDataRow(x)).ToList();

            //Unbind from the source data
            ((BindingSource)DataSource).RaiseListChangedEvents = false;

            while (rows.Count > 0)
            {
                data.Rows.Remove(rows[0]); //Bulk delete
                rows.RemoveAt(0);
            }

            ((BindingSource)DataSource).RaiseListChangedEvents = true; //Rebind to source data
            ((BindingSource)DataSource).ResetBindings(false); //Force data reload

            Task.Run(() => Init(true));

            if (this.Rows.Count > 0)
                SelectRow(resetindex < 0 ? 0 : resetindex);
        }

        internal struct ChangeSet
        {
            public DataGridViewRow Row { get; set; }
            public ChangeAction Action { get; set; }
            public int Index { get; private set; }

            public ChangeSet(DataGridViewRow row, ChangeAction action, int index = -2)
            {
                this.Row = (DataGridViewRow)row.Clone();
                for (int i = 0; i < row.Cells.Count; i++)
                    this.Row.Cells[i].Value = row.Cells[i].Value;

                this.Index = (index == -2 ? row.Index : index);
                this.Action = action;
            }
        }

        internal enum ChangeAction
        {
            Add,
            Delete,
            Update
        }
        #endregion
    }
}
