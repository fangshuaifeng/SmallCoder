using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SmallCoder
{
    public partial class FindReplaceControl : UserControl
    {
        private TextEditor editor;
        private SearchResultBackgroundRenderer renderer;
        private string _findText;
        private bool _changing = false;
        /// <summary>
        /// 左边距
        /// </summary>
        private int _LeftGapX = 5;
        /// <summary>
        /// 上边距
        /// </summary>
        private int _TopGapY = 5;
        /// <summary>
        /// 右边距
        /// </summary>
        private int _RigthGapX = 20;
        /// <summary>
        /// 下边距
        /// </summary>
        private int _BottomGapY = 20;
        /// <summary>
        /// 当前屏幕缩放比例
        /// </summary>
        private double _ScreenScaleValue = 1.0;

        public FindReplaceControl()
        {
            InitializeComponent();
            this.Visible = false;
            _ScreenScaleValue = Utils.GetScreenScaleValue(this);
            _RigthGapX = (int)(_RigthGapX * _ScreenScaleValue);
            _BottomGapY = (int)(_BottomGapY * _ScreenScaleValue);

            renderer = new SearchResultBackgroundRenderer();

            this.panelTitle.MouseDown += Panel_MouseDown;
            this.panelTitle.MouseMove += Panel_MouseMove;
            this.btnClose.MouseEnter += BtnClose_MouseEnter;
            this.btnClose.MouseLeave += BtnClose_MouseLeave;
            this.btnClose.Click += BtnClose_Click;
            this.txtFind2.TextChanged += TxtFind2_TextChanged;
            this.Paint += dropShadow;

            // 向上、向下查找
            HotKeyManager.AddFormControlHotKey(this.txtFind2, () => btnPrev_Click(null, null), Keys.Enter, true);
            HotKeyManager.AddFormControlHotKey(this.txtFind2, () => btnNext_Click(null, null), Keys.Enter);
            // 替换、替换全部
            HotKeyManager.AddFormControlHotKey(this.txtReplace, () => btnReplace_Click(null, null), Keys.Enter);
            HotKeyManager.AddFormControlHotKey(this.txtReplace, () => btnReplaceAll_Click(null, null), Keys.Enter, true);
        }

        private void TxtFind2_TextChanged(object sender, EventArgs e)
        {
            _findText = this.txtFind2.Text;
            this.FindAll();
        }

        #region 窗体拖动&关闭

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.CloseFindDialog();
        }

        private void BtnClose_MouseLeave(object sender, EventArgs e)
        {
            this.btnClose.BackColor = Color.Transparent;
            this.btnClose.ForeColor = Color.Black;
        }

        private void BtnClose_MouseEnter(object sender, EventArgs e)
        {
            this.btnClose.BackColor = ColorTranslator.FromHtml("#e81123");
            this.btnClose.ForeColor = Color.White;
        }

        System.Drawing.Point mPoint;
        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint = new System.Drawing.Point(e.X, e.Y);
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var newPointX = this.Location.X + e.X - mPoint.X;
                var newPointY = this.Location.Y + e.Y - mPoint.Y;
                var maxPointX = this.Parent.Width - this.Width - _RigthGapX;
                var maxPointY = this.Parent.Height - this.Height - _BottomGapY;
                if (newPointX < _LeftGapX) { newPointX = _LeftGapX; } else if (newPointX > maxPointX) { newPointX = maxPointX; }
                if (newPointY < _TopGapY) { newPointY = _TopGapY; } else if (newPointY > maxPointY) { newPointY = maxPointY; }
                this.Location = new System.Drawing.Point(newPointX, newPointY);
            }
        }

        #endregion

        #region 对外暴露方法

        /// <summary>
        /// 绑定编辑器
        /// </summary>
        public void BindEditor(TextEditor _editor)
        {
            this.editor = _editor;
            editor.TextArea.TextView.BackgroundRenderers.Add(renderer);
        }

        /// <summary>
        /// 将文本输入到搜索框，并自动弹出选中
        /// </summary>
        /// <param name="txt"></param>
        public void SendToFindText(string txt)
        {
            _findText = txt;
            this.txtFind2.Text = txt;

            this.txtFind2.Clear();
            this.txtFind2.SelectedText = txt;

            this.FindAll();
        }

        /// <summary>
        /// 打开搜索面板
        /// </summary>
        /// <param name="locationX">坐标X</param>
        /// <param name="locationY">坐标Y</param>
        public void OpenFindDialog(int? locationX = null, int? locationY = null)
        {
            var pointX = locationX.HasValue ? locationX.Value : (this.Parent.Width - this.Width - _RigthGapX);
            var pointY = locationY.HasValue ? locationY.Value : _TopGapY;

            if (!this.Visible || this.Location.X > pointX || this.Location.X < _LeftGapX)
            {
                this.Location = new System.Drawing.Point(pointX, pointY);
            }
            if (!this.Visible) this.Visible = true;
            this.Focus();
        }

        /// <summary>
        /// 关闭搜索面板
        /// </summary>
        public void CloseFindDialog()
        {
            this.Visible = false;
            this.ClearSelection(true);
        }

        /// <summary>
        /// 查找全部
        /// </summary>
        public void FindAll()
        {
            if (_changing) return;

            this.ClearSelection(false);
            if (!string.IsNullOrEmpty(_findText))
            {
                this._changing = true;
                Regex regex = GetRegEx(_findText);
                if (regex == null) return;
                var matchs = regex.Matches(editor.Text, 0);
                if (matchs.Count > 0)
                {
                    foreach (Match match in matchs)
                    {
                        var item = new SearchResult { StartOffset = match.Index, Length = match.Length, Data = match };
                        renderer.CurrentResults.Add(item);
                    }
                }
            }
            editor.TextArea.TextView.InvalidateLayer(KnownLayer.Selection);
            this._changing = false;
        }

        #endregion

        #region 内部方法

        private void ClearSelection(bool invalidateLayer)
        {
            //editor.TextArea.TextView.BackgroundRenderers.Remove(renderer);
            renderer.CurrentResults.Clear();
            editor.TextArea.ClearSelection();
            if (invalidateLayer)
            {
                editor.TextArea.TextView.InvalidateLayer(KnownLayer.Selection);
            }
        }

        private bool FindNext(string textToFind, bool searchUp = false)
        {
            Regex regex = GetRegEx(textToFind, searchUp);
            if (regex == null) return false;
            int start = regex.Options.HasFlag(RegexOptions.RightToLeft) ?
            editor.SelectionStart : editor.SelectionStart + editor.SelectionLength;
            Match match = regex.Match(editor.Text, start);

            if (!match.Success)  // start again from beginning or end
            {
                if (regex.Options.HasFlag(RegexOptions.RightToLeft))
                    match = regex.Match(editor.Text, editor.Text.Length);
                else
                    match = regex.Match(editor.Text, 0);
            }

            if (match.Success)
            {
                editor.Select(match.Index, match.Length);
                TextLocation loc = editor.Document.GetLocation(match.Index);
                editor.ScrollTo(loc.Line, loc.Column);
            }

            return match.Success;
        }

        private Regex GetRegEx(string textToFind, bool searchUp = false)
        {
            try
            {
                RegexOptions options = RegexOptions.None;
                if (cbCaseSensitive.Checked == false)
                    options |= RegexOptions.IgnoreCase;
                if (searchUp)
                    options |= RegexOptions.RightToLeft;

                if (cbRegex.Checked == true)
                {
                    return new Regex(textToFind, options);
                }
                else
                {
                    string pattern = Regex.Escape(textToFind);
                    if (cbWildcards.Checked == true)
                        pattern = pattern.Replace("\\*", ".*").Replace("\\?", ".");
                    if (cbWholeWord.Checked == true)
                        pattern = "\\b" + pattern + "\\b";
                    return new Regex(pattern, options);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (!FindNext(txtFind2.Text, true))
                SystemSounds.Beep.Play();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!FindNext(txtFind2.Text))
                SystemSounds.Beep.Play();
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            Regex regex = GetRegEx(txtFind2.Text);
            if (regex == null) return;
            string input = editor.Text.Substring(editor.SelectionStart, editor.SelectionLength);
            Match match = regex.Match(input);
            bool replaced = false;
            if (match.Success && match.Index == 0 && match.Length == input.Length)
            {
                editor.Document.Replace(editor.SelectionStart, editor.SelectionLength, txtReplace.Text);
                replaced = true;
                this.FindAll();
            }

            if (!FindNext(txtFind2.Text) && !replaced)
                SystemSounds.Beep.Play();
        }

        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            Regex regex = GetRegEx(txtFind2.Text);
            if (regex == null) return;
            int offset = 0;
            editor.BeginChange();
            foreach (Match match in regex.Matches(editor.Text))
            {
                editor.Document.Replace(offset + match.Index, match.Length, txtReplace.Text);
                offset += txtReplace.Text.Length - match.Length;
            }
            editor.EndChange();
            this.ClearSelection(false);
        }

        #endregion


        private void dropShadow(object sender, PaintEventArgs e)
        {

        }
    }
}
