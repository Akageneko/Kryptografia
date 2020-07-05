using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Math.Gmp.Native;

namespace Projekt_2.logika
{
    class Game
    {
        int ile_graczy;
        string sciezka_cert;

        X509Certificate2 cert;
        string haslo_cert = "Qwerty";

        List<Uczestnik> uczestnicy = new List<Uczestnik>();
        List<bool> rsaUser = new List<bool>();
        List<Krypto> krypto = new List<Krypto>();
        bool rsaUsing = true;

        mpz_t g;
        mpz_t p;

        

        struct user_gxi
        {
            public mpz_t gxi;
            public int id;
            public user_gxi(int id, string g)
            {
                this.id = id;
                gxi = new mpz_t();
                gmp_lib.mpz_init(gxi);
                gxi = (mpz_t)g;
            }
        }
        struct user_ZKP_gx
        {
            public mpz_t s;
            public mpz_t gv;
            string h;
            public int id;
            public user_ZKP_gx(int id, string gvs, string ss,  string hh)
            {
                this.id = id;
                s = new mpz_t();
                gmp_lib.mpz_init(s);
                s = (mpz_t)ss;
                gv = new mpz_t();
                gmp_lib.mpz_init(gv);
                gv = (mpz_t)gvs;
                this.h = hh;
            }
        }

        List<user_gxi> uzytkownik_gxi;
        List<user_gxi> uzytkownik_glos;
        List<user_ZKP_gx> uzytkownik_zpk_gx;

        public Game(int ile_graczy, string certyfikaty)
        {
            this.ile_graczy = ile_graczy;
            this.sciezka_cert = certyfikaty;
            this.cert = new X509Certificate2(sciezka_cert + "servcert.p12", haslo_cert);


            for (int i = 1; i <= ile_graczy; i++) {
                krypto.Add(new Krypto(cert));

                uczestnicy.Add(new Uczestnik(i, sciezka_cert, this));
                rsaUser.Add(true);

            }

            uzytkownik_gxi = new List<user_gxi>();
            uzytkownik_glos = new List<user_gxi>();
            uzytkownik_zpk_gx = new List<user_ZKP_gx>();

            foreach (Uczestnik u in uczestnicy) u.Greatings();

            MakeServerGame();
        }

        private void MakeServerGame()
        {
            //ustalanie grupy
            g = new mpz_t();
           /* gmp_lib.mpz_init(g);
            gmp_lib.mpz_random(g, 25);
            gmp_lib.mpz_nextprime(g,g);
           */ p = new mpz_t();
            /* gmp_lib.mpz_init(p);
             gmp_lib.mpz_random(p, 25);
             gmp_lib.mpz_mul(p,p,g);
             gmp_lib.mpz_nextprime(p, p);
           */

            gmp_lib.mpz_init_set_ui(g,11);
            gmp_lib.mpz_init_set_ui(p, 23);

            // byte[] pb = Encoding.UTF8.GetBytes(p.ToString());
            // byte[] gb = Encoding.UTF8.GetBytes(g.ToString());
            for (int i = 1; i<=uczestnicy.Count; i++)
            {
                SEND("GrupaDane|"+g.ToString()+"|"+p.ToString(), i);
            }
            //wysłano grupy

        }


        private void Dodaj_GXI(string gx, int id )
        {
            if (uzytkownik_gxi.Count == 0)
            {
                uzytkownik_gxi.Add(new user_gxi(id, gx));
            }
            else
            {
                int i = 0;
                while (i < uzytkownik_gxi.Count && uzytkownik_gxi[i].id < id)
                {
                    i++;
                }
                uzytkownik_gxi.Insert(i, new user_gxi(id, gx));
            }
        }

        private void Dodaj_GXI_Proof(string h, string gv, string s, int id)
        {
            if (uzytkownik_zpk_gx.Count == 0)
            {
                uzytkownik_zpk_gx.Add(new user_ZKP_gx(id, gv, s, h));
            }
            else
            {
                int i = 0;
                while (i < uzytkownik_zpk_gx.Count && uzytkownik_zpk_gx[i].id < id)
                {
                    i++;
                }
                uzytkownik_zpk_gx.Insert(i, new user_ZKP_gx(id, gv, s, h));
            }
        }

        private void Dodaj_GLOS(string gx, int id)
        {
            if (uzytkownik_glos.Count == 0)
            {
                uzytkownik_glos.Add(new user_gxi(id, gx));
            }
            else
            {
                int i = 0;
                while (i < uzytkownik_glos.Count && uzytkownik_glos[i].id < id)
                {
                    i++;
                }
                uzytkownik_glos.Insert(i, new user_gxi(id, gx));
            }
        }

        private void SEND(string s, int id)
        {
            //Print(id.ToString() + " " + s);
            byte[] wiadomosc;
            if (rsaUser[id-1])
            {
                wiadomosc = krypto[id-1].ZakodujRSA(s);
            }
            else
            {
                wiadomosc = krypto[id - 1].ZakodujAES(s);
            }


            uczestnicy[id-1].RECIVE(wiadomosc);
        }

        public void RECIVE(byte[] s, int id)
        {
            if (Encoding.UTF8.GetString(s).CompareTo("HelloClient") == 0)
            {
                ManageMessages(Encoding.UTF8.GetString(s), id);
                return;
            }

            string wiado;
            if (rsaUser[id-1])
            {
                wiado = krypto[id - 1].OdkodujRSA(s);
            }
            else
            {
                wiado = krypto[id - 1].OdkodujAES(s);
            }
            ManageMessages(wiado,id);
        }

        private void ManageMessages(string s, int id)
        {
            string[] temp = s.Split('|');
              Print(s);
            switch (temp[0])
            {
                case "HelloClient":
                        string ss = "HelloServer|" + krypto[id-1].GetPublicKeyXML(); 
                        SEND(ss,id);
                        
                    break;
                case "AESData":
                    krypto[id - 1].AESKeySet((temp[1]));
                    rsaUser[id - 1] = false;

                        
                    break;
                case "GXI":
                    Dodaj_GXI(temp[1], id);
                    Dodaj_GXI_Proof(temp[2], temp[3], temp[4], id);

                    if(uzytkownik_gxi.Count == uczestnicy.Count) // jezeli masz juz wszystkie
                    {
                        string sss = "GotoweDoGY";
                        foreach (user_gxi u in uzytkownik_gxi)
                        {
                            sss += "|" + u.id + "|" + u.gxi;
                        }
                        for (int i= 1;i<=uczestnicy.Count; i++)
                        {
                            SEND(sss,i);
                        }
                        sss = "Glosuj";
                        for (int i = 1; i <= uczestnicy.Count; i++)//glosowoanie
                        {
                            SEND(sss, i);
                        }
                    }
                    break;
                case "GLOS":
                    Dodaj_GLOS(temp[1], id);

                    if (uzytkownik_glos.Count == uczestnicy.Count) // jezeli masz juz wszystkie
                    {
                        string sss = "Wyniki";
                        foreach (user_gxi u in uzytkownik_glos)
                        {
                            sss += "|" + u.id + "|" + u.gxi;
                        }
                        for (int i = 1; i <= uczestnicy.Count; i++)
                        {
                            SEND(sss, i);
                        }
                    }
                    break;
            }
        }

        private void Print(String s)
        {
            Console.WriteLine("SERVER: {0}", s);
        }
    }
}
