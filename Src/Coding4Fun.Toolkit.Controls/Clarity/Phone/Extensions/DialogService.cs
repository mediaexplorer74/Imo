// Decompiled with JetBrains decompiler
// Type: Clarity.Phone.Extensions.DialogService
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using Coding4Fun.Toolkit.Controls;
using Coding4Fun.Toolkit.Controls.Binding;
using Coding4Fun.Toolkit.Controls.Common;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;


namespace Clarity.Phone.Extensions
{
  public class DialogService
  {
    private const string SlideUpStoryboard = "\r\n        <Storyboard  xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.RenderTransform).(TranslateTransform.Y)\">\r\n                <EasingDoubleKeyFrame KeyTime=\"0\" Value=\"150\"/>\r\n                <EasingDoubleKeyFrame KeyTime=\"0:0:0.35\" Value=\"0\">\r\n                    <EasingDoubleKeyFrame.EasingFunction>\r\n                        <ExponentialEase EasingMode=\"EaseOut\" Exponent=\"6\"/>\r\n                    </EasingDoubleKeyFrame.EasingFunction>\r\n                </EasingDoubleKeyFrame>\r\n            </DoubleAnimationUsingKeyFrames>\r\n            <DoubleAnimation Storyboard.TargetProperty=\"(UIElement.Opacity)\" From=\"0\" To=\"1\" Duration=\"0:0:0.350\">\r\n                <DoubleAnimation.EasingFunction>\r\n                    <ExponentialEase EasingMode=\"EaseOut\" Exponent=\"6\"/>\r\n                </DoubleAnimation.EasingFunction>\r\n            </DoubleAnimation>\r\n        </Storyboard>";
    private const string SlideHorizontalInStoryboard = "\r\n        <Storyboard  xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.RenderTransform).(TranslateTransform.X)\" >\r\n                    <EasingDoubleKeyFrame KeyTime=\"0\" Value=\"-150\"/>\r\n                    <EasingDoubleKeyFrame KeyTime=\"0:0:0.35\" Value=\"0\">\r\n                        <EasingDoubleKeyFrame.EasingFunction>\r\n                            <ExponentialEase EasingMode=\"EaseOut\" Exponent=\"6\"/>\r\n                        </EasingDoubleKeyFrame.EasingFunction>\r\n                    </EasingDoubleKeyFrame>\r\n                </DoubleAnimationUsingKeyFrames>\r\n            <DoubleAnimation Storyboard.TargetProperty=\"(UIElement.Opacity)\" From=\"0\" To=\"1\" Duration=\"0:0:0.350\" >\r\n                <DoubleAnimation.EasingFunction>\r\n                    <ExponentialEase EasingMode=\"EaseOut\" Exponent=\"6\"/>\r\n                </DoubleAnimation.EasingFunction>\r\n            </DoubleAnimation>\r\n        </Storyboard>";
    private const string SlideHorizontalOutStoryboard = "\r\n        <Storyboard  xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.RenderTransform).(TranslateTransform.X)\">\r\n                <EasingDoubleKeyFrame KeyTime=\"0\" Value=\"0\"/>\r\n                <EasingDoubleKeyFrame KeyTime=\"0:0:0.25\" Value=\"150\">\r\n                    <EasingDoubleKeyFrame.EasingFunction>\r\n                        <ExponentialEase EasingMode=\"EaseIn\" Exponent=\"6\"/>\r\n                    </EasingDoubleKeyFrame.EasingFunction>\r\n                </EasingDoubleKeyFrame>\r\n            </DoubleAnimationUsingKeyFrames>\r\n            <DoubleAnimation Storyboard.TargetProperty=\"(UIElement.Opacity)\" From=\"1\" To=\"0\" Duration=\"0:0:0.25\">\r\n                <DoubleAnimation.EasingFunction>\r\n                    <ExponentialEase EasingMode=\"EaseIn\" Exponent=\"6\"/>\r\n                </DoubleAnimation.EasingFunction>\r\n            </DoubleAnimation>\r\n        </Storyboard>";
    private const string SlideDownStoryboard = "\r\n        <Storyboard  xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.RenderTransform).(TranslateTransform.Y)\">\r\n                <EasingDoubleKeyFrame KeyTime=\"0\" Value=\"0\"/>\r\n                <EasingDoubleKeyFrame KeyTime=\"0:0:0.25\" Value=\"150\">\r\n                    <EasingDoubleKeyFrame.EasingFunction>\r\n                        <ExponentialEase EasingMode=\"EaseIn\" Exponent=\"6\"/>\r\n                    </EasingDoubleKeyFrame.EasingFunction>\r\n                </EasingDoubleKeyFrame>\r\n            </DoubleAnimationUsingKeyFrames>\r\n            <DoubleAnimation Storyboard.TargetProperty=\"(UIElement.Opacity)\" From=\"1\" To=\"0\" Duration=\"0:0:0.25\">\r\n                <DoubleAnimation.EasingFunction>\r\n                    <ExponentialEase EasingMode=\"EaseIn\" Exponent=\"6\"/>\r\n                </DoubleAnimation.EasingFunction>\r\n            </DoubleAnimation>\r\n        </Storyboard>";
    private const string SwivelInStoryboard = "<Storyboard xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimation \r\n\t\t\t\tTo=\".5\"\r\n                Storyboard.TargetProperty=\"(UIElement.Projection).(PlaneProjection.CenterOfRotationY)\" />\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.Projection).(PlaneProjection.RotationX)\">\r\n                <EasingDoubleKeyFrame KeyTime=\"0\" Value=\"-30\"/>\r\n                <EasingDoubleKeyFrame KeyTime=\"0:0:0.35\" Value=\"0\">\r\n                    <EasingDoubleKeyFrame.EasingFunction>\r\n                        <ExponentialEase EasingMode=\"EaseOut\" Exponent=\"6\"/>\r\n                    </EasingDoubleKeyFrame.EasingFunction>\r\n                </EasingDoubleKeyFrame>\r\n            </DoubleAnimationUsingKeyFrames>\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.Opacity)\">\r\n                <DiscreteDoubleKeyFrame KeyTime=\"0\" Value=\"1\" />\r\n            </DoubleAnimationUsingKeyFrames>\r\n        </Storyboard>";
    private const string SwivelOutStoryboard = "<Storyboard xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimation BeginTime=\"0:0:0\" Duration=\"0\" \r\n                                Storyboard.TargetProperty=\"(UIElement.Projection).(PlaneProjection.CenterOfRotationY)\" \r\n                                To=\".5\"/>\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.Projection).(PlaneProjection.RotationX)\">\r\n                <EasingDoubleKeyFrame KeyTime=\"0\" Value=\"0\"/>\r\n                <EasingDoubleKeyFrame KeyTime=\"0:0:0.25\" Value=\"45\">\r\n                    <EasingDoubleKeyFrame.EasingFunction>\r\n                        <ExponentialEase EasingMode=\"EaseIn\" Exponent=\"6\"/>\r\n                    </EasingDoubleKeyFrame.EasingFunction>\r\n                </EasingDoubleKeyFrame>\r\n            </DoubleAnimationUsingKeyFrames>\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.Opacity)\">\r\n                <DiscreteDoubleKeyFrame KeyTime=\"0\" Value=\"1\" />\r\n                <DiscreteDoubleKeyFrame KeyTime=\"0:0:0.267\" Value=\"0\" />\r\n            </DoubleAnimationUsingKeyFrames>\r\n        </Storyboard>";
    private const string FadeInStoryboard = "<Storyboard xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimation \r\n\t\t\t\tDuration=\"0:0:0.2\" \r\n\t\t\t\tStoryboard.TargetProperty=\"(UIElement.Opacity)\" \r\n                To=\"1\"/>\r\n        </Storyboard>";
    private const string FadeOutStoryboard = "<Storyboard xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimation \r\n\t\t\t\tDuration=\"0:0:0.2\"\r\n\t\t\t\tStoryboard.TargetProperty=\"(UIElement.Opacity)\" \r\n                To=\"0\"/>\r\n        </Storyboard>";
    private Panel _popupContainer;
    private Frame _rootFrame;
    private PhoneApplicationPage _page;
    private Grid _childPanel;
    private Grid _overlay;
    private bool _isOverlayApplied = true;
    private bool _deferredShowToLoaded;
    private static readonly object Lockobj = new object();

