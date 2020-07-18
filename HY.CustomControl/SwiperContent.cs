using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace HY.CustomControl
{
    /// <summary>
    ///     轮播控件
    /// </summary>
    [DefaultProperty("Items")]
    [ContentProperty("Items")]
    //[TemplatePart(Name = ElementPanelPage, Type = typeof(Panel))]
    [TemplatePart(Name = ElementItemsControl, Type = typeof(ItemsPresenter))]
    public class SwiperContent : ListBox, IDisposable
    {
        public SwiperContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwiperContent), new FrameworkPropertyMetadata(typeof(SwiperContent)));
            CommandBindings.Add(new CommandBinding(Prev, ButtonPrev_OnClick));
            CommandBindings.Add(new CommandBinding(Next, ButtonNext_OnClick));
        }
        public static readonly DependencyProperty AllItemSourceProperty = DependencyProperty.Register("AllItemSource", typeof(IEnumerable<dynamic>), typeof(SwiperContent));
        public IEnumerable<dynamic> AllItemSource
        {
            get { return (IEnumerable<dynamic>)GetValue(AllItemSourceProperty); }
            set { SetValue(AllItemSourceProperty, value); }
        }


        private void ButtonPrev_OnClick(object sender, RoutedEventArgs e) => PageIndex--;

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e) => PageIndex++;
        private const string ElementItemsControl = "PART_ItemsControl";
        private int _pageIndex = 0;
        private int _pageCount = 0;
        /// <summary>
        ///     页码
        /// </summary>
        public int PageIndex
        {
            get => _pageIndex;
            set
            {
                if (Items.Count == 0) return;

                if (value < 0) return;
               
                if (_pageCount== value) return;
                _pageIndex = value;
                UpdatePageButtons(_pageIndex);
            }
        }
        //private Panel _panelPage;
        private ItemsPresenter _itemsControl;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _itemsControl = GetTemplateChild(ElementItemsControl) as ItemsPresenter;

            if (AllItemSource != null && AllItemSource.Count()>4)
            {
                _pageCount = Convert.ToInt32(Math.Ceiling(AllItemSource.Count() / (double)4));;
            }

            Update();
        }
        private void Update()
        {
            UpdatePageButtons(_pageIndex);
        }
        /// <summary>
        ///     更新页按钮
        /// </summary>
        public void UpdatePageButtons(int index)
        {
            if (AllItemSource != null && AllItemSource.Count() > 4)
            {
                var items = AllItemSource.Skip(4 * index).Take(4);
                _itemsControl.BeginAnimation(MarginProperty,
             CreateAnimation(new Thickness(1, 0, 0, 0)));
                ItemsSource = items;
            }

        }

    
        /// <summary>
        ///  更新项的位置
        /// </summary>
        private void UpdateItemsPosition()
        {

            if (Items.Count == 0) return;
            _itemsControl.BeginAnimation(MarginProperty,
                CreateAnimation(new Thickness(1, 0, 0, 0)));
            
        }
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateItemsPosition();
        }
       
        /// <summary>
        ///     上一个
        /// </summary>
        public static RoutedCommand Prev { get; } = new RoutedCommand(nameof(Prev), typeof(SwiperContent));

        /// <summary>
        ///     下一个
        /// </summary>
        public static RoutedCommand Next { get; } = new RoutedCommand(nameof(Next), typeof(SwiperContent));

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     创建一个Thickness动画
        /// </summary>
        /// <param name="thickness"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static ThicknessAnimation CreateAnimation(Thickness thickness = default, double milliseconds = 200)
        {
            return new ThicknessAnimation(thickness, new Duration(TimeSpan.FromMilliseconds(milliseconds)))
            {
                EasingFunction = new PowerEase { EasingMode = EasingMode.EaseInOut }
            };
        }
    }
}
