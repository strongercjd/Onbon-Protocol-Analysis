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
            this.label1 = new System.Windows.Forms.Label();
            this.data_after_transform_richTextBox = new System.Windows.Forms.RichTextBox();
            this.data_listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.analysis_button = new System.Windows.Forms.Button();
            this.Protocol_selection = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.out_excel_button = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.联系开发者ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Raw_data_textBox = new System.Windows.Forms.RichTextBox();
            this.region_preview_button = new System.Windows.Forms.Button();
            this.LED_type_comboBox = new System.Windows.Forms.ComboBox();
            this.data_preview_comboBox = new System.Windows.Forms.ComboBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 354);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "除去帧头帧尾，转义后的数据";
            // 
            // data_after_transform_richTextBox
            // 
            this.data_after_transform_richTextBox.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.data_after_transform_richTextBox.ForeColor = System.Drawing.Color.Red;
            this.data_after_transform_richTextBox.Location = new System.Drawing.Point(12, 369);
            this.data_after_transform_richTextBox.Name = "data_after_transform_richTextBox";
            this.data_after_transform_richTextBox.Size = new System.Drawing.Size(475, 303);
            this.data_after_transform_richTextBox.TabIndex = 3;
            this.data_after_transform_richTextBox.Text = "\n\n\n\n      仰邦协议解析工具（C）2019";
            // 
            // data_listView
            // 
            this.data_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.data_listView.FullRowSelect = true;
            this.data_listView.HideSelection = false;
            this.data_listView.Location = new System.Drawing.Point(493, 28);
            this.data_listView.Name = "data_listView";
            this.data_listView.Size = new System.Drawing.Size(680, 644);
            this.data_listView.TabIndex = 4;
            this.data_listView.UseCompatibleStateImageBehavior = false;
            this.data_listView.View = System.Windows.Forms.View.Details;
            this.data_listView.SelectedIndexChanged += new System.EventHandler(this.data_listView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "参数";
            this.columnHeader1.Width = 138;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "数据";
            this.columnHeader2.Width = 114;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "描述";
            this.columnHeader3.Width = 397;
            // 
            // analysis_button
            // 
            this.analysis_button.Location = new System.Drawing.Point(305, 43);
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
            this.Protocol_selection.Location = new System.Drawing.Point(12, 60);
            this.Protocol_selection.Name = "Protocol_selection";
            this.Protocol_selection.Size = new System.Drawing.Size(133, 20);
            this.Protocol_selection.TabIndex = 6;
            this.Protocol_selection.SelectedIndexChanged += new System.EventHandler(this.Protocol_selection_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "协议选择";
            // 
            // out_excel_button
            // 
            this.out_excel_button.Enabled = false;
            this.out_excel_button.Location = new System.Drawing.Point(399, 42);
            this.out_excel_button.Name = "out_excel_button";
            this.out_excel_button.Size = new System.Drawing.Size(78, 38);
            this.out_excel_button.TabIndex = 8;
            this.out_excel_button.Text = "输出Excel";
            this.out_excel_button.UseVisualStyleBackColor = true;
            this.out_excel_button.Click += new System.EventHandler(this.out_excel_button_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.帮助ToolStripMenuItem,
            this.联系开发者ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1301, 25);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.帮助ToolStripMenuItem.Text = "帮助";
            this.帮助ToolStripMenuItem.Click += new System.EventHandler(this.帮助ToolStripMenuItem_Click);
            // 
            // 联系开发者ToolStripMenuItem
            // 
            this.联系开发者ToolStripMenuItem.Name = "联系开发者ToolStripMenuItem";
            this.联系开发者ToolStripMenuItem.Size = new System.Drawing.Size(80, 21);
            this.联系开发者ToolStripMenuItem.Text = "联系开发者";
            this.联系开发者ToolStripMenuItem.Click += new System.EventHandler(this.联系开发者ToolStripMenuItem_Click);
            // 
            // Raw_data_textBox
            // 
            this.Raw_data_textBox.Location = new System.Drawing.Point(14, 87);
            this.Raw_data_textBox.Name = "Raw_data_textBox";
            this.Raw_data_textBox.Size = new System.Drawing.Size(473, 264);
            this.Raw_data_textBox.TabIndex = 10;
            this.Raw_data_textBox.Text = resources.GetString("Raw_data_textBox.Text");
            // 
            // region_preview_button
            // 
            this.region_preview_button.Enabled = false;
            this.region_preview_button.Location = new System.Drawing.Point(1194, 78);
            this.region_preview_button.Name = "region_preview_button";
            this.region_preview_button.Size = new System.Drawing.Size(85, 37);
            this.region_preview_button.TabIndex = 11;
            this.region_preview_button.Text = "区域位置预览";
            this.region_preview_button.UseVisualStyleBackColor = true;
            this.region_preview_button.Click += new System.EventHandler(this.region_preview_button_Click);
            // 
            // LED_type_comboBox
            // 
            this.LED_type_comboBox.FormattingEnabled = true;
            this.LED_type_comboBox.Items.AddRange(new object[] {
            "单色",
            "双色",
            "三色",
            "全彩"});
            this.LED_type_comboBox.Location = new System.Drawing.Point(1194, 34);
            this.LED_type_comboBox.Name = "LED_type_comboBox";
            this.LED_type_comboBox.Size = new System.Drawing.Size(85, 20);
            this.LED_type_comboBox.TabIndex = 13;
            // 
            // data_preview_comboBox
            // 
            this.data_preview_comboBox.FormattingEnabled = true;
            this.data_preview_comboBox.Items.AddRange(new object[] {
            "不预览",
            "ID：0",
            "ID：1",
            "ID：2",
            "ID：3",
            "ID：4"});
            this.data_preview_comboBox.Location = new System.Drawing.Point(1194, 145);
            this.data_preview_comboBox.Name = "data_preview_comboBox";
            this.data_preview_comboBox.Size = new System.Drawing.Size(85, 20);
            this.data_preview_comboBox.TabIndex = 14;
            // 
            // Onbon_Protocol_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 684);
            this.Controls.Add(this.data_preview_comboBox);
            this.Controls.Add(this.LED_type_comboBox);
            this.Controls.Add(this.region_preview_button);
            this.Controls.Add(this.Raw_data_textBox);
            this.Controls.Add(this.out_excel_button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Protocol_selection);
            this.Controls.Add(this.analysis_button);
            this.Controls.Add(this.data_listView);
            this.Controls.Add(this.data_after_transform_richTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Onbon_Protocol_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "onbon协议解析 V0.1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox data_after_transform_richTextBox;
        private System.Windows.Forms.ListView data_listView;
        private System.Windows.Forms.Button analysis_button;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ComboBox Protocol_selection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button out_excel_button;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 联系开发者ToolStripMenuItem;
        private System.Windows.Forms.RichTextBox Raw_data_textBox;
        private System.Windows.Forms.Button region_preview_button;
        private System.Windows.Forms.ComboBox LED_type_comboBox;
        private System.Windows.Forms.ComboBox data_preview_comboBox;
    }
}