    public bool IsOverlayApplied
    {
      get => this._isOverlayApplied;
      set => this._isOverlayApplied = value;
    }

    public FrameworkElement Child { get; set; }

    public DialogService.AnimationTypes AnimationType { get; set; }

    public TimeSpan MainBodyDelay { get; set; }

    public double VerticalOffset { get; set; }

    internal double ControlVerticalOffset { get; set; }

    public bool BackButtonPressed { get; set; }

    public Brush BackgroundBrush { get; set; }

    internal bool IsOpen { get; set; }

    protected internal bool IsBackKeyOverride { get; set; }

    public event EventHandler Closed;

    public event EventHandler Opened;

    public bool HasPopup { get; set; }

    internal PhoneApplicationPage Page
    {
      get
      {
        return this._page ?? (this._page = this.RootFrame.GetFirstLogicalChildByType<PhoneApplicationPage>(false));
      }
    }

    internal Frame RootFrame => this._rootFrame ?? (this._rootFrame = ApplicationSpace.RootFrame);

    internal Panel PopupContainer
    {
      get
      {
        if (this._popupContainer == null)
        {
          IEnumerable<ContentPresenter> logicalChildrenByType1 = this.RootFrame.GetLogicalChildrenByType<ContentPresenter>(false);
          for (int index = 0; index < logicalChildrenByType1.Count<ContentPresenter>(); ++index)
          {
            IEnumerable<Panel> logicalChildrenByType2 = logicalChildrenByType1.ElementAt<ContentPresenter>(index).GetLogicalChildrenByType<Panel>(false);
            if (logicalChildrenByType2.Any<Panel>())
            {
              this._popupContainer = logicalChildrenByType2.First<Panel>();
              break;
            }
          }
        }
        return this._popupContainer;
      }
    }

