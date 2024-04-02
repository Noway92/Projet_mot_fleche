using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

namespace Projet3
{
    public class Jeu
    {

        private Joueur[] joueurs;
        private Plateau[] plateaux;
        private Dictionnaire dico;

        public Jeu(Joueur[] joueurs, Plateau[] plateaux, Dictionnaire dico)
        {
            this.joueurs = joueurs;
            this.plateaux = plateaux;
            this.dico = dico;                                        
        }

        public Plateau[] Plateaux
        {
            get { return this.plateaux; }
            set { this.plateaux = value; }
        }

        public Joueur[] Joueurs
        {
            get { return this.joueurs; }
            set { this.joueurs = value; }
        }

        public Dictionnaire Dico
        {
            get { return this.dico; }
        }
        /// <summary>
        /// Cette fonction va permettre de faire jouer un niveau de difficulté à tous les joueurs à partir de l'index_reprise (utile pour la sauvegarde)
        /// </summary>
        public void Jouer(int choix, int niveau, int chrono,int reprise,int index_reprise)
        {
            for(int i =index_reprise;i<joueurs.Length; i++)
            {
                // Cette sauvegarde permet simplement de changer l'indice du joueur jouant actuellement 
                Enregistrement(choix, niveau, chrono, i);
                DateTime Debut = DateTime.Now;
                // simplement pour afficher joueur 1 au lieu de joueur 0            
                Console.WriteLine("\nC'est au tour du joueur " + (i + 1) + " : " + joueurs[i].Nom);
                // C'est pour éviter de réénitialiser les mot trouvee d'un joueur après sauvegarde
                if (reprise != 1)
                {
                    // On initialise les mots trouvés par le joueurs en fonction du nomre de mot à rechercher
                    joueurs[i].Mot_trouvee = new List<string>(plateaux[i].Mot_a_rechercher.Count);
                }
                // Si le joueur à trouvé tous les mots ou le temps est terminé le tour du joueur s'arrète
                // On compare durant toute la boucle que le temps n'a pas été dépassé. 
                while ((plateaux[i].Mot_a_rechercher.Count!=0) && ((DateTime.Now-Debut).TotalSeconds<chrono))  
                {
                    // On affiche le plateau
                    Console.WriteLine("\n" + plateaux[i].toString());
                    // On initialise toutes les variables utiles pour la demande à l'utilisateur
                    string mot = null;
                    string direction = null; 
                    int ligne = -1;                                     
                    int colonne = -1;
                    //Ce tableau est ce qui va correspondre a ce que l'utilisateur va écrire
                    string mot_essayer;
                    //C'est le test pour voir si l'utilisateur a bien rentré ce qui était demandé
                    bool entrer;
                    do
                    {
                        // On écrit le temps restant à chaque tentative du joueur.
                        Console.WriteLine("Attention il vous reste " + (chrono - ((DateTime.Now - Debut).TotalSeconds)) + " secondes.\n");
                        entrer = false;
                        Console.WriteLine("Veuillez entrer les informations du mot que vous avez trouvé comme suit : 'mot direction ligne Colonne' (Mettre des espaces entre chaque informations, Ex de direction : E ou NO, on commence à la ligne 1 et la colonne 1)");
                        mot_essayer = Console.ReadLine();
                        string[] mot_essayer_tab = mot_essayer.Split(' ');
                        if (mot_essayer_tab.Length == 4)
                        {
                             mot = mot_essayer_tab[0].ToUpper();
                             //On teste si le mot appartient à la liste des mots à rechercher
                             if (mot.Length != 0 && plateaux[i].Mot_a_rechercher.Contains(mot) == true)
                             {
                                 direction = mot_essayer_tab[1].ToUpper();
                                 // On test si la direction appartient bien au direction possible
                                 if (direction == "E" || direction == "S" || direction == "O" || direction == "N" || direction == "SO" || direction == "SE" || direction == "NO" || direction == "NE")
                                 {
                                     ligne = -1;
                                     // On test si l'utilisateur a bien entré un entier pour la ligne
                                     try
                                     {
                                         ligne = Convert.ToInt32(mot_essayer_tab[2]) - 1;
                                     }
                                     catch (Exception e)
                                     {
                                         Console.WriteLine(e.Message + "\nAttention à bien donner un entier pour la ligne");
                                     }
                                     //On test que la ligne rentré est bien dans le tableau
                                     if (ligne < plateaux[i].Grille.GetLength(0) && ligne >= 0)
                                     {
                                         colonne = -1;
                                         // On test si l'utilisateur a bien entré un entier pour la colonne
                                         try
                                         {
                                             colonne = Convert.ToInt32(mot_essayer_tab[3]) - 1;
                                         }
                                         catch (Exception e)
                                         {
                                             Console.WriteLine(e.Message + "\n Attention à bien donner un entier pour la colonne");
                                         }
                                         //On test que la colonne rentré est bien dans le tableau
                                         if (colonne < plateaux[i].Grille.GetLength(1) && colonne >= 0)
                                         {
                                             entrer = true;

                                         }
                                     }
                                 }
                             }
                             else
                             {
                                 // On aide juste l'utilisateur s'il donne des mots n'appartenant pas à la liste des mots à recherchés.
                                 if (mot.Length != 0)
                                 {
                                     Console.WriteLine("Attention à proposer un mot qui appartient bien à la liste des mots à recherchés\n");
                                 }
                             }
                        }
                        Console.WriteLine("\n" + plateaux[i].toString());
                        //Ce while permet de sortir directement lorsque le timer est dépassé ou que le joueur a entrer des informations plausible
                    } while ((mot_essayer == null || mot_essayer.Length == 0 || entrer == false) && (DateTime.Now - Debut).TotalSeconds <= chrono);

                    if ((DateTime.Now - Debut).TotalSeconds <chrono)
                    {
                        //Console.Clear();
                        // On test si le mot entrer est bien à sa position 
                        if (plateaux[i].Test_Plateau(mot, ligne, colonne, direction) == true)
                        {
                            Console.Clear();
                            Console.WriteLine("\nBRAVO!! Vous avez trouvé un mot\n");
                            joueurs[i].Add_Mot(mot);
                            plateaux[i].Mot_a_rechercher.Remove(mot);
                            // On considère que le nomre de point par mot correspond aux nombres de lettres du mot
                            joueurs[i].Add_Score(mot.Length) ;
                            plateaux[i].ToFile("plateau_niveau" + plateaux[i].Niveau_de_difficulté + "_joueur" + (i + 1));
                            Enregistrement(choix,niveau,chrono,i);
                        }
                        else
                        {
                            Console.WriteLine("\nDOMMAGE, le mot n'est pas à la bonne place ou a déja été trouvé\n");
                        }
                    }
                }
                Console.Clear();
                // Ce IF permet d'ajouter des points en fonction du temps qu'il reste au joueur
                if (plateaux[i].Mot_a_rechercher.Count==0)
                { 
                    double secondes_restantes = chrono - (DateTime.Now - Debut).TotalSeconds;
                    // On considère que 1 seconde = 1 point
                    int score_à_ajouter = (int)secondes_restantes;
                    //On ajoute une valeur par rapport au temps qu'il reste : 1 s = 1 point
                    joueurs[i].Add_Score(score_à_ajouter);                    
                }
                else
                {
                    Console.WriteLine("Le temps maximal a été dépassé\n");
                }
            }
        }  
        /// <summary>
        /// Fonction principale pour enregistrer, elle permet de sauvegarder chaque joueur et les caractéristiques de la partie : langue, choix (plateaux aléatoires ou existants à l'avance), difficulté, niveau, chrono, index du joueur actuel
        /// </summary>
        /// <param name="Choix"></param>
        /// <param name="niveau"></param>
        /// <param name="chrono"></param>
        /// <param name="index_reprise"></param>
        public void Enregistrement(int Choix,int niveau,int chrono,int index_reprise)
        {
            StreamWriter writer = new StreamWriter("Enregistrement.cvs");
            writer.WriteLine("1;");
            writer.WriteLine(dico.Langue + ";" + Choix + ";" + joueurs.Length + ";" + plateaux[0].Niveau_de_difficulté + ";" + niveau +";"+chrono+";"+index_reprise) ;
            for(int i =0;i<joueurs.Length;i++)
            {
                writer.Write(joueurs[i].Nom + ";" + joueurs[i].Score+";");
                if(joueurs[i].Mot_trouvee!=null && joueurs[i].Mot_trouvee.Count!=0)
                {
                    for (int j = 0; j < joueurs[i].Mot_trouvee.Count; j++)
                    {
                        writer.Write(joueurs[i].Mot_trouvee[j] + ";");
                    }
                }
                writer.WriteLine();
            }
            writer.Close();
        }
    }
}
