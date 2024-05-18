using Microsoft.Win32.SafeHandles;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace Projet_SQL
{
    internal class Program
    {
        static readonly string mdp = "";
        static readonly string chemin = "SERVER=127.0.0.1;PORT=3306;DATABASE=SalleDeSport;UID=root;PASSWORD="+mdp+";";

        static void Main()
        {
            Console.Clear();
            Console.WriteLine("====== Menu Principal ======");
            Console.WriteLine("Bienvenue dans le système de gestion de salle de sport");
            while (true)
            {
                Console.WriteLine("Entrer votre Pseudo : ");
                string pseudo = Console.ReadLine();

                string textmembres = "select Niveau\nfrom Membres m\nwhere m.pseudo = \"" + pseudo + "\";";
                MySqlConnection maConnection = new MySqlConnection(chemin);
                maConnection.Open();
                MySqlCommand command = maConnection.CreateCommand();
                command.CommandText = textmembres;
                MySqlDataReader reader = command.ExecuteReader();
                string[] valuetab = new string[reader.FieldCount];
                if (!reader.HasRows)
                {
                    Console.WriteLine("Pseudo introuvable");
                    Thread.Sleep(1000);
                    Main();
                    break;
                }
                else
                {
                    while (reader.Read())
                    {
                        bool etat = true;
                        while (etat)
                        {
                            Console.WriteLine("Entrer votre mots de passe : ");
                            string mdp = Console.ReadLine();
                            string mdpmemebres = "select MembreID\nfrom Membres m\nwhere m.Mots_de_passe = \"" + mdp + "\";";
                            MySqlConnection maConnectionn = new MySqlConnection(chemin);
                            maConnectionn.Open();
                            MySqlCommand commandd = maConnectionn.CreateCommand();
                            commandd.CommandText = mdpmemebres;
                            MySqlDataReader readerr = commandd.ExecuteReader();
                            string[] valuetab2 = new string[readerr.FieldCount];
                            if (readerr.FieldCount != 1) Console.WriteLine("Mots de passe introuvable");
                            else
                            {
                                etat = false;
                                while (readerr.Read())
                                {
                                    if (reader.GetValue(0).ToString() == "3")
                                    {
                                        ShowMainMenuT(Convert.ToInt32(readerr.GetValue(0).ToString()));
                                    }
                                    else if (reader.GetValue(0).ToString() == "1")
                                    {
                                        ShowMainMenuF(Convert.ToInt32(readerr.GetValue(0).ToString()));
                                    }
                                }
                            }
                            maConnectionn.Close();
                        }
                    }
                    maConnection.Close();
                }
            }
        }
        static void ShowMainMenuT(int id)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("====== Menu Admin ======");
                Console.WriteLine("1. Gestion des membres");
                Console.WriteLine("2. Gestion des cours");
                Console.WriteLine("3. Gestion des coachs");
                Console.WriteLine("4. Rapports");
                Console.WriteLine("5. Quitter");
                Console.Write("Veuillez choisir une option (1-5): ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Gestion_membres();
                        break;
                    case "2":
                        Gestion_cours();
                        break;
                    case "3":
                        Gestion_coachs();
                        break;
                    case "4":
                        if (AdminOrSousAdmin(id))
                        {
                            Console.WriteLine("Vous ne pouvez pas accéder à ce module");
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Rapport(id);
                        }
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Option invalide. Veuillez choisir une option entre 1 et 5.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        static bool AdminOrSousAdmin(int id)
        {
            MySqlConnection maConnection = new MySqlConnection(chemin);
            string text = "select niveau\nfrom membres\nwhere membreid=" + id;
            maConnection.Open();
            MySqlCommand command = maConnection.CreateCommand();
            command.CommandText = text;
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetValue(0).ToString() == "2") return true;
            }
            return false;
        }
        static void SQLLecteur(string text)
        {
            MySqlConnection maConnection = new MySqlConnection(chemin);
            maConnection.Open();
            MySqlCommand command = maConnection.CreateCommand();
            command.CommandText = text;
            MySqlDataReader reader = command.ExecuteReader();
            string[] valuetab = new string[reader.FieldCount];
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    valuetab[i] = reader.GetValue(i).ToString();
                    Console.Write(valuetab[i] + "\t");
                }
                Console.WriteLine();
            }
            maConnection.Close();
        }
        static void SQLWriter(string text)
        {
            MySqlConnection maConnection = new MySqlConnection(chemin);
            maConnection.Open();
            MySqlCommand cmd = new MySqlCommand(text, maConnection);
            cmd.ExecuteNonQuery();
            maConnection.Close();
        }
        static void Gestion_membres()
        {
            Console.Clear();
            bool exit = false;
            Console.WriteLine("=== Gestion des Membres ===");
            while (!exit)
            {
                Console.WriteLine("1. Ajouter membres");
                Console.WriteLine("2. Valider adhésion");
                Console.WriteLine("3. Valider réservation");
                Console.WriteLine("4. Quitter");
                Console.Write("Veuillez choisir une option (1-4): ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Ajouter_membre();
                        break;
                    case "2":
                        Valider_inscription_adhésion();
                        break;
                    case "3":
                        Valider_inscription_reservation();
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Option invalide. Veuillez choisir une option entre 1 et 5.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        static void Gestion_cours()
        {
            Console.Clear();
            Console.WriteLine("=== Gestion des Cours ===");
            Console.WriteLine("Voici la liste des cours");
            SQLLecteur("select * from cours;");
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n\n1. Ajouter un Cours");
                Console.WriteLine("2. Supprimer un Cours");
                Console.WriteLine("3. Modifier un Cours");
                Console.WriteLine("4. Quitter");
                Console.Write("Veuillez choisir une option (1-4): ");
                switch (Console.ReadLine())
                {
                    case "1":
                        Ajouter_Cours();
                        break;
                    case "2":
                        Supression_Cours();
                        break;
                    case "3":
                        Modifier_Cours();
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Option invalide. Veuillez choisir une option entre 1 et 5.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        static void Gestion_coachs()
        {
            Console.Clear();
            Console.WriteLine("=== Gestion des Coachs ===");
            Console.WriteLine("1. Ajouter un coach");
            Console.WriteLine("2. Supprimer un coach");
            Console.WriteLine("3. Modifier un coach");
            Console.WriteLine("4. Afficher tous les coachs");
            Console.WriteLine("5. Quitter");
            Console.Write("Veuillez choisir une option (1-5): ");

            string choix = Console.ReadLine();
            switch (choix)
            {
                case "1":
                    Console.Clear();
                    AjouterCoach();
                    Debug();

                    Gestion_coachs();

                    break;
                case "2":
                    Console.Clear();
                    SupprimerCoach();
                    Debug();

                    Gestion_coachs();

                    break;
                case "3":
                    Console.Clear();
                    ModifierCoach();
                    Debug();

                    Gestion_coachs();
                    break;
                case "4":
                    Console.Clear();
                    AfficherTousLesCoachs();
                    Debug();

                    Gestion_coachs();
                    break;
                case "5":

                default:
                    Console.WriteLine("Option invalide. Veuillez choisir une option entre 1 et 5.");
                    break;
            }
        }
        static void Debug()
        {
            Console.WriteLine("Appuyer pour retourner au menu");
            Console.ReadKey();
        }
        static void AjouterCoach()
        {
            Console.Clear();
            Console.WriteLine("Voici la listes des coachs : \n");
            AfficherTousLesCoachs();
            string connectionString = chemin;
            MySqlConnection maConnection = new MySqlConnection(connectionString);
            maConnection.Open();
            MySqlCommand command = maConnection.CreateCommand();
            command.CommandText = "SELECT MAX(CoachID) FROM Coachs;";
            MySqlDataReader reader = command.ExecuteReader();
            int CoachId = 0;
            if (reader.Read() && reader[0] != DBNull.Value)
            {
                CoachId = Convert.ToInt32(reader.GetValue(0)) + 1;
            }
            reader.Close();
            Console.WriteLine("Entrez les informations du nouveau coach.");
            Console.Write("Nom: ");
            string nom = Console.ReadLine();
            Console.Write("Prénom: ");
            string prenom = Console.ReadLine();
            Console.Write("Numéro de téléphone: ");
            string telephone = Console.ReadLine();
            string text = $"INSERT INTO Coachs (Coachid, Nom, Prenom, Telephone) VALUES ('" + CoachId + "', '" + nom + "', '" + prenom + "', '" + telephone + "');";
            SQLWriter(text);
            Console.WriteLine("Coach ajouté avec succès.");
        }
        static void SupprimerCoach()
        {
            Console.Clear();
            Console.WriteLine("Voici la listes des coachs : \n");
            AfficherTousLesCoachs();
            Console.Write("Entrez l'ID du coach à supprimer: ");
            int id = Convert.ToInt32(Console.ReadLine());
            string text = $"DELETE FROM Coachs WHERE CoachID = {id};";
            SQLWriter(text);
            Console.WriteLine("Coach supprimé avec succès.");
        }
        static void ModifierCoach()
        {
            Console.Clear();
            Console.WriteLine("Voici la listes des coachs : \n");
            AfficherTousLesCoachs();
            Console.Write("Entrez l'ID du coach à modifier: ");
            int coachId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Nouveau nom (laisser vide si pas de changement): ");
            string nom = Console.ReadLine();
            Console.Write("Nouveau prénom (laisser vide si pas de changement): ");
            string prenom = Console.ReadLine();
            Console.Write("Nouveau numéro de téléphone (laisser vide si pas de changement): ");
            string telephone = Console.ReadLine();

            List<string> lists = new List<string>();
            if (!string.IsNullOrEmpty(nom)) lists.Add($"Nom = '{nom}'");
            if (!string.IsNullOrEmpty(prenom)) lists.Add($"Prenom = '{prenom}'");
            if (!string.IsNullOrEmpty(telephone)) lists.Add($"Telephone = '{telephone}'");//

            if (lists.Count > 0)
            {
                string updateQuery = $"UPDATE Coachs SET {string.Join(", ", lists)} WHERE CoachID = {coachId};";
                SQLWriter(updateQuery);
                Console.WriteLine("Coach modifié avec succès.");
            }
            else
            {
                Console.WriteLine("Aucune information modifiée.");
            }
        }
        static void AfficherTousLesCoachs()
        {
            string text = "SELECT CoachID, Nom, Prenom, Telephone FROM Coachs;";
            SQLLecteur(text);
        }
        static void Rapport(int id)
        {
            Console.WriteLine("=== Génération de Rapports ===");
            Console.WriteLine("1. Rapport sur les informations des membres");
            Console.WriteLine("2. Rapport sur la participation aux cours collectifs");
            Console.WriteLine("3. Rapport sur les coachs les plus suivis");
            Console.WriteLine("4. Retour au menu principal");
            Console.Write("Veuillez choisir une option (1-4): ");

            string choix = Console.ReadLine();
            switch (choix)
            {
                case "1":
                    Console.Clear();
                    SQLLecteur("select membreid, nom, prenom from membres;");
                    Console.WriteLine("");
                    Rapport(id);
                    break;
                case "2":
                    Console.Clear();
                    SQLLecteur("select * from Cours;");
                    Console.WriteLine("");
                    Rapport(id);
                    break;
                case "3":
                    Console.Clear();
                    SQLLecteur("select coachid, nom, prenom from Coachs;");
                    Console.WriteLine("");
                    Rapport(id);
                    break;
                case "4":
                    ShowMainMenuT(id);
                    break;
                default:
                    Console.WriteLine("Option invalide. Veuillez choisir une option entre 1 et 4.");
                    Console.ReadKey();
                    Rapport(id);
                    break;
            }
        }
        static void ShowMainMenuF(int id)
        {
            Console.Clear();
            Console.WriteLine("Bienvenue dans le système de gestion de salle de sport - Interface Membre");
            MySqlConnection maConnection = new MySqlConnection(chemin);
            string text = "select DateDebut,DateFin\nfrom adhesions\nwhere membreid=" + id;
            maConnection.Open();
            MySqlCommand command = maConnection.CreateCommand();
            command.CommandText = text;
            MySqlDataReader reader = command.ExecuteReader();
            string[] valuetab = new string[reader.FieldCount];
            string datedebut = "";
            string datefin = "";
            while (reader.Read())
            {
                datedebut = reader.GetValue(0).ToString();
                datefin = reader.GetValue(1).ToString();
            }
            reader.Close();
            DateTime Db = convertSQL(datedebut);
            DateTime Df = convertSQL(datefin);
            if (DateTime.Now < Db || DateTime.Now > Df)
            {
                Console.WriteLine("Vous n'avez pas d'adhésions");
                Console.WriteLine("Voulez vous souscrire à une adhésions (oui ou non)");
                string rep = "";
                while (rep.ToLower() != "oui" && rep.ToLower() != "non")
                {
                    rep = Console.ReadLine();
                }
                if (rep == "non") return;
                if (rep =="oui")
                {
                    Console.WriteLine("Annuelle ou mensuelle(A ou M)");
                    string rep1 = "";
                    while (rep1.ToLower() != "a" && rep1.ToLower() != "m")
                    {
                        rep1 = Console.ReadLine();
                    }
                    bool e = rep1 == "a" ? true : false;
                    string text1 = "select Count(*)\nfrom adhesions";
                    command.CommandText = text1;
                    reader = command.ExecuteReader();
                    int ida = 0;
                    while (reader.Read())
                    {
                       ida = Convert.ToInt32(reader.GetValue(0).ToString());
                    }
                    string datedebutt = DateTime.Now.ToString();
                    DateTime date = DateTime.Now;
                    string dateb = "CURRENT_DATE()";
                    string datef = e ? "DATE_ADD(CURDATE(), INTERVAL 1 YEAR)" : "DATE_ADD(CURDATE(), INTERVAL 1 MONTH)";
                    datefin = e ? date.AddYears(1).ToString() : date.AddMonths(1).ToString();
                    datedebutt = datedebutt.Substring(0, 2) + "-" + datedebutt.Substring(3, 2) + "-" + datedebutt.Substring(6, 4);
                    SQLWriter("delete from Adhesions where membreid = " + id + ";");
                    ida++;
                    string texte = "INSERT INTO Adhesions (AdhesionID, MembreID, DateDebut, DateFin, Statut)\r\nVALUES \r\n(" + ida + ", " + id + ", " + dateb + ", " + datef + ", False)";
                    SQLWriter(texte);
                    return;
                }
            }
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("====== Menu Membre ======");
                Console.WriteLine("1. Inscription à un cours");
                Console.WriteLine("2. Historique des cours suivis");
                Console.WriteLine("3. Annulation de réservation");
                Console.WriteLine("4. Quitter");
                Console.Write("Veuillez choisir une option (1-4): ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Incription_Cours(id);
                        break;
                    case "2":
                        Visuel_Historique_Cours(id);
                        break;
                    case "3":
                        Annule_Reservation(id);
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Option invalide. Veuillez choisir une option entre 1 et 4.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        static void Incription_Cours(int id)
        {
            Console.Clear();
            Console.WriteLine("=== Inscription à un cours ===");
            MySqlConnection maConnection = new MySqlConnection(chemin);
            string text = "select statut\nfrom adhesions\nwhere membreid=" + id;
            maConnection.Open();
            MySqlCommand command = maConnection.CreateCommand();
            command.CommandText = text;
            MySqlDataReader reader = command.ExecuteReader();
            string[] valuetab = new string[reader.FieldCount];
            while (reader.Read())
            {
                valuetab[0] = reader.GetValue(0).ToString();
                if (valuetab[0].ToLower() != "true")
                {
                    Console.WriteLine("Vous ne possèdez pas d'ahdésions");
                    Thread.Sleep(1000);
                    return;
                }
            }
            reader.Close();

            text = "SELECT * from cours";
            command.CommandText = text;
            reader = command.ExecuteReader();
            valuetab = new string[reader.FieldCount];
            List<int> list = new List<int>();
            while (reader.Read())
            {
                int placerestante = PlaceRestante(reader.GetValue(0).ToString());
                if (placerestante > 0)
                {
                    list.Add(Convert.ToInt32(reader.GetValue(0).ToString()));
                    Console.WriteLine("Identifiant : " + reader.GetValue(0).ToString());
                    Console.WriteLine("Nom du cours : " + reader.GetValue(1).ToString());
                    Console.WriteLine("Horaire : " + reader.GetValue(2).ToString());
                    Console.WriteLine("Places restantes : " + placerestante);
                    Console.WriteLine("Nom du coachs : " + IdCoachAffichage(reader.GetValue(3).ToString()));
                }
            }
            Console.WriteLine("Choisir l'identifiant qui vous convient");
            int choix = Convert.ToInt32(Console.ReadLine());
            if (!list.Contains(choix))
            {
                Incription_Cours(id);
                return;
            }
            reader.Close();

            text = "SELECT Count(*) from reservations";
            command.CommandText = text;
            reader = command.ExecuteReader();
            int taillereservation = 0;
            while (reader.Read())
            {
                taillereservation = Convert.ToInt32(reader.GetValue(0).ToString());
            }
            reader.Close();

            text = "SELECT Count(*) from HistoriqueCours";
            command.CommandText = text;
            reader = command.ExecuteReader();
            int taillehistorique = 0;
            while (reader.Read())
            {
                taillehistorique = Convert.ToInt32(reader.GetValue(0).ToString());
            }

            text = "INSERT INTO Reservations (ReservationID, MembreID, CoursID, DateReservation,Statut)\n\r\nVALUES (" + (taillereservation + 1) + "," + id + ", " + choix + ", CURRENT_DATE, FALSE);";
            SQLWriter(text);
            Console.WriteLine("Dans combien de jours ?");
            int jours = Convert.ToInt32(Console.ReadLine());
            text = "INSERT INTO HistoriqueCours (HistoriqueID, MembreID, CoursID, DateCours)\r\nVALUES (" + (taillehistorique + 1) + "," + id + "," + choix + ", DATE_ADD(CURRENT_DATE, INTERVAL " + jours + " DAY));";
            SQLWriter(text);
            Console.ReadKey();

        }
        static int PlaceRestante(string idcours)
        {
            MySqlConnection maConnection = new MySqlConnection(chemin);
            maConnection.Open();
            MySqlCommand command = maConnection.CreateCommand();
            string text = "SELECT COUNT(*) AS NombreInscrits FROM Reservations WHERE CoursID =" + idcours + " AND Statut = TRUE";
            command.CommandText = text;
            MySqlDataReader reader = command.ExecuteReader();
            int places = 0;
            while (reader.Read())
            {
                places = Convert.ToInt32(reader.GetValue(0).ToString());
            }
            reader.Close();
            text = "select CapaciteMax from Cours where CoursID = " + idcours + ";";
            command.CommandText = text;
            reader = command.ExecuteReader();
            int capacitéMax = 0;
            while (reader.Read())
            {
                capacitéMax = Convert.ToInt32(reader.GetValue(0).ToString());
            }
            maConnection.Close();
            return capacitéMax - places;
        }
        static void Visuel_Historique_Cours(int id)
        {
            Console.Clear();
            Console.WriteLine("=== Historique des cours suivis ===");
            MySqlConnection maConnection = new MySqlConnection(chemin);
            string text = "select * from HistoriqueCours where membreid = " + id +";";
            maConnection.Open();
            MySqlCommand command = maConnection.CreateCommand();
            command.CommandText = text;
            MySqlDataReader reader = command.ExecuteReader();
            string[] valuetab = new string[reader.FieldCount];
            while (reader.Read())
            {
                Console.WriteLine("Identifiant de l'historique : " + reader.GetValue(0).ToString());
                Console.WriteLine("Membre : ");
                IdMembreAffichage(reader.GetValue(1).ToString());
                Console.WriteLine("\nCours : ");
                CoursAffichage(reader.GetValue(2).ToString());
                Console.WriteLine("Date du cours : " + reader.GetValue(3).ToString());
                Console.WriteLine();
            }
            Console.ReadKey();
        }
        static void Annule_Reservation(int id)
        {
            Console.Clear();
            Console.WriteLine("=== Annulation de réservation ===");
            MySqlConnection maConnection = new MySqlConnection(chemin);
            string text = "select *\nfrom reservations\nwhere membreid=" + id;
            maConnection.Open();
            MySqlCommand command = maConnection.CreateCommand();
            command.CommandText = text;
            MySqlDataReader reader = command.ExecuteReader();
            string[] valuetab = new string[reader.FieldCount];
            string coursid = "";
            List<int> list = new List<int>();
            while (reader.Read())
            {
                if (reader.GetValue(0).ToString() == null)
                {
                    Console.WriteLine("Vous n'avez pas de reservation.");
                    return;
                }
                list.Add(Convert.ToInt32(reader.GetValue(0).ToString()));
                coursid = reader.GetValue(2).ToString();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Identifiant de reservation : " + reader.GetValue(0).ToString());
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Membre : ");
                IdMembreAffichage(reader.GetValue(1).ToString());
                Console.WriteLine("\nCours : ");
                CoursAffichage(reader.GetValue(2).ToString());
                Console.WriteLine("Date : " + reader.GetValue(3).ToString().Substring(0, 10));
                Console.WriteLine("Statut : " + reader.GetValue(4));
                Console.WriteLine("\n");
            }
            Console.WriteLine("Quelle reservation voulez vous supprimer : \n");
            int val = -1;
            while (!list.Contains(val))
            {
              val = Convert.ToInt32(Console.ReadLine());
            }
            string texte = "DELETE FROM reservations\nWHERE ReservationID = " + val + "; ";
            SQLWriter(texte);
            Console.WriteLine("Reservation annulée");
            Debug();
        }
        static void CoursAffichage(string idcours)
        {
            MySqlConnection maConnection = new MySqlConnection(chemin);
            string text = "SELECT * from Cours where CoursID=" + idcours;
            maConnection.Open();
            MySqlCommand command = maConnection.CreateCommand();
            command.CommandText = text;
            MySqlDataReader reader = command.ExecuteReader();
            string[] valuetab = new string[reader.FieldCount];
            List<int> list = new List<int>();
            while (reader.Read())
            {
                int placerestante = PlaceRestante(reader.GetValue(0).ToString());
                if (placerestante > 0)
                {
                    list.Add(Convert.ToInt32(reader.GetValue(0).ToString()));
                    Console.WriteLine("Identifiant du cours : " + reader.GetValue(0).ToString());
                    Console.WriteLine("Nom du cours : " + reader.GetValue(1).ToString());
                    Console.WriteLine("Horaire : " + reader.GetValue(2).ToString());
                    Console.WriteLine("Places restantes : " + placerestante);
                    Console.WriteLine("Nom du coachs : " + IdCoachAffichage(reader.GetValue(3).ToString()));
                }
            }
        }
        static void Ajouter_membre()
        {
            Console.Clear();
            Console.WriteLine("Voici la liste des membres actuel : \n");
            SQLLecteur("select membreid, nom, prenom from membres");
            Console.WriteLine("\nVeuillez entrer les informations du nouveau membre.");

            string connectionString = chemin;
            using (MySqlConnection maConnection = new MySqlConnection(connectionString))
            {
                maConnection.Open();
                MySqlCommand command = maConnection.CreateCommand();
                command.CommandText = "SELECT MAX(MembreID) FROM membres;";
                MySqlDataReader reader = command.ExecuteReader();
                int membreId = 0;
                if (reader.Read() && reader[0] != DBNull.Value)
                {
                    membreId = Convert.ToInt32(reader.GetValue(0)) + 1;
                }
                reader.Close();

                Console.Write("Nom: ");
                string nom = Console.ReadLine();

                Console.Write("Prénom: ");
                string prenom = Console.ReadLine();

                Console.Write("Adresse: ");
                string adresse = Console.ReadLine();

                string telephone;
                while (true)
                {
                    Console.Write("Téléphone: ");
                    telephone = Console.ReadLine();
                    if (EstNumerique(telephone)) break;
                }

                string mail;
                while (true)
                {
                    Console.Write("Mail: ");
                    mail = Console.ReadLine();
                    if (mail.Contains('@')) break;
                }

                Console.Write("Type d'adhésion : \n");
                string typeAdhesion = "";
                Console.WriteLine("1. Mensuelle");
                Console.WriteLine("2. Annuelle");
                Console.WriteLine("3. Annuler");

                switch (Console.ReadLine())
                {
                    case "1":
                        typeAdhesion = "Mensuelle";
                        break;
                    case "2":
                        typeAdhesion = "Annuelle";
                        break;
                    case "3":
                        return;
                }

                string sql = "INSERT INTO Membres (MembreID, Nom, Prenom, Adresse, Telephone, Email, DateInscription, TypeAdhesion) VALUES (@Id, @Nom, @Prenom, @Adresse, @Telephone, @Email, CURDATE(), @TypeAdhesion);";
                MySqlCommand insertCommand = new MySqlCommand(sql, maConnection);
                insertCommand.Parameters.AddWithValue("@Id", membreId);
                insertCommand.Parameters.AddWithValue("@Nom", nom);
                insertCommand.Parameters.AddWithValue("@Prenom", prenom);
                insertCommand.Parameters.AddWithValue("@Adresse", adresse);
                insertCommand.Parameters.AddWithValue("@Telephone", telephone);
                insertCommand.Parameters.AddWithValue("@Email", mail);
                insertCommand.Parameters.AddWithValue("@TypeAdhesion", typeAdhesion);

                insertCommand.ExecuteNonQuery();
                Console.Clear();
                Console.WriteLine("Membre ajouté avec succès.\n");
                Console.WriteLine("Voici la liste des membres actuel : \n");
                SQLLecteur("select membreid, nom, prenom from membres");
                Thread.Sleep(1000);
            }
        }
        static void Valider_inscription_adhésion()
        {
            Console.Clear();
            string text = "select * from adhesions where statut is false";
            MySqlConnection maConnection = new MySqlConnection(chemin);
            maConnection.Open();
            MySqlCommand command = maConnection.CreateCommand();
            command.CommandText = text;
            MySqlDataReader reader = command.ExecuteReader();
            string[] valuetab = new string[reader.FieldCount];
            List<int> ints = new List<int>();
            while (reader.Read())
            {
                ints.Add(Convert.ToInt32(reader.GetValue(0).ToString()));
                Console.WriteLine("Identifiant de l'adhésion : " + reader.GetValue(0).ToString()) ;
                IdMembreAffichage(reader.GetValue(1).ToString());
            }
            reader.Close();
            Console.WriteLine("\nChoisir l'identifiant de l'adhésion qu'il faut valider");
            int reponse = -1; 
            while (!ints.Contains(reponse))
            {
                reponse = Convert.ToInt32(Console.ReadLine());
                if (reponse == 0) return;
            }
            if (ints.Contains(reponse))
            {
                string ajout = "UPDATE Adhesions\nSET Statut = true\nWHERE adhesionID = " + reponse + ";";
                SQLWriter(ajout);
            }
            maConnection.Close();


        }
        static void IdMembreAffichage(string membreid)
        {
            string text = "select nom,prenom from membres where MembreId =\"" + membreid + "\";";
            MySqlConnection maConnection = new MySqlConnection(chemin);
            maConnection.Open();
            MySqlCommand command = maConnection.CreateCommand();
            command.CommandText = text;
            MySqlDataReader reader = command.ExecuteReader();
            string[] valuetab = new string[reader.FieldCount];
            while (reader.Read())
            {
                Console.Write("Identifiant du membre = " + membreid + " : " + reader.GetValue(0).ToString() + " " + reader.GetValue(1).ToString() + "\n");
            }
        }
        static string IdCoachAffichage(string coachid)
        {
            string text = "select nom,prenom from Coachs where CoachID =\"" + coachid + "\";";
            MySqlConnection maConnection = new MySqlConnection(chemin);
            maConnection.Open();
            MySqlCommand command = maConnection.CreateCommand();
            command.CommandText = text;
            MySqlDataReader reader = command.ExecuteReader();
            string[] valuetab = new string[reader.FieldCount];
            string retour = "";
            while (reader.Read())
            {
                retour += reader.GetValue(0).ToString() + " " + reader.GetValue(1).ToString();
            }
            return retour;
        }
        static void Valider_inscription_reservation()
        {
            string text = "select * from Reservations where Statut is false";
            MySqlConnection maConnection = new MySqlConnection(chemin);
            maConnection.Open();
            MySqlCommand command = maConnection.CreateCommand();
            command.CommandText = text;
            MySqlDataReader reader = command.ExecuteReader();
            string[] valuetab = new string[reader.FieldCount];
            List<int> ints = new List<int>();
            while (reader.Read())
            {
                ints.Add(Convert.ToInt32(reader.GetValue(0).ToString()));
                IdMembreAffichage(reader.GetValue(1).ToString());
                for (int i = 2; i < reader.FieldCount - 2; i++)
                {
                    valuetab[i] = reader.GetValue(i).ToString();
                }
                Console.WriteLine();
            }
            Console.WriteLine("Choisir l'identifiant de la personne qu'il faut valider");
            int reponse = Convert.ToInt32(Console.ReadLine());
            if (ints.Contains(reponse))
            {
                string ajout = "UPDATE Reservations\nSET Statut = true\nWHERE MembreID = " + reponse + ";";
                SQLWriter(ajout);
            }
            else
            {
                Valider_inscription_reservation();
            }
            maConnection.Close();
        }
        static void Ajouter_Cours()
        {
            Console.Clear();
            Console.WriteLine("Veuillez entrer les information du Cours : \n\n");
            string connectionString = chemin;
            using (MySqlConnection maConnection = new MySqlConnection(connectionString))
            {
                maConnection.Open();
                MySqlCommand command = maConnection.CreateCommand();
                command.CommandText = "SELECT MAX(CoursID) FROM Cours;";
                MySqlDataReader reader = command.ExecuteReader();
                int coursId = 0;
                if (reader.Read() && reader[0] != DBNull.Value)
                {
                    coursId = Convert.ToInt32(reader.GetValue(0)) + 1;
                }
                reader.Close();

                Console.Write("Veuillez entrée l'heure a laquelle se déroule le cours : ");
                string heure = Console.ReadLine();
                while (!EstNumerique(heure)) 
                {
                    Console.Write("Invalide ! Entrée l'heure a laquelle se déroule le cours : ");
                    heure = Console.ReadLine();
                }
                int coachid = Ajout_Coach(heure);

                Console.WriteLine("Capacité maximal : ");
                int CapMax = Convert.ToInt32(Console.ReadLine());

                string sql = "INSERT INTO Cours (CoursID, Horaire, CoachID, CapaciteMax) VALUES (@Id, @Horaire, @CoachID, @CapaciteMax);";
                MySqlCommand insertCommand = new MySqlCommand(sql, maConnection);
                insertCommand.Parameters.AddWithValue("@Id", coursId);
                insertCommand.Parameters.AddWithValue("@Horaire", heure+":00");
                insertCommand.Parameters.AddWithValue("@CoachID", coachid);
                insertCommand.Parameters.AddWithValue("@CapaciteMax", CapMax);
                Console.Clear();
                insertCommand.ExecuteNonQuery();
                Console.WriteLine("Cours ajouté avec succès.");
                SQLLecteur("select * from Cours;");
            }
        }
        static bool EstNumerique(string S)
        {
            return Regex.IsMatch(S, @"^[0-9]+$");
        }
        static int Ajout_Coach(string horaire)
        {
            string connectionString = chemin;
            int coachid = -1;
            
            using (MySqlConnection maConnection = new MySqlConnection(connectionString))
            {
                maConnection.Open();

                string txt = "select Co.* from coachs Co join Cours C on Co.CoachID = C.CoachID where C.horaire != '"+horaire+ ":00' group by Co.coachid;";
                MySqlCommand command = maConnection.CreateCommand();
                command.CommandText = txt;
                MySqlDataReader reader = command.ExecuteReader();
                Console.WriteLine("Voici les coachs disponibles :\n");
                while (reader.Read())
                {
                    int coachID = reader.GetInt32("CoachID");
                    string nom = reader.GetString("Nom");
                    string prenom = reader.GetString("Prenom");
                    Console.WriteLine($"ID: {coachID}, Nom: {nom}, Prénom: {prenom}");
                }
                Console.WriteLine("\nVeuillez entrer l'identifiant du coach souhaité : ");
                coachid = Convert.ToInt32(Console.ReadLine());

                maConnection.Close();
            }

            return coachid;
        }
        static void Supression_Cours()
        {
            Console.Clear();
            Console.WriteLine("\nVoici tous les cours :\n\nID Cours\tHoraire Cours\tID Coach");
            string text = "select * from Cours;";
            MySqlConnection maConnection = new MySqlConnection(chemin);
            maConnection.Open();
            MySqlCommand command = maConnection.CreateCommand();
            command.CommandText = text;
            MySqlDataReader reader = command.ExecuteReader();
            string[] valuetab = new string[reader.FieldCount];
            List<int> ints = new List<int>();
            while (reader.Read())
            {
                ints.Add(Convert.ToInt32(reader.GetValue(0).ToString()));
                for (int i = 0; i < reader.FieldCount - 1; i++)
                {
                    valuetab[i] = reader.GetValue(i).ToString();
                    Console.Write(valuetab[i] + "\t");
                }
                Console.WriteLine();
            }
            if (ints.Count == 0)
            {
                Console.WriteLine("\nAucun cours n'est disponible pour le moment.");
                maConnection.Close();
                return;
            }
            Console.WriteLine("\nVeuillez entrer l'identifiant du cours à supprimer : ");
            string input = Console.ReadLine();
            int coursid;

            if (int.TryParse(input, out coursid))
            {
                coursid = Convert.ToInt32(input);
            }
            else Console.WriteLine("La valeur entrée n'est pas un chiffre.");
            if (ints.Contains(coursid))
            {
                SQLWriter("delete from reservations where coursid = " + coursid + ";");
                SQLWriter("delete from HistoriqueCours where coursid = " + coursid + ";");
                SQLWriter("delete from Cours where coursid = " + coursid + ";");
                Console.Clear();
                Console.WriteLine("\n\n Voici les cours restant :\n");
                SQLLecteur("select * from cours;");
            }
            else
            {
                Supression_Cours();
            }
            maConnection.Close();
            
        }
        static void Modifier_Cours()
        {
            Console.Clear();
            Console.WriteLine("Voici les cours disponible : \n");
            SQLLecteur("select * from Cours;");
            int idCours = Modif_Coursid();
            Console.WriteLine("\nQue voulez-vous modifier ?");
            Console.WriteLine("1. Horaire");
            Console.WriteLine("2. Coach");
            Console.WriteLine("3. Capacité Maximale");
            Console.WriteLine("4. Retour au menu précédent");
            Console.Write("Veuillez choisir une option (1-4): ");

            string choix = Console.ReadLine();
            switch (choix)
            {
                case "1":
                    Console.WriteLine("Entrez le nouvel horaire pour ce cours : ");
                    string Nheure = Console.ReadLine();
                    while (!EstNumerique(Nheure))
                    {
                        Console.Write("Horaire invalide ! Veuillez entrer un horaire valide ");
                        Nheure = Console.ReadLine();
                    }
                    SQLWriter("UPDATE cours SET horaire = '" + Nheure + ":00' WHERE coursid = " + idCours + ";");
                    Console.WriteLine("Horaire du cours modifié avec succès !");
                    Thread.Sleep(2000);
                    break;
                case "2":
                    string heure = Heure_Cour(idCours);
                    int Ncoach = Ajout_Coach(heure);
                    SQLWriter("UPDATE cours SET CoachID = " + Ncoach + " WHERE coursid = " + idCours + ";");
                    Console.WriteLine("Coach du cours modifié avec succès !");
                    Thread.Sleep(2000);
                    break;
                case "3":
                    Console.WriteLine("Veuillez entrer la nouvelle capacité maximale pour ce cours : ");
                    int NCap = Convert.ToInt32(Console.ReadLine());
                    SQLWriter("UPDATE cours SET CapaciteMax = " + NCap + " WHERE coursid = " + idCours + ";");
                    Console.WriteLine("Capacité maximale du cours modifiée avec succès !");
                    Thread.Sleep(2000);
                    break;
                case "4":
                    Console.WriteLine("Retour au menu précédent.");
                    break;
                default:
                    Console.WriteLine("Option invalide. Veuillez choisir une option entre 1 et 4.");
                    break;
            }
        }
        static int Modif_Coursid()
        {
            string connectionString = chemin;
            int coursid = -1;
            using (MySqlConnection maConnection = new MySqlConnection(connectionString))
            {
                maConnection.Open();
                string text = "select * from cours;";
                MySqlCommand command = maConnection.CreateCommand();
                command.CommandText = text;
                MySqlDataReader reader = command.ExecuteReader();
                string[] valuetab = new string[reader.FieldCount];
                List<int> ints = new List<int>();
                while (reader.Read())
                {
                    ints.Add(Convert.ToInt32(reader.GetValue(0).ToString()));
                    for (int i = 0; i < reader.FieldCount - 1; i++)
                    {
                        valuetab[i] = reader.GetValue(i).ToString();
                    }
                }
                Console.WriteLine("\nVeuillez entré l'identifiant du cours à modifier : ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out coursid))
                {
                    coursid = Convert.ToInt32(input);
                }
                if (ints.Contains(coursid))
                {
                    return coursid;
                }
                else Modif_Coursid();
                maConnection.Close();
            }

            return coursid;
        }
        static string Heure_Cour(int id)
        {

            string heureCours = "";
            string connectionString = chemin;
            using (MySqlConnection maConnection = new MySqlConnection(connectionString))
            {
                maConnection.Open();
                string text = "select substring(horaire, 1, 2) from Cours where coursid = " + id + ";";
                MySqlCommand command = maConnection.CreateCommand();
                command.CommandText = text;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    heureCours = reader.GetString(0);
                }
                maConnection.Close();
            }
            return heureCours;
        }
        static DateTime convertSQL(string dateString)
        {
            DateTime result = DateTime.Parse(dateString);
            Console.WriteLine(result);
            return result;
            DateTime resultExact = DateTime.ParseExact(dateString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            Console.WriteLine(resultExact);
        }
    }
}