    public DialogService()
    {
      this.AnimationType = DialogService.AnimationTypes.Slide;
      this.BackButtonPressed = false;
    }

    private void InitializePopup()
    {
      this._childPanel = this.CreateGrid();
      if (this.IsOverlayApplied)
      {
        this._overlay = this.CreateGrid();
        PreventScrollBinding.SetIsEnabled((DependencyObject) this._overlay, true);
      }
      this.ApplyOverlayBackground();
      if (this.PopupContainer != null)
      {
        if (this._overlay != null)
          this.PopupContainer.Children.Add((UIElement) this._overlay);
        this.PopupContainer.Children.Add((UIElement) this._childPanel);
        this._childPanel.Children.Add((UIElement) this.Child);
      }
      else
      {
        this._deferredShowToLoaded = true;
        this.RootFrame.Loaded += new RoutedEventHandler(this.RootFrameDeferredShowLoaded);
      }
    }

    internal void ApplyOverlayBackground()
    {
      if (!this.IsOverlayApplied || this.BackgroundBrush == null)
        return;
      this._overlay.Background = this.BackgroundBrush;
    }

    private Grid CreateGrid()
    {
      Grid grid = new Grid();
      grid.Name = Guid.NewGuid().ToString();
      Grid element = grid;
      Grid.SetColumnSpan((FrameworkElement) element, int.MaxValue);
      Grid.SetRowSpan((FrameworkElement) element, int.MaxValue);
      element.Opacity = 0.0;
      this.CalculateVerticalOffset((Panel) element);
      return element;
    }

    internal void CalculateVerticalOffset()
    {
      this.CalculateVerticalOffset((Panel) this._childPanel);
    }

    internal void CalculateVerticalOffset(Panel panel)
    {
      if (panel == null)
        return;
      int num = 0;
      if (SystemTray.IsVisible && SystemTray.Opacity < 1.0 && SystemTray.Opacity > 0.0)
        num += 32;
      panel.Margin = new Thickness(0.0, this.VerticalOffset + (double) num + this.ControlVerticalOffset, 0.0, 0.0);
    }

    private void RootFrameDeferredShowLoaded(object sender, RoutedEventArgs e)
    {
      this.RootFrame.Loaded -= new RoutedEventHandler(this.RootFrameDeferredShowLoaded);
      this._deferredShowToLoaded = false;
      this.Show();
    }

    protected internal void SetAlignmentsOnOverlay(
      HorizontalAlignment horizontalAlignment,
      VerticalAlignment verticalAlignment)
    {
      if (this._childPanel == null)
        return;
      this._childPanel.HorizontalAlignment = horizontalAlignment;
      this._childPanel.VerticalAlignment = verticalAlignment;
    }

