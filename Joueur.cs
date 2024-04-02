using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet3
{
    public class Joueur
    {

        private string nom;
        private int score;
        private List<string> mot_trouvee;

        public Joueur(string nom)
        {
            // On fait les tests pour voir si le nom existe
            while (nom == null || nom.Length == 0)
            {
                Console.WriteLine("Quel est le nom de ce Joueur ?");
                nom = Console.ReadLine();
            }                    
            this.nom = nom;
            this.score = 0;
            // l'initialisation se fait directement dans le jeu car elle est faite en fonction des mots à chercher
            this.mot_trouvee = null;
        }
        /// <summary>
        /// 2e constructeur utile pour initialiser les joueurs en provenance d'une sauvegarde
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="score"></param>
        public Joueur(string nom,int score)
        {
            this.nom = nom;
            this.score = score;
            this.mot_trouvee = new List<string>();

        }
        /// <summary>
        /// Une des différentes propriétés des différents attribus. Ici on pourra lire le score et le changer.
        /// </summary>
        public int Score
        {
            get { return this.score; }
            set { this.score = value; }
        }

        public string Nom
        {
            get { return this.nom; }
            set { this.nom = value; }
        }
        /// <summary>
        /// Le tableau des mots trouvés va donc augmenter au fur et à mesure.
        /// </summary>
        public List<string> Mot_trouvee
        {
            get { return this.mot_trouvee; }
            set { this.mot_trouvee = value; }
        }
        /// <summary>
        /// Cette méthode permet d'ajouter un mot dans la liste des mots trouvés (pas très utile car la fonction existe déjà pour les listes)
        /// </summary>
        /// <param name="mot"></param>
        public void Add_Mot(string mot)
        {
            mot_trouvee.Add(mot);
        }
        /// <summary>
        /// Cette méthode va décrire un joueur
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            string a = "nom : " + nom + "\nscore : " + score;
            // On teste bien si mot_trouvee != null pour ne rien écrire si aucun mot n'a été trouvé
            if (mot_trouvee != null && mot_trouvee.Count!=0) 
            {
                a += "\nLes mots trouvés sont ceux-ci : " + mot_trouvee[0];
                for (int i = 1; i < mot_trouvee.Count; i++)
                {
                    a += ", " + mot_trouvee[i];                
                  
                }
            }
            return a;
        }
        /// <summary>
        /// Je l'utilise simplement pour donner le score final
        /// </summary>
        /// <returns></returns>
        public string toString_Final()                  
        {
            string a = nom + " avec un score de " + score;
            return a;
        }
        /// <summary>
        /// Cette méthode va ajouter une valeur au score actuel du joueur.
        /// </summary>
        /// <param name="val"></param>
        public void Add_Score(int val)
        {
            score += val;
        }
    }
}
