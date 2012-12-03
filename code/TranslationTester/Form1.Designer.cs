namespace TranslationTester
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.input = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.converted = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.translated = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.providers = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.srcLanguage = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "TranslationProvider";
            // 
            // input
            // 
            this.input.Location = new System.Drawing.Point(181, 98);
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(740, 125);
            this.input.TabIndex = 1;
            this.input.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Input (with ctc)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 243);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Converted";
            // 
            // converted
            // 
            this.converted.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.converted.Location = new System.Drawing.Point(181, 243);
            this.converted.Name = "converted";
            this.converted.ReadOnly = true;
            this.converted.Size = new System.Drawing.Size(740, 115);
            this.converted.TabIndex = 5;
            this.converted.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(64, 387);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Translated";
            // 
            // translated
            // 
            this.translated.Location = new System.Drawing.Point(181, 387);
            this.translated.Name = "translated";
            this.translated.ReadOnly = true;
            this.translated.Size = new System.Drawing.Size(740, 116);
            this.translated.TabIndex = 7;
            this.translated.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(743, 530);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Convert";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(846, 530);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Translate";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // providers
            // 
            this.providers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.providers.FormattingEnabled = true;
            this.providers.Items.AddRange(new object[] {
            "google",
            "bing"});
            this.providers.Location = new System.Drawing.Point(181, 29);
            this.providers.Name = "providers";
            this.providers.Size = new System.Drawing.Size(121, 21);
            this.providers.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(329, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Source Language";
            // 
            // srcLanguage
            // 
            this.srcLanguage.FormattingEnabled = true;
            this.srcLanguage.Location = new System.Drawing.Point(445, 29);
            this.srcLanguage.Name = "srcLanguage";
            this.srcLanguage.Size = new System.Drawing.Size(121, 21);
            this.srcLanguage.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(617, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Always translated to en";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 570);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.srcLanguage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.providers);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.translated);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.converted);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.input);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox input;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox converted;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox translated;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox providers;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox srcLanguage;
        private System.Windows.Forms.Label label6;

    }
}

