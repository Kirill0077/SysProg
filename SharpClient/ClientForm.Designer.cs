namespace SharpClient
{
    partial class ClientForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientForm));
            this.ClientName_But = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Client_Text = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ClientName_But
            // 
            this.ClientName_But.BackColor = System.Drawing.Color.Lime;
            this.ClientName_But.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ClientName_But.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.ClientName_But.ForeColor = System.Drawing.Color.DarkViolet;
            this.ClientName_But.Location = new System.Drawing.Point(670, 320);
            this.ClientName_But.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ClientName_But.Name = "ClientName_But";
            this.ClientName_But.Size = new System.Drawing.Size(301, 81);
            this.ClientName_But.TabIndex = 0;
            this.ClientName_But.Text = "SEND";
            this.ClientName_But.UseVisualStyleBackColor = true;
            this.ClientName_But.Click += new System.EventHandler(this.ClientName_But_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.label1.Font = new System.Drawing.Font("Showcard Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.GreenYellow;
            this.label1.Location = new System.Drawing.Point(34, 78);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "ENTER NAME";
            // 
            // Client_Text
            // 
            this.Client_Text.Font = new System.Drawing.Font("Century Gothic", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Client_Text.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Client_Text.Location = new System.Drawing.Point(235, 78);
            this.Client_Text.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Client_Text.Multiline = true;
            this.Client_Text.Name = "Client_Text";
            this.Client_Text.Size = new System.Drawing.Size(427, 51);
            this.Client_Text.TabIndex = 2;
            this.Client_Text.Text = "name";
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Aquamarine;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1019, 466);
            this.Controls.Add(this.Client_Text);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ClientName_But);
            this.Font = new System.Drawing.Font("Showcard Gothic", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ClientForm";
            this.Text = "ClientForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ClientForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button ClientName_But;
        private Label label1;
        private TextBox Client_Text;
    }
}