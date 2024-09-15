// Decompiled with JetBrains decompiler
// Type: Microsoft.Phone.Controls.TransitionFrame
// Assembly: Coding4Fun.Toolkit.Controls, Version=2.1.7.0, Culture=neutral, PublicKeyToken=c5fd7b72b1a17ce4
// MVID: 213D8A75-4B66-4004-95C2-D82C49ACB764
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\Coding4Fun.Toolkit.Controls.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;


namespace Microsoft.Phone.Controls
{
  [TemplatePart(Name = "FirstContentPresenter", Type = typeof (ContentPresenter))]
  [TemplatePart(Name = "SecondContentPresenter", Type = typeof (ContentPresenter))]
  internal class TransitionFrame : PhoneApplicationFrame
  {
    private const string FirstTemplatePartName = "FirstContentPresenter";
    private const string SecondTemplatePartName = "SecondContentPresenter";
    internal static readonly CacheMode BitmapCacheMode = (CacheMode) new BitmapCache();
    private ContentPresenter _firstContentPresenter;
    private ContentPresenter _secondContentPresenter;
    private ContentPresenter _newContentPresenter;
    private ContentPresenter _oldContentPresenter;
    private bool _isForwardNavigation;
    private bool _useFirstAsNew;
    private bool _readyToTransitionToNewContent;
    private bool _contentReady;
    private bool _performingExitTransition;
    private bool _navigationStopped;
    private ITransition _storedNewTransition;
    private NavigationInTransition _storedNavigationInTransition;
    private ITransition _storedOldTransition;
    private NavigationOutTransition _storedNavigationOutTransition;

    public TransitionFrame()
    {
      this.DefaultStyleKey = (object) typeof (TransitionFrame);
      this.Navigating += new NavigatingCancelEventHandler(this.OnNavigating);
      this.NavigationStopped += new NavigationStoppedEventHandler(this.OnNavigationStopped);
    }

    private void FlipPresenters()
    {
      this._newContentPresenter = this._useFirstAsNew ? this._firstContentPresenter : this._secondContentPresenter;
      this._oldContentPresenter = this._useFirstAsNew ? this._secondContentPresenter : this._firstContentPresenter;
      this._useFirstAsNew = !this._useFirstAsNew;
    }

    private void OnNavigating(object sender, NavigatingCancelEventArgs e)
    {
      if (!e.IsNavigationInitiator)
        return;
      this._isForwardNavigation = e.NavigationMode != NavigationMode.Back;
      if (!(this.Content is UIElement content))
        return;
      this.EnsureLastTransitionIsComplete();
      this.FlipPresenters();
      TransitionElement transitionElement = (TransitionElement) null;
      ITransition transition = (ITransition) null;
      NavigationOutTransition navigationOutTransition = TransitionService.GetNavigationOutTransition(content);
      if (navigationOutTransition != null)
        transitionElement = this._isForwardNavigation ? navigationOutTransition.Forward : navigationOutTransition.Backward;
      if (transitionElement != null)
        transition = transitionElement.GetTransition(content);
      if (transition != null)
      {
        TransitionFrame.EnsureStoppedTransition(transition);
        this._storedNavigationOutTransition = navigationOutTransition;
        this._storedOldTransition = transition;
        transition.Completed += new EventHandler(this.OnExitTransitionCompleted);
        this._performingExitTransition = true;
        TransitionFrame.PerformTransition((NavigationTransition) navigationOutTransition, this._oldContentPresenter, transition);
        TransitionFrame.PrepareContentPresenterForCompositor(this._oldContentPresenter);
      }
      else
        this._readyToTransitionToNewContent = true;
    }

    private void OnNavigationStopped(object sender, NavigationEventArgs e)
    {
      this._navigationStopped = true;
    }

    private void EnsureLastTransitionIsComplete()
    {
      this._readyToTransitionToNewContent = false;
      this._contentReady = false;
      if (!this._performingExitTransition)
        return;
      this._oldContentPresenter.Content = (object) null;
      this._storedOldTransition.Stop();
      this._storedNavigationOutTransition = (NavigationOutTransition) null;
      this._storedOldTransition = (ITransition) null;
      if (this._storedNewTransition != null)
      {
        this._storedNewTransition.Stop();
        this._storedNewTransition = (ITransition) null;
        this._storedNavigationInTransition = (NavigationInTransition) null;
      }
      this._performingExitTransition = false;
    }

