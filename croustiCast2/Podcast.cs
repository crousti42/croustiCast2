using System;
using log4net;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace croustiCast
{
    class Podcast : MainModel
    {
        public int iId = 0;
        public string sUrl = "";
        public string sName = "";
        public string sDesc = "";
        public string sImgUrl = "";
        public string sImgPath = "";
        public List<Episode> aAllEpisodes;

        public void initialize(int iInId, string sInUrl, string sInName, string sInDesc, string sInImgUrl, string sInImgPath, List<Episode> aInAllEpisodes)
        {
            iId = iInId;
            sUrl = sInUrl;
            sName = sInName;
            sDesc = sInDesc;
            sImgUrl = sInImgUrl;
            aAllEpisodes = aInAllEpisodes;

            setImgPath(sInImgPath); // Gestion de l'image en local
        }

        public void initWithId(int iInId)
        {
            iId = iInId;

            System.Data.SqlClient.SqlConnection conn = getConnectionData();
            List<Episode> aEpisodesList = new List<Episode>();
            String requete = "";

            try
            {
                conn.Open();

                requete = "SELECT PodcastUrl, " +
                                "PodcastName, " +
                                "PodcastImgUrl, " +
                                "PodcastImgPath, " +
                                "PodcastDesc " +
                                "FROM Podcasts " +
                                "WHERE PodcastId=" + iId;

                SqlCommand command = new SqlCommand(requete, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    sUrl = reader["PodcastUrl"].ToString();
                    sName = reader["PodcastName"].ToString();
                    sDesc = reader["PodcastDesc"].ToString();
                    sImgUrl = reader["PodcastImgUrl"].ToString();

                    setEpisodes(); // Gestion des épisodes
                    setImgPath(reader["PodcastImgPath"].ToString()); // Gestion de l'image en local
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion à la base de donnée!");
                log.Error("--------------------\n" + "Requete: " + requete + "\n" + ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        private void setImgPath(String sTempImgPath)
        {
            if (sTempImgPath == "")
            {
                sImgPath = getImgPath(sName);
            }
            else
            {
                sImgPath = sTempImgPath;
            }

            if (!File.Exists(sImgPath))
            {
                downloadPodcastImage();        
            }
        }

        private String getImgPath(String sPodcastName)
        {
            return Path.Combine(sContentPath, "img", sPodcastName.Replace(" ", string.Empty) + ".png");
        }

        public void insertPodcast()
        {
            System.Data.SqlClient.SqlConnection conn = getConnectionData();

            conn.Open();

            String requete = "INSERT INTO Podcasts(PodcastUrl, PodcastName, PodcastImgUrl, PodcastImgPath, PodcastDesc) " +
                                "VALUES (@PodcastUrl, @PodcastName, @PodcastImgUrl, @PodcastImgPath, @PodcastDesc)";

            SqlCommand command = new SqlCommand(requete, conn);
            command.Parameters.AddWithValue("@PodcastUrl", sUrl);
            command.Parameters.AddWithValue("@PodcastName", sName);
            command.Parameters.AddWithValue("@PodcastImgUrl", sImgUrl);
            command.Parameters.AddWithValue("@PodcastImgPath", sImgPath);
            command.Parameters.AddWithValue("@PodcastDesc", sDesc);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur d'écriture dans la base de données.");
                log.Error("--------------------\n" + "Requete: " + requete + "\n" + ex.ToString());
            }
            finally
            {
                command.Dispose();
            }
        }
        
        public List<String> getEpisodesNames()
        {
            List<String> aAllEpisodesNames = new List<String>();
            String sCurrentDate = "";

            foreach (Episode oCurrentEpisode in aAllEpisodes)
            {
                sCurrentDate = oCurrentEpisode.oDate.ToString("yyyy/MM/dd");
                aAllEpisodesNames.Add(sCurrentDate + " - " + oCurrentEpisode.sName);
            }

            return aAllEpisodesNames;
        }

        private void downloadPodcastImage()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sImgUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {
                using (Stream inputStream = response.GetResponseStream())
                using (Stream outputStream = File.OpenWrite(sImgPath))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }
            }
            else
            {
                System.IO.File.Copy(sAppPath + "\\icones\\photo_not_available.png", sImgPath, true);
            }
            Console.WriteLine(response.ContentType);
        }

        private void setEpisodes()
        {
            System.Data.SqlClient.SqlConnection conn = getConnectionData();
            List<Episode> aEpisodesList = new List<Episode>();
            String requete = "";

            try
            {
                conn.Open();

                requete = "SELECT EpisodeId, " +
                                "EpisodePodcastId, " +
                                "EpisodeUrl, " +
                                "EpisodeName, " +
                                "EpisodeDesc, " +
                                "EpisodeDate, " +
                                "EpisodeDuration, " +
                                "EpisodePlayed, " +
                                "EpisodeFinished " +
                                "FROM Episodes " +
                                "WHERE EpisodePodcastId=" + iId;
                
                SqlCommand command = new SqlCommand(requete, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Episode oCurrentEpisode = new Episode();
                    oCurrentEpisode.initialize(Convert.ToInt32(reader["EpisodeId"]),
                                                Convert.ToInt32(reader["EpisodePodcastId"]),
                                                reader["EpisodeUrl"].ToString(),
                                                reader["EpisodeName"].ToString(),
                                                reader["EpisodeDesc"].ToString(),
                                                Convert.ToDateTime(reader["EpisodeDate"]),
                                                Convert.ToInt32(reader["EpisodeDuration"]),
                                                Convert.ToInt32(reader["EpisodePlayed"]),
                                                Convert.ToBoolean(reader["EpisodeFinished"])
                                                );

                    aEpisodesList.Add(oCurrentEpisode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion à la base de donnée!");
                log.Error("--------------------\n" + "Requete: " + requete + "\n" + ex.ToString());
            }
            finally
            {
                conn.Close();
            }

            aAllEpisodes = aEpisodesList;
        }
    }
}
