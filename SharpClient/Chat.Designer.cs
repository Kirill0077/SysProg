namespace SharpClient
{
    partial class Chat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chat));
            this.Message_List = new System.Windows.Forms.ListBox();
            this.Clients_List = new System.Windows.Forms.ListBox();
            this.Messeages = new System.Windows.Forms.TextBox();
            this.Send_But = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Message_List
            // 
            this.Message_List.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Message_List.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Message_List.ForeColor = System.Drawing.Color.Black;
            this.Message_List.FormattingEnabled = true;
            this.Message_List.ItemHeight = 21;
            this.Message_List.Location = new System.Drawing.Point(12, 27);
            this.Message_List.Name = "Message_List";
            this.Message_List.Size = new System.Drawing.Size(537, 319);
            this.Message_List.TabIndex = 0;
            // 
            // Clients_List
            // 
            this.Clients_List.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Clients_List.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Clients_List.ForeColor = System.Drawing.Color.Black;
            this.Clients_List.FormattingEnabled = true;
            this.Clients_List.ItemHeight = 21;
            this.Clients_List.Location = new System.Drawing.Point(999, 27);
            this.Clients_List.Name = "Clients_List";
            this.Clients_List.Size = new System.Drawing.Size(270, 319);
            this.Clients_List.TabIndex = 1;
            // 
            // Messeages
            // 
            this.Messeages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Messeages.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Messeages.ForeColor = System.Drawing.Color.Black;
            this.Messeages.Location = new System.Drawing.Point(12, 385);
            this.Messeages.Multiline = true;
            this.Messeages.Name = "Messeages";
            this.Messeages.Size = new System.Drawing.Size(537, 110);
            this.Messeages.TabIndex = 2;
            // 
            // Send_But
            // 
            this.Send_But.BackColor = System.Drawing.Color.Crimson;
            this.Send_But.Font = new System.Drawing.Font("Century Gothic", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Send_But.ForeColor = System.Drawing.Color.Gold;
            this.Send_But.Location = new System.Drawing.Point(612, 394);
            this.Send_But.Name = "Send_But";
            this.Send_But.Size = new System.Drawing.Size(283, 92);
            this.Send_But.TabIndex = 3;
            this.Send_But.Text = "SEND MESSAGE";
            this.Send_But.UseVisualStyleBackColor = false;
            this.Send_But.Click += new System.EventHandler(this.Send_But_Click);
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1295, 573);
            this.Controls.Add(this.Send_But);
            this.Controls.Add(this.Messeages);
            this.Controls.Add(this.Clients_List);
            this.Controls.Add(this.Message_List);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Chat";
            this.Text = "CHAT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing_1);
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox Message_List;
        private ListBox Clients_List;
        private TextBox Messeages;
        private Button Send_But;
    }
}