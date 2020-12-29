namespace lab6
{
    partial class Form2
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.text_box_HL = new System.Windows.Forms.TextBox();
            this.text_box_HR = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_R = new System.Windows.Forms.TextBox();
            this.button_start = new System.Windows.Forms.Button();
            this.clear_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBox1.Location = new System.Drawing.Point(178, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(822, 555);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "HL";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(123, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "HR";
            // 
            // text_box_HL
            // 
            this.text_box_HL.Location = new System.Drawing.Point(12, 107);
            this.text_box_HL.Name = "text_box_HL";
            this.text_box_HL.Size = new System.Drawing.Size(72, 22);
            this.text_box_HL.TabIndex = 11;
            this.text_box_HL.Text = "10";
            this.text_box_HL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // text_box_HR
            // 
            this.text_box_HR.Location = new System.Drawing.Point(100, 107);
            this.text_box_HR.Name = "text_box_HR";
            this.text_box_HR.Size = new System.Drawing.Size(72, 22);
            this.text_box_HR.TabIndex = 12;
            this.text_box_HR.Text = "10";
            this.text_box_HR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(24, 331);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "Шероховатость";
            // 
            // textBox_R
            // 
            this.textBox_R.Location = new System.Drawing.Point(28, 377);
            this.textBox_R.Name = "textBox_R";
            this.textBox_R.Size = new System.Drawing.Size(123, 22);
            this.textBox_R.TabIndex = 14;
            this.textBox_R.Text = "1";
            this.textBox_R.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_start
            // 
            this.button_start.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button_start.Location = new System.Drawing.Point(35, 429);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(103, 38);
            this.button_start.TabIndex = 15;
            this.button_start.Text = "Построить";
            this.button_start.UseVisualStyleBackColor = false;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // clear_button
            // 
            this.clear_button.Location = new System.Drawing.Point(35, 488);
            this.clear_button.Name = "clear_button";
            this.clear_button.Size = new System.Drawing.Size(103, 29);
            this.clear_button.TabIndex = 16;
            this.clear_button.Text = "Очистить ";
            this.clear_button.UseVisualStyleBackColor = true;
            this.clear_button.Click += new System.EventHandler(this.clear_button_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 618);
            this.Controls.Add(this.clear_button);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.textBox_R);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.text_box_HR);
            this.Controls.Add(this.text_box_HL);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox text_box_HL;
        private System.Windows.Forms.TextBox text_box_HR;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_R;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button clear_button;
    }
}