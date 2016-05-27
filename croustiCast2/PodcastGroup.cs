using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace croustiCast
{
    class PodcastsGroup : MainModel
    {
        
        public List<Podcast> getAllPodcasts()
        {
            System.Data.SqlClient.SqlConnection conn = getConnectionData();
            List<Podcast> aPodcastsList = new List<Podcast>();
            String requete = "";

            try
            {
                conn.Open();

                requete = "SELECT PodcastId " +
                                "FROM Podcasts";
                
                SqlCommand command = new SqlCommand(requete, conn);
                SqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    Podcast oCurrentPodcast = new Podcast();
                    oCurrentPodcast.initWithId(Convert.ToInt32(reader["PodcastId"]));

                    aPodcastsList.Add(oCurrentPodcast);
                }

                reader.Close();
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

            return aPodcastsList;
        }

    }


}
