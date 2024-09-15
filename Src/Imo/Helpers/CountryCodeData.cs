// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.CountryCodeData
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll


namespace ImoSilverlightApp.Helpers
{
  public class CountryCodeData
  {
    public string PhoneCC { get; set; }

    public string Country { get; set; }

    public string CC { get; set; }

    public string PhoneCCNoPlus => this.PhoneCC.Substring(1);

    public CountryCodeData(string cc, string country, string phoneCC)
    {
      this.CC = cc;
      this.Country = country;
      this.PhoneCC = phoneCC;
    }
  }
}
