// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.UI.Views.ShareToMembersPageViewModel
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using ImoSilverlightApp.Storage.Models;
using Microsoft.Xna.Framework.Media;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace ImoSilverlightApp.UI.Views
{
  internal class ShareToMembersPageViewModel : ViewModelBase
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (ShareToMembersPageViewModel).Name);
    private bool isShareEnabled;
    private string filePath;
    private string objectId;
    private string objectType;
    private ContactSelectorViewModel contactSelectorVideModel;

    public ShareToMembersPageViewModel(
      FrameworkElement el,
      string filePath,
      string objectId,
      string objectType,
      ContactSelectorView contactSelector)
      : base(el)
    {
      this.contactSelectorVideModel = contactSelector.ViewModel;
      this.contactSelectorVideModel.SelectedItems.CollectionChanged += new NotifyCollectionChangedEventHandler(this.SelectedItems_CollectionChanged);
      this.filePath = filePath;
      this.objectId = objectId;
      this.objectType = objectType;
    }

    public override void Dispose()
    {
      base.Dispose();
      if (this.contactSelectorVideModel == null)
        return;
      this.contactSelectorVideModel.SelectedItems.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.SelectedItems_CollectionChanged);
      this.contactSelectorVideModel = (ContactSelectorViewModel) null;
    }

    private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.IsShareEnabled = this.contactSelectorVideModel.SelectedItems.Count != 0;
    }

    public bool IsShareEnabled
    {
      get => this.isShareEnabled;
      set
      {
        if (this.isShareEnabled == value)
          return;
        this.isShareEnabled = value;
        this.OnPropertyChanged(nameof (IsShareEnabled));
      }
    }

    public string VideoFilePath => this.filePath;

    internal string GetSingleSelectedBuid()
    {
      IList<Contact> selectedContacts = this.contactSelectorVideModel.GetSelectedContacts();
      return selectedContacts.Count == 1 ? selectedContacts[0].Buid : (string) null;
    }

    internal async Task DoShare()
    {
      IList<Contact> selectedContacts = this.contactSelectorVideModel.GetSelectedContacts();
      if (string.IsNullOrEmpty(this.objectId))
      {
        if (string.IsNullOrEmpty(this.filePath))
        {
          ShareToMembersPageViewModel.log.Error("objectId and filePath are null/empty", 97, nameof (DoShare));
        }
        else
        {
          List<VideoMessage> previewMessages = new List<VideoMessage>();
          foreach (Contact contact in (IEnumerable<Contact>) selectedContacts)
          {
            VideoMessage videoMessage = IMO.ConversationsManager.GetOrCreateConversation(contact.Buid).AddSendVideoMessage(this.filePath);
            previewMessages.Add(videoMessage);
          }
          IMO.MediaUploader.UploadVideo(this.filePath, (Action<string>) (videoId =>
          {
            int index = 0;
            foreach (Contact contact in (IEnumerable<Contact>) selectedContacts)
            {
              IMO.ConversationsManager.GetOrCreateConversation(contact.Buid).SendUploadedVideo(videoId, previewMessages[index]);
              ++index;
            }
          }));
        }
      }
      else if (this.objectType.StartsWith("file"))
      {
        string members = await Conversation.SendPhotoToMembers(await FSUtils.GetFilePathFromPicture(new MediaLibrary().GetPictureFromToken(this.objectId), "sendPhoto." + Utils.GetRandomString(8) + " .tmp"), selectedContacts.Select<Contact, Conversation>((Func<Contact, Conversation>) (x => IMO.ConversationsManager.GetOrCreateConversation(x.Buid))));
      }
      else
      {
        foreach (Contact contact in (IEnumerable<Contact>) selectedContacts)
        {
          Conversation conversation = IMO.ConversationsManager.GetOrCreateConversation(contact.Buid);
          if (this.objectType.StartsWith("photo"))
            conversation.SharePhotoWith(contact.Buid, this.objectId);
          else if (this.objectType.StartsWith("video"))
            conversation.ShareVideoWith(contact.Buid, this.objectId);
        }
      }
    }
  }
}
