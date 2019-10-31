namespace Onbon_Protocol_analysis
{
    partial class Data_Preview
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Data_Preview));
            this.data_pictureBox = new System.Windows.Forms.PictureBox();
            this.Save_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.data_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // data_pictureBox
            // 
            this.data_pictureBox.Location = new System.Drawing.Point(95, 59);
            this.data_pictureBox.Name = "data_pictureBox";
            this.data_pictureBox.Size = new System.Drawing.Size(456, 305);
            this.data_pictureBox.TabIndex = 0;
            this.data_pictureBox.TabStop = false;
            // 
            // Save_button
            // 
            this.Save_button.Location = new System.Drawing.Point(95, 12);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(77, 31);
            this.Save_button.TabIndex = 1;
            this.Save_button.Text = "保存图片";
            this.Save_button.UseVisualStyleBackColor = true;
            this.Save_button.Click += new System.EventHandler(this.Save_button_Click);
            // 
            // Data_Preview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.data_pictureBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Data_Preview";
            this.Text = "显示数据预览";
            ((System.ComponentModel.ISupportInitialize)(this.data_pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox data_pictureBox;
        private System.Windows.Forms.Button Save_button;
    }
}