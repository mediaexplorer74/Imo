// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.DispatcherViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Collections;
using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class DispatcherViewModel : ViewModelBase
  {
    private string filterText;
    private ObservableCollection<Pair<string, string>> processedMessages;
    private ObservableCollection<Pair<string, string>> sentMessages;
    private ObservableCollection<Pair<string, string>> receivedMessages;

    public DispatcherViewModel(FrameworkElement el)
      : base(el)
    {
    }

    protected override void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.ProcessedMessages = new ObservableCollection<Pair<string, string>>(IMO.EventDispatcher.GetProcessedMessages().Select<TimestampedObject<JObject>, Pair<string, string>>((Func<TimestampedObject<JObject>, Pair<string, string>>) (x => new Pair<string, string>(Utils.ToFormattedSystemTimestamp(x.Timestamp), x.Value.ToString()))));
      this.ReceivedMessages = new ObservableCollection<Pair<string, string>>(IMO.Network.GetReceivedMessages().Select<TimestampedObject<JObject>, Pair<string, string>>((Func<TimestampedObject<JObject>, Pair<string, string>>) (x => new Pair<string, string>(Utils.ToFormattedSystemTimestamp(x.Timestamp), x.Value.ToString()))));
      this.SentMessages = new ObservableCollection<Pair<string, string>>(IMO.Network.GetSentMessages().Select<TimestampedObject<JObject>, Pair<string, string>>((Func<TimestampedObject<JObject>, Pair<string, string>>) (x => new Pair<string, string>(Utils.ToFormattedSystemTimestamp(x.Timestamp), x.Value.ToString()))));
      IMO.EventDispatcher.IncommingMessage += new EventHandler<EventArg<TimestampedObject<JObject>>>(this.ProcessedMessageHandler);
      IMO.Network.ReceivedMessage += new EventHandler<EventArg<TimestampedObject<JObject>>>(this.ReceivedMessageHandler);
      IMO.Network.SentMessage += new EventHandler<EventArg<TimestampedObject<JObject>>>(this.SentMessageHandler);
    }

    protected override void OnUnloaded(object sender, RoutedEventArgs e)
    {
      IMO.EventDispatcher.IncommingMessage -= new EventHandler<EventArg<TimestampedObject<JObject>>>(this.ProcessedMessageHandler);
      IMO.Network.ReceivedMessage -= new EventHandler<EventArg<TimestampedObject<JObject>>>(this.ReceivedMessageHandler);
      IMO.Network.SentMessage -= new EventHandler<EventArg<TimestampedObject<JObject>>>(this.SentMessageHandler);
    }

    private void SentMessageHandler(object sender, EventArg<TimestampedObject<JObject>> e)
    {
      this.sentMessages.Add(new Pair<string, string>(Utils.ToFormattedSystemTimestamp(e.Arg.Timestamp), e.Arg.Value.ToString()));
    }

    private void ReceivedMessageHandler(object sender, EventArg<TimestampedObject<JObject>> e)
    {
      this.receivedMessages.Add(new Pair<string, string>(Utils.ToFormattedSystemTimestamp(e.Arg.Timestamp), e.Arg.Value.ToString()));
    }

    private void ProcessedMessageHandler(object sender, EventArg<TimestampedObject<JObject>> e)
    {
      this.processedMessages.Add(new Pair<string, string>(Utils.ToFormattedSystemTimestamp(e.Arg.Timestamp), e.Arg.Value.ToString()));
    }

    public ObservableCollection<Pair<string, string>> ProcessedMessages
    {
      get => this.processedMessages;
      set
      {
        this.processedMessages = value;
        this.OnPropertyChanged(nameof (ProcessedMessages));
      }
    }

    public ObservableCollection<Pair<string, string>> SentMessages
    {
      get => this.sentMessages;
      set
      {
        this.sentMessages = value;
        this.OnPropertyChanged(nameof (SentMessages));
      }
    }

    public ObservableCollection<Pair<string, string>> ReceivedMessages
    {
      get => this.receivedMessages;
      set
      {
        this.receivedMessages = value;
        this.OnPropertyChanged(nameof (ReceivedMessages));
      }
    }

    public string FilterText
    {
      get => this.filterText;
      set
      {
        this.filterText = value;
        this.OnPropertyChanged(nameof (FilterText));
      }
    }
  }
}
