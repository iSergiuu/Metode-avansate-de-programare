namespace TaskManager.UI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridViewTasks = new DataGridView();
            textBoxTitle = new TextBox();
            buttonAdd = new Button();
            buttonDelete = new Button();
            buttonRefresh = new Button();
            comboBoxStatus = new ComboBox();
            buttonComplete = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTasks).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewTasks
            // 
            dataGridViewTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewTasks.Location = new Point(39, 183);
            dataGridViewTasks.Name = "dataGridViewTasks";
            dataGridViewTasks.Size = new Size(691, 243);
            dataGridViewTasks.TabIndex = 0;
            // 
            // textBoxTitle
            // 
            textBoxTitle.Location = new Point(39, 59);
            textBoxTitle.Name = "textBoxTitle";
            textBoxTitle.Size = new Size(100, 23);
            textBoxTitle.TabIndex = 1;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(212, 66);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(75, 23);
            buttonAdd.TabIndex = 2;
            buttonAdd.Text = "Add";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(329, 66);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(75, 23);
            buttonDelete.TabIndex = 3;
            buttonDelete.Text = "Delete";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += buttonDelete_Click;
            // 
            // buttonRefresh
            // 
            buttonRefresh.Location = new Point(445, 66);
            buttonRefresh.Name = "buttonRefresh";
            buttonRefresh.Size = new Size(75, 23);
            buttonRefresh.TabIndex = 4;
            buttonRefresh.Text = "Refresh";
            buttonRefresh.UseVisualStyleBackColor = true;
            buttonRefresh.Click += buttonRefresh_Click;
            // 
            // comboBoxStatus
            // 
            comboBoxStatus.FormattingEnabled = true;
            comboBoxStatus.Location = new Point(39, 101);
            comboBoxStatus.Name = "comboBoxStatus";
            comboBoxStatus.Size = new Size(121, 23);
            comboBoxStatus.TabIndex = 5;
            comboBoxStatus.SelectedIndexChanged += comboBoxStatus_SelectedIndexChanged;
            // 
            // buttonComplete
            // 
            buttonComplete.Location = new Point(560, 66);
            buttonComplete.Name = "buttonComplete";
            buttonComplete.Size = new Size(75, 23);
            buttonComplete.TabIndex = 6;
            buttonComplete.Text = "Complete";
            buttonComplete.UseVisualStyleBackColor = true;
            buttonComplete.Click += buttonComplete_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonComplete);
            Controls.Add(comboBoxStatus);
            Controls.Add(buttonRefresh);
            Controls.Add(buttonDelete);
            Controls.Add(buttonAdd);
            Controls.Add(textBoxTitle);
            Controls.Add(dataGridViewTasks);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridViewTasks).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridViewTasks;
        private TextBox textBoxTitle;
        private Button buttonAdd;
        private Button buttonDelete;
        private Button buttonRefresh;
        private ComboBox comboBoxStatus;
        private Button buttonComplete;
    }
}
