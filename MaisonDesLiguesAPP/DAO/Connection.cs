﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaisonDesLiguesAPP.Business;
using MySql.Data.MySqlClient;

namespace MaisonDesLiguesAPP
{
    public class Connection
    {
        private string connexionParams;

        public Connection()
        {
            Initialize();
        }

        private void Initialize()
        {
            string server = "localhost";
            string port = "3306";
            string database = "bdd_app";
            string uid = "root";
            string password = "";
            connexionParams = "SERVER=" + server + ";" + "PORT=" + port + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        }
        public void ajouterClub(Club club)
        {
            using (MySqlConnection connexion = new MySqlConnection(connexionParams))
            {
                connexion.Open();
                string requete = "INSERT INTO club (id_club, titre_club, url_club, " +
                    "adresse_club, codepostal_club, ville_club, mail_club, telephone_club, " +
                    "id_type_club) VALUES (NULL, @titre, @url, @adresse, @codepostal, " +
                    "@ville, @mail, @telephone, @id_club);";
                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                cmd.Parameters.AddWithValue("@titre", club.nom);
                cmd.Parameters.AddWithValue("@url", club.lien);
                cmd.Parameters.AddWithValue("@adresse", club.adresse);
                cmd.Parameters.AddWithValue("@codepostal", club.codepostal);
                cmd.Parameters.AddWithValue("@ville", club.ville);
                cmd.Parameters.AddWithValue("@mail", club.email);
                cmd.Parameters.AddWithValue("@telephone", club.telephone);
                cmd.Parameters.AddWithValue("@id_club", club.idClub);
                cmd.ExecuteNonQuery();
            }
        }
        public void ajouterAdherent(Adhérents adhérents)
        {
            using (MySqlConnection connexion = new MySqlConnection(connexionParams))
            {
                connexion.Open();
                string requete = "INSERT INTO adherent (id_adherent, numero_licence, nom_adherent, " +
                    "prenom_adherent, datenaissance_adherent, adresse_adherent, codepostal_adherent, " +
                    "ville_adherent, cotisation_adherent, id_club) VALUES (NULL, NULL, " +
                    "@nom, @prenom, @date, @adresse, @codepostal, @ville, NULL, NULL);";
                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                cmd.Parameters.AddWithValue("@nom", adhérents.Nom);
                cmd.Parameters.AddWithValue("@prenom", adhérents.Prenom);
                cmd.Parameters.AddWithValue("@date", adhérents.Naissance);
                cmd.Parameters.AddWithValue("@adresse", adhérents.Adresse);
                cmd.Parameters.AddWithValue("@codepostal", adhérents.Codepostal);
                cmd.Parameters.AddWithValue("@ville", adhérents.Ville);
                cmd.ExecuteNonQuery();
            }
        }

