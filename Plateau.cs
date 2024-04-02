using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Projet3
{
    public class Plateau
    {

        private char[,] grille;
        private int niveau_de_difficulté;
        // J'ai mis une liste plutôt qu'un plateau pour pouvoir enlever les éléments facilement.
        private List<string> mot_a_rechercher;     
        private Dictionnaire dico;

        // Les entiers choix et numéro sont utiles simplement pour le ToRead
        public Plateau(int niveau_de_difficulte,Dictionnaire dico,int choix,int numéro)
        {
            this.niveau_de_difficulté = niveau_de_difficulte;
            this.dico = dico;
            // On rentre ici si on initalise les plateaux à partir de plateaux déjà créés
            if (choix==0)
            {
                ToRead("plateau_niveau" + niveau_de_difficulte + "_joueur" + (numéro + 1) + dico.Langue, "plateaux_initiaux/");
            }
            else
            {
                // Ce switch permet d'initialiser grille et mot à rechercher en fonction de la difficulté
                switch (niveau_de_difficulte)
                {
                    case 1:
                        this.grille = new char[7, 7];
                        this.mot_a_rechercher = new List<string>(8);
                        break;
                    case 2:
                        this.grille = new char[8, 8];
                        this.mot_a_rechercher = new List<string>(13);
                        break;
                    case 3:
                        this.grille = new char[9, 9];
                        this.mot_a_rechercher = new List<string>(18);
                        break;
                    case 4:
                        this.grille = new char[11, 11];
                        this.mot_a_rechercher = new List<string>(23);
                        break;
                    case 5:
                        this.grille = new char[13, 13];
                        this.mot_a_rechercher = new List<string>(28);
                        break;
                }
                // On remplit tous les caractères avec un espace
                for (int i = 0; i < grille.GetLength(0); i++)
                {
                    for (int j = 0; j < grille.GetLength(1); j++)
                    {
                        grille[i, j] = ' ';
                    }
                }
                this.grille = Creer_Tableau();
            }
        }
        /// <summary>
        /// 2e constructeur nécessaire pour la création des plateaux en provenance d'une sauvegarde
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="path"></param>
        /// <param name="dico"></param>
        public Plateau(string nom,string path,Dictionnaire dico)
        {
            ToRead(nom,path);
            this.dico = dico;
        }

        public char[,] Grille
        {
            get { return this.grille; }
            set { this.grille = value; }
        }

        public int Niveau_de_difficulté
        {
            get { return this.niveau_de_difficulté; }
            set { this.niveau_de_difficulté = value; }
        }

        public List<string> Mot_a_rechercher
        {
            get { return this.mot_a_rechercher; }
            set { this.mot_a_rechercher = value; }
        }

        public Dictionnaire Dico
        {
            get { return this.Dico; }
        }
        /// <summary>
        /// Cette méthode toString va permettre d'écrire le plateau pour que l'utilisateur puisse le voir
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            string a = niveau_de_difficulté + " " +grille.GetLength(0)+ " "+grille.GetLength(1) + " " +mot_a_rechercher.Count+"\n";
            bool test = false;
            for(int j =0;j<mot_a_rechercher.Count && test==false;j++)
            {
                a += mot_a_rechercher[j] + " ";
            }
            a += "\n";
            for(int i =0;i<grille.GetLength(0);i++)
            {
                for(int j=0; j<grille.GetLength(1);j++)
                {
                    a += grille[i, j]+ " ";
                }
                a += "\n";
            }

            return a;
        }
        /// <summary>
        /// Méthode qui va permettre d'enregistrer toutes les informations d'un plateau sur un fichier CVS
        /// </summary>
        /// <param name="nomfile"></param>
        public void ToFile(string nomfile)
        {
            StreamWriter writer = new StreamWriter("plateau/"+nomfile + ".cvs");           
            writer.WriteLine(niveau_de_difficulté + ";" + grille.GetLength(0) + ";" + grille.GetLength(1) + ";" + mot_a_rechercher.Count + ";;;;");
            // On remplit les mots à rechercher sur un fichier CVS
            if(mot_a_rechercher.Count!=0)
            {
                writer.Write(mot_a_rechercher[0]);
                for(int i =1;i<mot_a_rechercher.Count;i++)
                {
                    writer.Write(";"+mot_a_rechercher [i]);
                }
            }
            writer.WriteLine();
            // On remplit le document CVS par la grille
            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    writer.Write(grille[i, j] + ";");
                }
                writer.WriteLine(";");
            }
            writer.Close();    
        }
        /// <summary>
        /// Méthode qui va récupérer toutes les informations d'un plateau en provanance d'un fichier CVS
        /// </summary>
        /// <param name="nomfile"></param>
        /// <param name="path"></param>
        public void ToRead(string nomfile,string path)
        {
            //mettre le nomfile ici
            string[] plateau = File.ReadAllLines(path+nomfile+".cvs");
            // longueur va correspondre à un tableau avec pour chaque case une ligne du fichier
            string[] longueur = plateau[0].Split(';') ;
            // On va initialiser toutes les variables suivantes en fonction du document CVS
            niveau_de_difficulté = Convert.ToInt32(longueur[0]+"");
            int ligne = Convert.ToInt32(longueur[1]+"");
            int colonne = Convert.ToInt32(longueur[2]+"");
            int nb_mots = Convert.ToInt32(longueur[3]+"");
            // On initialise grille et mot à rechercher
            grille = new char[ligne,colonne];
            mot_a_rechercher = new List<string>(nb_mots);
            // On créé un tableau intermédiaire car 'mot_a_rechercher' est une liste et non un tableau.
            string[] intermediaire = plateau[1].Split(';');
            // On initialise 'mot_a_rechercher' 
            for(int i= 0;i< nb_mots; i++)
            {
                mot_a_rechercher.Add(intermediaire[i]);
            }
            // On créé un tableau de tableau intermédiaire car 'grille' est une matrice.
            string[][] intermediaire2 = new string [ligne][];            
            for (int i = 0; i < ligne; i++)
            {
                intermediaire2[i] = new string[colonne];
                intermediaire2[i] = plateau[i + 2].Split(';');

            }
            //On initialise la grille
            for(int i=0;i<intermediaire2.Length;i++)
            {
                for(int j =0; j < intermediaire2[i].Length;j++)
                {
                    // La fonction .Split va créer des tableaux vides : on veur éviter cela
                    if(intermediaire2[i][j] != null  && intermediaire2[i][j].Length!=0)
                    {
                        grille[i, j] = Convert.ToChar(intermediaire2[i][j]);
                    }
                    
                }
            }

        }

        /// <summary>
        /// Cette fonction permet de comprendre ce que signifie la direction et de la convertir en déplacement
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="ligne"></param>
        /// <param name="colonne"></param>
        /// <returns></returns>
        public static int[] Direction(string direction, int ligne, int colonne)
        {
            
            switch(direction)
            {
                case "N":
                    ligne--;
                    break;
                case "S":
                    ligne++;
                    break;
                case "E":
                    colonne++;
                    break;
                case "O":
                    colonne--;
                    break;
                case "NE":
                    colonne++;
                    ligne--;
                    break;
                case "NO":
                    colonne--;
                    ligne--;
                    break;
                case "SE":
                    colonne++;
                    ligne++;
                    break;
                case "SO":
                    colonne--;
                    ligne++;
                    break;
            }
            int[] result = { ligne, colonne };
            return result ;
        }
        /// <summary>
        /// Cette méthode va permettre de voir si on peut ou pas placer un mot en fonction de ses coordoonées. Elle sevira pour créer les plateaux. Cependant elle permettra aussi de voir si un joueur a trouvé un mot aux bonnes positions.
        /// </summary>
        /// <param name="mot"></param>
        /// <param name="ligne"></param>
        /// <param name="colonne"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool Test_Plateau(string mot, int ligne, int colonne, string direction)
        {
            bool rep = false;
            // On teste si le mot appartient n'est pas null et qu'il n'est pas le caractère vide
            if (mot!=null && mot.Length!=0)
            {
                // On teste si le mot appartient bien au dictionnaire
                if (dico.RechDichoRecursif(mot, dico.Mots[mot.Length - 2].Length) == true)
                {
                    // On considère que la reponse finale est vraie et on va tester si oui sinon elle devient fausse
                    rep = true;
                    for (int i = 0; i < mot.Length && rep == true; i++)
                    {
                        // on teste si les lignes et colonnes données appartiennent bien à la grille
                        if (ligne >= grille.GetLength(0) || ligne < 0 || colonne >= grille.GetLength(1) || colonne < 0)
                        {
                            rep = false;
                        }
                        else
                        {
                            //On teste caractère par caractère
                            if (mot[i] == grille[ligne, colonne] || grille[ligne, colonne] == ' ')
                            {
                                int[] tab = Plateau.Direction(direction, ligne, colonne);
                                ligne = tab[0];
                                colonne = tab[1];
                            }
                            else
                            {
                                rep = false;
                            }
                        }
                    }
                }

            }
                 
            return rep;
        }
        /// <summary>
        /// Créer un plateau aléatoire
        /// </summary>
        /// <returns></returns>
        public char[,]Creer_Tableau()
        {
            Random aleatoire = new Random();
            string[] direction = { "E", "S", "O", "N", "SO", "SE", "NO", "NE" };     
            string[] direction_possible = null;
            //le switch permet d'initialiser les directions possibles en fonction du niveau
            switch (niveau_de_difficulté)            
            {
                case 1:
                    direction_possible = new string[2];
                    break;
                case 2:
                    direction_possible = new string[4];
                    break;
                case 3:
                    direction_possible = new string[5];
                    break;
                case 4:
                    direction_possible = new string[6];
                    break;
                case 5:
                    direction_possible = new string[8];
                    break;
            }
            // Ici on va remplir direction possible en fonction du niveau
            for(int j =0;j<direction_possible.Length;j++)
            {
                direction_possible[j] = direction[j];
            }
            // On continue de remplir tant qu'il n'y a pas le nombre de mots attendu
            while (mot_a_rechercher.Count != mot_a_rechercher.Capacity)
            {
                // Il choisit une direction parmi les directions possibles
                int d = aleatoire.Next(0, direction_possible.Length);      
                int colonne = 0;
                int ligne = 0;
                //ATENTION La Longueur ne correspondra pas au nombre de caractères par mot mais à la position dans le dictionnaire EX : longueur 0 donnera un mot de 2 lettres
                int longueur = 0;
                // Ce switch permet d'initialiser la longueur du mot et sa position initiale. On prend des nombres aléatoires pouvant rentrer dans le tableau pour que le mot choisit est plus de chance de se placer
                switch (d)
                { 
                    case 0:         // EST                                   
                        longueur = aleatoire.Next(1, grille.GetLength(1) - 1);  
                        colonne = aleatoire.Next(0, grille.GetLength(1) - longueur - 1);
                        ligne = aleatoire.Next(0, grille.GetLength(0));
                        break;
                    case 1:         //SUD
                        longueur = aleatoire.Next(1, grille.GetLength(0) - 1);
                        colonne = aleatoire.Next(0,grille.GetLength(1));
                        ligne = aleatoire.Next(0, grille.GetLength(0)-longueur-1);
                        break;
                    case 2:         //OUEST 
                        longueur = aleatoire.Next(1, grille.GetLength(1) - 1);
                        colonne = aleatoire.Next(longueur+1, grille.GetLength(1));
                        ligne = aleatoire.Next(0, grille.GetLength(0));
                        break;
                    case 3:         //NORD 
                        longueur = aleatoire.Next(1, grille.GetLength(0) - 1);
                        colonne = aleatoire.Next(0, grille.GetLength(1));
                        ligne = aleatoire.Next(longueur +1, grille.GetLength(0));
                        break;
                    case 4:         // SUD OUEST      

                        longueur = aleatoire.Next(1, Math.Min(grille.GetLength(0),grille.GetLength(1)) - 1); 
                        colonne = aleatoire.Next(longueur + 1, grille.GetLength(1));
                        ligne = aleatoire.Next(0, grille.GetLength(0) - longueur - 1);
                        break;
                    case 5:         // SUD EST  
                        longueur = aleatoire.Next(1, Math.Min(grille.GetLength(0), grille.GetLength(1)) - 1);
                        colonne = aleatoire.Next(0, grille.GetLength(1) - longueur - 1);
                        ligne = aleatoire.Next(0, grille.GetLength(0) - longueur - 1);
                        break;
                    case 6:         // NORD OUEST 
                        longueur = aleatoire.Next(1, Math.Min(grille.GetLength(0), grille.GetLength(1)) - 1);
                        colonne = aleatoire.Next(longueur + 1, grille.GetLength(1));
                        ligne = aleatoire.Next(longueur + 1, grille.GetLength(0));
                        break;
                    case 7:         // NORD EST
                        longueur = aleatoire.Next(1, Math.Min(grille.GetLength(0), grille.GetLength(1)) - 1);
                        colonne = aleatoire.Next(0, grille.GetLength(1) - longueur - 1);
                        ligne = aleatoire.Next(longueur + 1, grille.GetLength(0));
                        break;
                }
                // Il choisit l'index d'un mot au hasard et donc va définir le mot que l'on veut tester
                int index = aleatoire.Next(0, dico.Mots[longueur].Length);
                // On teste si on peut remplir le mot dans la grille et qu'il n'est pas déjà dans la liste des mts à recherchés
                if ((mot_a_rechercher.Contains(dico.Mots[longueur][index]) == false) && (Test_Plateau(dico.Mots[longueur][index], ligne, colonne, direction_possible[d]) == true))   
                {
                    // On ajoute le mot que l'on va remplir dans le tableau des mots à rechercher
                    mot_a_rechercher.Add(dico.Mots[longueur][index]);
                    // On remplit la grille ici
                    for (int remplir = 0; remplir < dico.Mots[longueur][index].Length; remplir++)                                
                    {
                        grille[ligne, colonne] = dico.Mots[longueur][index][remplir];
                        int[] tab = Plateau.Direction(direction_possible[d], ligne, colonne);
                        ligne = tab[0];
                        colonne = tab[1];
                    }
                    //Console.WriteLine(toString());      permet d'afficher le plateau au fur et à mesure de sa création                                
                }                             
            }
            // Ici on va simplement combler tous les espaces par des lettres après l'avoir rempli par tous les mots à rechercher
            for(int l=0;l<grille.GetLength(0);l++)
            {
                for(int m=0;m<grille.GetLength(1);m++)
                {
                    if (grille[l,m]==' ')
                    {
                        char[] lettres = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
                        int aleatoires = aleatoire.Next(0, lettres.Length);
                        grille[l,m] = lettres[aleatoires];
                    }
                }
            }

            return grille;

        }
    }
}
