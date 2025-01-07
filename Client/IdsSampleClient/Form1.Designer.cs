namespace IdsSampleClient
{
    partial class MainForm
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
            components = new System.ComponentModel.Container();
            OpenBasketFileButton = new Button();
            label1 = new Label();
            BasketXmlFileTextBox = new TextBox();
            ToolTip = new ToolTip(components);
            OpenBasketFileDialog = new OpenFileDialog();
            label2 = new Label();
            ShopUrlTextBox = new TextBox();
            SendBasketToShopButton = new Button();
            label3 = new Label();
            HookUriTextBox = new TextBox();
            IdsVersionComboBox = new ComboBox();
            label4 = new Label();
            SuspendLayout();
            // 
            // OpenBasketFileButton
            // 
            OpenBasketFileButton.Location = new Point(601, 130);
            OpenBasketFileButton.Name = "OpenBasketFileButton";
            OpenBasketFileButton.Size = new Size(27, 29);
            OpenBasketFileButton.TabIndex = 0;
            OpenBasketFileButton.Text = "...";
            OpenBasketFileButton.UseVisualStyleBackColor = true;
            OpenBasketFileButton.Click += OnOpenBasketFile;
            // 
            // label1
            // 
            label1.AutoEllipsis = true;
            label1.AutoSize = true;
            label1.Location = new Point(12, 134);
            label1.Name = "label1";
            label1.Size = new Size(55, 20);
            label1.TabIndex = 1;
            label1.Text = "Basket:";
            ToolTip.SetToolTip(label1, "An xml file with basket informations inside");
            // 
            // BasketXmlFileTextBox
            // 
            BasketXmlFileTextBox.Enabled = false;
            BasketXmlFileTextBox.HideSelection = false;
            BasketXmlFileTextBox.Location = new Point(109, 131);
            BasketXmlFileTextBox.Name = "BasketXmlFileTextBox";
            BasketXmlFileTextBox.PlaceholderText = "Choose a xml file...";
            BasketXmlFileTextBox.Size = new Size(486, 27);
            BasketXmlFileTextBox.TabIndex = 2;
            // 
            // OpenBasketFileDialog
            // 
            OpenBasketFileDialog.DefaultExt = "xml";
            OpenBasketFileDialog.FileName = "basket";
            OpenBasketFileDialog.Filter = "xml files | *.xml";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 55);
            label2.Name = "label2";
            label2.Size = new Size(71, 20);
            label2.TabIndex = 3;
            label2.Text = "Shop-Url:";
            // 
            // ShopUrlTextBox
            // 
            ShopUrlTextBox.Enabled = false;
            ShopUrlTextBox.Location = new Point(111, 52);
            ShopUrlTextBox.Name = "ShopUrlTextBox";
            ShopUrlTextBox.Size = new Size(486, 27);
            ShopUrlTextBox.TabIndex = 4;
            // 
            // SendBasketToShopButton
            // 
            SendBasketToShopButton.Location = new Point(111, 182);
            SendBasketToShopButton.Name = "SendBasketToShopButton";
            SendBasketToShopButton.Size = new Size(193, 29);
            SendBasketToShopButton.TabIndex = 5;
            SendBasketToShopButton.Text = "Send basket to shop";
            SendBasketToShopButton.UseVisualStyleBackColor = true;
            SendBasketToShopButton.Click += OnSendBasketToShop;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 94);
            label3.Name = "label3";
            label3.Size = new Size(73, 20);
            label3.TabIndex = 6;
            label3.Text = "Hook-Url:";
            // 
            // HookUriTextBox
            // 
            HookUriTextBox.Enabled = false;
            HookUriTextBox.Location = new Point(111, 91);
            HookUriTextBox.Name = "HookUriTextBox";
            HookUriTextBox.PlaceholderText = "Choose a hook url...";
            HookUriTextBox.Size = new Size(486, 27);
            HookUriTextBox.TabIndex = 7;
            // 
            // IdsVersionComboBox
            // 
            IdsVersionComboBox.FormattingEnabled = true;
            IdsVersionComboBox.Items.AddRange(new object[] { "2.5" });
            IdsVersionComboBox.Location = new Point(111, 18);
            IdsVersionComboBox.Name = "IdsVersionComboBox";
            IdsVersionComboBox.Size = new Size(102, 28);
            IdsVersionComboBox.Sorted = true;
            IdsVersionComboBox.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 21);
            label4.Name = "label4";
            label4.Size = new Size(89, 20);
            label4.TabIndex = 9;
            label4.Text = "IDS-Version:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(893, 452);
            Controls.Add(label4);
            Controls.Add(IdsVersionComboBox);
            Controls.Add(HookUriTextBox);
            Controls.Add(label3);
            Controls.Add(SendBasketToShopButton);
            Controls.Add(ShopUrlTextBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(BasketXmlFileTextBox);
            Controls.Add(OpenBasketFileButton);
            Name = "MainForm";
            Text = "IDS Test Client";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button OpenBasketFileButton;
        private Label label1;
        private TextBox BasketXmlFileTextBox;
        private ToolTip ToolTip;
        private OpenFileDialog OpenBasketFileDialog;
        private Label label2;
        private TextBox ShopUrlTextBox;
        private Button SendBasketToShopButton;
        private Label label3;
        private TextBox HookUriTextBox;
        private ComboBox IdsVersionComboBox;
        private Label label4;
    }
}
