﻿namespace childApp
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.закрытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.окноToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.лицевыеСчетаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.начисленияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.периодыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.печатьКвитанцийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьВсеЛСToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьЛСToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.лицевыеСчетаToolStripMenuItem,
            this.начисленияToolStripMenuItem,
            this.окноToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.MdiWindowListItem = this.окноToolStripMenuItem;
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1006, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.закрытьToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // закрытьToolStripMenuItem
            // 
            this.закрытьToolStripMenuItem.Name = "закрытьToolStripMenuItem";
            this.закрытьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.закрытьToolStripMenuItem.Text = "Закрыть";
            // 
            // окноToolStripMenuItem
            // 
            this.окноToolStripMenuItem.Name = "окноToolStripMenuItem";
            this.окноToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.окноToolStripMenuItem.Text = "Окно";
            // 
            // лицевыеСчетаToolStripMenuItem
            // 
            this.лицевыеСчетаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьВсеЛСToolStripMenuItem,
            this.добавитьЛСToolStripMenuItem});
            this.лицевыеСчетаToolStripMenuItem.Name = "лицевыеСчетаToolStripMenuItem";
            this.лицевыеСчетаToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.лицевыеСчетаToolStripMenuItem.Text = "Лицевые счета";
            // 
            // начисленияToolStripMenuItem
            // 
            this.начисленияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.периодыToolStripMenuItem,
            this.печатьКвитанцийToolStripMenuItem});
            this.начисленияToolStripMenuItem.Name = "начисленияToolStripMenuItem";
            this.начисленияToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
            this.начисленияToolStripMenuItem.Text = "Начисления";
            // 
            // периодыToolStripMenuItem
            // 
            this.периодыToolStripMenuItem.Name = "периодыToolStripMenuItem";
            this.периодыToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.периодыToolStripMenuItem.Text = "Периоды";
            // 
            // печатьКвитанцийToolStripMenuItem
            // 
            this.печатьКвитанцийToolStripMenuItem.Name = "печатьКвитанцийToolStripMenuItem";
            this.печатьКвитанцийToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.печатьКвитанцийToolStripMenuItem.Text = "Печать квитанций";
            // 
            // открытьВсеЛСToolStripMenuItem
            // 
            this.открытьВсеЛСToolStripMenuItem.Name = "открытьВсеЛСToolStripMenuItem";
            this.открытьВсеЛСToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.открытьВсеЛСToolStripMenuItem.Text = "Открыть все ЛС";
            // 
            // добавитьЛСToolStripMenuItem
            // 
            this.добавитьЛСToolStripMenuItem.Name = "добавитьЛСToolStripMenuItem";
            this.добавитьЛСToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.добавитьЛСToolStripMenuItem.Text = "Добавить ЛС";
            this.добавитьЛСToolStripMenuItem.Click += new System.EventHandler(this.добавитьЛСToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 542);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Сервисное обслуживание";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem закрытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem окноToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem лицевыеСчетаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьВсеЛСToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьЛСToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem начисленияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem периодыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem печатьКвитанцийToolStripMenuItem;
    }
}

