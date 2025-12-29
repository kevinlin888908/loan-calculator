namespace loan
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.calendarControl = new System.Windows.Forms.MonthCalendar();
            this.groupBoxEventInput = new System.Windows.Forms.GroupBox();
            this.btnAddEvent = new System.Windows.Forms.Button();
            this.lblEventDateTime = new System.Windows.Forms.Label();
            this.dateTimePickerEvent = new System.Windows.Forms.DateTimePicker();
            this.lblEventDescription = new System.Windows.Forms.Label();
            this.txtEventDescription = new System.Windows.Forms.TextBox();
            this.lblEventTitle = new System.Windows.Forms.Label();
            this.txtEventTitle = new System.Windows.Forms.TextBox();
            this.groupBoxEventList = new System.Windows.Forms.GroupBox();
            this.btnImportFromFile = new System.Windows.Forms.Button();
            this.btnExportToFile = new System.Windows.Forms.Button();
            this.lblEventDetails = new System.Windows.Forms.Label();
            this.btnEditEvent = new System.Windows.Forms.Button();
            this.btnDeleteEvent = new System.Windows.Forms.Button();
            this.listBoxEvents = new System.Windows.Forms.ListBox();
            this.groupBoxEventInput.SuspendLayout();
            this.groupBoxEventList.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Lucida Sans Unicode", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblTitle.Location = new System.Drawing.Point(12, 35);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(220, 23);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "【行事曆】管理系統";
            // 
            // calendarControl
            // 
            this.calendarControl.Location = new System.Drawing.Point(12, 75);
            this.calendarControl.Name = "calendarControl";
            this.calendarControl.TabIndex = 1;
            this.calendarControl.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.CalendarControl_DateChanged);
            // 
            // groupBoxEventInput
            // 
            this.groupBoxEventInput.Controls.Add(this.btnAddEvent);
            this.groupBoxEventInput.Controls.Add(this.lblEventDateTime);
            this.groupBoxEventInput.Controls.Add(this.dateTimePickerEvent);
            this.groupBoxEventInput.Controls.Add(this.lblEventDescription);
            this.groupBoxEventInput.Controls.Add(this.txtEventDescription);
            this.groupBoxEventInput.Controls.Add(this.lblEventTitle);
            this.groupBoxEventInput.Controls.Add(this.txtEventTitle);
            this.groupBoxEventInput.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBoxEventInput.Location = new System.Drawing.Point(250, 75);
            this.groupBoxEventInput.Name = "groupBoxEventInput";
            this.groupBoxEventInput.Size = new System.Drawing.Size(320, 220);
            this.groupBoxEventInput.TabIndex = 2;
            this.groupBoxEventInput.TabStop = false;
            this.groupBoxEventInput.Text = "+ 新增事件";
            // 
            // txtEventTitle
            // 
            this.txtEventTitle.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtEventTitle.Location = new System.Drawing.Point(80, 30);
            this.txtEventTitle.Name = "txtEventTitle";
            this.txtEventTitle.Size = new System.Drawing.Size(220, 23);
            this.txtEventTitle.TabIndex = 0;
            // 
            // lblEventTitle
            // 
            this.lblEventTitle.AutoSize = true;
            this.lblEventTitle.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblEventTitle.Location = new System.Drawing.Point(15, 33);
            this.lblEventTitle.Name = "lblEventTitle";
            this.lblEventTitle.Size = new System.Drawing.Size(56, 15);
            this.lblEventTitle.TabIndex = 1;
            this.lblEventTitle.Text = "事件標題";
            // 
            // txtEventDescription
            // 
            this.txtEventDescription.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtEventDescription.Location = new System.Drawing.Point(80, 70);
            this.txtEventDescription.Multiline = true;
            this.txtEventDescription.Name = "txtEventDescription";
            this.txtEventDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEventDescription.Size = new System.Drawing.Size(220, 60);
            this.txtEventDescription.TabIndex = 2;
            // 
            // lblEventDescription
            // 
            this.lblEventDescription.AutoSize = true;
            this.lblEventDescription.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblEventDescription.Location = new System.Drawing.Point(15, 73);
            this.lblEventDescription.Name = "lblEventDescription";
            this.lblEventDescription.Size = new System.Drawing.Size(56, 15);
            this.lblEventDescription.TabIndex = 3;
            this.lblEventDescription.Text = "事件內容";
            // 
            // dateTimePickerEvent
            // 
            this.dateTimePickerEvent.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateTimePickerEvent.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dateTimePickerEvent.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEvent.Location = new System.Drawing.Point(80, 150);
            this.dateTimePickerEvent.Name = "dateTimePickerEvent";
            this.dateTimePickerEvent.ShowUpDown = true;
            this.dateTimePickerEvent.Size = new System.Drawing.Size(140, 23);
            this.dateTimePickerEvent.TabIndex = 4;
            // 
            // lblEventDateTime
            // 
            this.lblEventDateTime.AutoSize = true;
            this.lblEventDateTime.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblEventDateTime.Location = new System.Drawing.Point(15, 153);
            this.lblEventDateTime.Name = "lblEventDateTime";
            this.lblEventDateTime.Size = new System.Drawing.Size(56, 15);
            this.lblEventDateTime.TabIndex = 5;
            this.lblEventDateTime.Text = "事件時間";
            // 
            // btnAddEvent
            // 
            this.btnAddEvent.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAddEvent.Location = new System.Drawing.Point(225, 185);
            this.btnAddEvent.Name = "btnAddEvent";
            this.btnAddEvent.Size = new System.Drawing.Size(75, 25);
            this.btnAddEvent.TabIndex = 6;
            this.btnAddEvent.Text = "新增事件";
            this.btnAddEvent.UseVisualStyleBackColor = true;
            this.btnAddEvent.Click += new System.EventHandler(this.BtnAddEvent_Click);
            // 
            // groupBoxEventList
            // 
            this.groupBoxEventList.Controls.Add(this.btnImportFromFile);
            this.groupBoxEventList.Controls.Add(this.btnExportToFile);
            this.groupBoxEventList.Controls.Add(this.lblEventDetails);
            this.groupBoxEventList.Controls.Add(this.btnEditEvent);
            this.groupBoxEventList.Controls.Add(this.btnDeleteEvent);
            this.groupBoxEventList.Controls.Add(this.listBoxEvents);
            this.groupBoxEventList.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBoxEventList.Location = new System.Drawing.Point(12, 305);
            this.groupBoxEventList.Name = "groupBoxEventList";
            this.groupBoxEventList.Size = new System.Drawing.Size(558, 280);
            this.groupBoxEventList.TabIndex = 3;
            this.groupBoxEventList.TabStop = false;
            this.groupBoxEventList.Text = "■ 事件列表";
            // 
            // listBoxEvents
            // 
            this.listBoxEvents.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listBoxEvents.FormattingEnabled = true;
            this.listBoxEvents.ItemHeight = 15;
            this.listBoxEvents.Location = new System.Drawing.Point(15, 25);
            this.listBoxEvents.Name = "listBoxEvents";
            this.listBoxEvents.Size = new System.Drawing.Size(250, 169);
            this.listBoxEvents.TabIndex = 0;
            this.listBoxEvents.SelectedIndexChanged += new System.EventHandler(this.ListBoxEvents_SelectedIndexChanged);
            // 
            // btnDeleteEvent
            // 
            this.btnDeleteEvent.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDeleteEvent.Location = new System.Drawing.Point(15, 210);
            this.btnDeleteEvent.Name = "btnDeleteEvent";
            this.btnDeleteEvent.Size = new System.Drawing.Size(75, 25);
            this.btnDeleteEvent.TabIndex = 1;
            this.btnDeleteEvent.Text = "刪除事件";
            this.btnDeleteEvent.UseVisualStyleBackColor = true;
            this.btnDeleteEvent.Click += new System.EventHandler(this.BtnDeleteEvent_Click);
            // 
            // btnEditEvent
            // 
            this.btnEditEvent.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnEditEvent.Location = new System.Drawing.Point(110, 210);
            this.btnEditEvent.Name = "btnEditEvent";
            this.btnEditEvent.Size = new System.Drawing.Size(75, 25);
            this.btnEditEvent.TabIndex = 2;
            this.btnEditEvent.Text = "編輯事件";
            this.btnEditEvent.UseVisualStyleBackColor = true;
            this.btnEditEvent.Click += new System.EventHandler(this.BtnEditEvent_Click);
            // 
            // lblEventDetails
            // 
            this.lblEventDetails.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblEventDetails.Location = new System.Drawing.Point(280, 25);
            this.lblEventDetails.Name = "lblEventDetails";
            this.lblEventDetails.Size = new System.Drawing.Size(260, 169);
            this.lblEventDetails.TabIndex = 3;
            this.lblEventDetails.Text = "請選擇一個事件查看詳細資訊";
            // 
            // btnExportToFile
            // 
            this.btnExportToFile.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnExportToFile.Location = new System.Drawing.Point(15, 245);
            this.btnExportToFile.Name = "btnExportToFile";
            this.btnExportToFile.Size = new System.Drawing.Size(100, 25);
            this.btnExportToFile.TabIndex = 4;
            this.btnExportToFile.Text = "匯出到檔案";
            this.btnExportToFile.UseVisualStyleBackColor = true;
            this.btnExportToFile.Click += new System.EventHandler(this.BtnExportToFile_Click);
            // 
            // btnImportFromFile
            // 
            this.btnImportFromFile.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnImportFromFile.Location = new System.Drawing.Point(140, 245);
            this.btnImportFromFile.Name = "btnImportFromFile";
            this.btnImportFromFile.Size = new System.Drawing.Size(100, 25);
            this.btnImportFromFile.TabIndex = 5;
            this.btnImportFromFile.Text = "從檔案匯入";
            this.btnImportFromFile.UseVisualStyleBackColor = true;
            this.btnImportFromFile.Click += new System.EventHandler(this.BtnImportFromFile_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(584, 600);
            this.Controls.Add(this.groupBoxEventList);
            this.Controls.Add(this.groupBoxEventInput);
            this.Controls.Add(this.calendarControl);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "【行事曆】管理系統";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxEventInput.ResumeLayout(false);
            this.groupBoxEventInput.PerformLayout();
            this.groupBoxEventList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private MonthCalendar calendarControl;
        private GroupBox groupBoxEventInput;
        private TextBox txtEventTitle;
        private Label lblEventTitle;
        private TextBox txtEventDescription;
        private Label lblEventDescription;
        private DateTimePicker dateTimePickerEvent;
        private Label lblEventDateTime;
        private Button btnAddEvent;
        private GroupBox groupBoxEventList;
        private ListBox listBoxEvents;
        private Button btnDeleteEvent;
        private Button btnEditEvent;
        private Label lblEventDetails;
        private Button btnExportToFile;
        private Button btnImportFromFile;
    }
}