    private void OnExitTransitionCompleted(object sender, EventArgs e)
    {
      this._readyToTransitionToNewContent = true;
      this._performingExitTransition = false;
      if (this._navigationStopped)
      {
        TransitionFrame.CompleteTransition((NavigationTransition) this._storedNavigationOutTransition, this._oldContentPresenter, this._storedOldTransition);
        this._navigationStopped = false;
      }
      else
        TransitionFrame.CompleteTransition((NavigationTransition) this._storedNavigationOutTransition, (ContentPresenter) null, this._storedOldTransition);
      this._storedNavigationOutTransition = (NavigationOutTransition) null;
      this._storedOldTransition = (ITransition) null;
      if (!this._contentReady)
        return;
      ITransition storedNewTransition = this._storedNewTransition;
      NavigationInTransition navigationInTransition = this._storedNavigationInTransition;
      this._storedNewTransition = (ITransition) null;
      this._storedNavigationInTransition = (NavigationInTransition) null;
      this.TransitionNewContent(storedNewTransition, navigationInTransition);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this._firstContentPresenter = this.GetTemplateChild("FirstContentPresenter") as ContentPresenter;
      this._secondContentPresenter = this.GetTemplateChild("SecondContentPresenter") as ContentPresenter;
      this._newContentPresenter = this._secondContentPresenter;
      this._oldContentPresenter = this._firstContentPresenter;
      this._useFirstAsNew = true;
      this._readyToTransitionToNewContent = true;
      if (this.Content == null)
        return;
      this.OnContentChanged((object) null, this.Content);
    }

    protected override void OnContentChanged(object oldContent, object newContent)
    {
      base.OnContentChanged(oldContent, newContent);
      this._contentReady = true;
      UIElement uiElement = oldContent as UIElement;
      UIElement element = newContent as UIElement;
      if (this._firstContentPresenter == null || this._secondContentPresenter == null || element == null)
        return;
      NavigationInTransition navigationInTransition = TransitionService.GetNavigationInTransition(element);
      ITransition newTransition = (ITransition) null;
      TransitionElement transitionElement = (TransitionElement) null;
      if (navigationInTransition != null)
        transitionElement = this._isForwardNavigation ? navigationInTransition.Forward : navigationInTransition.Backward;
      if (transitionElement != null)
      {
        element.UpdateLayout();
        newTransition = transitionElement.GetTransition(element);
        TransitionFrame.PrepareContentPresenterForCompositor(this._newContentPresenter);
      }
      this._newContentPresenter.Opacity = 0.0;
      this._newContentPresenter.Visibility = Visibility.Visible;
      this._newContentPresenter.Content = (object) element;
      this._oldContentPresenter.Opacity = 1.0;
      this._oldContentPresenter.Visibility = Visibility.Visible;
      this._oldContentPresenter.Content = (object) uiElement;
      if (this._readyToTransitionToNewContent)
      {
        this.TransitionNewContent(newTransition, navigationInTransition);
      }
      else
      {
        this._storedNewTransition = newTransition;
        this._storedNavigationInTransition = navigationInTransition;
      }
    }

    private void TransitionNewContent(
      ITransition newTransition,
      NavigationInTransition navigationInTransition)
    {
      if (this._oldContentPresenter != null)
      {
        this._oldContentPresenter.Visibility = Visibility.Collapsed;
        this._oldContentPresenter.Content = (object) null;
      }
      if (newTransition == null)
      {
        TransitionFrame.RestoreContentPresenterInteractivity(this._newContentPresenter);
      }
      else
      {
        TransitionFrame.EnsureStoppedTransition(newTransition);
        newTransition.Completed += (EventHandler) ((_param1, _param2) => TransitionFrame.CompleteTransition((NavigationTransition) navigationInTransition, this._newContentPresenter, newTransition));
        this._readyToTransitionToNewContent = false;
        this._storedNavigationInTransition = (NavigationInTransition) null;
        this._storedNewTransition = (ITransition) null;
        TransitionFrame.PerformTransition((NavigationTransition) navigationInTransition, this._newContentPresenter, newTransition);
      }
    }

    private static void EnsureStoppedTransition(ITransition transition)
    {
      if (transition == null || transition.GetCurrentState() == ClockState.Stopped)
        return;
      transition.Stop();
    }

    private static void PerformTransition(
      NavigationTransition navigationTransition,
      ContentPresenter presenter,
      ITransition transition)
    {
      navigationTransition?.OnBeginTransition();
      if (presenter != null && presenter.Opacity != 1.0)
        presenter.Opacity = 1.0;
      transition?.Begin();
    }

    private static void CompleteTransition(
      NavigationTransition navigationTransition,
      ContentPresenter presenter,
      ITransition transition)
    {
      transition?.Stop();
      TransitionFrame.RestoreContentPresenterInteractivity(presenter);
      navigationTransition?.OnEndTransition();
    }

    private static void PrepareContentPresenterForCompositor(
      ContentPresenter presenter,
      bool applyBitmapCache = true)
    {
      if (presenter == null)
        return;
      if (applyBitmapCache)
        presenter.CacheMode = TransitionFrame.BitmapCacheMode;
      presenter.IsHitTestVisible = false;
    }

    private static void RestoreContentPresenterInteractivity(ContentPresenter presenter)
    {
      if (presenter == null)
        return;
      presenter.CacheMode = (CacheMode) null;
      presenter.Opacity = 1.0;
      presenter.IsHitTestVisible = true;
    }
  }
}
