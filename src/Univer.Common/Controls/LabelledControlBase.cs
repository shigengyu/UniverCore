// The MIT License (MIT)
// 
// Copyright (c) 2012-2013 Univer Shi
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Univer.Common.Controls
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TControl">The type of the control.</typeparam>
    public class LabelledControlBase<TControl> : UserControl where TControl : Control, new()
    {
        private Label _labelTitle;
        private Panel _panel;
        private string _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelledControlBase{TControl}"/> class.
        /// </summary>
        public LabelledControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialize"></param>
        public LabelledControlBase(bool initialize)
            : this()
        {
            Title = "Title";
            TitleWidth = 100;

            InnerControl = new TControl();
            Height = InnerControl.Height;
            InnerControl.Dock = DockStyle.Fill;

            SuspendLayout();
            _panel.Controls.Add(InnerControl);
            ResumeLayout();
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                _labelTitle.Text = _title + ":";
            }
        }

        /// <summary>
        /// Gets or sets the width of the title.
        /// </summary>
        /// <value>
        /// The width of the title.
        /// </value>
        public int TitleWidth
        {
            get { return _labelTitle.Width; }
            set { _labelTitle.Width = value; }
        }

        /// <summary>
        /// Gets the inner control.
        /// </summary>
        /// <value>
        /// The inner control.
        /// </value>
        public TControl InnerControl { get; private set; }

        /// <summary>
        /// </summary>
        /// <returns>The text associated with this control.</returns>
        public override string Text
        {
            get { return InnerControl.Text; }
            set { InnerControl.Text = value; }
        }

        private void InitializeComponent()
        {
            _labelTitle = new Label();
            _panel = new Panel();
            SuspendLayout();
            // 
            // labelTitle
            // 
            _labelTitle.Dock = DockStyle.Left;
            _labelTitle.Location = new Point(0, 0);
            _labelTitle.Name = "_labelTitle";
            _labelTitle.Size = new Size(35, 50);
            _labelTitle.TabIndex = 0;
            _labelTitle.Text = "Title";
            _labelTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel
            // 
            _panel.Dock = DockStyle.Fill;
            _panel.Location = new Point(35, 0);
            _panel.Name = "_panel";
            _panel.Size = new Size(165, 50);
            _panel.TabIndex = 1;
            // 
            // LabelledControlBase
            // 
            Controls.Add(_panel);
            Controls.Add(_labelTitle);
            Name = "LabelledControlBase";
            Size = new Size(200, 50);
            ResumeLayout(false);
        }
    }
}