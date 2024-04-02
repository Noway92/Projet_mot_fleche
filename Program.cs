using System;
using System.IO;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Projet3
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Nécessaire pour tester si une sauvegarde existe
            string[] sauvegarde = File.ReadAllLines("Enregistrement.cvs");
            string[] test = sauvegarde[0].Split(';');

            // On initialise toutes les variables utiles
            string langue;
            int nb_joueur = 0;
            // Variables pour choisir si on créé des plateaux aléatoirement ou on reprend des plateaux déjà existant
            int choix = -1;
            // variable qui va dire si on reprend ou pas la dernière partie
            int reprendre = 0;
            //Utile pour la sauvegarde 
            int difficulte_initiale = 1;
            // Elle nous dit quel mode de jeu on veut faire (5 niveaux à la suite ou 1 seul niveau)
            int niveau = -1;
            //Variable qui va gérer le temps
            int Chrono = -1;
            //nécessaire pour savoir quel joueur va jouer
            int index_reprise = 0;
            Joueur[] joueurs;
            Plateau[] plateaux;
            Dictionnaire dico;
            Jeu jeu;


            // On teste s'il y a une sauvegarde
            if (Convert.ToInt32(test[0]+"") == 1)
            {
                do
                {
                    reprendre = -1;
                    Console.WriteLine("Voulez vous reprendre la partie précédente?\nSi oui ecrivez 1 sinon 0");
                    try
                    {
                        reprendre = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                } while (reprendre > 1 || reprendre < 0);

            } 
            // Si l'utilisateur ne veut pas ou ne peux pas reprendre la sauvegarde
            if (reprendre==0)
            {
                Console.WriteLine("Avec quelle langue voulez vous jouer ?\nLes langues possibles sont le français: 'FR' et l'anglais : 'EN'");
                // le test pour voir si la langue existe est fait dans le dictionnaire
                langue = Console.ReadLine().ToUpper();
                dico = new Dictionnaire(langue);                
                // retourne un entier compris entre 0 et 1
                do
                {
                    Console.WriteLine("Pour toute la partie, voulez-vous jouer avec des plateaux crées aléatoirement ou des plateaux déjà existants ?\nSi vous voulez jouer avec des plateaux déjà existants saisissez 0 sinon saisissez 1");
                    try
                    {
                        choix = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                } while (choix < 0 || choix > 1);


                // Sert simplement de test pour être sûr que le nombre de joueur est le bon si celui-ci est trop élevé
                string nb_joueurs = "oui";
                do
                {
                    nb_joueur = -1;
                    nb_joueurs = "oui";
                    Console.WriteLine("Quel est le nombre de Joueur ?");
                    //Cela permet de tester si nb est bien un int
                    try
                    {
                        nb_joueur = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    if (choix== 1 && nb_joueur > 10)
                    {
                        Console.WriteLine("Etes-vous sûr de vouloir jouer à autant? Si oui écrivez oui");
                        nb_joueurs = Console.ReadLine();
                    }
                    if(choix == 0 && nb_joueur>3)
                    {
                        Console.WriteLine("Il n'y a des plateaux déjà existants que pour 3 joueurs, pas plus");
                        nb_joueurs = "nan";
                    }
                } while (nb_joueur <= 0 || nb_joueurs != "oui");

                // On initialise.
                joueurs = new Joueur[nb_joueur];
                plateaux = new Plateau[nb_joueur];
                joueurs = Creation_Joueurs(joueurs, nb_joueur);
                niveau = mode_de_jeu();

            }
            // On reprend là ou en étaient tous les joueurs avant d'avoir quitté
            else
            {
                // Cette boucle permet de récupérer toutes les informations de la sauvegarde.
                string[] Ligne1 = sauvegarde[1].Split(';');
                langue = Ligne1[0];
                dico = new Dictionnaire(langue);
                choix = Convert.ToInt32(Ligne1[1] + "");
                nb_joueur = Convert.ToInt32(Ligne1[2] + "");
                difficulte_initiale = Convert.ToInt32(Ligne1[3] + "");
                niveau = Convert.ToInt32(Ligne1[4] + "");
                Chrono = Convert.ToInt32(Ligne1[5] + "");
                index_reprise = Convert.ToInt32(Ligne1[6] + "");
                joueurs = new Joueur[nb_joueur];
                plateaux = new Plateau[nb_joueur];
                // Cette boucle sert simplement à initialiser chaque joueur en fonction de la sauvegarde
                for(int i =0;i<nb_joueur;i++)
                {
                    string[] Ligne_actuel = sauvegarde[i+2].Split(';');
                    joueurs[i] = new Joueur(Ligne_actuel[0], Convert.ToInt32(Ligne_actuel[1] + ""));
                    // Cette 2nd boucle permet d'initialiser tous les mots trouvés.
                    for(int j=0;j<Ligne_actuel.Length-2;j++)
                    {
                        if (Ligne_actuel[j+2]!=null && Ligne_actuel[j + 2].Length!=0)
                        {
                            joueurs[i].Mot_trouvee.Add(Ligne_actuel[j + 2]);
                        }
                        
                    }                                                      
                }
            }        
            // Il y a 2 niveaux : le premier où l'on peut faire les 5 niveaux de difficulté, le second où  l'on fait simplement 1 seul niveau
            switch (niveau)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("Vous commencez donc à la difficulté "+difficulte_initiale);
                    if(reprendre!=1)
                    {
                        Chrono = -1;
                        // Boucle qui rend un temps en seconde positif
                        do
                        {
                            Console.WriteLine("Quel est le temps maximum pour trouver tous les mots dans chaque niveau? (donner le temps en secondes)");
                            try
                            {
                                Chrono = Convert.ToInt32(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        } while (Chrono <= 0);
                    }
                    //Cette boucle permet de faire jouer à tous les niveaux de difficulté ou de reprendre à un certain niveau en fonction de la sauvegarde
                    for(int difficulte_actuel = difficulte_initiale ;difficulte_actuel<=5;difficulte_actuel++)
                    {
                        //Seulement lors de la première boucle suite à la sauvegarde
                        if (reprendre==1)
                        {
                            
                            for (int i = 0; i < nb_joueur; i++)
                            {
                                plateaux[i] = new Plateau("plateau_niveau"+ difficulte_actuel +"_joueur"+ (i + 1), "plateau/",dico);
                            }
                        }
                        else
                        {
                            index_reprise = 0;
                            // On initialise tous les plateaux et on écrit ces plateaux sur des documents CSV
                            for (int i = 0; i < nb_joueur; i++)
                            {
                                plateaux[i] = new Plateau(difficulte_actuel, dico, choix, i);
                                plateaux[i].ToFile("plateau_niveau" + difficulte_actuel + "_joueur" + (i + 1));
                            }
                        }

                        // On initialise la variable jeu
                        jeu = new Jeu(joueurs, plateaux, dico);
                        // On commence à enregistrer la partie + elle permet d'enregistrer chaque niveau de difficulté  
                        jeu.Enregistrement(choix, niveau,Chrono,index_reprise);
                        jeu.Jouer(choix, niveau, Chrono,reprendre,index_reprise);
                        // Cette variable permet de reprendre une partie normale après que la sauvegarde ait été prise en compte.
                        reprendre = 0;
                        if (difficulte_actuel<5)
                        { 
                            Console.WriteLine("Voici le score actuel et les mots trouvés durant cette partie");
                            for (int i = 0; i < nb_joueur; i++)
                            {
                                Console.WriteLine(joueurs[i].toString());
                            }
                            Console.WriteLine("\nVous passez à la difficulté suivante : " + (difficulte_actuel + 1));
                            //On laisse afficher cela 3 secondes pour que le joueur puisse lire 
                            DateTime attendre = DateTime.Now;
                            while((DateTime.Now-attendre).TotalSeconds<3)
                            {

                            }
                        }
                        // On affiche quelque chose de différent si le programme est finit
                        else
                        {
                            Console.WriteLine("Voici le score final");
                            for (int i = 0; i < nb_joueur; i++)
                            {
                                Console.WriteLine(joueurs[i].toString_Final());
                            }
                        }

                    }
                    break;
                case 2:                 
                    // C'est le cas ou on prend la sauvegarde pour initialiser les joueurs
                    if (reprendre == 1)
                    {
                        for (int i = 0; i < nb_joueur; i++)
                        {
                            plateaux[i] = new Plateau("plateau_niveau" + difficulte_initiale + "_joueur" + (i + 1), "plateau/",dico);
                        }
                    }
                    else
                    {
                        // Retourne un temps max en secondes
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("Quel est le temps maximum pour trouver tous les mots dans ce niveau? (donner le temps en secondes)");
                            try
                            {
                                Chrono = Convert.ToInt32(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        } while (Chrono <= 0);
                        difficulte_initiale = -1;
                        do
                        {
                            Console.WriteLine("le niveau de difficulté doit être compris entre 1 et 5");
                            try
                            {
                                difficulte_initiale = Convert.ToInt32(Console.ReadLine());
                            }

                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }


                        } while (difficulte_initiale < 1 || difficulte_initiale > 5) ;

                        for (int i = 0; i < nb_joueur; i++)
                        {
                            plateaux[i] = new Plateau(difficulte_initiale, dico, choix, i);
                            plateaux[i].ToFile("plateau_niveau" + difficulte_initiale + "_joueur" + (i + 1));
                        }
                    }
                    jeu = new Jeu(joueurs, plateaux, dico);
                    // On commence à enregistrer la partie
                    jeu.Enregistrement(choix,niveau,Chrono,index_reprise);
                    jeu.Jouer(choix,niveau,Chrono,reprendre,index_reprise);
                    Console.WriteLine("Voici le score final et les mots trouvés durant la partie par chaque joueur");
                    for (int i = 0; i < nb_joueur; i++)
                    {
                        Console.WriteLine(joueurs[i].toString());
                    }
                    break;
            }
            // A la fin de la partie l'enregistrement n'est plus possible on met simplement 0
            StreamWriter writer = new StreamWriter("Enregistrement.cvs");
            writer.WriteLine("0;");
            writer.Close();

            Console.ReadKey();
        }
        /// <summary>
        /// On demande à l'utilisateur à quel jeu veut-il jouer
        /// </summary>
        /// <returns></returns>
        static int mode_de_jeu()
        {
            Console.WriteLine("A quel mode de jeu voulez vous jouer ?");
            int niveau = 0;
            do
            {
                Console.WriteLine("Ecrivez 1 si vous voulez faire 5 niveaux à la suite\nEcrivez 2 si vous voulez faire simplement un seul niveau de difficulté");
                try
                {
                    niveau = Convert.ToInt32(Console.ReadLine());
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            } while (niveau != 1 && niveau !=2);
            return niveau;                       
        }
        /// <summary>
        /// On créé les joueurs en fonction de leur nom
        /// </summary>
        /// <param name="joueurs"></param>
        /// <param name="nb"></param>
        /// <returns></returns>
        static Joueur[] Creation_Joueurs(Joueur[] joueurs,int nb)
        {
            string nom;
            for (int i = 0; i < nb; i++)
            {
                // c'est simplement pour écrire joueur 1 au lieu de joueur 0
                int nombre = i + 1;
                bool test_Point_Virgule = true;
                do
                {
                    Console.WriteLine("Quel est le nom du Joueur " + nombre + " ?");
                    // le test pour voir si le nom est possible est directement fait dans la classe joueur
                    nom = Console.ReadLine();
                    for (int j = 0; j < nom.Length && test_Point_Virgule == true; j++)
                    {
                        if (nom[j] == ';')
                        {
                            test_Point_Virgule = false;
                        }
                    }

                } while (test_Point_Virgule == false);
                                                      
                joueurs[i] = new Joueur(nom);
            }
            return joueurs;
        }
        
    }
}