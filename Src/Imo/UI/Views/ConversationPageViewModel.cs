// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ConversationPageViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using Microsoft.Phone.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  public class ConversationPageViewModel : ViewModelBase
  {
    private string buid;
    private Contact contact;
    private Conversation conversation;
    private ChatHeadsViewModel chatHeadsViewModel;
    private ChatLogViewModel chatLogViewModel;
    private string messageText;
    private bool showBottomMenu = true;
    private bool showStickers;
    private bool showBusyIndicator;
    private readonly IList<string> sendVideoFileTypes = (IList<string>) new List<string>()
    {
      "mpeg",
      "mpg",
      "mpe",
      "m1s",
      "mpa",
      "mp2",
      "m2a",
      "mp2v",
      "m2v",
      "m2s",
      "avi",
      "mov",
      "qt",
      "asf",
      "asx",
      "wmv",
      "wma",
      "wmx",
      "rm",
      "ra",
      "ram",
      "rmvb",
      "mp4",
      "3gp",
      "ogm",
      "mkv",
      "flv",
      "webm",
      "m4v"
    };
    private readonly IList<string> sendPhotoFileTypes = (IList<string>) new List<string>()
    {
      "png",
      "jpg",
      "jpeg",
      "gif",
      "webp",
      "mp"
    };

    public ConversationPageViewModel(string buid, FrameworkElement element, ChatLog chatLog)
      : base(element)
    {
      this.buid = buid;
      this.contact = IMO.ContactsManager.GetOrCreateContact(buid);
      this.conversation = IMO.ConversationsManager.GetOrCreateConversation(buid);
      this.chatHeadsViewModel = new ChatHeadsViewModel(buid, element);
      this.chatLogViewModel = new ChatLogViewModel(buid, chatLog);
      this.conversation.PropertyChanged += new PropertyChangedEventHandler(this.Conversation_PropertyChanged);
      this.conversation.Messages.PropertyChanged += new PropertyChangedEventHandler(this.Messages_PropertyChanged);
      this.UpdateShowBusyIndicator();
    }

    public override void Dispose()
    {
      base.Dispose();
      this.conversation.PropertyChanged -= new PropertyChangedEventHandler(this.Conversation_PropertyChanged);
      this.conversation.Messages.PropertyChanged -= new PropertyChangedEventHandler(this.Messages_PropertyChanged);
    }

    private void Messages_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "Count"))
        return;
      this.UpdateShowBusyIndicator();
    }

    private void Conversation_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (!(e.PropertyName == "IsGettingRecent"))
        return;
      this.UpdateShowBusyIndicator();
    }

    private void UpdateShowBusyIndicator()
    {
      this.ShowBusyIndicator = this.conversation.Messages.Count == 0 && this.conversation.IsGettingRecent;
    }

    public IList<string> GetSendVideoFileTypes() => this.sendVideoFileTypes;

    public IList<string> GetSendPhotoFileTypes() => this.sendPhotoFileTypes;

    public Contact Contact => this.contact;

    public Conversation Conversation => this.conversation;

    public ChatHeadsViewModel ChatHeadsViewModel => this.chatHeadsViewModel;

    public ChatLogViewModel ChatLogViewModel => this.chatLogViewModel;

    public string MessageText
    {
      get => this.messageText;
      set
      {
        if (!(this.messageText != value))
          return;
        this.messageText = value;
        this.OnPropertyChanged(nameof (MessageText));
        this.OnPropertyChanged("HasMessage");
      }
    }

    public bool ShowBottomMenu
    {
      get => this.showBottomMenu;
      set
      {
        if (this.showBottomMenu == value)
          return;
        this.showBottomMenu = value;
        this.OnPropertyChanged(nameof (ShowBottomMenu));
      }
    }

    public bool ShowBusyIndicator
    {
      get => this.showBusyIndicator;
      set
      {
        if (this.showBusyIndicator == value)
          return;
        this.showBusyIndicator = value;
        this.OnPropertyChanged(nameof (ShowBusyIndicator));
      }
    }

    public bool ShowStickers
    {
      get => this.showStickers;
      set
      {
        if (this.showStickers == value)
          return;
        this.showStickers = value;
        this.OnPropertyChanged(nameof (ShowStickers));
      }
    }

    public bool HasMessage => !string.IsNullOrEmpty(this.messageText);

    internal void SendMesage()
    {
      if (string.IsNullOrEmpty(this.messageText))
        return;
      this.Conversation.SendMessage(this.messageText, new JObject());
    }

    internal async void SendPhoto(PhotoResult photoResult)
    {
      if (photoResult.ChosenPhoto == null)
        return;
      string pathFromPhotoResult = await FSUtils.GetFilePathFromPhotoResult(photoResult, "sendPhoto." + Utils.GetRandomString(8) + " .tmp");
      if (pathFromPhotoResult == null)
        return;
      this.conversation.SendPhoto(pathFromPhotoResult);
    }
  }
}