    public void Show()
    {
      lock (DialogService.Lockobj)
      {
        this.Page.BackKeyPress -= new EventHandler<CancelEventArgs>(this.OnBackKeyPress);
        this.IsOpen = true;
        this.InitializePopup();
        if (this._deferredShowToLoaded)
          return;
        if (!this.IsBackKeyOverride)
          this.Page.BackKeyPress += new EventHandler<CancelEventArgs>(this.OnBackKeyPress);
        this.Page.NavigationService.Navigated += new NavigatedEventHandler(this.OnNavigated);
        this.RunShowStoryboard((UIElement) this._overlay, DialogService.AnimationTypes.Fade);
        this.RunShowStoryboard((UIElement) this._childPanel, this.AnimationType, this.MainBodyDelay);
        if (this.Opened == null)
          return;
        this.Opened((object) this, (EventArgs) null);
      }
    }

    private void RunShowStoryboard(UIElement grid, DialogService.AnimationTypes animation)
    {
      this.RunShowStoryboard(grid, animation, TimeSpan.MinValue);
    }

    private async void RunShowStoryboard(
      UIElement grid,
      DialogService.AnimationTypes animation,
      TimeSpan delay)
    {
      if (grid == null)
        return;
      Storyboard storyboard;
      switch (animation)
      {
        case DialogService.AnimationTypes.Slide:
          storyboard = XamlReader.Load("\r\n        <Storyboard  xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.RenderTransform).(TranslateTransform.Y)\">\r\n                <EasingDoubleKeyFrame KeyTime=\"0\" Value=\"150\"/>\r\n                <EasingDoubleKeyFrame KeyTime=\"0:0:0.35\" Value=\"0\">\r\n                    <EasingDoubleKeyFrame.EasingFunction>\r\n                        <ExponentialEase EasingMode=\"EaseOut\" Exponent=\"6\"/>\r\n                    </EasingDoubleKeyFrame.EasingFunction>\r\n                </EasingDoubleKeyFrame>\r\n            </DoubleAnimationUsingKeyFrames>\r\n            <DoubleAnimation Storyboard.TargetProperty=\"(UIElement.Opacity)\" From=\"0\" To=\"1\" Duration=\"0:0:0.350\">\r\n                <DoubleAnimation.EasingFunction>\r\n                    <ExponentialEase EasingMode=\"EaseOut\" Exponent=\"6\"/>\r\n                </DoubleAnimation.EasingFunction>\r\n            </DoubleAnimation>\r\n        </Storyboard>") as Storyboard;
          grid.RenderTransform = (Transform) new TranslateTransform();
          break;
        case DialogService.AnimationTypes.SlideHorizontal:
          storyboard = XamlReader.Load("\r\n        <Storyboard  xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.RenderTransform).(TranslateTransform.X)\" >\r\n                    <EasingDoubleKeyFrame KeyTime=\"0\" Value=\"-150\"/>\r\n                    <EasingDoubleKeyFrame KeyTime=\"0:0:0.35\" Value=\"0\">\r\n                        <EasingDoubleKeyFrame.EasingFunction>\r\n                            <ExponentialEase EasingMode=\"EaseOut\" Exponent=\"6\"/>\r\n                        </EasingDoubleKeyFrame.EasingFunction>\r\n                    </EasingDoubleKeyFrame>\r\n                </DoubleAnimationUsingKeyFrames>\r\n            <DoubleAnimation Storyboard.TargetProperty=\"(UIElement.Opacity)\" From=\"0\" To=\"1\" Duration=\"0:0:0.350\" >\r\n                <DoubleAnimation.EasingFunction>\r\n                    <ExponentialEase EasingMode=\"EaseOut\" Exponent=\"6\"/>\r\n                </DoubleAnimation.EasingFunction>\r\n            </DoubleAnimation>\r\n        </Storyboard>") as Storyboard;
          grid.RenderTransform = (Transform) new TranslateTransform();
          break;
        case DialogService.AnimationTypes.Fade:
          storyboard = XamlReader.Load("<Storyboard xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimation \r\n\t\t\t\tDuration=\"0:0:0.2\" \r\n\t\t\t\tStoryboard.TargetProperty=\"(UIElement.Opacity)\" \r\n                To=\"1\"/>\r\n        </Storyboard>") as Storyboard;
          break;
        default:
          storyboard = XamlReader.Load("<Storyboard xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimation \r\n\t\t\t\tTo=\".5\"\r\n                Storyboard.TargetProperty=\"(UIElement.Projection).(PlaneProjection.CenterOfRotationY)\" />\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.Projection).(PlaneProjection.RotationX)\">\r\n                <EasingDoubleKeyFrame KeyTime=\"0\" Value=\"-30\"/>\r\n                <EasingDoubleKeyFrame KeyTime=\"0:0:0.35\" Value=\"0\">\r\n                    <EasingDoubleKeyFrame.EasingFunction>\r\n                        <ExponentialEase EasingMode=\"EaseOut\" Exponent=\"6\"/>\r\n                    </EasingDoubleKeyFrame.EasingFunction>\r\n                </EasingDoubleKeyFrame>\r\n            </DoubleAnimationUsingKeyFrames>\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.Opacity)\">\r\n                <DiscreteDoubleKeyFrame KeyTime=\"0\" Value=\"1\" />\r\n            </DoubleAnimationUsingKeyFrames>\r\n        </Storyboard>") as Storyboard;
          grid.Projection = (Projection) new PlaneProjection();
          break;
      }
      if (storyboard == null)
        return;
      foreach (Timeline child in (PresentationFrameworkCollection<Timeline>) storyboard.Children)
      {
        if (child is DoubleAnimationUsingKeyFrames)
        {
          foreach (DoubleKeyFrame keyFrame in (PresentationFrameworkCollection<DoubleKeyFrame>) (child as DoubleAnimationUsingKeyFrames).KeyFrames)
            keyFrame.KeyTime = KeyTime.FromTimeSpan(keyFrame.KeyTime.TimeSpan.Add(delay));
        }
      }
      this.Page.Dispatcher.BeginInvoke((Action) (() =>
      {
        foreach (Timeline child in (PresentationFrameworkCollection<Timeline>) storyboard.Children)
          Storyboard.SetTarget(child, (DependencyObject) grid);
        storyboard.Begin();
      }));
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
      if (!e.IsNavigationInitiator)
        return;
      this.Hide();
    }

