using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecitingWord
{
    class AboutViewMode:MVVM.ViewModeBase
    {
        static AboutViewMode _Instance = new AboutViewMode();
        public static AboutViewMode Instance
        {
            get
            {
                return _Instance;
            }

        }
        private AboutViewMode()
        {
            Text = @"
Setting 界面 Paste 粘贴 或 在TypeWord界面输入想要学习的单词
单击 Start Play 开始播放
单击 WordPlay界面的单词 暂停
单击 Delete 标记当前显示的单词已经学会

Setting 界面下的三个滚动条可以调节单词显示时间 淡入淡出时间
Setting 界面下的 Random 复选框可以选择是否随机显示单词列表
Setting 界面下的 Show Explain 复选框可以选择是否一直显示释义
将鼠标放在单词上可以显示释义


n.   （noun）名词
adv. （adverb）副词
adj. （adjective）形容词
v.   （verb）动词
conj.（conjunction）连词
prep.（preposition）介词
";
        }
        private string _Text;
        public string Text
        {
            get { return _Text; }
            set { SetProperty(ref _Text, value, nameof(Text)); }
        }
    } 
}
