// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.Helpers.CountryCodesHelper
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using System;
using System.Collections.Generic;
using System.Linq;


namespace ImoSilverlightApp.Helpers
{
  internal class CountryCodesHelper
  {
    private static IDictionary<string, CountryCodeData> map = (IDictionary<string, CountryCodeData>) new Dictionary<string, CountryCodeData>()
    {
      {
        "VU",
        new CountryCodeData("VU", "Vanuatu", "+678")
      },
      {
        "EC",
        new CountryCodeData("EC", "Ecuador", "+593")
      },
      {
        "VN",
        new CountryCodeData("VN", "Vietnam", "+84")
      },
      {
        "VI",
        new CountryCodeData("VI", "Virgin Islands US", "+1")
      },
      {
        "DZ",
        new CountryCodeData("DZ", "Algeria", "+213")
      },
      {
        "VG",
        new CountryCodeData("VG", "British Virgin Islands", "+1")
      },
      {
        "DM",
        new CountryCodeData("DM", "Dominica", "+1")
      },
      {
        "VE",
        new CountryCodeData("VE", "Venezuela", "+58")
      },
      {
        "DO",
        new CountryCodeData("DO", "Dominican Republic", "+1")
      },
      {
        "VC",
        new CountryCodeData("VC", "Saint Vincent & the Grenadines", "+1")
      },
      {
        "VA",
        new CountryCodeData("VA", "Vatican City", "+379")
      },
      {
        "DE",
        new CountryCodeData("DE", "Germany", "+49")
      },
      {
        "UZ",
        new CountryCodeData("UZ", "Uzbekistan", "+998")
      },
      {
        "UY",
        new CountryCodeData("UY", "Uruguay", "+598")
      },
      {
        "DK",
        new CountryCodeData("DK", "Denmark", "+45")
      },
      {
        "DJ",
        new CountryCodeData("DJ", "Djibouti", "+253")
      },
      {
        "UG",
        new CountryCodeData("UG", "Uganda", "+256")
      },
      {
        "UA",
        new CountryCodeData("UA", "Ukraine", "+380")
      },
      {
        "ET",
        new CountryCodeData("ET", "Ethiopia", "+251")
      },
      {
        "ES",
        new CountryCodeData("ES", "Spain", "+34")
      },
      {
        "ER",
        new CountryCodeData("ER", "Eritrea", "+291")
      },
      {
        "EH",
        new CountryCodeData("EH", "Western Sahara", "+212")
      },
      {
        "EG",
        new CountryCodeData("EG", "Egypt", "+20")
      },
      {
        "EE",
        new CountryCodeData("EE", "Estonia", "+372")
      },
      {
        "TZ",
        new CountryCodeData("TZ", "Tanzania", "+255")
      },
      {
        "TT",
        new CountryCodeData("TT", "Trinidad & Tobago", "+1")
      },
      {
        "TW",
        new CountryCodeData("TW", "Taiwan", "+886")
      },
      {
        "TV",
        new CountryCodeData("TV", "Tuvalu", "+688")
      },
      {
        "GD",
        new CountryCodeData("GD", "Grenada", "+1")
      },
      {
        "GE",
        new CountryCodeData("GE", "Georgia", "+995")
      },
      {
        "GF",
        new CountryCodeData("GF", "French Guiana", "+594")
      },
      {
        "GA",
        new CountryCodeData("GA", "Gabon", "+241")
      },
      {
        "GB",
        new CountryCodeData("GB", "United Kingdom", "+44")
      },
      {
        "FR",
        new CountryCodeData("FR", "France", "+33")
      },
      {
        "FO",
        new CountryCodeData("FO", "Faroe Islands", "+298")
      },
      {
        "FK",
        new CountryCodeData("FK", "Falkland Islands Malvinas", "+500")
      },
      {
        "FJ",
        new CountryCodeData("FJ", "Fiji", "+679")
      },
      {
        "FM",
        new CountryCodeData("FM", "Micronesia", "+691")
      },
      {
        "FI",
        new CountryCodeData("FI", "Finland", "+358")
      },
      {
        "WS",
        new CountryCodeData("WS", "Samoa", "+685")
      },
      {
        "GY",
        new CountryCodeData("GY", "Guyana", "+592")
      },
      {
        "GW",
        new CountryCodeData("GW", "Guinea-Bissau", "+245")
      },
      {
        "GU",
        new CountryCodeData("GU", "Guam", "+1")
      },
      {
        "GT",
        new CountryCodeData("GT", "Guatemala", "+502")
      },
      {
        "GR",
        new CountryCodeData("GR", "Greece", "+30")
      },
      {
        "GQ",
        new CountryCodeData("GQ", "Equatorial Guinea", "+240")
      },
      {
        "GP",
        new CountryCodeData("GP", "Guadeloupe", "+590")
      },
      {
        "WF",
        new CountryCodeData("WF", "Wallis and Futuna", "+681")
      },
      {
        "GN",
        new CountryCodeData("GN", "Guinea", "+224")
      },
      {
        "GM",
        new CountryCodeData("GM", "Gambia", "+220")
      },
      {
        "GL",
        new CountryCodeData("GL", "Greenland", "+299")
      },
      {
        "GI",
        new CountryCodeData("GI", "Gibraltar", "+350")
      },
      {
        "GH",
        new CountryCodeData("GH", "Ghana", "+233")
      },
      {
        "GG",
        new CountryCodeData("GG", "Guernsey", "+44")
      },
      {
        "RE",
        new CountryCodeData("RE", "Reunion", "+262")
      },
      {
        "RO",
        new CountryCodeData("RO", "Romania", "+40")
      },
      {
        "AT",
        new CountryCodeData("AT", "Austria", "+43")
      },
      {
        "AS",
        new CountryCodeData("AS", "American Samoa", "+1")
      },
      {
        "AR",
        new CountryCodeData("AR", "Argentina", "+54")
      },
      {
        "AX",
        new CountryCodeData("AX", "Aland Islands", "+358")
      },
      {
        "AW",
        new CountryCodeData("AW", "Aruba", "+297")
      },
      {
        "QA",
        new CountryCodeData("QA", "Qatar", "+974")
      },
      {
        "AU",
        new CountryCodeData("AU", "Australia", "+61")
      },
      {
        "AZ",
        new CountryCodeData("AZ", "Azerbaijan", "+994")
      },
      {
        "BA",
        new CountryCodeData("BA", "Bosnia & Herzegovina", "+387")
      },
      {
        "AC",
        new CountryCodeData("AC", "Ascension Island", "+247")
      },
      {
        "PT",
        new CountryCodeData("PT", "Portugal", "+351")
      },
      {
        "AD",
        new CountryCodeData("AD", "Andorra", "+376")
      },
      {
        "PW",
        new CountryCodeData("PW", "Palau", "+680")
      },
      {
        "AG",
        new CountryCodeData("AG", "Antigua & Barbuda", "+1")
      },
      {
        "AE",
        new CountryCodeData("AE", "United Arab Emirates", "+971")
      },
      {
        "PR",
        new CountryCodeData("PR", "Puerto Rico", "+1")
      },
      {
        "AF",
        new CountryCodeData("AF", "Afghanistan", "+93")
      },
      {
        "PS",
        new CountryCodeData("PS", "Palestinian Territory, Occupied", "+970")
      },
      {
        "AL",
        new CountryCodeData("AL", "Albania", "+355")
      },
      {
        "AI",
        new CountryCodeData("AI", "Anguilla", "+1")
      },
      {
        "AO",
        new CountryCodeData("AO", "Angola", "+244")
      },
      {
        "PY",
        new CountryCodeData("PY", "Paraguay", "+595")
      },
      {
        "AM",
        new CountryCodeData("AM", "Armenia", "+374")
      },
      {
        "BW",
        new CountryCodeData("BW", "Botswana", "+267")
      },
      {
        "TG",
        new CountryCodeData("TG", "Togo", "+228")
      },
      {
        "BY",
        new CountryCodeData("BY", "Belarus", "+375")
      },
      {
        "TD",
        new CountryCodeData("TD", "Chad", "+235")
      },
      {
        "BS",
        new CountryCodeData("BS", "Bahamas", "+1")
      },
      {
        "TK",
        new CountryCodeData("TK", "Tokelau", "+690")
      },
      {
        "BR",
        new CountryCodeData("BR", "Brazil", "+55")
      },
      {
        "TJ",
        new CountryCodeData("TJ", "Tajikistan", "+992")
      },
      {
        "BT",
        new CountryCodeData("BT", "Bhutan", "+975")
      },
      {
        "TH",
        new CountryCodeData("TH", "Thailand", "+66")
      },
      {
        "TO",
        new CountryCodeData("TO", "Tonga", "+676")
      },
      {
        "TN",
        new CountryCodeData("TN", "Tunisia", "+216")
      },
      {
        "TM",
        new CountryCodeData("TM", "Turkmenistan", "+993")
      },
      {
        "CA",
        new CountryCodeData("CA", "Canada", "+1")
      },
      {
        "TL",
        new CountryCodeData("TL", "East Timor", "+670")
      },
      {
        "BZ",
        new CountryCodeData("BZ", "Belize", "+501")
      },
      {
        "TR",
        new CountryCodeData("TR", "Turkey", "+90")
      },
      {
        "BF",
        new CountryCodeData("BF", "Burkina Faso", "+226")
      },
      {
        "BG",
        new CountryCodeData("BG", "Bulgaria", "+359")
      },
      {
        "SV",
        new CountryCodeData("SV", "El Salvador", "+503")
      },
      {
        "BH",
        new CountryCodeData("BH", "Bahrain", "+973")
      },
      {
        "BI",
        new CountryCodeData("BI", "Burundi", "+257")
      },
      {
        "ST",
        new CountryCodeData("ST", "Sao Tome & Principe", "+239")
      },
      {
        "BB",
        new CountryCodeData("BB", "Barbados", "+1")
      },
      {
        "SY",
        new CountryCodeData("SY", "Syria", "+963")
      },
      {
        "SZ",
        new CountryCodeData("SZ", "Swaziland", "+268")
      },
      {
        "BD",
        new CountryCodeData("BD", "Bangladesh", "+880")
      },
      {
        "BE",
        new CountryCodeData("BE", "Belgium", "+32")
      },
      {
        "BN",
        new CountryCodeData("BN", "Brunei Darussalam", "+673")
      },
      {
        "BO",
        new CountryCodeData("BO", "Bolivia", "+591")
      },
      {
        "BJ",
        new CountryCodeData("BJ", "Benin", "+229")
      },
      {
        "TC",
        new CountryCodeData("TC", "Turks and Caicos Islands", "+1")
      },
      {
        "BM",
        new CountryCodeData("BM", "Bermuda", "+1")
      },
      {
        "CZ",
        new CountryCodeData("CZ", "Czech Republic", "+420")
      },
      {
        "SD",
        new CountryCodeData("SD", "Sudan", "+249")
      },
      {
        "CY",
        new CountryCodeData("CY", "Cyprus", "+357")
      },
      {
        "SC",
        new CountryCodeData("SC", "Seychelles", "+248")
      },
      {
        "CX",
        new CountryCodeData("CX", "Christmas Island", "+61")
      },
      {
        "SE",
        new CountryCodeData("SE", "Sweden", "+46")
      },
      {
        "CV",
        new CountryCodeData("CV", "Cape Verde", "+238")
      },
      {
        "SH",
        new CountryCodeData("SH", "Saint Helena", "+290")
      },
      {
        "CU",
        new CountryCodeData("CU", "Cuba", "+53")
      },
      {
        "SG",
        new CountryCodeData("SG", "Singapore", "+65")
      },
      {
        "SJ",
        new CountryCodeData("SJ", "Svalbard and Jan Mayen Islands", "+47")
      },
      {
        "SI",
        new CountryCodeData("SI", "Slovenia", "+386")
      },
      {
        "SL",
        new CountryCodeData("SL", "Sierra Leone", "+232")
      },
      {
        "SK",
        new CountryCodeData("SK", "Slovak Republic", "+421")
      },
      {
        "SN",
        new CountryCodeData("SN", "Senegal", "+221")
      },
      {
        "SM",
        new CountryCodeData("SM", "San Marino", "+378")
      },
      {
        "SO",
        new CountryCodeData("SO", "Somalia", "+252")
      },
      {
        "SR",
        new CountryCodeData("SR", "Suriname", "+597")
      },
      {
        "CI",
        new CountryCodeData("CI", "Cote D'Ivoire Ivory Coast", "+225")
      },
      {
        "RS",
        new CountryCodeData("RS", "Serbia", "+381")
      },
      {
        "CG",
        new CountryCodeData("CG", "Congo", "+242")
      },
      {
        "CH",
        new CountryCodeData("CH", "Switzerland", "+41")
      },
      {
        "RU",
        new CountryCodeData("RU", "Russian Federation", "+7")
      },
      {
        "CF",
        new CountryCodeData("CF", "Central African Republic", "+236")
      },
      {
        "RW",
        new CountryCodeData("RW", "Rwanda", "+250")
      },
      {
        "CC",
        new CountryCodeData("CC", "Cocos Keeling Islands", "+61")
      },
      {
        "CD",
        new CountryCodeData("CD", "Congo, Democratic Republic", "+243")
      },
      {
        "CR",
        new CountryCodeData("CR", "Costa Rica", "+506")
      },
      {
        "CO",
        new CountryCodeData("CO", "Colombia", "+57")
      },
      {
        "CM",
        new CountryCodeData("CM", "Cameroon", "+237")
      },
      {
        "CN",
        new CountryCodeData("CN", "China", "+86")
      },
      {
        "CK",
        new CountryCodeData("CK", "Cook Islands", "+682")
      },
      {
        "SA",
        new CountryCodeData("SA", "Saudi Arabia", "+966")
      },
      {
        "CL",
        new CountryCodeData("CL", "Chile", "+56")
      },
      {
        "SB",
        new CountryCodeData("SB", "Solomon Islands", "+677")
      },
      {
        "LV",
        new CountryCodeData("LV", "Latvia", "+371")
      },
      {
        "LU",
        new CountryCodeData("LU", "Luxembourg", "+352")
      },
      {
        "LT",
        new CountryCodeData("LT", "Lithuania", "+370")
      },
      {
        "LY",
        new CountryCodeData("LY", "Libya", "+218")
      },
      {
        "LS",
        new CountryCodeData("LS", "Lesotho", "+266")
      },
      {
        "LR",
        new CountryCodeData("LR", "Liberia", "+231")
      },
      {
        "MG",
        new CountryCodeData("MG", "Madagascar", "+261")
      },
      {
        "MH",
        new CountryCodeData("MH", "Marshall Islands", "+692")
      },
      {
        "ME",
        new CountryCodeData("ME", "Montenegro", "+382")
      },
      {
        "MF",
        new CountryCodeData("MF", "Saint Martin French", "+590")
      },
      {
        "MK",
        new CountryCodeData("MK", "Macedonia", "+389")
      },
      {
        "ML",
        new CountryCodeData("ML", "Mali", "+223")
      },
      {
        "MC",
        new CountryCodeData("MC", "Monaco", "+377")
      },
      {
        "MD",
        new CountryCodeData("MD", "Moldova", "+373")
      },
      {
        "MA",
        new CountryCodeData("MA", "Morocco", "+212")
      },
      {
        "MV",
        new CountryCodeData("MV", "Maldives", "+960")
      },
      {
        "MU",
        new CountryCodeData("MU", "Mauritius", "+230")
      },
      {
        "MX",
        new CountryCodeData("MX", "Mexico", "+52")
      },
      {
        "MW",
        new CountryCodeData("MW", "Malawi", "+265")
      },
      {
        "MZ",
        new CountryCodeData("MZ", "Mozambique", "+258")
      },
      {
        "MY",
        new CountryCodeData("MY", "Malaysia", "+60")
      },
      {
        "MN",
        new CountryCodeData("MN", "Mongolia", "+976")
      },
      {
        "MM",
        new CountryCodeData("MM", "Myanmar Burma", "+95")
      },
      {
        "MP",
        new CountryCodeData("MP", "Northern Mariana Islands", "+1")
      },
      {
        "MO",
        new CountryCodeData("MO", "Macau", "+853")
      },
      {
        "MR",
        new CountryCodeData("MR", "Mauritania", "+222")
      },
      {
        "MQ",
        new CountryCodeData("MQ", "Martinique", "+596")
      },
      {
        "MT",
        new CountryCodeData("MT", "Malta", "+356")
      },
      {
        "MS",
        new CountryCodeData("MS", "Montserrat", "+1")
      },
      {
        "NF",
        new CountryCodeData("NF", "Norfolk Island", "+672")
      },
      {
        "NG",
        new CountryCodeData("NG", "Nigeria", "+234")
      },
      {
        "NI",
        new CountryCodeData("NI", "Nicaragua", "+505")
      },
      {
        "NL",
        new CountryCodeData("NL", "Netherlands", "+31")
      },
      {
        "NA",
        new CountryCodeData("NA", "Namibia", "+264")
      },
      {
        "NC",
        new CountryCodeData("NC", "New Caledonia", "+687")
      },
      {
        "NE",
        new CountryCodeData("NE", "Niger", "+227")
      },
      {
        "NZ",
        new CountryCodeData("NZ", "New Zealand", "+64")
      },
      {
        "NU",
        new CountryCodeData("NU", "Niue", "+683")
      },
      {
        "NR",
        new CountryCodeData("NR", "Nauru", "+674")
      },
      {
        "NP",
        new CountryCodeData("NP", "Nepal", "+977")
      },
      {
        "NO",
        new CountryCodeData("NO", "Norway", "+47")
      },
      {
        "OM",
        new CountryCodeData("OM", "Oman", "+968")
      },
      {
        "PL",
        new CountryCodeData("PL", "Poland", "+48")
      },
      {
        "PM",
        new CountryCodeData("PM", "Saint Pierre and Miquelon", "+508")
      },
      {
        "PH",
        new CountryCodeData("PH", "Philippines", "+63")
      },
      {
        "PK",
        new CountryCodeData("PK", "Pakistan", "+92")
      },
      {
        "PE",
        new CountryCodeData("PE", "Peru", "+51")
      },
      {
        "PF",
        new CountryCodeData("PF", "Tahiti French Polinesia", "+689")
      },
      {
        "PG",
        new CountryCodeData("PG", "Papua New Guinea", "+675")
      },
      {
        "PA",
        new CountryCodeData("PA", "Panama", "+507")
      },
      {
        "HK",
        new CountryCodeData("HK", "Hong Kong", "+852")
      },
      {
        "ZA",
        new CountryCodeData("ZA", "South Africa", "+27")
      },
      {
        "HN",
        new CountryCodeData("HN", "Honduras", "+504")
      },
      {
        "HR",
        new CountryCodeData("HR", "Croatia", "+385")
      },
      {
        "HT",
        new CountryCodeData("HT", "Haiti", "+509")
      },
      {
        "HU",
        new CountryCodeData("HU", "Hungary", "+36")
      },
      {
        "ZM",
        new CountryCodeData("ZM", "Zambia", "+260")
      },
      {
        "ID",
        new CountryCodeData("ID", "Indonesia", "+62")
      },
      {
        "ZW",
        new CountryCodeData("ZW", "Zimbabwe", "+263")
      },
      {
        "IE",
        new CountryCodeData("IE", "Ireland", "+353")
      },
      {
        "IL",
        new CountryCodeData("IL", "Israel", "+972")
      },
      {
        "IM",
        new CountryCodeData("IM", "Isle of Man", "+44")
      },
      {
        "IN",
        new CountryCodeData("IN", "India", "+91")
      },
      {
        "IO",
        new CountryCodeData("IO", "British Indian Ocean Territory", "+246")
      },
      {
        "IQ",
        new CountryCodeData("IQ", "Iraq", "+964")
      },
      {
        "IR",
        new CountryCodeData("IR", "Iran", "+98")
      },
      {
        "YE",
        new CountryCodeData("YE", "Yemen", "+967")
      },
      {
        "IS",
        new CountryCodeData("IS", "Iceland", "+354")
      },
      {
        "IT",
        new CountryCodeData("IT", "Italy", "+39")
      },
      {
        "JE",
        new CountryCodeData("JE", "Jersey", "+44")
      },
      {
        "YT",
        new CountryCodeData("YT", "Mayotte", "+262")
      },
      {
        "JP",
        new CountryCodeData("JP", "Japan", "+81")
      },
      {
        "JO",
        new CountryCodeData("JO", "Jordan", "+962")
      },
      {
        "JM",
        new CountryCodeData("JM", "Jamaica", "+1")
      },
      {
        "KI",
        new CountryCodeData("KI", "Kiribati", "+686")
      },
      {
        "KH",
        new CountryCodeData("KH", "Cambodia", "+855")
      },
      {
        "KG",
        new CountryCodeData("KG", "Kyrgyzstan", "+996")
      },
      {
        "KE",
        new CountryCodeData("KE", "Kenya", "+254")
      },
      {
        "KP",
        new CountryCodeData("KP", "North Korea", "+850")
      },
      {
        "KR",
        new CountryCodeData("KR", "South Korea", "+82")
      },
      {
        "KM",
        new CountryCodeData("KM", "Comoros", "+269")
      },
      {
        "KN",
        new CountryCodeData("KN", "Saint Kitts & Nevis", "+1")
      },
      {
        "KW",
        new CountryCodeData("KW", "Kuwait", "+965")
      },
      {
        "KY",
        new CountryCodeData("KY", "Cayman Islands", "+1")
      },
      {
        "KZ",
        new CountryCodeData("KZ", "Kazakhstan", "+7")
      },
      {
        "LA",
        new CountryCodeData("LA", "Laos", "+856")
      },
      {
        "LC",
        new CountryCodeData("LC", "Saint Lucia", "+1")
      },
      {
        "LB",
        new CountryCodeData("LB", "Lebanon", "+961")
      },
      {
        "LI",
        new CountryCodeData("LI", "Liechtenstein", "+423")
      },
      {
        "LK",
        new CountryCodeData("LK", "Sri Lanka", "+94")
      },
      {
        "BQ",
        new CountryCodeData("BQ", "Bonaire, Sint Eustatius and Saba", "+599")
      },
      {
        "CW",
        new CountryCodeData("CW", "Curaçao", "+599")
      },
      {
        "BL",
        new CountryCodeData("BL", "Saint Barthélemy", "+590")
      },
      {
        "SX",
        new CountryCodeData("SX", "Sint Maarten Dutch part", "+1")
      },
      {
        "SS",
        new CountryCodeData("SS", "South Sudan", "+211")
      }
    };
    private static IDictionary<string, int[]> phoneLengths = (IDictionary<string, int[]>) new Dictionary<string, int[]>()
    {
      {
        "AD",
        new int[1]{ 10 }
      },
      {
        "AE",
        new int[1]{ 13 }
      },
      {
        "AF",
        new int[1]{ 12 }
      },
      {
        "AG",
        new int[1]{ 12 }
      },
      {
        "AI",
        new int[1]{ 12 }
      },
      {
        "AL",
        new int[1]{ 13 }
      },
      {
        "AM",
        new int[1]{ 12 }
      },
      {
        "AO",
        new int[1]{ 13 }
      },
      {
        "AR",
        new int[1]{ 13 }
      },
      {
        "AS",
        new int[1]{ 12 }
      },
      {
        "AT",
        new int[3]{ 13, 14, 15 }
      },
      {
        "AU",
        new int[1]{ 12 }
      },
      {
        "AW",
        new int[1]{ 11 }
      },
      {
        "AZ",
        new int[1]{ 13 }
      },
      {
        "BA",
        new int[2]{ 12, 13 }
      },
      {
        "BB",
        new int[1]{ 12 }
      },
      {
        "BD",
        new int[1]{ 14 }
      },
      {
        "BE",
        new int[1]{ 12 }
      },
      {
        "BF",
        new int[1]{ 12 }
      },
      {
        "BG",
        new int[1]{ 13 }
      },
      {
        "BH",
        new int[1]{ 12 }
      },
      {
        "BI",
        new int[1]{ 12 }
      },
      {
        "BJ",
        new int[1]{ 12 }
      },
      {
        "BM",
        new int[1]{ 12 }
      },
      {
        "BN",
        new int[1]{ 11 }
      },
      {
        "BO",
        new int[1]{ 12 }
      },
      {
        "BQ",
        new int[1]{ 11 }
      },
      {
        "BR",
        new int[2]{ 13, 14 }
      },
      {
        "BS",
        new int[1]{ 12 }
      },
      {
        "BT",
        new int[1]{ 12 }
      },
      {
        "BW",
        new int[1]{ 12 }
      },
      {
        "BY",
        new int[1]{ 13 }
      },
      {
        "BZ",
        new int[1]{ 11 }
      },
      {
        "CA",
        new int[1]{ 12 }
      },
      {
        "CD",
        new int[1]{ 13 }
      },
      {
        "CF",
        new int[1]{ 12 }
      },
      {
        "CG",
        new int[1]{ 13 }
      },
      {
        "CH",
        new int[1]{ 12 }
      },
      {
        "CI",
        new int[1]{ 12 }
      },
      {
        "CK",
        new int[1]{ 9 }
      },
      {
        "CL",
        new int[1]{ 12 }
      },
      {
        "CM",
        new int[2]{ 12, 13 }
      },
      {
        "CN",
        new int[1]{ 14 }
      },
      {
        "CO",
        new int[1]{ 13 }
      },
      {
        "CR",
        new int[1]{ 12 }
      },
      {
        "CU",
        new int[1]{ 11 }
      },
      {
        "CV",
        new int[1]{ 11 }
      },
      {
        "CW",
        new int[1]{ 12 }
      },
      {
        "CY",
        new int[1]{ 12 }
      },
      {
        "CZ",
        new int[1]{ 13 }
      },
      {
        "DE",
        new int[2]{ 13, 14 }
      },
      {
        "DJ",
        new int[1]{ 12 }
      },
      {
        "DK",
        new int[1]{ 11 }
      },
      {
        "DM",
        new int[1]{ 12 }
      },
      {
        "DO",
        new int[1]{ 12 }
      },
      {
        "DZ",
        new int[1]{ 13 }
      },
      {
        "EC",
        new int[1]{ 13 }
      },
      {
        "EE",
        new int[2]{ 11, 12 }
      },
      {
        "EG",
        new int[1]{ 13 }
      },
      {
        "ER",
        new int[1]{ 11 }
      },
      {
        "ES",
        new int[1]{ 12 }
      },
      {
        "ET",
        new int[1]{ 13 }
      },
      {
        "FI",
        new int[2]{ 13, 14 }
      },
      {
        "FJ",
        new int[1]{ 11 }
      },
      {
        "FM",
        new int[1]{ 11 }
      },
      {
        "FO",
        new int[1]{ 10 }
      },
      {
        "FR",
        new int[1]{ 12 }
      },
      {
        "GA",
        new int[1]{ 12 }
      },
      {
        "GB",
        new int[1]{ 13 }
      },
      {
        "GD",
        new int[1]{ 12 }
      },
      {
        "GE",
        new int[1]{ 13 }
      },
      {
        "GF",
        new int[1]{ 13 }
      },
      {
        "GG",
        new int[1]{ 13 }
      },
      {
        "GH",
        new int[1]{ 13 }
      },
      {
        "GI",
        new int[1]{ 12 }
      },
      {
        "GL",
        new int[1]{ 10 }
      },
      {
        "GM",
        new int[1]{ 11 }
      },
      {
        "GN",
        new int[1]{ 13 }
      },
      {
        "GP",
        new int[1]{ 13 }
      },
      {
        "GQ",
        new int[1]{ 13 }
      },
      {
        "GR",
        new int[1]{ 13 }
      },
      {
        "GT",
        new int[1]{ 12 }
      },
      {
        "GU",
        new int[1]{ 12 }
      },
      {
        "GW",
        new int[2]{ 11, 13 }
      },
      {
        "GY",
        new int[1]{ 11 }
      },
      {
        "HK",
        new int[1]{ 12 }
      },
      {
        "HN",
        new int[1]{ 12 }
      },
      {
        "HR",
        new int[2]{ 12, 13 }
      },
      {
        "HT",
        new int[1]{ 12 }
      },
      {
        "HU",
        new int[1]{ 12 }
      },
      {
        "ID",
        new int[4]{ 12, 13, 14, 15 }
      },
      {
        "IE",
        new int[1]{ 13 }
      },
      {
        "IL",
        new int[1]{ 13 }
      },
      {
        "IM",
        new int[1]{ 13 }
      },
      {
        "IN",
        new int[1]{ 13 }
      },
      {
        "IO",
        new int[1]{ 11 }
      },
      {
        "IQ",
        new int[1]{ 14 }
      },
      {
        "IR",
        new int[1]{ 13 }
      },
      {
        "IS",
        new int[1]{ 11 }
      },
      {
        "IT",
        new int[2]{ 12, 13 }
      },
      {
        "JE",
        new int[1]{ 13 }
      },
      {
        "JM",
        new int[1]{ 12 }
      },
      {
        "JO",
        new int[1]{ 13 }
      },
      {
        "JP",
        new int[1]{ 13 }
      },
      {
        "KE",
        new int[1]{ 13 }
      },
      {
        "KG",
        new int[1]{ 13 }
      },
      {
        "KH",
        new int[2]{ 12, 13 }
      },
      {
        "KM",
        new int[1]{ 11 }
      },
      {
        "KN",
        new int[1]{ 12 }
      },
      {
        "KR",
        new int[1]{ 13 }
      },
      {
        "KW",
        new int[1]{ 12 }
      },
      {
        "KY",
        new int[1]{ 12 }
      },
      {
        "KZ",
        new int[1]{ 12 }
      },
      {
        "LA",
        new int[1]{ 14 }
      },
      {
        "LB",
        new int[2]{ 11, 12 }
      },
      {
        "LC",
        new int[1]{ 12 }
      },
      {
        "LK",
        new int[1]{ 12 }
      },
      {
        "LR",
        new int[2]{ 11, 13 }
      },
      {
        "LS",
        new int[1]{ 12 }
      },
      {
        "LT",
        new int[1]{ 12 }
      },
      {
        "LU",
        new int[1]{ 13 }
      },
      {
        "LV",
        new int[1]{ 12 }
      },
      {
        "LY",
        new int[1]{ 13 }
      },
      {
        "MA",
        new int[1]{ 13 }
      },
      {
        "MC",
        new int[1]{ 12 }
      },
      {
        "MD",
        new int[1]{ 12 }
      },
      {
        "ME",
        new int[2]{ 12, 13 }
      },
      {
        "MG",
        new int[1]{ 13 }
      },
      {
        "MK",
        new int[1]{ 12 }
      },
      {
        "ML",
        new int[1]{ 12 }
      },
      {
        "MM",
        new int[3]{ 11, 12, 13 }
      },
      {
        "MN",
        new int[1]{ 12 }
      },
      {
        "MO",
        new int[1]{ 12 }
      },
      {
        "MP",
        new int[1]{ 12 }
      },
      {
        "MQ",
        new int[1]{ 13 }
      },
      {
        "MR",
        new int[1]{ 12 }
      },
      {
        "MS",
        new int[1]{ 12 }
      },
      {
        "MT",
        new int[1]{ 12 }
      },
      {
        "MU",
        new int[1]{ 12 }
      },
      {
        "MV",
        new int[1]{ 11 }
      },
      {
        "MW",
        new int[1]{ 13 }
      },
      {
        "MX",
        new int[1]{ 13 }
      },
      {
        "MY",
        new int[2]{ 12, 13 }
      },
      {
        "MZ",
        new int[1]{ 13 }
      },
      {
        "NA",
        new int[1]{ 13 }
      },
      {
        "NC",
        new int[1]{ 10 }
      },
      {
        "NE",
        new int[1]{ 12 }
      },
      {
        "NG",
        new int[1]{ 14 }
      },
      {
        "NI",
        new int[1]{ 12 }
      },
      {
        "NL",
        new int[1]{ 12 }
      },
      {
        "NO",
        new int[1]{ 11 }
      },
      {
        "NP",
        new int[1]{ 14 }
      },
      {
        "NR",
        new int[1]{ 11 }
      },
      {
        "NZ",
        new int[3]{ 11, 12, 13 }
      },
      {
        "OM",
        new int[1]{ 12 }
      },
      {
        "PA",
        new int[1]{ 12 }
      },
      {
        "PE",
        new int[1]{ 12 }
      },
      {
        "PF",
        new int[1]{ 12 }
      },
      {
        "PG",
        new int[1]{ 12 }
      },
      {
        "PH",
        new int[1]{ 13 }
      },
      {
        "PK",
        new int[1]{ 13 }
      },
      {
        "PL",
        new int[1]{ 12 }
      },
      {
        "PR",
        new int[1]{ 12 }
      },
      {
        "PS",
        new int[1]{ 13 }
      },
      {
        "PT",
        new int[1]{ 13 }
      },
      {
        "PW",
        new int[1]{ 11 }
      },
      {
        "PY",
        new int[1]{ 13 }
      },
      {
        "QA",
        new int[1]{ 12 }
      },
      {
        "RE",
        new int[1]{ 13 }
      },
      {
        "RO",
        new int[1]{ 12 }
      },
      {
        "RS",
        new int[2]{ 12, 13 }
      },
      {
        "RU",
        new int[1]{ 12 }
      },
      {
        "RW",
        new int[1]{ 13 }
      },
      {
        "SA",
        new int[1]{ 13 }
      },
      {
        "SB",
        new int[1]{ 11 }
      },
      {
        "SC",
        new int[1]{ 11 }
      },
      {
        "SD",
        new int[1]{ 13 }
      },
      {
        "SE",
        new int[1]{ 12 }
      },
      {
        "SG",
        new int[1]{ 11 }
      },
      {
        "SI",
        new int[1]{ 12 }
      },
      {
        "SK",
        new int[1]{ 13 }
      },
      {
        "SL",
        new int[1]{ 12 }
      },
      {
        "SN",
        new int[1]{ 13 }
      },
      {
        "SO",
        new int[2]{ 12, 13 }
      },
      {
        "SR",
        new int[1]{ 11 }
      },
      {
        "SS",
        new int[1]{ 13 }
      },
      {
        "ST",
        new int[1]{ 11 }
      },
      {
        "SV",
        new int[1]{ 12 }
      },
      {
        "SX",
        new int[1]{ 12 }
      },
      {
        "SY",
        new int[1]{ 13 }
      },
      {
        "SZ",
        new int[1]{ 12 }
      },
      {
        "TC",
        new int[1]{ 12 }
      },
      {
        "TD",
        new int[1]{ 12 }
      },
      {
        "TG",
        new int[1]{ 12 }
      },
      {
        "TH",
        new int[1]{ 12 }
      },
      {
        "TJ",
        new int[1]{ 13 }
      },
      {
        "TL",
        new int[1]{ 12 }
      },
      {
        "TM",
        new int[1]{ 12 }
      },
      {
        "TN",
        new int[1]{ 12 }
      },
      {
        "TO",
        new int[1]{ 11 }
      },
      {
        "TR",
        new int[1]{ 13 }
      },
      {
        "TT",
        new int[1]{ 12 }
      },
      {
        "TV",
        new int[1]{ 10 }
      },
      {
        "TW",
        new int[1]{ 13 }
      },
      {
        "TZ",
        new int[1]{ 13 }
      },
      {
        "UA",
        new int[1]{ 13 }
      },
      {
        "UG",
        new int[1]{ 13 }
      },
      {
        "US",
        new int[1]{ 12 }
      },
      {
        "UY",
        new int[1]{ 12 }
      },
      {
        "UZ",
        new int[1]{ 13 }
      },
      {
        "VC",
        new int[1]{ 12 }
      },
      {
        "VE",
        new int[1]{ 13 }
      },
      {
        "VG",
        new int[1]{ 12 }
      },
      {
        "VI",
        new int[1]{ 12 }
      },
      {
        "VN",
        new int[2]{ 12, 13 }
      },
      {
        "VU",
        new int[1]{ 11 }
      },
      {
        "WS",
        new int[1]{ 11 }
      },
      {
        "YE",
        new int[1]{ 13 }
      },
      {
        "YT",
        new int[1]{ 13 }
      },
      {
        "ZA",
        new int[1]{ 12 }
      },
      {
        "ZM",
        new int[1]{ 13 }
      },
      {
        "ZW",
        new int[1]{ 13 }
      }
    };