    public void Hide()
    {
      if (!this.IsOpen)
        return;
      if (this.Page != null)
      {
        this.Page.BackKeyPress -= new EventHandler<CancelEventArgs>(this.OnBackKeyPress);
        this.Page.NavigationService.Navigated -= new NavigatedEventHandler(this.OnNavigated);
        this._page = (PhoneApplicationPage) null;
      }
      this.RunHideStoryboard(this._overlay, DialogService.AnimationTypes.Fade);
      this.RunHideStoryboard(this._childPanel, this.AnimationType);
    }

    private void RunHideStoryboard(Grid grid, DialogService.AnimationTypes animation)
    {
      if (grid == null)
        return;
      Storyboard storyboard;
      switch (animation)
      {
        case DialogService.AnimationTypes.Slide:
          storyboard = XamlReader.Load("\r\n        <Storyboard  xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.RenderTransform).(TranslateTransform.Y)\">\r\n                <EasingDoubleKeyFrame KeyTime=\"0\" Value=\"0\"/>\r\n                <EasingDoubleKeyFrame KeyTime=\"0:0:0.25\" Value=\"150\">\r\n                    <EasingDoubleKeyFrame.EasingFunction>\r\n                        <ExponentialEase EasingMode=\"EaseIn\" Exponent=\"6\"/>\r\n                    </EasingDoubleKeyFrame.EasingFunction>\r\n                </EasingDoubleKeyFrame>\r\n            </DoubleAnimationUsingKeyFrames>\r\n            <DoubleAnimation Storyboard.TargetProperty=\"(UIElement.Opacity)\" From=\"1\" To=\"0\" Duration=\"0:0:0.25\">\r\n                <DoubleAnimation.EasingFunction>\r\n                    <ExponentialEase EasingMode=\"EaseIn\" Exponent=\"6\"/>\r\n                </DoubleAnimation.EasingFunction>\r\n            </DoubleAnimation>\r\n        </Storyboard>") as Storyboard;
          break;
        case DialogService.AnimationTypes.SlideHorizontal:
          storyboard = XamlReader.Load("\r\n        <Storyboard  xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.RenderTransform).(TranslateTransform.X)\">\r\n                <EasingDoubleKeyFrame KeyTime=\"0\" Value=\"0\"/>\r\n                <EasingDoubleKeyFrame KeyTime=\"0:0:0.25\" Value=\"150\">\r\n                    <EasingDoubleKeyFrame.EasingFunction>\r\n                        <ExponentialEase EasingMode=\"EaseIn\" Exponent=\"6\"/>\r\n                    </EasingDoubleKeyFrame.EasingFunction>\r\n                </EasingDoubleKeyFrame>\r\n            </DoubleAnimationUsingKeyFrames>\r\n            <DoubleAnimation Storyboard.TargetProperty=\"(UIElement.Opacity)\" From=\"1\" To=\"0\" Duration=\"0:0:0.25\">\r\n                <DoubleAnimation.EasingFunction>\r\n                    <ExponentialEase EasingMode=\"EaseIn\" Exponent=\"6\"/>\r\n                </DoubleAnimation.EasingFunction>\r\n            </DoubleAnimation>\r\n        </Storyboard>") as Storyboard;
          break;
        case DialogService.AnimationTypes.Fade:
          storyboard = XamlReader.Load("<Storyboard xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimation \r\n\t\t\t\tDuration=\"0:0:0.2\"\r\n\t\t\t\tStoryboard.TargetProperty=\"(UIElement.Opacity)\" \r\n                To=\"0\"/>\r\n        </Storyboard>") as Storyboard;
          break;
        default:
          storyboard = XamlReader.Load("<Storyboard xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\r\n            <DoubleAnimation BeginTime=\"0:0:0\" Duration=\"0\" \r\n                                Storyboard.TargetProperty=\"(UIElement.Projection).(PlaneProjection.CenterOfRotationY)\" \r\n                                To=\".5\"/>\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.Projection).(PlaneProjection.RotationX)\">\r\n                <EasingDoubleKeyFrame KeyTime=\"0\" Value=\"0\"/>\r\n                <EasingDoubleKeyFrame KeyTime=\"0:0:0.25\" Value=\"45\">\r\n                    <EasingDoubleKeyFrame.EasingFunction>\r\n                        <ExponentialEase EasingMode=\"EaseIn\" Exponent=\"6\"/>\r\n                    </EasingDoubleKeyFrame.EasingFunction>\r\n                </EasingDoubleKeyFrame>\r\n            </DoubleAnimationUsingKeyFrames>\r\n            <DoubleAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.Opacity)\">\r\n                <DiscreteDoubleKeyFrame KeyTime=\"0\" Value=\"1\" />\r\n                <DiscreteDoubleKeyFrame KeyTime=\"0:0:0.267\" Value=\"0\" />\r\n            </DoubleAnimationUsingKeyFrames>\r\n        </Storyboard>") as Storyboard;
          break;
      }
      try
      {
        if (storyboard == null)
          return;
        storyboard.Completed += new EventHandler(this.HideStoryboardCompleted);
        foreach (Timeline child in (PresentationFrameworkCollection<Timeline>) storyboard.Children)
          Storyboard.SetTarget(child, (DependencyObject) grid);
        storyboard.Begin();
      }
      catch (Exception ex)
      {
        this.HideStoryboardCompleted((object) null, (EventArgs) null);
      }
    }

    private void HideStoryboardCompleted(object sender, EventArgs e)
    {
      this.IsOpen = false;
      try
      {
        if (this.PopupContainer != null && this.PopupContainer.Children != null)
        {
          if (this._overlay != null)
            this.PopupContainer.Children.Remove((UIElement) this._overlay);
          this.PopupContainer.Children.Remove((UIElement) this._childPanel);
        }
        this._childPanel.Children.Clear();
      }
      catch
      {
      }
      try
      {
        if (this.Closed == null)
          return;
        this.Closed((object) this, (EventArgs) null);
      }
      catch
      {
      }
    }

    public void OnBackKeyPress(object sender, CancelEventArgs e)
    {
      if (this.HasPopup)
      {
        e.Cancel = true;
      }
      else
      {
        if (!this.IsOpen)
          return;
        e.Cancel = true;
        this.BackButtonPressed = true;
        this.Hide();
      }
    }

    public enum AnimationTypes
    {
      Slide,
      SlideHorizontal,
      Swivel,
      SwivelHorizontal,
      Fade,
    }
  }
}
