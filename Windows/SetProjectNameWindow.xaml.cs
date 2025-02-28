using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EChartsTest.Windows
{
    /// <summary>
    /// SetProjectNameWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetProjectNameWindow : Window, INotifyPropertyChanged
    {
        private string _projectName;
        private string _projectType;
        private string _moreType;
        private List<string> _projectTypes;
        private List<string> _moreTypes;

        public string ProjectName
        {
            get => _projectName;
            set
            {
                if (_projectName != value)
                {
                    _projectName = value;
                    OnPropertyChanged(nameof(ProjectName));
                }
            }
        }

        public string ProjectType
        {
            get => _projectType;
            set
            {
                if (_projectType != value)
                {
                    _projectType = value;
                    OnPropertyChanged(nameof(ProjectType));
                }
            }
        }

        public string MoreType
        {
            get => _moreType;
            set
            {
                if (_moreType != value)
                {
                    _moreType = value;
                    OnPropertyChanged(nameof(MoreType));
                }
            }
        }

        public List<string> ProjectTypes
        {
            get => _projectTypes;
            set
            {
                if (_projectTypes != value)
                {
                    _projectTypes = value;
                    OnPropertyChanged(nameof(ProjectTypes));
                }
            }
        }

        public List<string> MoreTypes
        {
            get => _moreTypes;
            set
            {
                if (_moreTypes != value)
                {
                    _moreTypes = value;
                    OnPropertyChanged(nameof(MoreTypes));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SetProjectNameWindow()
        {
            InitializeComponent();
            // 初始化项目类型列表
            ProjectTypes = new List<string>
            {
                "河流沉积",
                "碳酸岩沉积",
                "浊流与深水沉积",
                "构造与其他沉积"
            };

            // 初始化更多类型列表
            MoreTypes = new List<string>();

            // 绑定数据源
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(projectName.Text))
            {
                MessageBox.Show("项目名不能为空！");
                return;
            }

            if (projectTypes.SelectedItem == null)
            {
                MessageBox.Show("请选择一个项目类型！");
                return;
            }

            if (moreTypes.SelectedItem == null)
            {
                MessageBox.Show("请选择一个更多类型！");
                return;
            }

            ProjectName = projectName.Text;
            ProjectType = projectTypes.SelectedItem.ToString();
            MoreType = moreTypes.SelectedItem.ToString();
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void projectTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedProjectType = projectTypes.SelectedItem as string;
            if (selectedProjectType != null)
            {
                switch (selectedProjectType)
                {
                    case "河流沉积":
                        MoreTypes = new List<string>
                        {
                            "短期河流沉积",
                            "长期河流沉积",
                            "多河道沉积"
                        };
                        break;
                    case "碳酸岩沉积":
                        MoreTypes = new List<string>
                        {
                            "生物礁沉积",
                            "碳酸盐岩碎屑沉积",
                            "碳酸盐岩沉积",
                            "河道作用下的台地边缘沉积",
                            "波浪作用下的台地边缘沉积"
                        };
                        break;
                    case "浊流与深水沉积":
                        MoreTypes = new List<string>
                        {
                            "短期浊积扇沉积",
                            "长期浊积扇沉积",
                            "浊积岩沉积"
                        };
                        break;
                    case "构造与其他沉积":
                        MoreTypes = new List<string>
                        {
                            "构造作用沉积",
                            "波浪作用沉积",
                            "长期沉积物运移",
                            "短期沉积物运移"
                        };
                        break;
                }
            }
        }

        private void moreTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 你可以在这里处理更多类型的变更事件
        }
    }
}