using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HY.CustomControl
{
    /// <summary>
    ///     轮播控件
    /// </summary>
    //[TemplatePart(Name = ElementPanelPage, Type = typeof(Panel))]
    [TemplatePart(Name = ElementItemsControl, Type = typeof(ItemsPresenter))]
    public class SwiperContent : ListBox, IDisposable
    {


        public SwiperContent()
        {
            CommandBindings.Add(new CommandBinding(Prev, ButtonPrev_OnClick));
            CommandBindings.Add(new CommandBinding(Next, ButtonNext_OnClick));

        }
        private void ButtonPrev_OnClick(object sender, RoutedEventArgs e) => PageIndex--;

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e) => PageIndex++;
        //private const string ElementPanelPage = "PART_PanelPage";
        private const string ElementItemsControl = "PART_ItemsControl";
        private int _pageIndex = -1;

        /// <summary>
        ///     页码
        /// </summary>
        public int PageIndex
        {
            get => _pageIndex;
            set
            {
                if (Items.Count == 0) return;
                if (_pageIndex == value) return;
                if (value < 0)
                    _pageIndex = Items.Count - 1;
                else if (value >= Items.Count)
                    _pageIndex = 0;
                else
                    _pageIndex = value;
                //UpdatePageButtons(_pageIndex);
            }
        }
        //private Panel _panelPage;
        private ItemsPresenter _itemsControl;
        public override void OnApplyTemplate()
        {


            //_panelPage?.RemoveHandler(Button.ClickEvent, new RoutedEventHandler(ButtonPages_OnClick));

            base.OnApplyTemplate();

            _itemsControl = GetTemplateChild(ElementItemsControl) as ItemsPresenter;
            //_panelPage = GetTemplateChild(ElementPanelPage) as Panel;
         
            if (ItemsSource == null)
            {
                return;

            }
            AllItemSource = (IEnumerable<dynamic>)ItemsSource;
            //_panelPage.AddHandler(Button.ClickEvent, new RoutedEventHandler(ButtonPages_OnClick));

            Update();
        }
        private void Update()
        {
            UpdatePageButtons(_pageIndex);
        }
        /// <summary>
        ///     更新页按钮
        /// </summary>
        public void UpdatePageButtons(int index = -1)
        {

            var count = AllItemSource.Count();

            var width = .0;
            this.ItemsSource = AllItemSource.First();
            //foreach (FrameworkElement item in ItemsSource)
            //{

            //    item.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            //    width += item.DesiredSize.Width;

            //}

            //_itemsControl.Width = _widthList.Last() + ExtendWidth;
            //_panelPage.Children.Clear();
            //for (var i = 0; i < count; i++)
            //{
            //    _panelPage.Children.Add(new RadioButton
            //    {
            //        Style = PageButtonStyle
            //    });
            //}

            //if (index == -1 && count > 0) index = 0;
            //if (index >= 0 && index < count)
            //{
            //    if (_panelPage.Children[index] is RadioButton button)
            //    {
            //        button.IsChecked = true;
            //        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, button));
            //        UpdateItemsPosition();
            //    }
            //}
        }
        public static readonly DependencyProperty PageButtonStyleProperty = DependencyProperty.Register(
           "PageButtonStyle", typeof(Style), typeof(SwiperContent), new PropertyMetadata(default(Style)));

        public Style PageButtonStyle
        {
            get => (Style)GetValue(PageButtonStyleProperty);
            set => SetValue(PageButtonStyleProperty, value);
        }

        public static readonly DependencyProperty AllItemSourcePropertyProperty =
  DependencyProperty.Register("ItemsSourceProperty", typeof(IEnumerable<dynamic>), typeof(SwiperContent), new PropertyMetadata(null));
        public IEnumerable<dynamic> AllItemSource
        {
            get => (IEnumerable<dynamic>)GetValue(AllItemSourcePropertyProperty);
            set => SetValue(AllItemSourcePropertyProperty, value);
        }

        /// <summary>
        ///  更新项的位置
        /// </summary>
        private void UpdateItemsPosition()
        {

            if (Items.Count == 0) return;
            //if (!IsCenter)
            //{
            //    _itemsControl.BeginAnimation(MarginProperty,
            //        AnimationHelper.CreateAnimation(new Thickness(1, 0, 0, 0)));
            //}
            //else
            //{
            //var ctl = (FrameworkElement)Items[PageIndex];
            //var ctlWidth = ctl.DesiredSize.Width;
            //_itemsControl.BeginAnimation(MarginProperty,
            //    AnimationHelper.CreateAnimation(
            //        new Thickness(4 / 2, 0, 0, 0)));
            //}
        }
        private void ButtonPages_OnClick(object sender, RoutedEventArgs e)
        {


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
    }
}
