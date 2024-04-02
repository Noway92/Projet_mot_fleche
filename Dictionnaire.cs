using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.ExceptionServices;

namespace Projet3
{
    public class Dictionnaire
    {

        private string langue;
        private string[][] mots;
        /// <summary>
        /// Ce constructeur va permettre de transformer les dictionnaires donnés en tableau de tableau de chaines de caractères. Sur chaque ligne il y tous les mots en fonction de leur nombre de lettres, et chaque mot est dans une case du tableau de la ligne.
        /// </summary>
        /// <param name="langue"></param>
        public Dictionnaire(string langue)
        {
            // On demande à l'utilisateur de bien retourner EN pour Anglais et FR pour français
            while (langue!= "FR" && langue!= "EN")
            {
                Console.WriteLine("Les langues possibles sont le français : 'FR' et l'anglais : 'EN', veuillez réécrire.");
                langue = Console.ReadLine().ToUpper() ;
            }
            // On initialise le nombre de ligne à 14 car il y a des mots de 2 lettres à 15 lettres dans tous les cas
            mots = new string[14][];
            // Ce compteur permit de remplir l'atribut mots
            int compteur = 0;
            // Le fichier que l'on va chercher est différent en fonction de la langue donnée
            string[] readText = File.ReadAllLines("MotsPossibles"+langue+".txt");
            for (int i = 0; i < readText.Length; i++)
            {
                // On veur remplit 'mots' que en fonction des i impairs car les mots sont sur des lignes impaires et les longueurS sur les longueurs paires     
                if (i % 2 != 0)
                {

                    mots[compteur] = readText[i].Split(' ');
                    compteur++;
                }
            }

            this.mots = mots;
            this.langue = langue;
        }

        public string[][] Mots
        {
            get { return this.mots;}
        }

        public string Langue
        {
            get { return this.langue; }
        }

        /// <summary>
        /// On teste si un mot appartient bien au dictionnaire
        /// </summary>
        /// <param name="mot"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public bool RechDichoRecursif(string mot,int max,int min=0)      
        {
            //On veut éviter l'entrée de mot null et de longueur 0
            if(mot != null && mot.Length!=0)
            {
                if (max == min)
                {
                    return false;
                }
                // On initialise à mot.Length - 2 pour comparer aux mots du même nombre de lettre
                if (mots[mot.Length - 2][(max + min) / 2] == mot)
                {
                    return true;
                }
                if (mots[mot.Length - 2][(max + min) / 2].CompareTo(mot) > 0)
                {
                    return RechDichoRecursif(mot, (max + min) / 2, min);
                }
                return RechDichoRecursif(mot, max, 1 + (max + min) / 2);
            }
            else
            {
                return false;
            }
            
        }
        /// <summary>
        /// Permet d'afficher le nombres de mots de chaque lettres du dictionnaire
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            string rep = "La langue est : "+langue+"\n";
            for(int i =0; i < mots.Length;i++)
            {
                rep += "Dans la longueur numéro " + i + " Il y a " + mots[i].Length+" mots.\n";
            }
            return rep;
        }
    }
}
