using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace croustiCast
{
    class Episode : MainModel
    {
        public int iId = 0;
        public int iPodcastId = 0;
        public string sUrl = "";
        public string sName = "";
        public string sDesc = "";
        public DateTime oDate;
        public int iDuration;
        public int iDurPlayed;
        public bool bIsFinished;

        public void initialize(int iInId, int iInPodcastId, string sInUrl, string sInName, string sInDesc, DateTime oInDate, int iInDuration, int iInDurPlayed, bool bInIsFinished)
        {
            iId = iInId;
            iPodcastId = iInPodcastId;
            sUrl = sInUrl;
            sName = sInName;
            sDesc = sInDesc;
            oDate = oInDate;
            iDuration = iInDuration;
            iDurPlayed = iInDurPlayed;
            bIsFinished = bInIsFinished;
        }


    }
}
