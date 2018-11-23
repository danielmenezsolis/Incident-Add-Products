namespace Incident_Add_Products
{
    partial class ControlCombo
    {
        /// <summar
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnAdd = new System.Windows.Forms.Button();
            this.CboProductos = new System.Windows.Forms.ComboBox();
            this.LblProductos = new System.Windows.Forms.Label();
            this.dataGridServicios = new System.Windows.Forms.DataGridView();
            this.txtSearch = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridServicios)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(554, 17);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // CboProductos
            // 
            this.CboProductos.FormattingEnabled = true;
            this.CboProductos.Location = new System.Drawing.Point(3, 19);
            this.CboProductos.Name = "CboProductos";
            this.CboProductos.Size = new System.Drawing.Size(534, 21);
            this.CboProductos.TabIndex = 1;
            this.CboProductos.Visible = false;
            this.CboProductos.SelectedIndexChanged += new System.EventHandler(this.CboProductos_SelectedIndexChanged);
            // 
            // LblProductos
            // 
            this.LblProductos.AutoSize = true;
            this.LblProductos.Location = new System.Drawing.Point(3, 3);
            this.LblProductos.Name = "LblProductos";
            this.LblProductos.Size = new System.Drawing.Size(82, 13);
            this.LblProductos.TabIndex = 2;
            this.LblProductos.Text = "Choose Service";
            // 
            // dataGridServicios
            // 
            this.dataGridServicios.AllowUserToAddRows = false;
            this.dataGridServicios.AllowUserToDeleteRows = false;
            this.dataGridServicios.AllowUserToOrderColumns = true;
            this.dataGridServicios.AllowUserToResizeColumns = false;
            this.dataGridServicios.AllowUserToResizeRows = false;
            this.dataGridServicios.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridServicios.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlDarkDark;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridServicios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridServicios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlDarkDark;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridServicios.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridServicios.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridServicios.Location = new System.Drawing.Point(3, 45);
            this.dataGridServicios.MultiSelect = false;
            this.dataGridServicios.Name = "dataGridServicios";
            this.dataGridServicios.RowHeadersVisible = false;
            this.dataGridServicios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridServicios.Size = new System.Drawing.Size(626, 104);
            this.dataGridServicios.TabIndex = 5;
            this.dataGridServicios.TabStop = false;
            
            this.dataGridServicios.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridServicios_CellDoubleClick);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(3, 19);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(198, 20);
            this.txtSearch.TabIndex = 6;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // ControlCombo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.dataGridServicios);
            this.Controls.Add(this.LblProductos);
            this.Controls.Add(this.CboProductos);
            this.Controls.Add(this.btnAdd);
            this.Name = "ControlCombo";
            this.Size = new System.Drawing.Size(636, 154);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridServicios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ComboBox CboProductos;
        private System.Windows.Forms.Label LblProductos;
        private System.Windows.Forms.DataGridView dataGridServicios;
        private System.Windows.Forms.TextBox txtSearch;
    }
}
