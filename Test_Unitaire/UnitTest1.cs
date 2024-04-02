using System;
using static System.Formats.Asn1.AsnWriter;

namespace Projet3
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]

        public void TestAdd_Score()
        {
            Joueur a = new Joueur("Nono");
            a.Add_Score(7);
            Assert.AreEqual(7, a.Score);
        }

        [TestMethod]
        public void TestAdd_Mot()
        {
            Joueur a = new Joueur("Nono");
            a.Mot_trouvee = new List<string>();
            string mot = "TEST";
            a.Add_Mot(mot);
            Assert.AreEqual(true, a.Mot_trouvee.Contains(mot));
        }
        [TestMethod]
        public void Test_RechDichoRecursif()
        {
            //Test FR 
            Dictionnaire dico = new Dictionnaire("FR");
            string mot = "AA";
            Assert.AreEqual(true, dico.RechDichoRecursif(mot, dico.Mots[mot.Length - 2].Length));
            mot = "TRANSISTORISANT";
            Assert.AreEqual(true, dico.RechDichoRecursif(mot, dico.Mots[mot.Length - 2].Length));
            mot = "AAAAAA";
            Assert.AreEqual(false, dico.RechDichoRecursif(mot, dico.Mots[mot.Length - 2].Length));
            // Test EN 
            Dictionnaire dicobis = new Dictionnaire("EN");
            string mot2 = "GULF";
            Assert.AreEqual(true, dicobis.RechDichoRecursif(mot2, dicobis.Mots[mot2.Length - 2].Length));
            mot2 = "ZYGOPHYLLACEAE";
            Assert.AreEqual(true, dicobis.RechDichoRecursif(mot2, dicobis.Mots[mot2.Length - 2].Length));
            mot2 = "AAAAAA";
            Assert.AreEqual(false, dicobis.RechDichoRecursif(mot2, dicobis.Mots[mot2.Length - 2].Length)); 

        }
        [TestMethod]
        
        public void Test_toStringJoueur()
        {
            string nom = "Nono";
            Joueur a = new Joueur(nom);
            string rep = "nom : " + nom + "\nscore : " + a.Score;
            Assert.AreEqual(rep, a.toString());
            a.Mot_trouvee = new List<string>();
            a.Add_Mot("TEST");
            rep = rep + "\nLes mots trouvés sont ceux-ci : " + "TEST";
            Assert.AreEqual(rep, a.toString());

        }
        [TestMethod]
        public void Test_TestPlateau()
        {
            // Ce n'est pas le test pour remplir le plateau mais pour tester si le mot appartient au plateau après la création
            Dictionnaire dico = new Dictionnaire("FR");
            Plateau a = new Plateau(1,dico,1,1);
            string mot = "PAPAP";
            Assert.AreEqual(false, a.Test_Plateau(mot, 0, 0, "E"));
            // On test par rapport à un plateau déjà crée
            Plateau b = new Plateau("plateau_niveau1_joueur1FR", "plateaux_initiaux/", dico);
            string mot2 = "MIMOSA";
            Assert.AreEqual(true, b.Test_Plateau(mot2, 1, 4, "S"));

        }

    }
}