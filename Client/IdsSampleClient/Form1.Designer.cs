namespace IdsSampleClient
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
            components = new System.ComponentModel.Container();
            OpenBasketFileButton = new Button();
            label1 = new Label();
            BasketXmlFileTextBox = new TextBox();
            ToolTip = new ToolTip(components);
            OpenBasketFileDialog = new OpenFileDialog();
            label2 = new Label();
            ShopUrlTextBox = new TextBox();
            SendBasketToShopButton = new Button();
            ShopWebView = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)ShopWebView).BeginInit();
            SuspendLayout();
            // 
            // OpenBasketFileButton
            // 
            OpenBasketFileButton.Location = new Point(601, 75);
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
            label1.Location = new Point(10, 84);
            label1.Name = "label1";
            label1.Size = new Size(52, 20);
            label1.TabIndex = 1;
            label1.Text = "Basket";
            ToolTip.SetToolTip(label1, "An xml file with basket informations inside");
            // 
            // BasketXmlFileTextBox
            // 
            BasketXmlFileTextBox.Enabled = false;
            BasketXmlFileTextBox.HideSelection = false;
            BasketXmlFileTextBox.Location = new Point(109, 76);
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
            label2.Location = new Point(10, 33);
            label2.Name = "label2";
            label2.Size = new Size(71, 20);
            label2.TabIndex = 3;
            label2.Text = "Shop-Url:";
            // 
            // ShopUrlTextBox
            // 
            ShopUrlTextBox.Enabled = false;
            ShopUrlTextBox.Location = new Point(109, 30);
            ShopUrlTextBox.Name = "ShopUrlTextBox";
            ShopUrlTextBox.Size = new Size(486, 27);
            ShopUrlTextBox.TabIndex = 4;
            // 
            // SendBasketToShopButton
            // 
            SendBasketToShopButton.Location = new Point(109, 124);
            SendBasketToShopButton.Name = "SendBasketToShopButton";
            SendBasketToShopButton.Size = new Size(193, 29);
            SendBasketToShopButton.TabIndex = 5;
            SendBasketToShopButton.Text = "Send basket to shop";
            SendBasketToShopButton.UseVisualStyleBackColor = true;
            SendBasketToShopButton.Click += OnSendBasketToShop;
            // 
            // ShopWebView
            // 
            ShopWebView.AllowExternalDrop = true;
            ShopWebView.CreationProperties = null;
            ShopWebView.DefaultBackgroundColor = Color.White;
            ShopWebView.Location = new Point(52, 227);
            ShopWebView.Name = "ShopWebView";
            ShopWebView.Size = new Size(772, 159);
            ShopWebView.TabIndex = 6;
            ShopWebView.ZoomFactor = 1D;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(893, 452);
            Controls.Add(ShopWebView);
            Controls.Add(SendBasketToShopButton);
            Controls.Add(ShopUrlTextBox);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(BasketXmlFileTextBox);
            Controls.Add(OpenBasketFileButton);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)ShopWebView).EndInit();
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
        private Microsoft.Web.WebView2.WinForms.WebView2 ShopWebView;
    }
}