    public static IList<CountryCodeData> GetCountryCodesData()
    {
      return (IList<CountryCodeData>) CountryCodesHelper.map.Values.ToList<CountryCodeData>();
    }

    public static string GetPhoneCC(string cc) => CountryCodesHelper.map[cc].PhoneCC;

    public static string GetCountryByCC(string cc)
    {
      return CountryCodesHelper.GetCountryDataByCC(cc).Country;
    }

    public static CountryCodeData GetCountryDataByCC(string cc)
    {
      return string.IsNullOrEmpty(cc) ? (CountryCodeData) null : CountryCodesHelper.map[cc.ToUpper()];
    }

    public static CountryCodeData GetCountryDataByPhoneCC(string code)
    {
      if (string.IsNullOrEmpty(code))
        return (CountryCodeData) null;
      if (!code.StartsWith("+"))
        code = "+" + code;
      switch (code)
      {
        case "+1":
          return CountryCodesHelper.map["CA"];
        case "+212":
          return CountryCodesHelper.map["MA"];
        case "+358":
          return CountryCodesHelper.map["FI"];
        case "+44":
          return CountryCodesHelper.map["GB"];
        case "+47":
          return CountryCodesHelper.map["NO"];
        case "+590":
          return CountryCodesHelper.map["GP"];
        case "+61":
          return CountryCodesHelper.map["AU"];
        case "+7":
          return CountryCodesHelper.map["KZ"];
        default:
          return CountryCodesHelper.map.Select<KeyValuePair<string, CountryCodeData>, CountryCodeData>((Func<KeyValuePair<string, CountryCodeData>, CountryCodeData>) (x => x.Value)).Where<CountryCodeData>((Func<CountryCodeData, bool>) (x => x.PhoneCC == code)).FirstOrDefault<CountryCodeData>();
      }
    }

    public static PhoneLengthResult VerifyPhoneLength(string cc, string phoneNumberUnstripped)
    {
      string str = Utils.NormalizePhoneNumber(phoneNumberUnstripped);
      if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(cc))
        return PhoneLengthResult.TooShort;
      if (!CountryCodesHelper.phoneLengths.ContainsKey(cc))
        return PhoneLengthResult.OK;
      int[] phoneLength = CountryCodesHelper.phoneLengths[cc];
      if (phoneLength.Length == 0)
        return PhoneLengthResult.OK;
      string phoneCc = CountryCodesHelper.GetPhoneCC(cc);
      int num1 = str.Length + phoneCc.Length;
      int num2 = num1;
      if (str[0] == '0')
        --num1;
      bool flag = false;
      foreach (int num3 in phoneLength)
      {
        if (num3 >= num1 && num3 <= num2)
          return PhoneLengthResult.OK;
        if (num3 > num2)
          flag = true;
      }
      return !flag ? PhoneLengthResult.TooLong : PhoneLengthResult.TooShort;
    }
  }
}
