namespace UmaPoyofeatChatGPT2
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
            dateTimePicker1 = new DateTimePicker();
            btnSearchRaceInfo = new Button();
            lblRaceInfo = new Label();
            lblCondition = new Label();
            dataGridView1 = new DataGridView();
            listBox1 = new ListBox();
            richTextBoxHorseInfo = new RichTextBox();
            richTextBoxTweetMessage = new RichTextBox();
            btnPredict = new Button();
            btnPutnote = new Button();
            btnLINE = new Button();
            btnX = new Button();
            btnOrePuro = new Button();
            hiddenDateLabel = new Label();
            btnUpdateRaces = new Button();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(18, 18);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(138, 23);
            dateTimePicker1.TabIndex = 0;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            // 
            // btnSearchRaceInfo
            // 
            btnSearchRaceInfo.Location = new Point(175, 18);
            btnSearchRaceInfo.Name = "btnSearchRaceInfo";
            btnSearchRaceInfo.Size = new Size(123, 23);
            btnSearchRaceInfo.TabIndex = 1;
            btnSearchRaceInfo.Text = "レース情報取得";
            btnSearchRaceInfo.UseVisualStyleBackColor = true;
            btnSearchRaceInfo.Click += btnSearchRaceInfo_Click;
            // 
            // lblRaceInfo
            // 
            lblRaceInfo.AutoSize = true;
            lblRaceInfo.ForeColor = Color.White;
            lblRaceInfo.Location = new Point(311, 9);
            lblRaceInfo.Name = "lblRaceInfo";
            lblRaceInfo.Size = new Size(57, 15);
            lblRaceInfo.TabIndex = 2;
            lblRaceInfo.Text = "レース情報";
            // 
            // lblCondition
            // 
            lblCondition.AutoSize = true;
            lblCondition.ForeColor = Color.White;
            lblCondition.Location = new Point(311, 26);
            lblCondition.Name = "lblCondition";
            lblCondition.Size = new Size(66, 15);
            lblCondition.TabIndex = 3;
            lblCondition.Text = "コンディション";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(114, 56);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1235, 326);
            dataGridView1.TabIndex = 4;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(18, 56);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(90, 649);
            listBox1.TabIndex = 5;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // richTextBoxHorseInfo
            // 
            richTextBoxHorseInfo.BackColor = Color.FromArgb(238, 128, 20);
            richTextBoxHorseInfo.Location = new Point(114, 392);
            richTextBoxHorseInfo.Name = "richTextBoxHorseInfo";
            richTextBoxHorseInfo.Size = new Size(797, 311);
            richTextBoxHorseInfo.TabIndex = 6;
            richTextBoxHorseInfo.Text = "";
            // 
            // richTextBoxTweetMessage
            // 
            richTextBoxTweetMessage.BackColor = Color.FromArgb(238, 128, 20);
            richTextBoxTweetMessage.Location = new Point(917, 393);
            richTextBoxTweetMessage.Name = "richTextBoxTweetMessage";
            richTextBoxTweetMessage.Size = new Size(432, 310);
            richTextBoxTweetMessage.TabIndex = 7;
            richTextBoxTweetMessage.Text = "";
            // 
            // btnPredict
            // 
            btnPredict.Location = new Point(1274, 719);
            btnPredict.Name = "btnPredict";
            btnPredict.Size = new Size(75, 23);
            btnPredict.TabIndex = 8;
            btnPredict.Text = "予想";
            btnPredict.UseVisualStyleBackColor = true;
            btnPredict.Click += btnPredict_Click;
            // 
            // btnPutnote
            // 
            btnPutnote.Location = new Point(1193, 719);
            btnPutnote.Name = "btnPutnote";
            btnPutnote.Size = new Size(75, 23);
            btnPutnote.TabIndex = 9;
            btnPutnote.Text = "note更新";
            btnPutnote.UseVisualStyleBackColor = true;
            btnPutnote.Click += btnPutnote_Click;
            // 
            // btnLINE
            // 
            btnLINE.Location = new Point(1112, 719);
            btnLINE.Name = "btnLINE";
            btnLINE.Size = new Size(75, 23);
            btnLINE.TabIndex = 10;
            btnLINE.Text = "LINE";
            btnLINE.UseVisualStyleBackColor = true;
            btnLINE.Click += btnLINE_Click;
            // 
            // btnX
            // 
            btnX.Location = new Point(1031, 719);
            btnX.Name = "btnX";
            btnX.Size = new Size(75, 23);
            btnX.TabIndex = 11;
            btnX.Text = "予想ツイート";
            btnX.UseVisualStyleBackColor = true;
            // 
            // btnOrePuro
            // 
            btnOrePuro.Location = new Point(950, 719);
            btnOrePuro.Name = "btnOrePuro";
            btnOrePuro.Size = new Size(75, 23);
            btnOrePuro.TabIndex = 12;
            btnOrePuro.Text = "俺プロ登録";
            btnOrePuro.UseVisualStyleBackColor = true;
            // 
            // hiddenDateLabel
            // 
            hiddenDateLabel.AutoSize = true;
            hiddenDateLabel.ForeColor = Color.FromArgb(29, 64, 96);
            hiddenDateLabel.Location = new Point(1311, 9);
            hiddenDateLabel.Name = "hiddenDateLabel";
            hiddenDateLabel.Size = new Size(0, 15);
            hiddenDateLabel.TabIndex = 13;
            // 
            // btnUpdateRaces
            // 
            btnUpdateRaces.Location = new Point(18, 719);
            btnUpdateRaces.Name = "btnUpdateRaces";
            btnUpdateRaces.Size = new Size(90, 23);
            btnUpdateRaces.TabIndex = 14;
            btnUpdateRaces.Text = "レース登録";
            btnUpdateRaces.UseVisualStyleBackColor = true;
            btnUpdateRaces.Click += btnUpdateRaces_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 754);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1361, 22);
            statusStrip1.TabIndex = 15;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.BackColor = Color.Transparent;
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(118, 17);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(29, 64, 96);
            ClientSize = new Size(1361, 776);
            Controls.Add(statusStrip1);
            Controls.Add(btnUpdateRaces);
            Controls.Add(hiddenDateLabel);
            Controls.Add(btnOrePuro);
            Controls.Add(btnX);
            Controls.Add(btnLINE);
            Controls.Add(btnPutnote);
            Controls.Add(btnPredict);
            Controls.Add(richTextBoxTweetMessage);
            Controls.Add(richTextBoxHorseInfo);
            Controls.Add(listBox1);
            Controls.Add(dataGridView1);
            Controls.Add(lblCondition);
            Controls.Add(lblRaceInfo);
            Controls.Add(btnSearchRaceInfo);
            Controls.Add(dateTimePicker1);
            Name = "Form1";
            Text = "UMA-03 Mark.06";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker dateTimePicker1;
        private Button btnSearchRaceInfo;
        private Label lblRaceInfo;
        private Label lblCondition;
        private DataGridView dataGridView1;
        private ListBox listBox1;
        private RichTextBox richTextBoxHorseInfo;
        private RichTextBox richTextBoxTweetMessage;
        private Button btnPredict;
        private Button btnPutnote;
        private Button btnLINE;
        private Button btnX;
        private Button btnOrePuro;
        private Label hiddenDateLabel;
        private Button btnUpdateRaces;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
    }
}