        public void supprimerAdherent(Adhérents adhérents)
        {
            using (MySqlConnection connexion = new MySqlConnection(connexionParams))
            {
                connexion.Open();
                string requete = "DELETE FROM adherent WHERE id_adherent = @id";
                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                cmd.Parameters.AddWithValue("@id", adhérents.Id);
                cmd.ExecuteNonQuery();
            }
        }
        public void supprimerClub(Club club)
        {
            using (MySqlConnection connexion = new MySqlConnection(connexionParams))
            {
                connexion.Open();
                string requete = "DELETE FROM club WHERE id_club = @id";
                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                cmd.Parameters.AddWithValue("@id", club.id);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Adhérents> listeAdherents()
        {
            List<Adhérents> liste = new List<Adhérents>();
            Adhérents adhérents;
            using(MySqlConnection connexion = new MySqlConnection(connexionParams))
            {
                connexion.Open();
                string requete = "SELECT id_adherent, nom_adherent, prenom_adherent, datenaissance_adherent" +
                    ", adresse_adherent, codepostal_adherent, ville_adherent " +
                    "FROM adherent;";
                MySqlCommand cmd = new MySqlCommand(requete,connexion);
                using(MySqlDataReader datareader = cmd.ExecuteReader())
                {
                    while (datareader.Read())
                    {
                        adhérents = new Adhérents((string)datareader["nom_adherent"],
                            (string)datareader["prenom_adherent"],(DateTime)datareader["datenaissance_adherent"],
                            (string)datareader["adresse_adherent"], (string)datareader["codepostal_adherent"],
                            (string)datareader["ville_adherent"]);
                        adhérents.Id = (int)datareader["id_adherent"];
                        liste.Add(adhérents);
                    }
                }
            }
            return liste;
        }
        public List<Club> listeClubs()
        {
            List<Club> liste = new List<Club>();
            Club club;
            using (MySqlConnection connexion = new MySqlConnection(connexionParams))
            {
                connexion.Open();
                string requete = "SELECT id_club, titre_club, url_club, adresse_club" +
                    ", codepostal_club, ville_club, mail_club, telephone_club, id_type_club " +
                    "FROM club;";
                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                using (MySqlDataReader datareader = cmd.ExecuteReader())
                {
                    while (datareader.Read())
                    {
                        club = new Club((string)datareader["titre_club"],
                            (string)datareader["url_club"], (string)datareader["adresse_club"], 
                            (string)datareader["codepostal_club"], (string)datareader["ville_club"],
                            (string)datareader["telephone_club"], (string)datareader["mail_club"],
                            (int)datareader["id_type_club"]);
                        liste.Add(club);
                        club.id = (int)datareader["id_club"];
                    }
                }
            }
            return liste;
        }

        public List<Evenements> listeEvents(Club club)
        {
            List<Evenements> liste = new List<Evenements>();
            Evenements evenements;
            using (MySqlConnection connexion = new MySqlConnection(connexionParams))
            {
                connexion.Open();
                string requete = "SELECT id_evenement, titre_evenement, debut_evenement," +
                    " fin_evenement FROM evenement WHERE id_club = @idClub";
                                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                cmd.Parameters.AddWithValue("@idClub", club.id);
                cmd.ExecuteNonQuery();
                using (MySqlDataReader datareader = cmd.ExecuteReader())
                {
                    while (datareader.Read())
                    {
                        evenements = new Evenements((string)datareader["titre_evenement"],
                            (DateTime)datareader["debut_evenement"], (DateTime)datareader["fin_evenement"]);
                        evenements.id = (int)datareader["id_evenement"];
                        liste.Add(evenements);
                    }
                }
            }
            return liste;
        }
        public List<Adhérents> listeAdherentsInClub(Club club)
        {
            List<Adhérents> liste = new List<Adhérents>();
            Adhérents adh;
            using (MySqlConnection connexion = new MySqlConnection(connexionParams))
            {
                connexion.Open();
                string requete = "SELECT id_adherent, nom_adherent, prenom_adherent, datenaissance_adherent" +
                    ", adresse_adherent, codepostal_adherent, ville_adherent " +
                    "FROM adherent WHERE id_club = @idClub;";
                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                cmd.Parameters.AddWithValue("@idClub", club.id);
                cmd.ExecuteNonQuery();
                using (MySqlDataReader datareader = cmd.ExecuteReader())
                {
                    while (datareader.Read())
                    {
                        adh = new Adhérents((string)datareader["nom_adherent"],
                            (string)datareader["prenom_adherent"], (DateTime)datareader["datenaissance_adherent"],
                            (string)datareader["adresse_adherent"], (string)datareader["codepostal_adherent"],
                            (string)datareader["ville_adherent"]);
                        adh.Id = (int)datareader["id_adherent"];
                        liste.Add(adh);
                    }
                }
            }
            return liste;
        }
        public List<Adhérents> listeAdherentsSansClubs()
        {
            List<Adhérents> liste = new List<Adhérents>();
            Adhérents adhérents;
            using (MySqlConnection connexion = new MySqlConnection(connexionParams))
            {
                connexion.Open();
                string requete = "SELECT id_adherent, nom_adherent, prenom_adherent, datenaissance_adherent" +
                    ", adresse_adherent, codepostal_adherent, ville_adherent " +
                    "FROM adherent " +
                    "WHERE id_club IS NULL;";
                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                using (MySqlDataReader datareader = cmd.ExecuteReader())
                {
                    while (datareader.Read())
                    {
                        adhérents = new Adhérents((string)datareader["nom_adherent"],
                            (string)datareader["prenom_adherent"], (DateTime)datareader["datenaissance_adherent"],
                            (string)datareader["adresse_adherent"], (string)datareader["codepostal_adherent"],
                            (string)datareader["ville_adherent"]);
                        adhérents.Id = (int)datareader["id_adherent"];
                        liste.Add(adhérents);
                    }
                }
            }
            return liste;
        }

        public void affectation(Adhérents adhérents, Club club)
        {
            using(MySqlConnection connexion = new MySqlConnection(connexionParams))
            {
                connexion.Open();
                string requete = "UPDATE adherent " +
                    "SET id_club = @idClub " +
                    "WHERE id_adherent = @idAdherent;";
                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                cmd.Parameters.AddWithValue("@idClub", club.id);
                cmd.Parameters.AddWithValue("@idAdherent", adhérents.Id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
