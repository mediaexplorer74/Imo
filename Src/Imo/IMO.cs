// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.IMO
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.AV;
using ImoSilverlightApp.Connection;
using ImoSilverlightApp.Managers;
using ImoSilverlightApp.Storage;
using ImoSilverlightApp.Storage.Models;
using NLog;


namespace ImoSilverlightApp
{
  internal class IMO
  {
    private static readonly Logger log = LogManager.GetLogger(typeof (IMO).Name);
    public static AVManager AVManager;
    public static ImoAccount ImoAccount;
    public static AccountManager AccountManager;
    public static FeedbackManager FeedbackManager;
    public static Network Network;
    public static ImoProfile ImoProfile;
    public static MonitorLog MonitorLog;
    public static Session Session;
    public static QueryEngine QueryEngine;
    public static IM IM;
    public static FileUploader FileUploader;
    public static PixelDownloader PixelDownloader;
    public static MediaUploader MediaUploader;
    public static Pixel Pixel;
    public static PhotoStreamsManager PhotoStreamsManager;
    public static HttpsDownloader HttpsDownloader;
    public static StickersManager StickersManager;
    public static StickersService StickersService;
    public static PhonebookManager PhonebookManager;
    public static PushNotificationsManager PushNotificationsManager;

    public static ContactsManager ContactsManager { get; private set; }

    public static ConversationsManager ConversationsManager { get; private set; }

    public static NotificationsManager NotificationsManager { get; private set; }

    public static void Init()
    {
      IMO.Network = new Network();
      IMO.FileUploader = new FileUploader();
      IMO.PixelDownloader = new PixelDownloader();
      IMO.MediaUploader = new MediaUploader();
      IMO.Pixel = new Pixel();
      IMO.PhotoStreamsManager = new PhotoStreamsManager();
      IMO.AVManager = new AVManager();
      IMO.ImoAccount = new ImoAccount();
      IMO.AccountManager = new AccountManager();
      IMO.FeedbackManager = new FeedbackManager();
      IMO.Session = new Session();
      IMO.QueryEngine = new QueryEngine();
      IMO.ImoProfile = new ImoProfile();
      IMO.MonitorLog = new MonitorLog();
      IMO.IM = new IM();
      IMO.ContactsManager = new ContactsManager();
      IMO.ConversationsManager = new ConversationsManager();
      IMO.NotificationsManager = new NotificationsManager();
      IMO.HttpsDownloader = new HttpsDownloader();
      IMO.StickersService = new StickersService();
      IMO.StickersManager = new StickersManager();
      IMO.PhonebookManager = new PhonebookManager();
      IMO.PushNotificationsManager = new PushNotificationsManager();
      IMO.ImoDNSManager.Init();
    }

    public static ApplicationStorage ApplicationStorage => ApplicationStorage.Instance;

    public static ErrorReporter ErrorReporter => ErrorReporter.Instance;

    public static Dispatcher Dispatcher => Dispatcher.Instance;

    public static EventDispatcher EventDispatcher => EventDispatcher.Instance;

    public static User User => User.Instance;

    public static ApplicationSettings ApplicationSettings => ApplicationSettings.Instance;

    public static ApplicationProperties ApplicationProperties => ApplicationProperties.Instance;

    public static NavigationManager NavigationManager => NavigationManager.Instance;

    public static ImageLoader ImageLoader => ImageLoader.Instance;

    public static VideoLoader VideoLoader => VideoLoader.Instance;

    public static ImoDNSManager ImoDNSManager => ImoDNSManager.Instance;
  }
}
