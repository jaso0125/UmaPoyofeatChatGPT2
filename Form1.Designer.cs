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
            Wakuban = new DataGridViewTextBoxColumn();
            Umaban = new DataGridViewTextBoxColumn();
            Bamei = new DataGridViewTextBoxColumn();
            SexBarei = new DataGridViewTextBoxColumn();
            Kinryo = new DataGridViewTextBoxColumn();
            KisyuName = new DataGridViewTextBoxColumn();
            Bataijyu = new DataGridViewTextBoxColumn();
            Mark = new DataGridViewTextBoxColumn();
            Time = new DataGridViewTextBoxColumn();
            Comment = new DataGridViewTextBoxColumn();
            listBox1 = new ListBox();
            richTextBoxHorseInfo = new RichTextBox();
            richTextBoxTweetMessage = new RichTextBox();
            btnPredict = new Button();
            btnPutnote = new Button();
            btnLINE = new Button();
            btnX = new Button();
            btnOrePuro = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(18, 18);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(138, 23);
            dateTimePicker1.TabIndex = 0;
            // 
            // btnSearchRaceInfo
            // 
            btnSearchRaceInfo.Location = new Point(175, 18);
            btnSearchRaceInfo.Name = "btnSearchRaceInfo";
            btnSearchRaceInfo.Size = new Size(123, 23);
            btnSearchRaceInfo.TabIndex = 1;
            btnSearchRaceInfo.Text = "レース情報取得";
            btnSearchRaceInfo.UseVisualStyleBackColor = true;
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
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Wakuban, Umaban, Bamei, SexBarei, Kinryo, KisyuName, Bataijyu, Mark, Time, Comment });
            dataGridView1.Location = new Point(114, 56);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1235, 326);
            dataGridView1.TabIndex = 4;
            // 
            // Wakuban
            // 
            Wakuban.HeaderText = "枠";
            Wakuban.Name = "Wakuban";
            Wakuban.Width = 50;
            // 
            // Umaban
            // 
            Umaban.HeaderText = "馬番";
            Umaban.Name = "Umaban";
            Umaban.Width = 60;
            // 
            // Bamei
            // 
            Bamei.HeaderText = "馬名";
            Bamei.Name = "Bamei";
            Bamei.Width = 200;
            // 
            // SexBarei
            // 
            SexBarei.HeaderText = "性齢";
            SexBarei.Name = "SexBarei";
            SexBarei.Width = 70;
            // 
            // Kinryo
            // 
            Kinryo.HeaderText = "斤量";
            Kinryo.Name = "Kinryo";
            Kinryo.Width = 60;
            // 
            // KisyuName
            // 
            KisyuName.HeaderText = "騎手名";
            KisyuName.Name = "KisyuName";
            // 
            // Bataijyu
            // 
            Bataijyu.HeaderText = "馬体重(増減)";
            Bataijyu.Name = "Bataijyu";
            // 
            // Mark
            // 
            Mark.HeaderText = "予想印";
            Mark.Name = "Mark";
            Mark.Width = 70;
            // 
            // Time
            // 
            Time.HeaderText = "調教タイム";
            Time.Name = "Time";
            Time.Width = 85;
            // 
            // Comment
            // 
            Comment.HeaderText = "厩舎コメント";
            Comment.Name = "Comment";
            Comment.Width = 380;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(18, 56);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(90, 649);
            listBox1.TabIndex = 5;
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
            // 
            // btnPutnote
            // 
            btnPutnote.Location = new Point(1193, 719);
            btnPutnote.Name = "btnPutnote";
            btnPutnote.Size = new Size(75, 23);
            btnPutnote.TabIndex = 9;
            btnPutnote.Text = "note更新";
            btnPutnote.UseVisualStyleBackColor = true;
            // 
            // btnLINE
            // 
            btnLINE.Location = new Point(1112, 719);
            btnLINE.Name = "btnLINE";
            btnLINE.Size = new Size(75, 23);
            btnLINE.TabIndex = 10;
            btnLINE.Text = "LINE";
            btnLINE.UseVisualStyleBackColor = true;
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(29, 64, 96);
            ClientSize = new Size(1361, 754);
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
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DateTimePicker dateTimePicker1;
        private Button btnSearchRaceInfo;
        private Label lblRaceInfo;
        private Label lblCondition;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn Wakuban;
        private DataGridViewTextBoxColumn Umaban;
        private DataGridViewTextBoxColumn Bamei;
        private DataGridViewTextBoxColumn SexBarei;
        private DataGridViewTextBoxColumn Kinryo;
        private DataGridViewTextBoxColumn KisyuName;
        private DataGridViewTextBoxColumn Bataijyu;
        private DataGridViewTextBoxColumn Mark;
        private DataGridViewTextBoxColumn Time;
        private DataGridViewTextBoxColumn Comment;
        private ListBox listBox1;
        private RichTextBox richTextBoxHorseInfo;
        private RichTextBox richTextBoxTweetMessage;
        private Button btnPredict;
        private Button btnPutnote;
        private Button btnLINE;
        private Button btnX;
        private Button btnOrePuro;
    }
}
