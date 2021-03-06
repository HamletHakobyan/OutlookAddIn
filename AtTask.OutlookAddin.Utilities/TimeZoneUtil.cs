﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtTask.OutlookAddIn.Business
{
    public static class TimeZoneUtil
    {
        private static readonly Dictionary<string, long> ServerTimeZoneOffsets = new Dictionary<string, long>()
        {
            {"Etc/GMT+12", -43200000L},
            {"Etc/GMT+11", -39600000L},
            {"Pacific/Midway", -39600000L},
            {"Pacific/Niue", -39600000L},
            {"Pacific/Pago_Pago", -39600000L},
            {"Pacific/Samoa", -39600000L},
            {"US/Samoa", -39600000L},
            {"America/Adak", -36000000L},
            {"America/Atka", -36000000L},
            {"Etc/GMT+10", -36000000L},
            {"HST", -36000000L},
            {"Pacific/Honolulu", -36000000L},
            {"Pacific/Johnston", -36000000L},
            {"Pacific/Rarotonga", -36000000L},
            {"Pacific/Tahiti", -36000000L},
            {"SystemV/HST10", -36000000L},
            {"US/Aleutian", -36000000L},
            {"US/Hawaii", -36000000L},
            {"Pacific/Marquesas", -34200000L},
            {"AST", -32400000L},
            {"America/Anchorage", -32400000L},
            {"America/Juneau", -32400000L},
            {"America/Nome", -32400000L},
            {"America/Sitka", -32400000L},
            {"America/Yakutat", -32400000L},
            {"Etc/GMT+9", -32400000L},
            {"Pacific/Gambier", -32400000L},
            {"SystemV/YST9", -32400000L},
            {"SystemV/YST9YDT", -32400000L},
            {"US/Alaska", -32400000L},
            {"America/Dawson", -28800000L},
            {"America/Ensenada", -28800000L},
            {"America/Los_Angeles", -28800000L},
            {"America/Metlakatla", -28800000L},
            {"America/Santa_Isabel", -28800000L},
            {"America/Tijuana", -28800000L},
            {"America/Vancouver", -28800000L},
            {"America/Whitehorse", -28800000L},
            {"Canada/Pacific", -28800000L},
            {"Canada/Yukon", -28800000L},
            {"Etc/GMT+8", -28800000L},
            {"Mexico/BajaNorte", -28800000L},
            {"PST", -28800000L},
            {"PST8PDT", -28800000L},
            {"Pacific/Pitcairn", -28800000L},
            {"SystemV/PST8", -28800000L},
            {"SystemV/PST8PDT", -28800000L},
            {"US/Pacific", -28800000L},
            {"US/Pacific-New", -28800000L},
            {"America/Boise", -25200000L},
            {"America/Cambridge_Bay", -25200000L},
            {"America/Chihuahua", -25200000L},
            {"America/Creston", -25200000L},
            {"America/Dawson_Creek", -25200000L},
            {"America/Denver", -25200000L},
            {"America/Edmonton", -25200000L},
            {"America/Hermosillo", -25200000L},
            {"America/Inuvik", -25200000L},
            {"America/Mazatlan", -25200000L},
            {"America/Ojinaga", -25200000L},
            {"America/Phoenix", -25200000L},
            {"America/Shiprock", -25200000L},
            {"America/Yellowknife", -25200000L},
            {"Canada/Mountain", -25200000L},
            {"Etc/GMT+7", -25200000L},
            {"MST", -25200000L},
            {"MST7MDT", -25200000L},
            {"Mexico/BajaSur", -25200000L},
            {"Navajo", -25200000L},
            {"PNT", -25200000L},
            {"SystemV/MST7", -25200000L},
            {"SystemV/MST7MDT", -25200000L},
            {"US/Arizona", -25200000L},
            {"US/Mountain", -25200000L},
            {"America/Bahia_Banderas", -21600000L},
            {"America/Belize", -21600000L},
            {"America/Cancun", -21600000L},
            {"America/Chicago", -21600000L},
            {"America/Costa_Rica", -21600000L},
            {"America/El_Salvador", -21600000L},
            {"America/Guatemala", -21600000L},
            {"America/Indiana/Knox", -21600000L},
            {"America/Indiana/Tell_City", -21600000L},
            {"America/Knox_IN", -21600000L},
            {"America/Managua", -21600000L},
            {"America/Matamoros", -21600000L},
            {"America/Menominee", -21600000L},
            {"America/Merida", -21600000L},
            {"America/Mexico_City", -21600000L},
            {"America/Monterrey", -21600000L},
            {"America/North_Dakota/Beulah", -21600000L},
            {"America/North_Dakota/Center", -21600000L},
            {"America/North_Dakota/New_Salem", -21600000L},
            {"America/Rainy_River", -21600000L},
            {"America/Rankin_Inlet", -21600000L},
            {"America/Regina", -21600000L},
            {"America/Resolute", -21600000L},
            {"America/Swift_Current", -21600000L},
            {"America/Tegucigalpa", -21600000L},
            {"America/Winnipeg", -21600000L},
            {"CST", -21600000L},
            {"CST6CDT", -21600000L},
            {"Canada/Central", -21600000L},
            {"Canada/East-Saskatchewan", -21600000L},
            {"Canada/Saskatchewan", -21600000L},
            {"Chile/EasterIsland", -21600000L},
            {"Etc/GMT+6", -21600000L},
            {"Mexico/General", -21600000L},
            {"Pacific/Easter", -21600000L},
            {"Pacific/Galapagos", -21600000L},
            {"SystemV/CST6", -21600000L},
            {"SystemV/CST6CDT", -21600000L},
            {"US/Central", -21600000L},
            {"US/Indiana-Starke", -21600000L},
            {"America/Atikokan", -18000000L},
            {"America/Bogota", -18000000L},
            {"America/Cayman", -18000000L},
            {"America/Coral_Harbour", -18000000L},
            {"America/Detroit", -18000000L},
            {"America/Fort_Wayne", -18000000L},
            {"America/Grand_Turk", -18000000L},
            {"America/Guayaquil", -18000000L},
            {"America/Havana", -18000000L},
            {"America/Indiana/Indianapolis", -18000000L},
            {"America/Indiana/Marengo", -18000000L},
            {"America/Indiana/Petersburg", -18000000L},
            {"America/Indiana/Vevay", -18000000L},
            {"America/Indiana/Vincennes", -18000000L},
            {"America/Indiana/Winamac", -18000000L},
            {"America/Indianapolis", -18000000L},
            {"America/Iqaluit", -18000000L},
            {"America/Jamaica", -18000000L},
            {"America/Kentucky/Louisville", -18000000L},
            {"America/Kentucky/Monticello", -18000000L},
            {"America/Lima", -18000000L},
            {"America/Louisville", -18000000L},
            {"America/Montreal", -18000000L},
            {"America/Nassau", -18000000L},
            {"America/New_York", -18000000L},
            {"America/Nipigon", -18000000L},
            {"America/Panama", -18000000L},
            {"America/Pangnirtung", -18000000L},
            {"America/Port-au-Prince", -18000000L},
            {"America/Thunder_Bay", -18000000L},
            {"America/Toronto", -18000000L},
            {"Canada/Eastern", -18000000L},
            {"Cuba", -18000000L},
            {"EST", -18000000L},
            {"EST5EDT", -18000000L},
            {"Etc/GMT+5", -18000000L},
            {"IET", -18000000L},
            {"Jamaica", -18000000L},
            {"SystemV/EST5", -18000000L},
            {"SystemV/EST5EDT", -18000000L},
            {"US/East-Indiana", -18000000L},
            {"US/Eastern", -18000000L},
            {"US/Michigan", -18000000L},
            {"America/Caracas", -16200000L},
            {"America/Anguilla", -14400000L},
            {"America/Antigua", -14400000L},
            {"America/Argentina/San_Luis", -14400000L},
            {"America/Aruba", -14400000L},
            {"America/Asuncion", -14400000L},
            {"America/Barbados", -14400000L},
            {"America/Blanc-Sablon", -14400000L},
            {"America/Boa_Vista", -14400000L},
            {"America/Campo_Grande", -14400000L},
            {"America/Cuiaba", -14400000L},
            {"America/Curacao", -14400000L},
            {"America/Dominica", -14400000L},
            {"America/Eirunepe", -14400000L},
            {"America/Glace_Bay", -14400000L},
            {"America/Goose_Bay", -14400000L},
            {"America/Grenada", -14400000L},
            {"America/Guadeloupe", -14400000L},
            {"America/Guyana", -14400000L},
            {"America/Halifax", -14400000L},
            {"America/Kralendijk", -14400000L},
            {"America/La_Paz", -14400000L},
            {"America/Lower_Princes", -14400000L},
            {"America/Manaus", -14400000L},
            {"America/Marigot", -14400000L},
            {"America/Martinique", -14400000L},
            {"America/Moncton", -14400000L},
            {"America/Montserrat", -14400000L},
            {"America/Port_of_Spain", -14400000L},
            {"America/Porto_Acre", -14400000L},
            {"America/Porto_Velho", -14400000L},
            {"America/Puerto_Rico", -14400000L},
            {"America/Rio_Branco", -14400000L},
            {"America/Santiago", -14400000L},
            {"America/Santo_Domingo", -14400000L},
            {"America/St_Barthelemy", -14400000L},
            {"America/St_Kitts", -14400000L},
            {"America/St_Lucia", -14400000L},
            {"America/St_Thomas", -14400000L},
            {"America/St_Vincent", -14400000L},
            {"America/Thule", -14400000L},
            {"America/Tortola", -14400000L},
            {"America/Virgin", -14400000L},
            {"Antarctica/Palmer", -14400000L},
            {"Atlantic/Bermuda", -14400000L},
            {"Brazil/Acre", -14400000L},
            {"Brazil/West", -14400000L},
            {"Canada/Atlantic", -14400000L},
            {"Chile/Continental", -14400000L},
            {"Etc/GMT+4", -14400000L},
            {"PRT", -14400000L},
            {"SystemV/AST4", -14400000L},
            {"SystemV/AST4ADT", -14400000L},
            {"America/St_Johns", -12600000L},
            {"CNT", -12600000L},
            {"Canada/Newfoundland", -12600000L},
            {"AGT", -10800000L},
            {"America/Araguaina", -10800000L},
            {"America/Argentina/Buenos_Aires", -10800000L},
            {"America/Argentina/Catamarca", -10800000L},
            {"America/Argentina/ComodRivadavia", -10800000L},
            {"America/Argentina/Cordoba", -10800000L},
            {"America/Argentina/Jujuy", -10800000L},
            {"America/Argentina/La_Rioja", -10800000L},
            {"America/Argentina/Mendoza", -10800000L},
            {"America/Argentina/Rio_Gallegos", -10800000L},
            {"America/Argentina/Salta", -10800000L},
            {"America/Argentina/San_Juan", -10800000L},
            {"America/Argentina/Tucuman", -10800000L},
            {"America/Argentina/Ushuaia", -10800000L},
            {"America/Bahia", -10800000L},
            {"America/Belem", -10800000L},
            {"America/Buenos_Aires", -10800000L},
            {"America/Catamarca", -10800000L},
            {"America/Cayenne", -10800000L},
            {"America/Cordoba", -10800000L},
            {"America/Fortaleza", -10800000L},
            {"America/Godthab", -10800000L},
            {"America/Jujuy", -10800000L},
            {"America/Maceio", -10800000L},
            {"America/Mendoza", -10800000L},
            {"America/Miquelon", -10800000L},
            {"America/Montevideo", -10800000L},
            {"America/Paramaribo", -10800000L},
            {"America/Recife", -10800000L},
            {"America/Rosario", -10800000L},
            {"America/Santarem", -10800000L},
            {"America/Sao_Paulo", -10800000L},
            {"Antarctica/Rothera", -10800000L},
            {"Atlantic/Stanley", -10800000L},
            {"BET", -10800000L},
            {"Brazil/East", -10800000L},
            {"Etc/GMT+3", -10800000L},
            {"America/Noronha", -7200000L},
            {"Atlantic/South_Georgia", -7200000L},
            {"Brazil/DeNoronha", -7200000L},
            {"Etc/GMT+2", -7200000L},
            {"America/Scoresbysund", -3600000L},
            {"Atlantic/Azores", -3600000L},
            {"Atlantic/Cape_Verde", -3600000L},
            {"Etc/GMT+1", -3600000L},
            {"Africa/Abidjan", 0L},
            {"Africa/Accra", 0L},
            {"Africa/Bamako", 0L},
            {"Africa/Banjul", 0L},
            {"Africa/Bissau", 0L},
            {"Africa/Casablanca", 0L},
            {"Africa/Conakry", 0L},
            {"Africa/Dakar", 0L},
            {"Africa/El_Aaiun", 0L},
            {"Africa/Freetown", 0L},
            {"Africa/Lome", 0L},
            {"Africa/Monrovia", 0L},
            {"Africa/Nouakchott", 0L},
            {"Africa/Ouagadougou", 0L},
            {"Africa/Sao_Tome", 0L},
            {"Africa/Timbuktu", 0L},
            {"America/Danmarkshavn", 0L},
            {"Atlantic/Canary", 0L},
            {"Atlantic/Faeroe", 0L},
            {"Atlantic/Faroe", 0L},
            {"Atlantic/Madeira", 0L},
            {"Atlantic/Reykjavik", 0L},
            {"Atlantic/St_Helena", 0L},
            {"Eire", 0L},
            {"Etc/GMT", 0L},
            {"Etc/GMT+0", 0L},
            {"Etc/GMT-0", 0L},
            {"Etc/GMT0", 0L},
            {"Etc/Greenwich", 0L},
            {"Etc/UCT", 0L},
            {"Etc/UTC", 0L},
            {"Etc/Universal", 0L},
            {"Etc/Zulu", 0L},
            {"Europe/Belfast", 0L},
            {"Europe/Dublin", 0L},
            {"Europe/Guernsey", 0L},
            {"Europe/Isle_of_Man", 0L},
            {"Europe/Jersey", 0L},
            {"Europe/Lisbon", 0L},
            {"Europe/London", 0L},
            {"GB", 0L},
            {"GB-Eire", 0L},
            {"GMT", 0L},
            {"GMT0", 0L},
            {"Greenwich", 0L},
            {"Iceland", 0L},
            {"Portugal", 0L},
            {"UCT", 0L},
            {"UTC", 0L},
            {"Universal", 0L},
            {"WET", 0L},
            {"Zulu", 0L},
            {"Africa/Algiers", 3600000L},
            {"Africa/Bangui", 3600000L},
            {"Africa/Brazzaville", 3600000L},
            {"Africa/Ceuta", 3600000L},
            {"Africa/Douala", 3600000L},
            {"Africa/Kinshasa", 3600000L},
            {"Africa/Lagos", 3600000L},
            {"Africa/Libreville", 3600000L},
            {"Africa/Luanda", 3600000L},
            {"Africa/Malabo", 3600000L},
            {"Africa/Ndjamena", 3600000L},
            {"Africa/Niamey", 3600000L},
            {"Africa/Porto-Novo", 3600000L},
            {"Africa/Tunis", 3600000L},
            {"Africa/Windhoek", 3600000L},
            {"Arctic/Longyearbyen", 3600000L},
            {"Atlantic/Jan_Mayen", 3600000L},
            {"CET", 3600000L},
            {"ECT", 3600000L},
            {"Etc/GMT-1", 3600000L},
            {"Europe/Amsterdam", 3600000L},
            {"Europe/Andorra", 3600000L},
            {"Europe/Belgrade", 3600000L},
            {"Europe/Berlin", 3600000L},
            {"Europe/Bratislava", 3600000L},
            {"Europe/Brussels", 3600000L},
            {"Europe/Budapest", 3600000L},
            {"Europe/Copenhagen", 3600000L},
            {"Europe/Gibraltar", 3600000L},
            {"Europe/Ljubljana", 3600000L},
            {"Europe/Luxembourg", 3600000L},
            {"Europe/Madrid", 3600000L},
            {"Europe/Malta", 3600000L},
            {"Europe/Monaco", 3600000L},
            {"Europe/Oslo", 3600000L},
            {"Europe/Paris", 3600000L},
            {"Europe/Podgorica", 3600000L},
            {"Europe/Prague", 3600000L},
            {"Europe/Rome", 3600000L},
            {"Europe/San_Marino", 3600000L},
            {"Europe/Sarajevo", 3600000L},
            {"Europe/Skopje", 3600000L},
            {"Europe/Stockholm", 3600000L},
            {"Europe/Tirane", 3600000L},
            {"Europe/Vaduz", 3600000L},
            {"Europe/Vatican", 3600000L},
            {"Europe/Vienna", 3600000L},
            {"Europe/Warsaw", 3600000L},
            {"Europe/Zagreb", 3600000L},
            {"Europe/Zurich", 3600000L},
            {"MET", 3600000L},
            {"Poland", 3600000L},
            {"ART", 7200000L},
            {"Africa/Blantyre", 7200000L},
            {"Africa/Bujumbura", 7200000L},
            {"Africa/Cairo", 7200000L},
            {"Africa/Gaborone", 7200000L},
            {"Africa/Harare", 7200000L},
            {"Africa/Johannesburg", 7200000L},
            {"Africa/Kigali", 7200000L},
            {"Africa/Lubumbashi", 7200000L},
            {"Africa/Lusaka", 7200000L},
            {"Africa/Maputo", 7200000L},
            {"Africa/Maseru", 7200000L},
            {"Africa/Mbabane", 7200000L},
            {"Africa/Tripoli", 7200000L},
            {"Asia/Amman", 7200000L},
            {"Asia/Beirut", 7200000L},
            {"Asia/Damascus", 7200000L},
            {"Asia/Gaza", 7200000L},
            {"Asia/Hebron", 7200000L},
            {"Asia/Istanbul", 7200000L},
            {"Asia/Jerusalem", 7200000L},
            {"Asia/Nicosia", 7200000L},
            {"Asia/Tel_Aviv", 7200000L},
            {"CAT", 7200000L},
            {"EET", 7200000L},
            {"Egypt", 7200000L},
            {"Etc/GMT-2", 7200000L},
            {"Europe/Athens", 7200000L},
            {"Europe/Bucharest", 7200000L},
            {"Europe/Chisinau", 7200000L},
            {"Europe/Helsinki", 7200000L},
            {"Europe/Istanbul", 7200000L},
            {"Europe/Kiev", 7200000L},
            {"Europe/Mariehamn", 7200000L},
            {"Europe/Nicosia", 7200000L},
            {"Europe/Riga", 7200000L},
            {"Europe/Simferopol", 7200000L},
            {"Europe/Sofia", 7200000L},
            {"Europe/Tallinn", 7200000L},
            {"Europe/Tiraspol", 7200000L},
            {"Europe/Uzhgorod", 7200000L},
            {"Europe/Vilnius", 7200000L},
            {"Europe/Zaporozhye", 7200000L},
            {"Israel", 7200000L},
            {"Libya", 7200000L},
            {"Turkey", 7200000L},
            {"Africa/Addis_Ababa", 10800000L},
            {"Africa/Asmara", 10800000L},
            {"Africa/Asmera", 10800000L},
            {"Africa/Dar_es_Salaam", 10800000L},
            {"Africa/Djibouti", 10800000L},
            {"Africa/Juba", 10800000L},
            {"Africa/Kampala", 10800000L},
            {"Africa/Khartoum", 10800000L},
            {"Africa/Mogadishu", 10800000L},
            {"Africa/Nairobi", 10800000L},
            {"Antarctica/Syowa", 10800000L},
            {"Asia/Aden", 10800000L},
            {"Asia/Baghdad", 10800000L},
            {"Asia/Bahrain", 10800000L},
            {"Asia/Kuwait", 10800000L},
            {"Asia/Qatar", 10800000L},
            {"Asia/Riyadh", 10800000L},
            {"EAT", 10800000L},
            {"Etc/GMT-3", 10800000L},
            {"Europe/Kaliningrad", 10800000L},
            {"Europe/Minsk", 10800000L},
            {"Indian/Antananarivo", 10800000L},
            {"Indian/Comoro", 10800000L},
            {"Indian/Mayotte", 10800000L},
            {"Asia/Riyadh87", 11224000L},
            {"Asia/Riyadh88", 11224000L},
            {"Asia/Riyadh89", 11224000L},
            {"Mideast/Riyadh87", 11224000L},
            {"Mideast/Riyadh88", 11224000L},
            {"Mideast/Riyadh89", 11224000L},
            {"Asia/Tehran", 12600000L},
            {"Iran", 12600000L},
            {"Asia/Baku", 14400000L},
            {"Asia/Dubai", 14400000L},
            {"Asia/Muscat", 14400000L},
            {"Asia/Tbilisi", 14400000L},
            {"Asia/Yerevan", 14400000L},
            {"Etc/GMT-4", 14400000L},
            {"Europe/Moscow", 14400000L},
            {"Europe/Samara", 14400000L},
            {"Europe/Volgograd", 14400000L},
            {"Indian/Mahe", 14400000L},
            {"Indian/Mauritius", 14400000L},
            {"Indian/Reunion", 14400000L},
            {"NET", 14400000L},
            {"W-SU", 14400000L},
            {"Asia/Kabul", 16200000L},
            {"Antarctica/Mawson", 18000000L},
            {"Asia/Aqtau", 18000000L},
            {"Asia/Aqtobe", 18000000L},
            {"Asia/Ashgabat", 18000000L},
            {"Asia/Ashkhabad", 18000000L},
            {"Asia/Dushanbe", 18000000L},
            {"Asia/Karachi", 18000000L},
            {"Asia/Oral", 18000000L},
            {"Asia/Samarkand", 18000000L},
            {"Asia/Tashkent", 18000000L},
            {"Etc/GMT-5", 18000000L},
            {"Indian/Kerguelen", 18000000L},
            {"Indian/Maldives", 18000000L},
            {"PLT", 18000000L},
            {"Asia/Calcutta", 19800000L},
            {"Asia/Colombo", 19800000L},
            {"Asia/Kolkata", 19800000L},
            {"IST", 19800000L},
            {"Asia/Kathmandu", 20700000L},
            {"Asia/Katmandu", 20700000L},
            {"Antarctica/Vostok", 21600000L},
            {"Asia/Almaty", 21600000L},
            {"Asia/Bishkek", 21600000L},
            {"Asia/Dacca", 21600000L},
            {"Asia/Dhaka", 21600000L},
            {"Asia/Qyzylorda", 21600000L},
            {"Asia/Thimbu", 21600000L},
            {"Asia/Thimphu", 21600000L},
            {"Asia/Yekaterinburg", 21600000L},
            {"BST", 21600000L},
            {"Etc/GMT-6", 21600000L},
            {"Indian/Chagos", 21600000L},
            {"Asia/Rangoon", 23400000L},
            {"Indian/Cocos", 23400000L},
            {"Antarctica/Davis", 25200000L},
            {"Asia/Bangkok", 25200000L},
            {"Asia/Ho_Chi_Minh", 25200000L},
            {"Asia/Hovd", 25200000L},
            {"Asia/Jakarta", 25200000L},
            {"Asia/Novokuznetsk", 25200000L},
            {"Asia/Novosibirsk", 25200000L},
            {"Asia/Omsk", 25200000L},
            {"Asia/Phnom_Penh", 25200000L},
            {"Asia/Pontianak", 25200000L},
            {"Asia/Saigon", 25200000L},
            {"Asia/Vientiane", 25200000L},
            {"Etc/GMT-7", 25200000L},
            {"Indian/Christmas", 25200000L},
            {"VST", 25200000L},
            {"Antarctica/Casey", 28800000L},
            {"Asia/Brunei", 28800000L},
            {"Asia/Choibalsan", 28800000L},
            {"Asia/Chongqing", 28800000L},
            {"Asia/Chungking", 28800000L},
            {"Asia/Harbin", 28800000L},
            {"Asia/Hong_Kong", 28800000L},
            {"Asia/Kashgar", 28800000L},
            {"Asia/Krasnoyarsk", 28800000L},
            {"Asia/Kuala_Lumpur", 28800000L},
            {"Asia/Kuching", 28800000L},
            {"Asia/Macao", 28800000L},
            {"Asia/Macau", 28800000L},
            {"Asia/Makassar", 28800000L},
            {"Asia/Manila", 28800000L},
            {"Asia/Shanghai", 28800000L},
            {"Asia/Singapore", 28800000L},
            {"Asia/Taipei", 28800000L},
            {"Asia/Ujung_Pandang", 28800000L},
            {"Asia/Ulaanbaatar", 28800000L},
            {"Asia/Ulan_Bator", 28800000L},
            {"Asia/Urumqi", 28800000L},
            {"Australia/Perth", 28800000L},
            {"Australia/West", 28800000L},
            {"CTT", 28800000L},
            {"Etc/GMT-8", 28800000L},
            {"Hongkong", 28800000L},
            {"PRC", 28800000L},
            {"Singapore", 28800000L},
            {"Australia/Eucla", 31500000L},
            {"Asia/Dili", 32400000L},
            {"Asia/Irkutsk", 32400000L},
            {"Asia/Jayapura", 32400000L},
            {"Asia/Pyongyang", 32400000L},
            {"Asia/Seoul", 32400000L},
            {"Asia/Tokyo", 32400000L},
            {"Etc/GMT-9", 32400000L},
            {"JST", 32400000L},
            {"Japan", 32400000L},
            {"Pacific/Palau", 32400000L},
            {"ROK", 32400000L},
            {"ACT", 34200000L},
            {"Australia/Adelaide", 34200000L},
            {"Australia/Broken_Hill", 34200000L},
            {"Australia/Darwin", 34200000L},
            {"Australia/North", 34200000L},
            {"Australia/South", 34200000L},
            {"Australia/Yancowinna", 34200000L},
            {"AET", 36000000L},
            {"Antarctica/DumontDUrville", 36000000L},
            {"Asia/Yakutsk", 36000000L},
            {"Australia/ACT", 36000000L},
            {"Australia/Brisbane", 36000000L},
            {"Australia/Canberra", 36000000L},
            {"Australia/Currie", 36000000L},
            {"Australia/Hobart", 36000000L},
            {"Australia/Lindeman", 36000000L},
            {"Australia/Melbourne", 36000000L},
            {"Australia/NSW", 36000000L},
            {"Australia/Queensland", 36000000L},
            {"Australia/Sydney", 36000000L},
            {"Australia/Tasmania", 36000000L},
            {"Australia/Victoria", 36000000L},
            {"Etc/GMT-10", 36000000L},
            {"Pacific/Chuuk", 36000000L},
            {"Pacific/Guam", 36000000L},
            {"Pacific/Port_Moresby", 36000000L},
            {"Pacific/Saipan", 36000000L},
            {"Pacific/Truk", 36000000L},
            {"Pacific/Yap", 36000000L},
            {"Australia/LHI", 37800000L},
            {"Australia/Lord_Howe", 37800000L},
            {"Antarctica/Macquarie", 39600000L},
            {"Asia/Sakhalin", 39600000L},
            {"Asia/Vladivostok", 39600000L},
            {"Etc/GMT-11", 39600000L},
            {"Pacific/Efate", 39600000L},
            {"Pacific/Guadalcanal", 39600000L},
            {"Pacific/Kosrae", 39600000L},
            {"Pacific/Noumea", 39600000L},
            {"Pacific/Pohnpei", 39600000L},
            {"Pacific/Ponape", 39600000L},
            {"SST", 39600000L},
            {"Pacific/Norfolk", 41400000L},
            {"Antarctica/McMurdo", 43200000L},
            {"Antarctica/South_Pole", 43200000L},
            {"Asia/Anadyr", 43200000L},
            {"Asia/Kamchatka", 43200000L},
            {"Asia/Magadan", 43200000L},
            {"Etc/GMT-12", 43200000L},
            {"Kwajalein", 43200000L},
            {"NST", 43200000L},
            {"NZ", 43200000L},
            {"Pacific/Auckland", 43200000L},
            {"Pacific/Fiji", 43200000L},
            {"Pacific/Funafuti", 43200000L},
            {"Pacific/Kwajalein", 43200000L},
            {"Pacific/Majuro", 43200000L},
            {"Pacific/Nauru", 43200000L},
            {"Pacific/Tarawa", 43200000L},
            {"Pacific/Wake", 43200000L},
            {"Pacific/Wallis", 43200000L},
            {"NZ-CHAT", 45900000L},
            {"Pacific/Chatham", 45900000L},
            {"Etc/GMT-13", 46800000L},
            {"MIT", 46800000L},
            {"Pacific/Apia", 46800000L},
            {"Pacific/Enderbury", 46800000L},
            {"Pacific/Fakaofo", 46800000L},
            {"Pacific/Tongatapu", 46800000L},
            {"Etc/GMT-14", 50400000L},
            {"Pacific/Kiritimati", 50400000L}
        };

        public static string GetDefaultTimeZone()
        {
            return "UTC"; //Use this as default for now
        }

        public static DateTime GetDateForTimeZone(DateTime dateTime, string serverTimeZoneAlias)
        {
            if (ServerTimeZoneOffsets.ContainsKey(serverTimeZoneAlias))
            {
                return dateTime.AddMilliseconds(ServerTimeZoneOffsets[serverTimeZoneAlias]);
            }

            return dateTime;
        }
    }
}
