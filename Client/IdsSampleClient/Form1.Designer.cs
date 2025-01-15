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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            OpenBasketFileButton = new Button();
            label1 = new Label();
            BasketXmlFileTextBox = new TextBox();
            ToolTip = new ToolTip(components);
            OpenBasketFileDialog = new OpenFileDialog();
            label2 = new Label();
            ShopUrlTextBox = new TextBox();
            SendBasketToShopButton = new Button();
            label3 = new Label();
            BasketHookUriTextBox = new TextBox();
            IdsVersionComboBox = new ComboBox();
            label4 = new Label();
            label5 = new Label();
            SearchTermTextBox = new TextBox();
            SearchTermButton = new Button();
            label6 = new Label();
            DeepLinkSearchTextBox = new TextBox();
            DeepLinkSearchButton = new Button();
            OpenHeatingLabelFileDialog = new OpenFileDialog();
            groupBox1 = new GroupBox();
            RawBasketTreeView = new TreeView();
            groupBox2 = new GroupBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // OpenBasketFileButton
            // 
            resources.ApplyResources(OpenBasketFileButton, "OpenBasketFileButton");
            OpenBasketFileButton.Name = "OpenBasketFileButton";
            OpenBasketFileButton.UseVisualStyleBackColor = true;
            OpenBasketFileButton.Click += OnOpenBasketFile;
            // 
            // label1
            // 
            label1.AutoEllipsis = true;
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            ToolTip.SetToolTip(label1, resources.GetString("label1.ToolTip"));
            // 
            // BasketXmlFileTextBox
            // 
            resources.ApplyResources(BasketXmlFileTextBox, "BasketXmlFileTextBox");
            BasketXmlFileTextBox.HideSelection = false;
            BasketXmlFileTextBox.Name = "BasketXmlFileTextBox";
            // 
            // OpenBasketFileDialog
            // 
            OpenBasketFileDialog.DefaultExt = "xml";
            OpenBasketFileDialog.FileName = "basket";
            resources.ApplyResources(OpenBasketFileDialog, "OpenBasketFileDialog");
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // ShopUrlTextBox
            // 
            resources.ApplyResources(ShopUrlTextBox, "ShopUrlTextBox");
            ShopUrlTextBox.Name = "ShopUrlTextBox";
            // 
            // SendBasketToShopButton
            // 
            resources.ApplyResources(SendBasketToShopButton, "SendBasketToShopButton");
            SendBasketToShopButton.Name = "SendBasketToShopButton";
            SendBasketToShopButton.UseVisualStyleBackColor = true;
            SendBasketToShopButton.Click += OnSendBasketToShop;
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // BasketHookUriTextBox
            // 
            resources.ApplyResources(BasketHookUriTextBox, "BasketHookUriTextBox");
            BasketHookUriTextBox.Name = "BasketHookUriTextBox";
            // 
            // IdsVersionComboBox
            // 
            IdsVersionComboBox.FormattingEnabled = true;
            IdsVersionComboBox.Items.AddRange(new object[] { resources.GetString("IdsVersionComboBox.Items") });
            resources.ApplyResources(IdsVersionComboBox, "IdsVersionComboBox");
            IdsVersionComboBox.Name = "IdsVersionComboBox";
            IdsVersionComboBox.Sorted = true;
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // SearchTermTextBox
            // 
            resources.ApplyResources(SearchTermTextBox, "SearchTermTextBox");
            SearchTermTextBox.Name = "SearchTermTextBox";
            // 
            // SearchTermButton
            // 
            resources.ApplyResources(SearchTermButton, "SearchTermButton");
            SearchTermButton.Name = "SearchTermButton";
            SearchTermButton.UseVisualStyleBackColor = true;
            SearchTermButton.Click += OnSearchTerm;
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // DeepLinkSearchTextBox
            // 
            resources.ApplyResources(DeepLinkSearchTextBox, "DeepLinkSearchTextBox");
            DeepLinkSearchTextBox.Name = "DeepLinkSearchTextBox";
            // 
            // DeepLinkSearchButton
            // 
            resources.ApplyResources(DeepLinkSearchButton, "DeepLinkSearchButton");
            DeepLinkSearchButton.Name = "DeepLinkSearchButton";
            DeepLinkSearchButton.UseVisualStyleBackColor = true;
            DeepLinkSearchButton.Click += OnDeepLinkSearchTerm;
            // 
            // OpenHeatingLabelFileDialog
            // 
            OpenHeatingLabelFileDialog.DefaultExt = "xml";
            OpenHeatingLabelFileDialog.FileName = "heatingLabel";
            resources.ApplyResources(OpenHeatingLabelFileDialog, "OpenHeatingLabelFileDialog");
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(OpenBasketFileButton);
            groupBox1.Controls.Add(BasketXmlFileTextBox);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(SendBasketToShopButton);
            groupBox1.Controls.Add(BasketHookUriTextBox);
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // RawBasketTreeView
            // 
            resources.ApplyResources(RawBasketTreeView, "RawBasketTreeView");
            RawBasketTreeView.Name = "RawBasketTreeView";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(RawBasketTreeView);
            resources.ApplyResources(groupBox2, "groupBox2");
            groupBox2.Name = "groupBox2";
            groupBox2.TabStop = false;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox1);
            Controls.Add(DeepLinkSearchButton);
            Controls.Add(DeepLinkSearchTextBox);
            Controls.Add(label6);
            Controls.Add(SearchTermButton);
            Controls.Add(SearchTermTextBox);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(IdsVersionComboBox);
            Controls.Add(ShopUrlTextBox);
            Controls.Add(label2);
            Name = "MainForm";
            Click += OnSearchTerm;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
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
        private TextBox BasketHookUriTextBox;
        private ComboBox IdsVersionComboBox;
        private Label label4;
        private Label label5;
        private TextBox SearchTermTextBox;
        private Button SearchTermButton;
        private Label label6;
        private TextBox DeepLinkSearchTextBox;
        private Button DeepLinkSearchButton;
        private OpenFileDialog OpenHeatingLabelFileDialog;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private TreeView RawBasketTreeView;
    }
}
