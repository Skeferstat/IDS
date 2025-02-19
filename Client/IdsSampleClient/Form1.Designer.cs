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
            button1 = new Button();
            label8 = new Label();
            SearchTermTextBox = new TextBox();
            label3 = new Label();
            BasketHookUriTextBox = new TextBox();
            label7 = new Label();
            OpenBasketFileDialog = new OpenFileDialog();
            label2 = new Label();
            ShopUrlTextBox = new TextBox();
            SendBasketToShopButton = new Button();
            IdsVersionComboBox = new ComboBox();
            label4 = new Label();
            SearchTermButton = new Button();
            label6 = new Label();
            DeepLinkSearchTextBox = new TextBox();
            DeepLinkSearchButton = new Button();
            OpenHeatingLabelFileDialog = new OpenFileDialog();
            groupBox1 = new GroupBox();
            SaveBasketButton = new Button();
            groupBox3 = new GroupBox();
            CurrentRawBasketTreeView = new TreeView();
            groupBox2 = new GroupBox();
            ReceivedRawBasketTreeView = new TreeView();
            SaveBasketFileDialog = new SaveFileDialog();
            groupBox4 = new GroupBox();
            groupBox6 = new GroupBox();
            ReceivedRawArticlesTreeView = new TreeView();
            SearchArticleHookUriTextBox = new TextBox();
            groupBox5 = new GroupBox();
            tabControl1 = new TabControl();
            BaksetTabPage = new TabPage();
            ArticlesTabPage = new TabPage();
            DeppLinkTabPage = new TabPage();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox5.SuspendLayout();
            tabControl1.SuspendLayout();
            BaksetTabPage.SuspendLayout();
            ArticlesTabPage.SuspendLayout();
            DeppLinkTabPage.SuspendLayout();
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
            // button1
            // 
            resources.ApplyResources(button1, "button1");
            button1.Name = "button1";
            ToolTip.SetToolTip(button1, resources.GetString("button1.ToolTip"));
            button1.UseVisualStyleBackColor = true;
            button1.Click += OnCopyReceivedBasketToCurrentBasket;
            // 
            // label8
            // 
            label8.AutoEllipsis = true;
            resources.ApplyResources(label8, "label8");
            label8.Name = "label8";
            ToolTip.SetToolTip(label8, resources.GetString("label8.ToolTip"));
            // 
            // SearchTermTextBox
            // 
            resources.ApplyResources(SearchTermTextBox, "SearchTermTextBox");
            SearchTermTextBox.Name = "SearchTermTextBox";
            ToolTip.SetToolTip(SearchTermTextBox, resources.GetString("SearchTermTextBox.ToolTip"));
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            ToolTip.SetToolTip(label3, resources.GetString("label3.ToolTip"));
            // 
            // BasketHookUriTextBox
            // 
            resources.ApplyResources(BasketHookUriTextBox, "BasketHookUriTextBox");
            BasketHookUriTextBox.Name = "BasketHookUriTextBox";
            ToolTip.SetToolTip(BasketHookUriTextBox, resources.GetString("BasketHookUriTextBox.ToolTip"));
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
            ToolTip.SetToolTip(label7, resources.GetString("label7.ToolTip"));
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
            groupBox1.Controls.Add(SaveBasketButton);
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(button1);
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
            // SaveBasketButton
            // 
            resources.ApplyResources(SaveBasketButton, "SaveBasketButton");
            SaveBasketButton.Name = "SaveBasketButton";
            SaveBasketButton.UseVisualStyleBackColor = true;
            SaveBasketButton.Click += OnSaveBasket;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(CurrentRawBasketTreeView);
            resources.ApplyResources(groupBox3, "groupBox3");
            groupBox3.Name = "groupBox3";
            groupBox3.TabStop = false;
            // 
            // CurrentRawBasketTreeView
            // 
            resources.ApplyResources(CurrentRawBasketTreeView, "CurrentRawBasketTreeView");
            CurrentRawBasketTreeView.LabelEdit = true;
            CurrentRawBasketTreeView.Name = "CurrentRawBasketTreeView";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(ReceivedRawBasketTreeView);
            resources.ApplyResources(groupBox2, "groupBox2");
            groupBox2.Name = "groupBox2";
            groupBox2.TabStop = false;
            // 
            // ReceivedRawBasketTreeView
            // 
            resources.ApplyResources(ReceivedRawBasketTreeView, "ReceivedRawBasketTreeView");
            ReceivedRawBasketTreeView.LabelEdit = true;
            ReceivedRawBasketTreeView.Name = "ReceivedRawBasketTreeView";
            // 
            // SaveBasketFileDialog
            // 
            SaveBasketFileDialog.FileName = "basket";
            resources.ApplyResources(SaveBasketFileDialog, "SaveBasketFileDialog");
            SaveBasketFileDialog.RestoreDirectory = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(groupBox6);
            groupBox4.Controls.Add(label7);
            groupBox4.Controls.Add(label8);
            groupBox4.Controls.Add(SearchArticleHookUriTextBox);
            groupBox4.Controls.Add(SearchTermTextBox);
            groupBox4.Controls.Add(SearchTermButton);
            resources.ApplyResources(groupBox4, "groupBox4");
            groupBox4.Name = "groupBox4";
            groupBox4.TabStop = false;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(ReceivedRawArticlesTreeView);
            resources.ApplyResources(groupBox6, "groupBox6");
            groupBox6.Name = "groupBox6";
            groupBox6.TabStop = false;
            // 
            // ReceivedRawArticlesTreeView
            // 
            resources.ApplyResources(ReceivedRawArticlesTreeView, "ReceivedRawArticlesTreeView");
            ReceivedRawArticlesTreeView.LabelEdit = true;
            ReceivedRawArticlesTreeView.Name = "ReceivedRawArticlesTreeView";
            // 
            // SearchArticleHookUriTextBox
            // 
            resources.ApplyResources(SearchArticleHookUriTextBox, "SearchArticleHookUriTextBox");
            SearchArticleHookUriTextBox.Name = "SearchArticleHookUriTextBox";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(DeepLinkSearchButton);
            groupBox5.Controls.Add(DeepLinkSearchTextBox);
            groupBox5.Controls.Add(label6);
            resources.ApplyResources(groupBox5, "groupBox5");
            groupBox5.Name = "groupBox5";
            groupBox5.TabStop = false;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(BaksetTabPage);
            tabControl1.Controls.Add(ArticlesTabPage);
            tabControl1.Controls.Add(DeppLinkTabPage);
            resources.ApplyResources(tabControl1, "tabControl1");
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            // 
            // BaksetTabPage
            // 
            BaksetTabPage.Controls.Add(groupBox1);
            resources.ApplyResources(BaksetTabPage, "BaksetTabPage");
            BaksetTabPage.Name = "BaksetTabPage";
            BaksetTabPage.UseVisualStyleBackColor = true;
            // 
            // ArticlesTabPage
            // 
            ArticlesTabPage.Controls.Add(groupBox4);
            resources.ApplyResources(ArticlesTabPage, "ArticlesTabPage");
            ArticlesTabPage.Name = "ArticlesTabPage";
            ArticlesTabPage.UseVisualStyleBackColor = true;
            // 
            // DeppLinkTabPage
            // 
            DeppLinkTabPage.Controls.Add(groupBox5);
            resources.ApplyResources(DeppLinkTabPage, "DeppLinkTabPage");
            DeppLinkTabPage.Name = "DeppLinkTabPage";
            DeppLinkTabPage.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabControl1);
            Controls.Add(label4);
            Controls.Add(IdsVersionComboBox);
            Controls.Add(ShopUrlTextBox);
            Controls.Add(label2);
            Name = "MainForm";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            tabControl1.ResumeLayout(false);
            BaksetTabPage.ResumeLayout(false);
            ArticlesTabPage.ResumeLayout(false);
            DeppLinkTabPage.ResumeLayout(false);
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
        private TextBox SearchTermTextBox;
        private Button SearchTermButton;
        private Label label6;
        private TextBox DeepLinkSearchTextBox;
        private Button DeepLinkSearchButton;
        private OpenFileDialog OpenHeatingLabelFileDialog;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private TreeView ReceivedRawBasketTreeView;
        private Button button1;
        private Button SaveBasketButton;
        private SaveFileDialog SaveBasketFileDialog;
        private GroupBox groupBox4;
        private GroupBox groupBox6;
        private TreeView ReceivedRawArticlesTreeView;
        private Label label7;
        private Label label8;
        private TextBox SearchArticleHookUriTextBox;
        private GroupBox groupBox3;
        private TreeView CurrentRawBasketTreeView;
        private GroupBox groupBox5;
        private TabControl tabControl1;
        private TabPage BaksetTabPage;
        private TabPage ArticlesTabPage;
        private TabPage DeppLinkTabPage;
    }
}
