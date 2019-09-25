namespace Onbon_Protocol_analysis
{
    partial class Onbon_Protocol_Form
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Onbon_Protocol_Form));
            this.Raw_data_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.data_after_transform_richTextBox = new System.Windows.Forms.RichTextBox();
            this.data_listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.analysis_button = new System.Windows.Forms.Button();
            this.Protocol_selection = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Raw_data_textBox
            // 
            this.Raw_data_textBox.Location = new System.Drawing.Point(24, 52);
            this.Raw_data_textBox.Multiline = true;
            this.Raw_data_textBox.Name = "Raw_data_textBox";
            this.Raw_data_textBox.Size = new System.Drawing.Size(475, 285);
            this.Raw_data_textBox.TabIndex = 0;
            this.Raw_data_textBox.Text = resources.GetString("Raw_data_textBox.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 340);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "除去帧头帧尾，转义后的数据";
            // 
            // data_after_transform_richTextBox
            // 
            this.data_after_transform_richTextBox.Location = new System.Drawing.Point(24, 355);
            this.data_after_transform_richTextBox.Name = "data_after_transform_richTextBox";
            this.data_after_transform_richTextBox.Size = new System.Drawing.Size(475, 278);
            this.data_after_transform_richTextBox.TabIndex = 3;
            this.data_after_transform_richTextBox.Text = "";
            // 
            // data_listView
            // 
            this.data_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.data_listView.FullRowSelect = true;
            this.data_listView.HideSelection = false;
            this.data_listView.Location = new System.Drawing.Point(518, 13);
            this.data_listView.Name = "data_listView";
            this.data_listView.Size = new System.Drawing.Size(627, 620);
            this.data_listView.TabIndex = 4;
            this.data_listView.UseCompatibleStateImageBehavior = false;
            this.data_listView.View = System.Windows.Forms.View.Details;
            this.data_listView.SelectedIndexChanged += new System.EventHandler(this.data_listView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "参数";
            this.columnHeader1.Width = 145;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "数据";
            this.columnHeader2.Width = 114;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "描述";
            this.columnHeader3.Width = 321;
            // 
            // analysis_button
            // 
            this.analysis_button.Location = new System.Drawing.Point(421, 8);
            this.analysis_button.Name = "analysis_button";
            this.analysis_button.Size = new System.Drawing.Size(78, 38);
            this.analysis_button.TabIndex = 5;
            this.analysis_button.Text = "开始解析";
            this.analysis_button.UseVisualStyleBackColor = true;
            this.analysis_button.Click += new System.EventHandler(this.analysis_button_Click);
            // 
            // Protocol_selection
            // 
            this.Protocol_selection.FormattingEnabled = true;
            this.Protocol_selection.Location = new System.Drawing.Point(24, 26);
            this.Protocol_selection.Name = "Protocol_selection";
            this.Protocol_selection.Size = new System.Drawing.Size(133, 20);
            this.Protocol_selection.TabIndex = 6;
            this.Protocol_selection.SelectedIndexChanged += new System.EventHandler(this.Protocol_selection_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "协议选择";
            // 
            // Onbon_Protocol_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1157, 645);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Protocol_selection);
            this.Controls.Add(this.analysis_button);
            this.Controls.Add(this.data_listView);
            this.Controls.Add(this.data_after_transform_richTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Raw_data_textBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Onbon_Protocol_Form";
            this.Text = "onbon协议解析";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Raw_data_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox data_after_transform_richTextBox;
        private System.Windows.Forms.ListView data_listView;
        private System.Windows.Forms.Button analysis_button;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ComboBox Protocol_selection;
        private System.Windows.Forms.Label label2;
    }
}

