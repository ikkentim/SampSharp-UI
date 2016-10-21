﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampSharp.GameMode;
using SampSharp.GameMode.Definitions;
using SampSharp.GameMode.SAMP;

namespace SampSharp.UI
{
    public class TextArea : Panel, ITextControl
    {
        private Label _label;
        private string _text;

        private readonly BatchedPropertyCollection<Label> _properties = new BatchedPropertyCollection<Label>
        {
            ["Font"] = new BatchedProperty<Label, TextDrawFont>((t, v) => t.Font = v),
            ["ForeColor"] = new BatchedProperty<Label, Color>((t, v) => t.ForeColor = v),
            ["LetterSize"] = new BatchedProperty<Label, Vector2>((t, v) => t.LetterSize = v),
            ["Outline"] = new BatchedProperty<Label, int>((t, v) => t.Outline = v),
            ["Proportional"] = new BatchedProperty<Label, bool>((t, v) => t.Proportional = v),
            ["Shadow"] = new BatchedProperty<Label, int>((t, v) => t.Shadow = v),
            ["Text"] = new BatchedProperty<Label, string>((t, v) => t.Text = v),
        };

        public TextArea()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            Shadow = 0;
            LetterSize = new Vector2(0.18f, 0.9f);
            BackColor = 0x00000001;

            _label = new Label();

            Controls.Add(_label);
            
            _properties.SetContainer(_label);
        }

        private Vector2 GetTextSize(string value)
        {
            return ControlUtils.GetTextSize(value, Font, LetterSize, Proportional);
        }

        private int GetStringWithMaxWidth(string value, float width)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var len = 1;
            while (value.Length > len)
            {
                var cur = GetTextSize(value.Substring(0, len + 1)).X;
                if (cur > width)
                    return len;

                len++;
            }

            return value.Length;
        }

        private string MakeFitting(string value)
        {
            var lines = new List<string>();

            while (!string.IsNullOrWhiteSpace(value))
            {
                var line = GetStringWithMaxWidth(value, Width);

                lines.Add(value.Substring(0, line));

                value = value.Length > line ? value.Substring(line) : null;
            }

            return string.Join("\n", lines);
        }

        #region Implementation of ITextControl

        public Color ForeColor
        {
            get { return _properties.Get<Color>(); }
            set
            {
                AssertNotDisposed();

                if (_properties.Set(value))
                {
                    OnPropertyChanged();
                    Invalidate();
                }
            }
        }

        public Vector2 LetterSize
        {
            get { return _properties.Get<Vector2>(); }
            set
            {
                AssertNotDisposed();

                if (_properties.Set(value))
                {
                    OnPropertyChanged();
                    Invalidate();
                }
            }
        }

        public int Outline
        {
            get { return _properties.Get<int>(); }
            set
            {
                AssertNotDisposed();

                if (_properties.Set(value))
                {
                    OnPropertyChanged();
                    Invalidate();
                }
            }
        }

        public TextDrawFont Font
        {
            get { return _properties.Get<TextDrawFont>(); }
            set
            {
                AssertNotDisposed();

                if (_properties.Set(value))
                {
                    OnPropertyChanged();
                    Invalidate();
                }
            }
        }

        public bool Proportional
        {
            get { return _properties.Get<bool>(); }
            set
            {
                AssertNotDisposed();

                if (_properties.Set(value))
                {
                    OnPropertyChanged();
                    Invalidate();
                }
            }
        }

        public int Shadow
        {
            get { return _properties.Get<int>(); }
            set
            {
                AssertNotDisposed();

                if (_properties.Set(value))
                {
                    OnPropertyChanged();
                    Invalidate();
                }
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                AssertNotDisposed();
                
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged();
                }

                if (_properties.Set(MakeFitting(value)))
                {
                    Invalidate();
                }
            }
        }

        #endregion

        #region Overrides of Control

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();

            _properties.Set(MakeFitting(_text), "Text");
        }

        #endregion

        #region Overrides of Panel

        protected override void OnRender()
        {
            _label.SuspendLayout();
            _properties.Apply();
            _label.ResumeLayout();

            base.OnRender();
        }

        #endregion
    }
}